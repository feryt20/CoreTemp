using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CoreTemp.Data.Models.Site
{
    public class OrderDetail
    {
        public OrderDetail()
        {
            this.TrackingCode = "NotSendYet";
        }
        [Key]
        public long OrderDetailId { get; set; }
        public long OrderId { get; set; }
        public int OrderedCount { get; set; }
        public long ProductId { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:#,0 تومان}")]
        public long Price { get; set; }//قیمت هر عدد از اون کالا

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:#,0 تومان}")]
        public long Discount { get; set; }//این تخفیف خود کالاس

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:#,0 تومان}")]
        public long TotalPrice { get; set; }
        public string TrackingCode { get; set; }//TrackingCode


        //Navigation Properties

        public virtual Order Order { get; set; }
        public virtual Product Product { get; set; }

    }
}
