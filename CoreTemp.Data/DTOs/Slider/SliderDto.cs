using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CoreTemp.Data.DTOs.Slider
{
    public class SliderDto
    {
        public long? SliderId { get; set; }

        [StringLength(100)]
        [Description("نام اسلایدر")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string SliderName { get; set; }

        [StringLength(100)]
        [Description("تیتر اسلایدر")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string SliderTitle { get; set; }

        [Description("توضیحات اسلایدر")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string SliderDetail { get; set; }

        [Description("آدرس اسلایدر")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string SliderUrl { get; set; }

        [StringLength(150)]
        [Description("تصویر اسلایدر")]
        public string ImageUrl { get; set; }

        [Description("مهلت اسلایدر")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public DateTime SliderTime { get; set; }


        [Description("تصویر بالای اسلایدر باشد")]
        public bool SliderTop { get; set; }
        [Description("تصویر اسلایدر باشد")]
        public bool SliderMain { get; set; }

        public bool SliderMain2 { get; set; }
        public bool SliderMobileMain { get; set; }

        [Description("تصویر گوشه سمت راست باشد")]
        public bool SliderBeside { get; set; }

        public bool SliderBeside2 { get; set; }

        [Description("تصویر پایین پیشنهاد شگفت انگیز باشد")]
        public bool SliderUnder4 { get; set; }
        [Description("تصویر پایین محصولات پربازدید باشد")]
        public bool SliderUnder2 { get; set; }
        [Description("تصویر پایین محصولات منتخب باشد")]
        public bool SliderUnder1 { get; set; }

        public bool IsDeleted { get; set; }
        public IFormFile File { get; set; }
    }
}
