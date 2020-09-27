using CoreTemp.Data.Models.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CoreTemp.Data.Models.Identity
{
    public class MyToken : BaseEntity<string>
    {
        public MyToken()
        {
            Id = Guid.NewGuid().ToString();
            DateCreated = DateTime.Now;
            DateModified = DateTime.Now;
        }

        [Required]
        public string ClientId { get; set; }
        [Required]
        public string IP { get; set; }
        [Required]
        public string Value { get; set; }
        [Required]
        public DateTime ExpireTime { get; set; }
        [Required]
        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual MyUser User { get; set; }
    }
}
