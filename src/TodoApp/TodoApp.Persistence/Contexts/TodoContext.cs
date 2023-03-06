using Core.Domain.Entities;
using Core.Security.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TodoApp.Domain.Entities;

namespace TodoApp.Persistence.Contexts
{
    public class TodoContext : IdentityDbContext<User, Role, string>
    {
        public TodoContext(DbContextOptions options) : base(options)
        {
        }

        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<RefreshToken> RefreshTokens { get; set; }
        public virtual DbSet<ShoppingList> ShoppingLists { get; set; }
        public virtual DbSet<ShoppingListCategory> ShoppingListCategories { get; set; }
        public virtual DbSet<ShoppingListItem> ShoppingListItems { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ShoppingList>()
                .HasOne(e => e.Category)
                .WithMany(e => e.ShoppingLists)
                .HasForeignKey(e => e.ShoppingListCategoryId);

            builder.Entity<ShoppingListItem>()
                .HasKey(e => new { e.ProductId, e.ShoppingListId });

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

            builder.Entity<Product>().HasData(new Product[]
            {
                new("Bread") { Id = Guid.NewGuid() },
                new("Apple") { Id = Guid.NewGuid() },
                new("Pear") { Id = Guid.NewGuid() },
                new("Coke") { Id = Guid.NewGuid() },
                new("Meat") { Id = Guid.NewGuid() },
                new("Pen") { Id = Guid.NewGuid() },
                new("Book") { Id = Guid.NewGuid() },
                new("Notebook") { Id = Guid.NewGuid() }
            });

            builder.Entity<ShoppingListCategory>().HasData(new ShoppingListCategory[]
            {
                new("School") { Id = Guid.NewGuid() },
                new("Grocery") { Id = Guid.NewGuid() }
            });

            #region Default Account
            Role[] roles = new Role[]
            {
                new()
                {
                    Id = "7f3s213a-9l2q-321f-24ea-321f12op1453",
                    Name = "admin",
                    NormalizedName = "ADMIN".ToUpper()
                }
            };
            builder.Entity<Role>().HasData(roles);

            var hasher = new PasswordHasher<User>();
            User user = new()
            {
                Id = "8e445865-a24d-4543-a6c6-9443d048cdb9",
                Email = "admin@admin.com",
                NormalizedEmail = "ADMIN@ADMIN.COM",
                UserName = "admin",
                NormalizedUserName = "ADMIN",
                PhoneNumber = "+00 000 000 0000",
                SecurityStamp = "QTC5PI3U6GFOVY2KSSYWA32ZYUMFBPF6",
                ConcurrencyStamp = "d8599b1a-40e3-40a5-a033-6624d2123d21",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true
            };
            user.PasswordHash = hasher.HashPassword(user, "admin");
            builder.Entity<User>().HasData(user);

            IdentityUserRole<string>[] userRoles = new IdentityUserRole<string>[]
            {
                new()
                {
                    RoleId = "7f3s213a-9l2q-321f-24ea-321f12op1453",
                    UserId = "8e445865-a24d-4543-a6c6-9443d048cdb9"
                }
            };

            builder.Entity<IdentityUserRole<string>>().HasData(userRoles);
            #endregion
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
