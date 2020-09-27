using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CoreTemp.Data.DTOs.Product
{
    public class ProductDto
    {
        public long? ProductId { get; set; }

        [StringLength(20)]
        //[Index(IsUnique = true)]
        [Description("کد کالا")]
        public string ProductId2 { get; set; }

        [Description("گروه کالا")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public int ProductGroupId { get; set; }

        [Description("وضعیت نمایش کالا")]
        public bool ProductStatus { get; set; }

        [StringLength(400)]
        [Description("آدرس کالا")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string ProductUrl { get; set; }

        [StringLength(400)]
        [Description("عنوان کالا")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string ProductTitle { get; set; }

        [StringLength(400)]
        [Description("عنوان کوتاه کالا")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string ProductShortTitle { get; set; }

        [StringLength(400)]
        [Description("عنوان انگلیسی کالا")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string ProductEnglishTitle { get; set; }

        [Description("مختصر شرح کالا")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string ProductSummery { get; set; }

        [Description("شرح کالا")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        //[AllowHtml]
        public string ProductDescription { get; set; }

        [Description("قیمت کالا")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:#,0 تومان}")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public long ProductPrice { get; set; }

        [Description("امتیاز کالا")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public int ProductPoint { get; set; }

        [Description("تخفیف داشته باشد")]
        public bool HaveDiscount { get; set; }

        [Description("میزان تخفیف")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:#,0 تومان}")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public long ProductDiscount { get; set; }

        [Description("مدت تخفیف")]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        public DateTime DiscountTime { get; set; }

        [Description("وضعیت نمایش کامنت")]
        public bool CommentIsActive { get; set; }

        [Description("موجودی انبار")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public int ProductQtty { get; set; }

        [Description("کلمات کلیدی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string KeyWords { get; set; }

        [Description("تگ ها")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Tags { get; set; }

        [StringLength(150)]
        [Description("تصویر کالا")]
        public string ImageUrl { get; set; }


        [StringLength(150)]
        [Description("تصویر کوچک کالا")]
        public string ProductThumbnailImageUrl { get; set; }

        [StringLength(150)]
        [Description("تصویر کوچک کالا")]
        public string ProductThumbnailImageUrl2 { get; set; }

        [StringLength(150)]
        [Description("تصویر کوچک کالا")]
        public string ProductThumbnailImageUrl3 { get; set; }

        [StringLength(150)]
        [Description("تصویر کوچک کالا")]
        public string ProductThumbnailImageUrl4 { get; set; }

        [StringLength(150)]
        [Description("تصویر کوچک کالا")]
        public string ProductThumbnailImageUrl5 { get; set; }


        [Description("تعداد بازدید کالا")]
        public int ProductViews { get; set; }

        [Description("تعداد نظرات کالا")]
        public int ProductComments { get; set; }

        [Description("رای مثبت")]
        public int ProductLike { get; set; }

        [Description("تعداد فروش")]
        public int ProductSells { get; set; }

        [Description("تاریخ انقضا کالا")]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        public DateTime ExpireDate { get; set; }

        [Description("تاریخ ایجاد کالا")]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        public DateTime CreateTime { get; set; }

        [Description("تاریخ ویرایش کالا")]
        public DateTime EditTime { get; set; }

        [Description("فعال برای خرید")]
        public bool ProductBuy { get; set; }

        [Description("شگفت انگیز")]
        public bool ProductVip { get; set; }

        [Description("پیشنهاد لحظه ای")]
        public bool ProductVip2 { get; set; }

        [Description("مدت شگفت انگیز")]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        public DateTime VipDate { get; set; }

        [Description("آیا کد های تخفیفات سایت شاملش شود")]
        public bool IsDiscount { get; set; }


        public IFormFile File { get; set; }
    }
}
