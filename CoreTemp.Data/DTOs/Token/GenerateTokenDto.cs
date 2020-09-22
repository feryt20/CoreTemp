using System;
using System.Collections.Generic;
using System.Text;

namespace CoreTemp.Data.DTOs.Token
{
    public class GenerateTokenDto
    {
        public bool Status { get; set; } = false;
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public string Message { get; set; }
    }
}
