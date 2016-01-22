using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.Entity;

using AspNet.Identity.EntityFramework6;

using CustomClassesSample.Models;

namespace CustomClassesSample.Identity
{
    public class ApplicationRoleStore : RoleStore<ApplicationRole, ApplicationUserRole, ApplicationRoleClaim, DbContext, int>
    {
        public ApplicationRoleStore(DbContext context) : base(context) { }
    }
}
