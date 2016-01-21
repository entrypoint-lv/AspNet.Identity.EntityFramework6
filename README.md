# EF6 provider for Asp.Net Identity 3.0 RC1

As Identity 3.0 RC1 does not provide the support for EF6, here is the custom provider.

## Basic Usage

1. Reference both EntityFramework 6 and Microsoft.AspNet.Identity packages in your project.json, but DO NOT reference the Microsoft.AspNet.Identity.EntityFramework package:

          "frameworks": {
            "dnx451": {
              "dependencies": {
                "EntityFramework": "6.1.3",
                "Microsoft.AspNet.Identity": "3.0.0-rc1-final"
              },
              "frameworkAssemblies": {
                "System.Data": "4.0.0.0"
              }
            }
          } 

2. Make sure that you inherit your data context from the provided IdentityDbContext:

        using AspNet.Identity.EntityFramework6;
        
        public class AppDbContext : IdentityDbContext
        {
            public AppDbContext(string nameOrConnectionString) : base(nameOrConnectionString) { }
        }

2. Init Asp.Net Identity with provided IdentityUser, IdentityRole, UserStore and RoleStore in Startup.cs. Also, don't forget to configure the dependency injection for your data context as well:

        using AspNet.Identity.EntityFramework6;
        
        public class Startup
        {
            public void ConfigureServices(IServiceCollection services)
            {
                services.AddScoped<DbContext, AppDbContext>(f => {
                    return new AppDbContext(Configuration["Data:DefaultConnection:ConnectionString"]);
                });
    
                services.AddIdentity<IdentityUser, IdentityRole>()
                    .AddRoleStore<RoleStore>()
                    .AddUserStore<UserStore>()
                    .AddDefaultTokenProviders();
            }
        }


## Your own POCOs

Of course, you can inherit from provided POCOs in order to add new properties or change the type of the primary key for user and role. 
In the following example we add some properties and use Int32 instead of default String as the primary key type:

1. Create your own derived POCOs:

        using AspNet.Identity.EntityFramework6;
        
        public class AppUser: IdentityUser<int, AppUserLogin, AppUserRole, AppUserClaim> {
        
            public virtual string MyNewProperty { get; set; }

        }
    
        public class AppUserRole : IdentityUserRole<int> { }
    
        public class AppUserLogin : IdentityUserLogin<int> { }
    
        public class AppUserClaim : IdentityUserClaim<int> { }
    
        public class AppRoleClaim : IdentityRoleClaim<int> { }
    
        public class AppRole : IdentityRole<int, AppUserRole, AppRoleClaim> { }
    
2. Create your own derived data context:

        using AspNet.Identity.EntityFramework6;
        
        public class AppDbContext : IdentityDbContext<AppUser, AppRole, int, AppUserLogin, AppUserRole, AppUserClaim, AppRoleClaim>, IDisposable
        {
            public AppDbContext(string nameOrConnectionString) : base(nameOrConnectionString) { }
        }

3. Create your own derived UserStore and RoleStore:

        using AspNet.Identity.EntityFramework6;
        
        public class AppRoleStore : RoleStore<AppRole, AppUserRole, AppRoleClaim, DbContext, int>
        {
            public AppRoleStore(AppDbContext context) : base(context) { }
        }
    
        public class AppUserStore : UserStore<AppUser, AppRole, AppUserRole, AppUserClaim, AppUserLogin, AppRoleClaim, DbContext, int>
        {
            public AppUserStore(AppDbContext context) : base(context) { }
        }
    
4. Alter Startup.cs accordingly:

        using AspNet.Identity.EntityFramework6;
        
        public class Startup
        {
            public void ConfigureServices(IServiceCollection services)
            {
                services.AddScoped<AppDbContext, AppDbContext>(f => {
                    return new AppDbContext(Configuration["Data:DefaultConnection:ConnectionString"]);
                });
            
                services.AddIdentity<AppUser, AppRole>()
                    .AddRoleStore<AppRoleStore>()
                    .AddUserStore<AppUserStore>()
                    .AddDefaultTokenProviders();
          }
        }
    
