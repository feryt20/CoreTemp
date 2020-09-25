using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CoreTemp.Data.DTOs.Identity
{
    public class GetVerificationCodeDto
    {
        [Required]
        [Phone(ErrorMessage = "شماره موبایل صحیح نمیباشد")]
        [Description("مشاره موبایل با 0 شروع میشود")]
        public string Mobile { get; set; }
    }
}
