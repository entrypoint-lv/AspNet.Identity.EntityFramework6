# AspNet.Identity.EntityFramework6

As Identity 3.0 RC1 does not provide any support for EF6, here it the custom implementation.

## Basic Usage

Init Identity 3.0 with provided IdentityUser, IdentityRole, UserStore and RoleStore:

        using AspNet.Identity.EntityFramework6;
        
        public class Startup
        {
          public void ConfigureServices(IServiceCollection services)
          {
              services.AddIdentity<IdentityUser, IdentityRole>()
              .AddRoleStore<RoleStore>()
              .AddUserStore<UserStore>()
              .AddDefaultTokenProviders();
          }
        }


## Different type for primary keys

Here is how you can use Int32 instead of default String for primary keys:

1. Create your own POCOs by inheriting from provided ones:

        using AspNet.Identity.EntityFramework6;
        
        public class AppUser: IdentityUser<int, AppUserLogin, AppUserRole, AppUserClaim> { }
    
        public class AppUserRole : IdentityUserRole<int> { }
    
        public class AppUserLogin : IdentityUserLogin<int> { }
    
        public class AppUserClaim : IdentityUserClaim<int> { }
    
        public class AppRoleClaim : IdentityRoleClaim<int> { }
    
        public class AppRole : IdentityRole<int, AppUserRole, AppRoleClaim> { }
    
2. Create your own UserStore and RoleStore by inheriting from provided ones:

        using AspNet.Identity.EntityFramework6;
        
        public class AppRoleStore : RoleStore<AppRole, AppUserRole, AppRoleClaim, DbContext, int>
        {
            public AppRoleStore(DbContext context) : base(context) { }
        }
    
        public class AppUserStore : UserStore<AppUser, AppRole, AppUserRole, AppUserClaim, AppUserLogin, AppRoleClaim, DbContext, int>
        {
            public AppUserStore(DbContext context) : base(context) { }
        }
    
3. Alter Startup.cs accordingly:

        using AspNet.Identity.EntityFramework6;
        
        public class Startup
        {
          public void ConfigureServices(IServiceCollection services)
          {
              services.AddIdentity<AppUser, AppRole>()
              .AddRoleStore<AppRoleStore>()
              .AddUserStore<AppUserStore>()
              .AddDefaultTokenProviders();
          }
        }
    
