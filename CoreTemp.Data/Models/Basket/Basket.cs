using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CoreTemp.Data.Models.Basket
{
    public class Basket
    {
        [Key]
        public long Id { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        public string ProductId { get; set; }

        [Required]
        public int Count { get; set; }
    }
}
