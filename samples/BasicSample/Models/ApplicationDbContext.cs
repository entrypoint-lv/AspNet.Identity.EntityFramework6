using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.Entity;

using AspNet.Identity.EntityFramework6;

namespace BasicSample.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(string nameOrConnectionString) : base(nameOrConnectionString) { }
    }
}
