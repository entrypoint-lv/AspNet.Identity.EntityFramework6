using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AspNet.Identity.EntityFramework6;

namespace CustomClassesSample.Models
{
    public class ApplicationRole : IdentityRole<int, ApplicationUserRole, ApplicationRoleClaim>
    {
    }
}
