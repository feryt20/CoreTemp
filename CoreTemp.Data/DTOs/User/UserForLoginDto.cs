using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CoreTemp.Data.DTOs.User
{
    public class UserForLoginDto
    {
        [Required]
        [Description("نام کاربری")]
        public string UserName { get; set; }

        [Required]
        [Description("کلمه عبور")]
        public string Password { get; set; }

        [Required]
        [Description("مرا بخاطر بسپار")]
        public bool IsRemember { get; set; }
    }
}
