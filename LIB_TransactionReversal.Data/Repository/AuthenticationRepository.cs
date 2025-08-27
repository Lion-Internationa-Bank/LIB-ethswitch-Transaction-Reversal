using AutoMapper;
using LIB_Usermanagement.Application.ViewModels;
using LIB_Usermanagement.DAL;
using LIB_Usermanagement.DAL.Entity.Account;
using LIB_Usermanagement.DAL.Repository;
using LIB_Usermanagement.Infra.Data.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class AuthenticationRepository: IAuthenticationRepository
    {
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private User? _user;
        private TrasactionReversalDbContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthenticationRepository(IMapper mapper,
        UserManager<User> userManager, IConfiguration configuration, RoleManager<IdentityRole> roleManager,
        TrasactionReversalDbContext dbContext, IHttpContextAccessor httpContextAccessor)
        {
            _mapper = mapper;
            _userManager = userManager;
            _configuration = configuration;
            _roleManager = roleManager;
            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
        }

     
       

       

        public async Task<IdentityResult> ChangePassword(ChangePasswordDto changePassword)
        {
            try
            {
                var currentUser = _httpContextAccessor.HttpContext.User;

                if (!IsValidPassword(changePassword.NewPassword))
                {
                    return IdentityResult.Failed(new IdentityError()
                    {
                        Code = "",
                        Description = "New password didnt match minimum requirement"
                    });
                }
                var user = await _userManager.FindByNameAsync(currentUser.Identity.Name);
                var result =await _userManager.ChangePasswordAsync(user, changePassword.OldPassword, changePassword.NewPassword);
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private bool IsValidPassword(string password)
        {
            // Example password requirements
            int minLength = Convert.ToInt32(_configuration.GetSection("PasswordLength").Value);
            const string pattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$";

            // Check minimum length
            if (password.Length < minLength)
            {
                return false;
            }

            // Check pattern
            return Regex.IsMatch(password, pattern);
        }

     

        public async Task<LoginResult> CreateToken(bool populateExp, UserAuthenticationDto userForAuth)
        {
            var signingCredentials = GetSigningCredentials();
            var claims = await GetClaims();
            var tokenOptions = GenerateTokenOptions(signingCredentials, claims);

            var refreshToken = GenerateRefreshToken();
            _user.RefreshToken = refreshToken;
            if (populateExp)
                _user.RefreshTokenExpiryTime = DateTime.Now.AddDays(1);
            //await _userManager.UpdateAsync(_user);
            var accessToken = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
            var services = await _userManager.GetClaimsAsync(_user);
            string defualtPass = _configuration.GetSection("DefaultPassword").Value;
            if (userForAuth.Password == defualtPass)
            {
                return new LoginResult()
                {
                    Success = true,
                    Message = "Login successful",
                    Token = new JwtSecurityTokenHandler().WriteToken(
                              GenerateTokenOptions(signingCredentials, new List<Claim>
                                     {
                                     new Claim(ClaimTypes.Name, _user.UserName)
                                     })),
                    MustChangePassword = true,
                    Role = _user.Role,
                    Branch = _user.Branch_name,
                    BranchCode = _user.Branch,
                    Services = services.Select(p => p.Type).ToList()
                };
            }
            return new LoginResult()
            {
                Success = true,
                Message = "Login successful",
                Token = accessToken,
                Role = _user.Role,
                Branch = _user.Branch_name,
                BranchCode = _user.Branch,
                Services = services.Select(p=>p.Type).ToList()
            };
        }

        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }
        private SigningCredentials GetSigningCredentials()
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var key = Encoding.UTF8.GetBytes(jwtSettings["SecurityKey"]);
            var secret = new SymmetricSecurityKey(key);
            return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
        }
        private async Task<List<Claim>> GetClaims()
        {
            var claims = new List<Claim>
             {
             new Claim(ClaimTypes.Name, _user.UserName)
             };

            var userClaims = await _userManager.GetClaimsAsync(_user);
            claims.AddRange(userClaims);

           

            var roles = await _userManager.GetRolesAsync(_user);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            return claims;
        }

        private JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials,List<Claim> claims)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var tokenOptions = new JwtSecurityToken
            (
            issuer: jwtSettings["Issuer"],
            audience: jwtSettings["Audience"],
            claims: claims,
            expires: DateTime.Now.AddMinutes(Convert.ToDouble(jwtSettings["ExpirationTimeInMinutes"])),
            signingCredentials: signingCredentials
            );
            return tokenOptions;
        }

        public async Task<IEnumerable<User>> GetUsers(bool isAdmin, string username)
        {
            try
            {
                string role = "Administrator";
                var users = await _userManager.Users.Where( p=>
                    (username=="" || p.UserName.ToLower().Contains(username.ToLower())) &&
                    p.UserName != "SuperAdmin" && (isAdmin ? p.Role == role : p.Role != role)
                    ).ToListAsync();

                foreach (var user in users)
                {
                    var roles = await _userManager.GetRolesAsync(user);
                    user.Roles = (List<string>)roles;

                }
                return users;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<IdentityRole>> GetRoles()
        {
            var roles = await _roleManager.Roles.ToListAsync();
            return roles;
        }

        public async Task<IdentityResult> LockUser(UserRegistrationDto userForRegistration)
        {
            try
            {
                //var user = _mapper.Map<User>(userForRegistration);
                var user = await _userManager.FindByIdAsync(userForRegistration.Id.ToString());
                if (user != null)
                {
                    user.LockoutEnd = DateTime.Now.AddYears(20).ToUniversalTime().Date;
                    user.LockoutEnabled = true;
                }
                var result = await _userManager.UpdateAsync(user);
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IdentityResult> UnLockUser(UserRegistrationDto userForRegistration)
        {
            try
            {
                //var user = _mapper.Map<User>(userForRegistration);
                var user = await _userManager.FindByIdAsync(userForRegistration.Id.ToString());
                if (user != null)
                {
                    user.LockoutEnd = null;
                    user.LockoutEnabled = true;
                }
                var result = await _userManager.UpdateAsync(user);
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IdentityResult> RestUserPassword(string Id)
        {
            try
            {
                //var user = _mapper.Map<User>(userForRegistration);
                var user = await _userManager.FindByIdAsync(Id);
                if (user != null)
                {
                    var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                    var defualtPass = _configuration.GetSection("DefaultPassword").Value;
                    await _userManager.ResetPasswordAsync(user, token, defualtPass);
                }
                var result = await _userManager.UpdateAsync(user);
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<string>> GetUserPermition(string Id)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(Id);
                var userClaims = await _userManager.GetClaimsAsync(user);
                var services =userClaims.Where(p=>p.Value=="true").Select(p => p.Type).ToList();
                return services;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
