using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CoreTemp.Data.DTOs.Order
{
    public class PaymentLogDto
    {
        public long PaymentLogId { get; set; }
        public long? OrderId { get; set; }

        [StringLength(150)]
        [Description("کد تراکنش")]
        public string TrackingCode { get; set; }
        [StringLength(150)]
        [Description("کد پاسخ پرداخت")]
        public string PaymentResponseCode { get; set; }
        [StringLength(150)]
        [Description("پیام پاسخ پرداخت")]
        public string PaymentResponseMessage { get; set; }
        [Description("وضعیت")]
        public bool IsSuccessful { get; set; }
        [Description("تاریخ پرداخت")]
        public DateTime PaymentDate { get; set; }
    }
}
