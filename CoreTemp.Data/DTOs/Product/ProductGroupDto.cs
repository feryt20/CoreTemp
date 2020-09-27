using Microsoft.AspNetCore.Http;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CoreTemp.Data.DTOs.Product
{
    public class ProductGroupDto
    {
        [Description("شماره گروه کالا")]
        public int? ProductGroupId { get; set; }

        [Description( "والد گروه کالا")]
        public int? ParentId { get; set; }

        [StringLength(100)]
        [Description( "گروه کالا")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string ProductGroupTitle { get; set; }

        [StringLength(100)]
        [Description( "گروه کالا به انگلیسی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string ProductGroupTitleEnglish { get; set; }

        [StringLength(150)]
        [Description( "تصویر گروه کالا")]
        public string ImageUrl { get; set; }

        [Description( "تعداد محصولات")]
        public int Count { get; set; }

        [Description( "نمایش در محصولات منتخب و دستچین")]
        public bool Row { get; set; }

        [Description( "نمایش زیر گروه در منو")]
        public bool SubGroup { get; set; }

        [Description( "تغییر منو")]
        public bool MinSubGroup { get; set; }


        public IFormFile File { get; set; }
    }
}
