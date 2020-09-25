using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CoreTemp.Data.Models.Site
{
    public class ProductGroup
    {
        public ProductGroup()
        {
            this.Products = new HashSet<Product>();
            this.Count = 0;
        }
        [Key]
        public int ProductGroupId { get; set; }

        [Display(Name = "والد گروه کالا")]
        public int? ParentId { get; set; }

        [StringLength(100)]
        [Display(Name = "گروه کالا")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string ProductGroupTitle { get; set; }

        [StringLength(100)]
        [Display(Name = "گروه کالا به انگلیسی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string ProductGroupTitleEnglish { get; set; }

        [StringLength(150)]
        [Display(Name = "تصویر گروه کالا")]
        public string ImageUrl { get; set; }


        [Display(Name = "تعداد محصولات")]
        public int Count { get; set; }

        [Display(Name = "نمایش در محصولات منتخب و دستچین")]
        public bool Row { get; set; }

        [Display(Name = "نمایش زیر گروه در منو")]
        public bool SubGroup { get; set; }

        [Display(Name = "تغییر منو")]
        public bool MinSubGroup { get; set; }

        public bool IsDeleted { get; set; }
        //Navigation Properties
        public virtual ICollection<Product> Products { get; set; }
        public virtual ProductGroup Parent { get; set; }
    }
}
