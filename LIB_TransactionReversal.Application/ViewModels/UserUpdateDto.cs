using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIB_Usermanagement.Application.ViewModels
{
    public class UserUpdateDto
    {
        public Guid? Id { get; init; }
        public string FullName { get; init; }
        [Required(ErrorMessage = "Username is required")]
        public string UserName { get; init; }
        public string BranchName { get; init; }
        public string BranchCode { get; init; }
        [Required(ErrorMessage = "Departement is required")]
        public string Role { get; init; }
        public ICollection<string> Services { get; init; }
        //public ICollection<string>? Roles { get; init; }
    }
}
