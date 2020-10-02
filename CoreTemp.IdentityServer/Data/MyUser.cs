using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoreTemp.IdentityServer.Data
{
    public class MyUser : IdentityUser
    {

        [StringLength(100, MinimumLength = 0)]
        public string Name { get; set; }
        public bool Gender { get; set; }
        public DateTime DateOfBirth { get; set; }

        [StringLength(50, MinimumLength = 0)]
        public string City { get; set; }

        [StringLength(500, MinimumLength = 0)]
        public string Address { get; set; }

        [StringLength(10, MinimumLength = 10)]
        public string PostalCode { get; set; }

        public DateTime LastActive { get; set; }

        [StringLength(150, MinimumLength = 0)]
        public string ImageUrl { get; set; }

        public virtual ICollection<MyToken> MyTokens { get; set; }
        //public virtual ICollection<UserRole> UserRoles { get; set; }
    }
}
