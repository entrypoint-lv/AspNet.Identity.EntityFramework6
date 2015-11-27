using System;
using System.Collections.Generic;

namespace AspNet.Identity.EntityFramework6
{
    /// <summary>
    ///     EntityType that represents one specific user claim
    /// </summary>
    public class IdentityRoleClaim : IdentityRoleClaim<string>
    {
    }

    /// <summary>
    ///     EntityType that represents one specific role claim
    /// </summary>
    public class IdentityRoleClaim<TKey>
    {
        /// <summary>
        ///     Primary key
        /// </summary>
        public virtual int Id { get; set; }

        /// <summary>
        ///     User Id for the role this claim belongs to
        /// </summary>
        public virtual TKey RoleId { get; set; }

        /// <summary>
        ///     Claim type
        /// </summary>
        public virtual string ClaimType { get; set; }

        /// <summary>
        ///     Claim value
        /// </summary>
        public virtual string ClaimValue { get; set; }
    }
}
