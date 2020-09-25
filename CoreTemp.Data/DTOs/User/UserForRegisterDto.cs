using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CoreTemp.Data.DTOs.User
{
    public class UserForRegisterDto
    {
        [Required]
        [Description("نام کاربری")]
        public string UserName { get; set; }

        [Required]
        [Description("پسورد")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "پسورد باید بین 6 تا 20 کاراکترباشد")]
        public string Password { get; set; }


        [Required]
        [Description("نام و نام خانوادگی")]
        public string Name { get; set; }

        [Required]
        [Description("تلفن همراه")]
        public string PhoneNumber { get; set; }

        [Required]
        [Description("کد فعالسازی")]
        [StringLength(5, MinimumLength = 5, ErrorMessage = "کد فعالسازی باید 5 رقمی باشد")]
        public string Code { get; set; }

        [Required]
        [Description("نحوه ثبت نام")]
        public short HowToRegister { get; set; }//1=Phone, 2=Email, 3=Socail
    }
}
