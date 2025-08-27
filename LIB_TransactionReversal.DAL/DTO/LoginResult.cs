using System.Collections.Generic;

namespace LIB_Usermanagement.DAL
{
    public class LoginResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = null!;
        public string? Token { get; set; }
        public string RefreshToken { get; set; }
        public string Branch { get; set; }
        public string BranchCode { get; set; }
        public string Role { get; set; }
        public List<string> Services { get; set; }
        public bool MustChangePassword { get; set; }
    }
}
