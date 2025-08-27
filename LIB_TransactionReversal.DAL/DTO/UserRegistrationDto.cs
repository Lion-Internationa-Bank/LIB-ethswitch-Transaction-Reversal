using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LIB_Usermanagement.DAL
{
    public class UserRegistrationDto
    {
        public Guid? Id { get; init; }
        public string FullName { get; init; }
        [Required(ErrorMessage = "Username is required")]
        public string UserName { get; init; }
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; init; }
        public string Role { get; init; }
        public string BranchName { get; init; }
        //[Required(ErrorMessage = "Departement is required")]
        public string BranchCode { get; init; }
        public string phoneNumber { get; init; }
        public string Email { get; init; }
        public ICollection<string> Roles { get; init; }
        public ICollection<string>? Services { get; init; }

    }
}
