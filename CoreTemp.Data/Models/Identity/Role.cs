using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoreTemp.Data.Models.Identity
{
    public class Role : IdentityRole
    {
        public virtual ICollection<UserRole> UserRoles { get; set; }
    }
    //public class Role : IdentityRole
    //{
    //    public Role() : base() { }
    //    public Role(string name) : base(name) { }
    //}
}
