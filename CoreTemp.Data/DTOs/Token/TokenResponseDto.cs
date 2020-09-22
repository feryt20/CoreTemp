using System;
using System.Collections.Generic;
using CoreTemp.Data.Models.Identity;

namespace CoreTemp.Data.DTOs.Token
{
    public class TokenResponseDto
    {
        public string token { get; set; }
        public string refresh_token { get; set; }
        public bool status { get; set; }
        public string message { get; set; }
        public Models.Identity.User user { get; set; }
    }
}
