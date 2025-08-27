using AutoMapper;
using Azure.Core;
using LIB_Usermanagement.Application.Interface;
using LIB_Usermanagement.Application.ViewModels;
using LIB_Usermanagement.DAL;
using LIB_Usermanagement.DAL.Entity.Account;
using LIB_Usermanagement.DAL.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LIB_Usermanagement.Application.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private IAuthenticationRepository _iAuthenticationRepository;
        private IConfiguration _configuration;
        private readonly IMapper _mapper;
        public AuthenticationService(IAuthenticationRepository iAuthenticationRepository,
            IMapper mapper,
            IConfiguration configuration) 
        { 
            _iAuthenticationRepository = iAuthenticationRepository;
            _mapper = mapper;
            _configuration = configuration;
        }
        //public Task<IdentityResult> ChangePassword(ChangePasswordDto changePassword)
        //{
        //    return _iAuthenticationRepository.ChangePassword(changePassword);
        //}

        //public Task<LoginResult> CreateToken(bool CreateToken, UserAuthenticationDto userForAuth)
        //{
        //    var token = _iAuthenticationRepository.CreateToken(CreateToken, userForAuth);
        //    return token;

        //}

        //public Task<IEnumerable<IdentityRole>> GetRoles()
        //{
        //    return _iAuthenticationRepository.GetRoles();
        //}

        //public Task<IEnumerable<User>> GetUsers(bool isAdmin, string username)
        //{
        //    return _iAuthenticationRepository.GetUsers(isAdmin, username);
        //}

        //public Task<TokenDto> RefreshToken(TokenDto tokenDto)
        //{
        //    throw new NotImplementedException();
        //    //return _iAuthenticationRepository.RefreshToken(tokenDto);
        //}

        //public Task<IdentityResult> RegisterUser(UserRegistrationDto userForRegistration)
        //{
        //    return _iAuthenticationRepository.RegisterUser(userForRegistration);
        //}

        //public Task<IdentityResult> UpdateUser(UserUpdateDto userForUpdate)
        //{
        //    var usertoUpdate = _mapper.Map<UserRegistrationDto>(userForUpdate);
        //    return  _iAuthenticationRepository.UpdateUser(usertoUpdate);
        //}

        //public Task<LoginResult> ValidateUser(UserAuthenticationDto userForAuth)
        //{
        //    return _iAuthenticationRepository.ValidateUser(userForAuth);
        //}

        //public Task<IdentityResult> LockUser(UserRegistrationDto userForRegistration)
        //{
        //    return _iAuthenticationRepository.LockUser(userForRegistration);
        //}

        //public Task<IdentityResult> UnLockUser(UserRegistrationDto userForRegistration)
        //{
        //    return _iAuthenticationRepository.UnLockUser(userForRegistration);
        //}

        //public Task<IdentityResult> RestUserPassword(string Id)
        //{
        //    return _iAuthenticationRepository.RestUserPassword(Id);
        //}

        //public Task<List<string>> GetUserPermition(string Id)
        //{
        //    return _iAuthenticationRepository.GetUserPermition(Id);
        //}

        //public Task<IdentityResult> UpdateCoreUser(string UserName)
        //{
        //    return _iAuthenticationRepository.UpdateCoreUser(UserName);
        //}


    }
}
