using System;
using System.Collections.Generic;
using System.Text;

namespace CoreTemp.Data.DTOs.Order
{
    public class OrderDetailDto
    {
        public long OrderDetailId { get; set; }
        public long OrderId { get; set; }
        public int OrderedCount { get; set; }
        public long ProductId { get; set; }

        public long Price { get; set; }//قیمت هر عدد از اون کالا

        public long Discount { get; set; }//این تخفیف خود کالاس

        public long TotalPrice { get; set; }
        public string TrackingCode { get; set; }//TrackingCode


        //Navigation Properties

       // public virtual Product Product { get; set; }
    }
}
