using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Perf360.Server.Data.Models;
using Perf360.Server.Data.Models.Abstractions;
using System.Linq.Expressions;
using System.Reflection;

namespace Perf360.Server.Data
{
    public class AppDbContext : IdentityDbContext<User, Role, string>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<ReviewDimension> ReviewDimensions { get; set; } = default!;
        public DbSet<Review> Reviews { get; set; } = default!;
        public DbSet<ReviewRecord> ReviewRecords { get; set; } = default!;
        public DbSet<ReviewRole> ReviewRoles { get; set; } = default!;

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

            builder.Entity<Review>().HasMany(x => x.Participants).WithMany(x => x.Reviews).UsingEntity<UserReview>(
                l => l.HasOne(l => l.User).WithMany(u => u.UserReviews).HasForeignKey(l => l.UserId).IsRequired(false),
                r => r.HasOne(r => r.Review).WithMany(r => r.UserReviews).HasForeignKey(r => r.ReviewId).IsRequired(false),
                j => j.HasKey(ur => new { ur.UserId, ur.ReviewId, ur.ReviewRoleId })
            );
            builder.Entity<UserReview>().HasOne(ur => ur.ReviewRole).WithMany().IsRequired(false);
        }

        static LambdaExpression GetSoftDeleteQueryFilter<TEntity>() where TEntity : Entity
        {
            Expression<Func<TEntity, bool>> expr = x => x.DeleteAt == null;
            return expr;
        }
    }
}
