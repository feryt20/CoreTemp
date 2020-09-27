using CoreTemp.Data.DTOs.User;
using CoreTemp.Data.Models.Identity;
using CoreTemp.Data.Models.Site;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace CoreTemp.Data.DTOs.Order
{
    public class OrderDto
    {
        [Description("شماره سفارش")]
        public long OrderId { get; set; }
        public string UserId { get; set; }

        [Description("تاریخ سفارش")]
        public DateTime OrderDate { get; set; }

        [Description("تاریخ انقضاء سفارش")]
        public DateTime OrderExpireDate { get; set; }

        [Description("مبلغ")]
        public long Price { get; set; }

        [Description("کد تخفیف")]
        public string DiscCode { get; set; }

        [Description("میزان تخفیف")]
        public long DiscVar { get; set; }

        [Description("مبلغ کل")]
        public long TotalPrice { get; set; }
       

        [Description("هزینه پست")]
        public long PostalCost { get; set; }

        [Description("مبلغ کل")]
        public long FinalPrice { get; set; }
      

        [Description("وضعیت سفارش")]
        public bool IsFinalized { get; set; }

        public string TrackingCode { get; set; }
        public string IPAddress { get; set; }

        [Description("تاریخ ارسال")]
        public DateTime DeliveryDate { get; set; }
        public bool IsDeleted { get; set; }

        [Description("وضعیت ارسال سفارش")]
        public bool IsSend { get; set; }
        [Description("وضعیت تحویل سفارش")]
        public bool IsDelivered { get; set; }

        //Navigation Properties
        public virtual ICollection<OrderDetailDto> OrderDetails { get; set; }
        public virtual ICollection<PaymentLogDto> PaymentLogs { get; set; }

        public virtual UserForDetailedDto User { get; set; }
    }
}
