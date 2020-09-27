using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CoreTemp.Data.Models.Basket
{
    public class MyBasket
    {
        [Key]
        public long Id { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        public long ProductId { get; set; }

        [Required]
        public long ProductPrice { get; set; }

        [Required]
        public long ProductDiscount { get; set; }

        [Required]
        public long TotalPrice { get; set; }

        [Required]
        public int Count { get; set; }
    }
}
