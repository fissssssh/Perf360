using System.Text;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.IdentityModel.Tokens;
using Perf360.Server;
using Perf360.Server.Data;
using Perf360.Server.Data.Models;
using Perf360.Server.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var connectionString = builder.Configuration.GetConnectionString("Default");
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseMySql(connectionString, MySqlServerVersion.LatestSupportedServerVersion, mysqlOptions =>
    {
        mysqlOptions.EnableRetryOnFailure(maxRetryCount: 3, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
    });
    options.UseAsyncSeeding(async (context, _, cancellationToken) =>
    {
        var roleManger = context.GetService<RoleManager<Role>>();
        if (await roleManger.FindByNameAsync("admin") == null)
        {
            await roleManger.CreateAsync(new Role { Name = "admin" });
        }
        var userManager = context.GetService<UserManager<User>>();
        if (!userManager.Users.Any())
        {
            var admin = new User
            {
                UserName = "admin",
                NickName = "Admin",
                Email = "admin@admin.com",
            };
            await userManager.CreateAsync(admin, "Admin123!");
            await userManager.AddToRoleAsync(admin, "admin");
        }
    });
});

builder.Services.AddIdentityCore<User>(opt =>
{
    opt.Password.RequiredLength = 6;
    opt.Password.RequireLowercase = false;
    opt.Password.RequireUppercase = false;
    opt.Password.RequireDigit = false;
    opt.Password.RequireNonAlphanumeric = false;
}).AddRoles<Role>().AddEntityFrameworkStores<AppDbContext>();

builder.Services.AddControllers(opts =>
{
    opts.Conventions.Add(new RouteTokenTransformerConvention(new SlugifyParameterTransformer()));
}).AddNewtonsoftJson();

builder.Services.AddCors(b => b.AddDefaultPolicy(p =>
{
    p.AllowAnyHeader();
    p.AllowAnyMethod();
    p.AllowCredentials();
    p.SetIsOriginAllowed(origin => true);
}));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Mapster
var typeMappingRegisters = TypeAdapterConfig.GlobalSettings.Scan(typeof(Program).Assembly);
TypeAdapterConfig.GlobalSettings.Apply(typeMappingRegisters);

// Auth
var jwtSettings = new JwtBearerSettings();
builder.Configuration.GetSection("Jwt").Bind(jwtSettings);
builder.Services.Configure<JwtBearerSettings>(builder.Configuration.GetSection("Jwt"));
builder.Services.AddAuthentication().AddJwtBearer(opts =>
{
    opts.IncludeErrorDetails = true;
    opts.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = jwtSettings.Issuer,
        ValidAudience = jwtSettings.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.IssuerSigningKey)),
        ValidateIssuer = true,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ClockSkew = TimeSpan.Zero
    };
});
builder.Services.AddAuthorization();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<CurrentUser>();

var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await dbContext.Database.MigrateAsync();
}
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseDefaultFiles();
app.UseStaticFiles();
app.UseCors();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();
