using LIB_Usermanagement.DAL.Entity.Account;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIB_Usermanagement.DAL.Repository
{
    public interface IAuthenticationRepository
    {
        //Task<IdentityResult> RegisterUser(UserRegistrationDto userForRegistration);
        //Task<IdentityResult> UpdateUser(UserRegistrationDto userForRegistration);
        //Task<IdentityResult> ChangePassword(ChangePasswordDto changePassword);
        //Task<LoginResult> ValidateUser(UserAuthenticationDto userForAuth);
        //Task<LoginResult> CreateToken(bool populateExp, UserAuthenticationDto userForAuth);
        //Task<IEnumerable<User>> GetUsers(bool isAdmin, string username);
        //Task<IEnumerable<IdentityRole>> GetRoles();
        //Task<IdentityResult> LockUser(UserRegistrationDto userForRegistration);

        //Task<IdentityResult> UnLockUser(UserRegistrationDto userForRegistration);
        //Task<IdentityResult> RestUserPassword(string Id);

        Task<List<string>> GetUserPermition(string Id);



    }
}
