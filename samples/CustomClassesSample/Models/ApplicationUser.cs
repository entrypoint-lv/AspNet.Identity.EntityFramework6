using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AspNet.Identity.EntityFramework6;

namespace CustomClassesSample.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser<int, ApplicationUserLogin, ApplicationUserRole, ApplicationUserClaim>
    {
        public virtual string MyNewProperty { get; set; }
    }
}
