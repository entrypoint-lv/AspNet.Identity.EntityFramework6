using System;
using System.Collections.Generic;

namespace AspNet.Identity.EntityFramework6
{
    /// <summary>
    ///     Represents a Role entity
    /// </summary>
    public class IdentityRole : IdentityRole<string, IdentityUserRole, IdentityRoleClaim>
    {
        /// <summary>
        ///     Constructor
        /// </summary>
        public IdentityRole() : base()
        {
            Id = Guid.NewGuid().ToString();
        }

        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="roleName"></param>
        public IdentityRole(string roleName)
            : this()
        {
            Name = roleName;
        }
    }

    /// <summary>
    ///     Represents a Role entity
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TUserRole"></typeparam>
    public class IdentityRole<TKey, TUserRole, TRoleClaim>
        where TUserRole : IdentityUserRole<TKey>
        where TRoleClaim : IdentityRoleClaim<TKey>
    {
        /// <summary>
        ///     Constructor
        /// </summary>
        public IdentityRole()
        {
            Users = new List<TUserRole>();
            Claims = new List<TRoleClaim>();
            ConcurrencyStamp = Guid.NewGuid().ToString();
        }

        /// <summary>
        /// Initializes a new instance of <see cref="IdentityRole"/>.
        /// </summary>
        /// <param name="roleName">The role name.</param>
        /// <remarks>
        /// The Id property is initialized to from a new GUID string value.
        /// </remarks>
        public IdentityRole(string roleName) : this()
        {
            Name = roleName;
        }

        /// <summary>
        ///     Navigation property for users in the role
        /// </summary>
        public virtual ICollection<TUserRole> Users { get; }

        /// <summary>
        ///     Navigation property for claims in the role
        /// </summary>
        public virtual ICollection<TRoleClaim> Claims { get; }

        /// <summary>
        ///     Role id
        /// </summary>
        public virtual TKey Id { get; set; }

        /// <summary>
        ///     Role name
        /// </summary>
        public virtual string Name { get; set; }
        public virtual string NormalizedName { get; set; }

        /// <summary>
        /// A random value that should change whenever a role is persisted to the store
        /// </summary>
        public virtual string ConcurrencyStamp { get; set; }

        /// <summary>
        /// Returns a friendly name
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Name;
        }
    }
}
