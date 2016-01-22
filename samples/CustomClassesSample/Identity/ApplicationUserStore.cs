using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.Entity;

using AspNet.Identity.EntityFramework6;

using CustomClassesSample.Models;

namespace CustomClassesSample.Identity
{
    public class ApplicationUserStore : UserStore<ApplicationUser, ApplicationRole, ApplicationUserRole, ApplicationUserClaim, ApplicationUserLogin, ApplicationRoleClaim, DbContext, int>
    {
        public ApplicationUserStore(DbContext context) : base(context) { }
    }
}
