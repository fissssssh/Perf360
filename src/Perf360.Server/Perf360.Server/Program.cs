using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Perf360.Server.Data;
using Perf360.Server.Data.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var connectionString = builder.Configuration.GetConnectionString("Default");
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseMySql(connectionString, MySqlServerVersion.LatestSupportedServerVersion);
    options.UseAsyncSeeding(async (context, _, cancellationToken) =>
    {
        var userManager = context.GetService<UserManager<User>>();
        if (!userManager.Users.Any())
        {
            var admin = new User
            {
                UserName = "admin",
                Email = "admin@admin.com",
            };
            await userManager.CreateAsync(admin, "Admin123!");
        }
    });
});

builder.Services.AddIdentityCore<User>().AddRoleManager<Role>().AddEntityFrameworkStores<AppDbContext>();


builder.Services.AddControllers().AddNewtonsoftJson();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
