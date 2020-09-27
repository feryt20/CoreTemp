using System;
using System.Collections.Generic;
using System.Text;

namespace CoreTemp.Data.DTOs.User
{
    public class UserProfileDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public bool Gender { get; set; }
        public int Age { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string City { get; set; }
        public string NewPass { get; set; }
    }
}
