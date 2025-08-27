using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIB_Usermanagement.DAL
{
    public class ChangePasswordDto
    {
        public string NewPassword { get; set; }
        public string OldPassword { get; set; }
    }
}
