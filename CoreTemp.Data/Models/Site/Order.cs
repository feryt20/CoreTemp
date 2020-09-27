using CoreTemp.Data.Models.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CoreTemp.Data.Models.Site
{
    public class Order
    {
        public Order()
        {
            this.OrderDetails = new HashSet<OrderDetail>();
            this.PaymentLogs = new HashSet<PaymentLog>();
            this.PaymentUniqueNumbers = new HashSet<PaymentUniqueNumber>();
            this.DiscVar = 0;
            this.PostalCost = 10000;
            this.OrderExpireDate = DateTime.Now.AddMinutes(30);
            this.TrackingCode = "NotSendYet";
            this.DeliveryDate = DateTime.Now;
        }

        [Key]
        [Display(Name = "شماره سفارش")]
        public long OrderId { get; set; }
        public string UserId { get; set; }

        [Display(Name = "تاریخ سفارش")]
        public DateTime OrderDate { get; set; }

        [Display(Name = "تاریخ انقضاء سفارش")]
        public DateTime OrderExpireDate { get; set; }

        [Display(Name = "مبلغ")]
        public long Price { get; set; }

        [Display(Name = "کد تخفیف")]
        public string DiscCode { get; set; }

        [Display(Name = "میزان تخفیف")]
        public long DiscVar { get; set; }

        private long _TotalPrice { get; set; }
        [Display(Name = "مبلغ کل")]
        public long TotalPrice
        {
            get
            {
                return Price - DiscVar;
            }
            set
            {
                _TotalPrice = value;
            }
        }

        [Display(Name = "هزینه پست")]
        public long PostalCost { get; set; }


        private long _FinalPrice { get; set; }
        [Display(Name = "مبلغ کل")]
        public long FinalPrice
        {
            get
            {
                return TotalPrice + PostalCost;
            }
            set
            {
                _FinalPrice = value;
            }
        }

        [Display(Name = "وضعیت سفارش")]
        public bool IsFinalized { get; set; }

        public string TrackingCode { get; set; }
        public string IPAddress { get; set; }

        [Display(Name = "تاریخ ارسال")]
        public DateTime DeliveryDate { get; set; }
        public bool IsDeleted { get; set; }

        [Display(Name = "وضعیت ارسال سفارش")]
        public bool IsSend { get; set; }
        [Display(Name = "وضعیت تحویل سفارش")]
        public bool IsDelivered { get; set; }

        //Navigation Properties
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
        public virtual ICollection<PaymentLog> PaymentLogs { get; set; }
        public virtual ICollection<PaymentUniqueNumber> PaymentUniqueNumbers { get; set; }

        [ForeignKey("UserId")]
        public virtual MyUser User { get; set; }

    }
}
