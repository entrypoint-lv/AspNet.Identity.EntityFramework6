# EF6 provider for Asp.Net Identity 3.0 RC1

As Identity 3.0 RC1 does not provide the support for EF6, here is the custom provider.

## Basic Usage

1. Reference both EntityFramework 6 and Microsoft.AspNet.Identity packages in your project.json, but DO NOT reference the Microsoft.AspNet.Identity.EntityFramework package:

```json
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
```

2. Create you applicaiton user class by inheriting from the provided IdentityUser:

```cs
using AspNet.Identity.EntityFramework6;

public class ApplicationUser : IdentityUser
{
}
```

3. Make sure that you inherit your data context from the provided IdentityDbContext:

```cs
using AspNet.Identity.EntityFramework6;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
  public ApplicationDbContext(string nameOrConnectionString) : base(nameOrConnectionString) { }
}
```

4. Init Asp.Net Identity with your ApplicationUser class and provided IdentityRole, UserStore and RoleStore classes in Startup.cs. Also, don't forget to configure the dependency injection for your data context as well:

```cs
using System.Data.Entity;
using AspNet.Identity.EntityFramework6;

public class Startup
{
  public void ConfigureServices(IServiceCollection services)
  {
      services.AddScoped<ApplicationDbContext>(f => {
          return new ApplicationDbContext(Configuration["Data:DefaultConnection:ConnectionString"]);
      });

      services.AddIdentity<ApplicationUser, IdentityRole>()
      .AddUserStore<UserStore<ApplicationUser, ApplicationDbContext>>()
      .AddRoleStore<RoleStore<ApplicationDbContext>>()
      .AddDefaultTokenProviders();
  }
}
```

Please refer to the [Basic Sample](samples/BasicSample) for the full code sample.

## Your own classes

Of course, you can inherit from provided Identity classes in order to add new properties or change the type of the primary key for user and role. 
In the following example we add some properties and use Int32 instead of default String as the primary key type:

1. Create your own derived POCOs:

```cs
using AspNet.Identity.EntityFramework6;

public class ApplicationUser: IdentityUser<int, ApplicationUserLogin, ApplicationUserRole, ApplicationUserClaim> {

  public virtual string MyNewProperty { get; set; }

}

public class ApplicationUserRole : IdentityUserRole<int> { }

public class ApplicationUserLogin : IdentityUserLogin<int> { }

public class ApplicationUserClaim : IdentityUserClaim<int> { }

public class ApplicationRoleClaim : IdentityRoleClaim<int> { }

public class ApplicationRole : IdentityRole<int, ApplicationUserRole, ApplicationRoleClaim> { }
```

2. Create your own derived data context:

```cs
using AspNet.Identity.EntityFramework6;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, int, ApplicationUserLogin, ApplicationUserRole, ApplicationUserClaim, ApplicationRoleClaim>
{
  public ApplicationDbContext(string nameOrConnectionString) : base(nameOrConnectionString) { }
}
```

3. Create your own derived UserStore and RoleStore:

```cs
using AspNet.Identity.EntityFramework6;

public class ApplicationRoleStore : RoleStore<ApplicationRole, ApplicationUserRole, ApplicationRoleClaim, ApplicationDbContext, int>
{
  public ApplicationRoleStore(ApplicationDbContext context) : base(context) { }
}

public class ApplicationUserStore : UserStore<ApplicationUser, ApplicationRole, ApplicationUserRole, ApplicationUserClaim, ApplicationUserLogin, ApplicationRoleClaim, ApplicationDbContext, int>
{
  public ApplicationUserStore(ApplicationDbContext context) : base(context) { }
}
```

4. Alter Startup.cs accordingly:

```cs
using System.Data.Entity;
using AspNet.Identity.EntityFramework6;

public class Startup
{
  public void ConfigureServices(IServiceCollection services)
  {
      services.AddScoped<ApplicationDbContext>(f => {
          return new ApplicationDbContext(Configuration["Data:DefaultConnection:ConnectionString"]);
      });
  
      services.AddIdentity<ApplicationUser, ApplicationRole>()
          .AddRoleStore<ApplicationRoleStore>()
          .AddUserStore<ApplicationUserStore>()
          .AddDefaultTokenProviders();
}
}
```
Please refer to the [Custom Classes Sample](samples/CustomClassesSample) for the full code sample.
