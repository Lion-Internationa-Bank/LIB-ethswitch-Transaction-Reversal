using LIB_Usermanagement.Application.ViewModels;
using LIB_Usermanagement.DAL;
using LIB_Usermanagement.DAL.Entity.Account;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIB_Usermanagement.Application.Interface
{
    public interface IAuthenticationService
    {
        //Task<IdentityResult> RegisterUser(UserRegistrationDto userForRegistration);
        //Task<IdentityResult> UpdateUser(UserUpdateDto userForUpdate);
        //Task<IdentityResult> ChangePassword(ChangePasswordDto changePassword);
        //Task<LoginResult> ValidateUser(UserAuthenticationDto userForAuth);
        //Task<LoginResult> CreateToken(bool populateExp, UserAuthenticationDto userForAuth);
        //Task<IEnumerable<User>> GetUsers(bool isAdmin, string username);
        //Task<IEnumerable<IdentityRole>> GetRoles();
        //Task<TokenDto> RefreshToken(TokenDto tokenDto);

        //Task<IdentityResult> LockUser(UserRegistrationDto userForUpdate);
        //Task<IdentityResult> UnLockUser(UserRegistrationDto userForRegistration);
        //Task<IdentityResult> RestUserPassword(string Id);
        //Task<List<string>> GetUserPermition(string Id);

        //Task<IdentityResult> UpdateCoreUser(string UserName);

    }
}
