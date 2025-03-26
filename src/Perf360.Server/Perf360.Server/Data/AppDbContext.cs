using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Perf360.Server.Data.Models;
using System.Linq.Expressions;
using System.Reflection;

namespace Perf360.Server.Data
{
    public class AppDbContext : IdentityDbContext<User, Role, string>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Department> Departments { get; set; } = default!;
        public DbSet<ReviewDimension> ReviewDimensions { get; set; } = default!;
        public DbSet<Review> Reviews { get; set; } = default!;
        public DbSet<ReviewRecord> ReviewRecords { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.HasCharSet("utf8mb4 COLLATE utf8mb4_zh_0900_as_cs");

            base.OnModelCreating(builder);

            foreach (var entityType in builder.Model.GetEntityTypes())
            {
                if (entityType.ClrType.IsAssignableTo(typeof(Entity)))
                {
                    builder.Entity(entityType.ClrType).Property(nameof(Entity.CreateAt)).HasDefaultValueSql("CURRENT_TIMESTAMP(6)").ValueGeneratedOnAdd();
                    builder.Entity(entityType.ClrType).Property(nameof(Entity.UpdateAt)).HasDefaultValueSql("CURRENT_TIMESTAMP(6)").ValueGeneratedOnAddOrUpdate();
                    builder.Entity(entityType.ClrType).HasQueryFilter((LambdaExpression)typeof(AppDbContext).GetMethod(nameof(GetSoftDeleteQueryFilter), BindingFlags.Static | BindingFlags.NonPublic)!.MakeGenericMethod(entityType.ClrType).Invoke(null, null)!);

                }
            }

            builder.Entity<Review>().HasMany(x => x.Participants).WithMany(x => x.Reviews);
        }

        static LambdaExpression GetSoftDeleteQueryFilter<TEntity>() where TEntity : Entity
        {
            Expression<Func<TEntity, bool>> expr = x => x.DeleteAt == null;
            return expr;
        }
    }
}
