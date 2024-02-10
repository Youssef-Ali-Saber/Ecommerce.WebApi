using FirstWebApiProject_E_Commerce_.APIModels;
using FirstWebApiProject_E_Commerce_.Helpers;
using FirstWebApiProject_E_Commerce_.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace FirstWebApiProject_E_Commerce_.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly Jwt _jwt;
        public UserService(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, IOptions<Jwt> jwt)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _jwt = jwt.Value;
        }
        public async Task<ApiResponse<SignUpModel.Response>> SignUpAsync(SignUpModel.Request model)
        {
            var response = new ApiResponse<SignUpModel.Response>();
            if (await _userManager.FindByEmailAsync(model.Email) != null )
            {
                response.AddError("Email is already registered!");
                return response;
            }
            if (await _userManager.FindByNameAsync(model.UserName) != null)
            {
                response.AddError("UserName is already registered!");
                return response;
            }
            var user = new User() { 
                UserName=model.UserName,
                Email=model.Email,
                FirstName=model.FirstName,
                LastName=model.LastName,
                
                };
            var result = await _userManager.CreateAsync(user,model.Password);
            
            if (!result.Succeeded)
            {
                foreach(var error in result.Errors)
                    response.AddError(error.Description);
                return response;
            }
            await _userManager.AddToRoleAsync(user, "User");
            response.Data = new SignUpModel.Response();
            response.Data.Roles = (await _userManager.GetRolesAsync(user)).ToList();
            response.Data.JWToken = new JwtSecurityTokenHandler().WriteToken(await getJWT(user));
            response.Data.Email = model.Email;
            response.Data.UserName = model.UserName;
            response.Data.RefreshToken = getRefreshToken().Token;
            response.Data.RefreshTokenExpOn = getRefreshToken().ExpOn;
            //response.Data.ExpOn = DateTime.Now.AddDays(_jwt.DurationInDays);
            return response;
        }
        public async Task<ApiResponse<SignInModel.Response>> SignInAsync(SignInModel.Request model)
        {
            var response = new ApiResponse<SignInModel.Response>();
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user is null || !await _userManager.CheckPasswordAsync(user, model.Password))
            {
                response.AddError("Email Or Password is incorrect");
                return response;    
            }
            var token = await getJWT(user);
            response.Data = new SignInModel.Response(); 
            response.Data.Token= new JwtSecurityTokenHandler().WriteToken(token);
            
            if (user.RefreshTokens.Any(t=>t.IsActive))
            {
                var activeRefreshToken = user.RefreshTokens.FirstOrDefault(t => t.IsActive);
                response.Data.RefreshToken = activeRefreshToken.Token;
                response.Data.RefreshTokenExpon = activeRefreshToken.ExpOn;
            }
            else
            {
                var refreshToken = getRefreshToken();
                response.Data.RefreshToken = refreshToken.Token;
                response.Data.RefreshTokenExpon = refreshToken.ExpOn;
                user.RefreshTokens.Add(refreshToken);
                await _userManager.UpdateAsync(user); 
            }

            return response;            
        }
        public async Task<ApiResponse<CheckEmailModel.Response>> CheckEmailAsync(CheckEmailModel.Request model)
        {
            var response = new ApiResponse<CheckEmailModel.Response>();
            response.Data = new CheckEmailModel.Response();
            if (await _userManager.FindByEmailAsync(model.Email)==null)
            {
                
                return response;
            }
            
            response.Data.Exists = true;
            return response;
        }
        public async Task<ApiResponse> AddRoleAsync(AddRoleModel.Requset model)
        {
            ApiResponse response = new ApiResponse();
            var user = await _userManager.FindByIdAsync(model.userId);
            if (user is null || !await _roleManager.RoleExistsAsync(model.roleName))
            {
                response.AddError("Invalid UserId or Role Name");
                return response;
            }
            if (await _userManager.IsInRoleAsync(user,model.roleName))
            {
                response.AddError("User is already in this role");
                return response;
            }  
            var result = await _userManager.AddToRoleAsync(user,model.roleName);
            if (!result.Succeeded)
            {
                response.AddError("Something wrong please try again");
                return response;
            }
            return response;
            
        }
        public async Task<ApiResponse<SignInModel.Response>> RefreshTokenAsync(string token)
        {
            var response = new ApiResponse<SignInModel.Response>();
            var user = await _userManager.Users.SingleOrDefaultAsync(u => u.RefreshTokens.Any(u => u.Token == token));
            
            if (user == null)
            {
                response.AddError("Invalid token");
                return response;
            }
            var refreshToken = user.RefreshTokens.Single(r => r.Token==token);
            if (!refreshToken.IsActive)
            {
                response.AddError("Invalid token");
                return response;
            }
            refreshToken.RevokedIn= DateTime.UtcNow;
            var newRefreshToken = getRefreshToken();
            user.RefreshTokens.Add(newRefreshToken);
            await _userManager.UpdateAsync(user);
            var jwt = await getJWT(user);
            response.Data = new SignInModel.Response();
            response.Data.RefreshTokenExpon = newRefreshToken.ExpOn;
            response.Data.RefreshToken = newRefreshToken.Token;
            response.Data.Token= new JwtSecurityTokenHandler().WriteToken(jwt);
            return response;
        }
        public async Task<bool> RevokeTokenAsync(string token)
        {
            var user = await _userManager.Users.SingleOrDefaultAsync(u => u.RefreshTokens.Any(u => u.Token == token));

            if (user == null)
                return false;
            var refreshToken = user.RefreshTokens.Single(r => r.Token == token);
            if (!refreshToken.IsActive)
                return false;
            refreshToken.RevokedIn = DateTime.UtcNow;
            await _userManager.UpdateAsync(user);
            return true;
        }
        private RefreshToken getRefreshToken()
        {
            var randomNum = new byte[32];
            using var generator = new RNGCryptoServiceProvider();
            generator.GetBytes(randomNum);
            return new RefreshToken
            {
                Token = Convert.ToBase64String(randomNum),
                ExpOn = DateTime.UtcNow.AddDays(10),
                CreatedOn = DateTime.UtcNow,

            };

        }
        private async Task<JwtSecurityToken> getJWT(User user)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);
            var roleClaims = new List<Claim>();
            foreach (var role in roles)
                roleClaims.Add(new Claim("role", role));
            var claims = new[]
            {
                new Claim(ClaimTypes.Email,user.Email),
                new Claim (ClaimTypes.NameIdentifier,user.Id), 
                new Claim (ClaimTypes.Name,user.UserName) 
            }
            .Union(userClaims)
            .Union(roleClaims);
            var secureKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
            var signInCredentials = new SigningCredentials(secureKey, SecurityAlgorithms.HmacSha256);
            var tokenOptions = new JwtSecurityToken(
                issuer: _jwt.Issuer,
                audience: _jwt.Audience,
                claims: claims,
                expires: DateTime.Now.AddDays(_jwt.DurationInDays),
                signingCredentials: signInCredentials
                );
            return tokenOptions;
        }

    }

}
