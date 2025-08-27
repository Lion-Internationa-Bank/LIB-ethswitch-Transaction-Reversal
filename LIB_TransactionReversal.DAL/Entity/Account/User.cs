using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;


namespace LIB_Usermanagement.DAL.Entity.Account
{
    public class User: IdentityUser
    {
        public string Branch_name { get; set; }
        public string Branch { get; set; }
        public string Role { get; set; }
        public string Full_name { get; set; }

        [NotMapped]
        public List<string>? Roles { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }

        [NotMapped]
        private bool isLocked;

        [NotMapped]
        public bool IsLocked { get => isLocked = LockoutEnd != null ? LockoutEnd > DateTime.Now : false; 
            set => isLocked = value; }

    }
}
