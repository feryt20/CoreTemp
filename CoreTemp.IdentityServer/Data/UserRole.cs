using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreTemp.IdentityServer.Data
{
    public class UserRole : IdentityUserRole<string>
    {
        public virtual MyUser User { get; set; }
        public virtual Role Role { get; set; }
    }
}
