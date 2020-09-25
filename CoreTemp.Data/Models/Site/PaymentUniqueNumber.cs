using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CoreTemp.Data.Models.Site
{
    public class PaymentUniqueNumber
    {
        [Key]
        public long PaymentUniqueId { get; set; }
        public long? OrderId { get; set; }

        //Navigation Properties
        public virtual Order Order { get; set; }
    }
}
