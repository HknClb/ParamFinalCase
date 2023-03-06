using Core.Domain.Entities;
using Core.Security.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Identity.Api.Contexts
{
    public class TodoContext : IdentityDbContext<User, Role, string>
    {
        public TodoContext(DbContextOptions options) : base(options)
        {
        }

        public virtual DbSet<RefreshToken> RefreshTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<RefreshToken>()
                .HasKey(e => e.UserId);
            builder.Entity<User>()
                .HasOne(e => e.RefreshToken)
                .WithOne(e => e.User)
                .HasForeignKey<RefreshToken>(e => e.UserId);

            builder.Entity<User>()
                .HasMany(e => e.Roles)
                .WithMany(e => e.Users)
                .UsingEntity<IdentityUserRole<string>>(entity =>
                {
                    entity.HasKey("UserId", "RoleId");
                    entity.HasOne<User>().WithMany().HasForeignKey("UserId");
                    entity.HasOne<Role>().WithMany().HasForeignKey("RoleId");
                    entity.ToTable("AspNetUserRoles");
                });
        }

        private void Interceptor()
        {
            var entityEntries = ChangeTracker.Entries<Entity>();

            foreach (var entry in entityEntries)
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        if (entry.Entity.Id.Equals(default))
                            entry.Entity.Id = Guid.NewGuid();
                        entry.Entity.CreatedDate = DateTime.UtcNow;
                        break;
                    case EntityState.Modified:
                        entry.Entity.UpdatedDate = DateTime.UtcNow;
                        break;
                    case EntityState.Deleted:
                        break;
                    case EntityState.Detached:
                        break;
                    case EntityState.Unchanged:
                        break;
                    default:
                        break;
                }
            }
        }

        #region SaveChanges Overrides
        public override int SaveChanges()
        {
            Interceptor();
            return base.SaveChanges();
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            Interceptor();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            Interceptor();
            return base.SaveChangesAsync(cancellationToken);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            Interceptor();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }
        #endregion
    }
}
