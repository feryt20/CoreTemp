using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CoreTemp.Data.Models.Site
{
    public class PaymentLog
    {
        [Key]
        public long PaymentLogId { get; set; }
        public long? OrderId { get; set; }

        [StringLength(150)]
        [Display(Name = "کد تراکنش")]
        public string TrackingCode { get; set; }
        [StringLength(150)]
        [Display(Name = "کد پاسخ پرداخت")]
        public string PaymentResponseCode { get; set; }
        [StringLength(150)]
        [Display(Name = "پیام پاسخ پرداخت")]
        public string PaymentResponseMessage { get; set; }
        [Display(Name = "وضعیت")]
        public bool IsSuccessful { get; set; }
        [Display(Name = "تاریخ پرداخت")]
        public DateTime PaymentDate { get; set; }

        //Navigation Properties
        public virtual Order Order { get; set; }

    }
}
