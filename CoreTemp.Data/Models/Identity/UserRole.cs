using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoreTemp.Data.Models.Identity
{
    public class UserRole : IdentityUserRole<string>
    {
        public virtual MyUser User { get; set; }
        public virtual Role Role { get; set; }
    }
}
