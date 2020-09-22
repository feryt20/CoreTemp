using CoreTemp.Data.DTOs.User;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace CoreTemp.Data.DTOs.Identity
{
    public class LoginResponseDto
    {
        [Description("توکن مورد نیاز برای ارسال درخواست ها")]
        public string token { get; set; }
        [Description("رفرش توکن مورد نیاز برای بارگزاری مجدد توکن")]
        public string refresh_token { get; set; }
        [Description("مشخصات کاربری که وارد سیستم شده است")]
        public UserForDetailedDto user { get; set; }
    }
}
