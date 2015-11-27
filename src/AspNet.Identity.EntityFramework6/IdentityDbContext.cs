using System;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.Validation;

namespace AspNet.Identity.EntityFramework6
{
    /// <summary>
    /// Base class for the Entity Framework database context used for identity.
    /// </summary>
    public class IdentityDbContext : IdentityDbContext<IdentityUser> {
        public IdentityDbContext(string nameOrConnectionString) : base(nameOrConnectionString) { }
    }

    /// <summary>
    /// Base class for the Entity Framework database context used for identity.
    /// </summary>
    /// <typeparam name="TUser">The type of the user objects.</typeparam>
    public class IdentityDbContext<TUser> : IdentityDbContext<TUser, IdentityRole, string, IdentityUserLogin, IdentityUserRole, IdentityUserClaim, IdentityRoleClaim>
        where TUser : IdentityUser
    {
        public IdentityDbContext(string nameOrConnectionString) : base(nameOrConnectionString) { }
    }

    /// <summary>
    /// Generic IdentityDbContext base that can be customized with entity types that extend from the base IdentityUserXXX types.
    /// </summary>
    /// <typeparam name="TUser"></typeparam>
    /// <typeparam name="TRole"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TUserLogin"></typeparam>
    /// <typeparam name="TUserRole"></typeparam>
    /// <typeparam name="TUserClaim"></typeparam>
    /// <typeparam name="TRoleClaim"></typeparam>
    public class IdentityDbContext<TUser, TRole, TKey, TUserLogin, TUserRole, TUserClaim, TRoleClaim> : DbContext
        where TUser : IdentityUser<TKey, TUserLogin, TUserRole, TUserClaim>
        where TRole : IdentityRole<TKey, TUserRole, TRoleClaim>
        where TUserLogin : IdentityUserLogin<TKey>
        where TUserRole : IdentityUserRole<TKey>
        where TUserClaim : IdentityUserClaim<TKey>
        where TRoleClaim : IdentityRoleClaim<TKey>
    {
        public IdentityDbContext(string nameOrConnectionString) : base(nameOrConnectionString) { }

        /// <summary>
        /// Gets or sets the <see cref="DbSet{TEntity}"/> of Users.
        /// </summary>
        public DbSet<TUser> Users { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="DbSet{TEntity}"/> of User claims.
        /// </summary>
        public DbSet<TUserClaim> UserClaims { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="DbSet{TEntity}"/> of User logins.
        /// </summary>
        public DbSet<TUserLogin> UserLogins { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="DbSet{TEntity}"/> of User roles.
        /// </summary>
        public DbSet<TUserRole> UserRoles { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="DbSet{TEntity}"/> of roles.
        /// </summary>
        public DbSet<TRole> Roles { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="DbSet{TEntity}"/> of role claims.
        /// </summary>
        public DbSet<TRoleClaim> RoleClaims { get; set; }

        /// <summary>
        ///     If true validates that emails are unique
        /// </summary>
        public bool RequireUniqueEmail { get; set; }

        public IDbSet<TEntity> SetEntity<TEntity>() where TEntity : class
        {
            return base.Set<TEntity>();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            // Needed to ensure subclasses share the same table
            var user = modelBuilder.Entity<TUser>()
                .ToTable("AspNetUsers");
            user.Property(u => u.UserName)
                .IsRequired()
                .HasMaxLength(256)
                .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("UserNameIndex") { IsUnique = true }));
            user.Property(u => u.Email)
                .HasMaxLength(256)
                .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("UserEmailIndex") { IsUnique = true }));
            user.HasMany(u => u.Roles).WithRequired().HasForeignKey(ur => ur.UserId);
            user.HasMany(u => u.Claims).WithRequired().HasForeignKey(uc => uc.UserId);
            user.HasMany(u => u.Logins).WithRequired().HasForeignKey(ul => ul.UserId);

            modelBuilder.Entity<TUserRole>()
                .HasKey(r => new { r.UserId, r.RoleId })
                .ToTable("AspNetUserRoles");

            modelBuilder.Entity<TUserLogin>()
                .HasKey(l => new { l.LoginProvider, l.ProviderKey, l.UserId })
                .ToTable("AspNetUserLogins");

            modelBuilder.Entity<TUserClaim>()
                .ToTable("AspNetUserClaims");

            var role = modelBuilder.Entity<TRole>()
                .ToTable("AspNetRoles");
            role.Property(r => r.Name)
                .IsRequired()
                .HasMaxLength(256)
                .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("RoleNameIndex") { IsUnique = true }));
            role.HasMany(r => r.Users).WithRequired().HasForeignKey(ur => ur.RoleId);
            role.HasMany(r => r.Claims).WithRequired().HasForeignKey(ur => ur.RoleId);

            modelBuilder.Entity<TRoleClaim>()
                .ToTable("AspNetRoleClaims");
        }

        protected override DbEntityValidationResult ValidateEntity(DbEntityEntry entityEntry,
            IDictionary<object, object> items)
        {
            if (entityEntry != null && entityEntry.State == EntityState.Added)
            {
                var errors = new List<DbValidationError>();
                var user = entityEntry.Entity as TUser;
                //check for uniqueness of user name and email
                if (user != null)
                {
                    if (Users.Any(u => String.Equals(u.UserName, user.UserName)))
                    {
                        errors.Add(new DbValidationError("User",
                            String.Format(CultureInfo.CurrentCulture, "DuplicateUserName", user.UserName)));
                    }
                    if (RequireUniqueEmail && Users.Any(u => String.Equals(u.Email, user.Email)))
                    {
                        errors.Add(new DbValidationError("User",
                            String.Format(CultureInfo.CurrentCulture, "DuplicateEmail", user.Email)));
                    }
                }
                else
                {
                    var role = entityEntry.Entity as TRole;
                    //check for uniqueness of role name
                    if (role != null && Roles.Any(r => String.Equals(r.Name, role.Name)))
                    {
                        errors.Add(new DbValidationError("Role",
                            String.Format(CultureInfo.CurrentCulture, "RoleAlreadyExists", role.Name)));
                    }
                }
                if (errors.Any())
                {
                    return new DbEntityValidationResult(entityEntry, errors);
                }
            }
            return base.ValidateEntity(entityEntry, items);
        }
    }
}
