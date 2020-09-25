using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CoreTemp.Data.Models.Site
{
    public class Product
    {
        public Product()
        {
            this.OrderDetails = new HashSet<OrderDetail>();
            this.ProductViews = 0;
            this.ProductComments = 0;
            this.ProductLike = 0;
            this.ProductSells = 0;
            this.ProductPoint = 1;
            this.ExpireDate = DateTime.Now;
            this.CreateTime = DateTime.Now;
            this.EditTime = DateTime.Now;
            this.VipDate = DateTime.Now;
        }
        [Key]
        public long ProductId { get; set; }

        [StringLength(20)]
        //[Index(IsUnique = true)]
        [Display(Name = "کد کالا")]
        public string ProductId2 { get; set; }

        [Display(Name = "گروه کالا")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public int ProductGroupId { get; set; }

        [Display(Name = "برند کالا")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public int BrandGroupId { get; set; }

        [Display(Name = "وضعیت نمایش کالا")]
        public bool ProductStatus { get; set; }

        [StringLength(400)]
        [Display(Name = "آدرس کالا")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string ProductUrl { get; set; }

        [StringLength(400)]
        [Display(Name = "عنوان کالا")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string ProductTitle { get; set; }

        [StringLength(400)]
        [Display(Name = "عنوان کوتاه کالا")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string ProductShortTitle { get; set; }

        [StringLength(400)]
        [Display(Name = "عنوان انگلیسی کالا")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string ProductEnglishTitle { get; set; }

        [Display(Name = "مختصر شرح کالا")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string ProductSummery { get; set; }

        [Display(Name = "شرح کالا")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        //[AllowHtml]
        public string ProductDescription { get; set; }

        [Display(Name = "قیمت کالا")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:#,0 تومان}")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public long ProductPrice { get; set; }

        [Display(Name = "امتیاز کالا")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public int ProductPoint { get; set; }

        [Display(Name = "تخفیف داشته باشد")]
        public bool HaveDiscount { get; set; }

        [Display(Name = "میزان تخفیف")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:#,0 تومان}")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public long ProductDiscount { get; set; }

        [Display(Name = "مدت تخفیف")]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        public DateTime DiscountTime { get; set; }

        [Display(Name = "وضعیت نمایش کامنت")]
        public bool CommentIsActive { get; set; }

        [Display(Name = "موجودی انبار")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public int ProductQtty { get; set; }

        [Display(Name = "کلمات کلیدی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string KeyWords { get; set; }

        [Display(Name = "تگ ها")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Tags { get; set; }

        [StringLength(150)]
        [Display(Name = "تصویر کالا")]
        public string ImageUrl { get; set; }

        [Display(Name = "فروش فایل")]
        public bool IsFile { get; set; }

        [StringLength(150)]
        [Display(Name = "فایل دانلودی کالا")]
        public string FileUrl { get; set; }

        [Display(Name = "پسورد فایل")]
        public string FilePassword { get; set; }

        [StringLength(150)]
        [Display(Name = "سایر تصاویر کالا")]
        public string OtherImageUrl { get; set; }

        [StringLength(150)]
        [Display(Name = "تصویر کوچک کالا")]
        public string ProductThumbnailImageUrl { get; set; }

        [StringLength(150)]
        [Display(Name = "تصویر کوچک کالا")]
        public string ProductThumbnailImageUrl2 { get; set; }

        [StringLength(150)]
        [Display(Name = "تصویر کوچک کالا")]
        public string ProductThumbnailImageUrl3 { get; set; }

        [StringLength(150)]
        [Display(Name = "تصویر کوچک کالا")]
        public string ProductThumbnailImageUrl4 { get; set; }

        [StringLength(150)]
        [Display(Name = "تصویر کوچک کالا")]
        public string ProductThumbnailImageUrl5 { get; set; }


        [Display(Name = "تعداد بازدید کالا")]
        public int ProductViews { get; set; }

        [Display(Name = "تعداد نظرات کالا")]
        public int ProductComments { get; set; }

        [Display(Name = "رای مثبت")]
        public int ProductLike { get; set; }

        [Display(Name = "تعداد فروش")]
        public int ProductSells { get; set; }

        [Display(Name = "تاریخ انقضا کالا")]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        public DateTime ExpireDate { get; set; }

        [Display(Name = "تاریخ ایجاد کالا")]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        public DateTime CreateTime { get; set; }

        [Display(Name = "تاریخ ویرایش کالا")]
        public DateTime EditTime { get; set; }

        [Display(Name = "فعال برای خرید")]
        public bool ProductBuy { get; set; }

        [Display(Name = "شگفت انگیز")]
        public bool ProductVip { get; set; }

        [Display(Name = "پیشنهاد لحظه ای")]
        public bool ProductVip2 { get; set; }

        [Display(Name = "مدت شگفت انگیز")]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        public DateTime VipDate { get; set; }

        [Display(Name = "آیا کد های تخفیفات سایت شاملش شود")]
        public bool IsDiscount { get; set; }

        [StringLength(250)]
        [Display(Name = "شماره تماس")]
        public string OtherUrl { get; set; }

        [Display(Name = "تماس بگیرید")]
        public bool IsOtherUrl { get; set; }

        public bool IsDeleted { get; set; }



        //Navigation Properties
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
        public virtual ProductGroup ProductGroup { get; set; }

    }
}
