using CoreTemp.Data.Models.Site;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CoreTemp.Data.Models.Identity
{
    public class MyUser : IdentityUser
    {

        [Required]
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

        [Required]
        public bool IsActive { get; set; }

        public DateTime LastActive { get; set; }

        [StringLength(150, MinimumLength = 0)]
        public string ImageUrl { get; set; }

        [Required]
        public bool IsBanned { get; set; }

        public virtual ICollection<MyToken> MyTokens { get; set; }
        public virtual ICollection<UserRole> UserRoles { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}
