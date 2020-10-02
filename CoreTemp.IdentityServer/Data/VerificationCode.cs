using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoreTemp.IdentityServer.Data
{
    public class VerificationCode 
    {
        public VerificationCode()
        {
            Id = Guid.NewGuid().ToString();
            DateCreated = DateTime.Now;
            DateModified = DateTime.Now;
        }

        [Required]
        public string Id { get; set; }

        [Required]
        [StringLength(5, MinimumLength = 5)]
        public string Code { get; set; }

        [Required]
        public DateTime DateCreated { get; set; }

        [Required]
        public DateTime DateModified { get; set; }

        [Required]
        public DateTime ExpirationDate { get; set; }
        [Required]
        public DateTime RemoveDate { get; set; }
    }
}
