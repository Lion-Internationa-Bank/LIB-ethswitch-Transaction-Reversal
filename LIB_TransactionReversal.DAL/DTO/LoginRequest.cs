using System.ComponentModel.DataAnnotations;

namespace LIB_Usermanagement.DAL
{
    public class LoginRequest
    {
        public string UserName { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
