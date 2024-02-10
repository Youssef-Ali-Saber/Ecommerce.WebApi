using FirstWebApiProject_E_Commerce_.APIModels;
using FirstWebApiProject_E_Commerce_.Models;
using FirstWebApiProject_E_Commerce_.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FirstWebApiProject_E_Commerce_.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpPost("CheckEmail")]
        public async Task<ActionResult> checkEmail(CheckEmailModel.Request request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(await _userService.CheckEmailAsync(request));
        }
        [HttpPost("SignUp")]
        public async Task<ActionResult> SignUp([FromBody]SignUpModel.Request request)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var response = await _userService.SignUpAsync(request);
            if(response.Success)
                setRefreshTokenInCookie(response.Data.RefreshToken, response.Data.RefreshTokenExpOn);
            return Ok(response);
        }
        [HttpPost("LogIn")]
        public async Task<ActionResult> SignIn([FromBody]SignInModel.Request request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var response = await _userService.SignInAsync(request);
            if (response.Success)
                setRefreshTokenInCookie(response.Data.RefreshToken, response.Data.RefreshTokenExpon);
            return Ok(response);
        }
        [HttpPost("AddRole")]
        public async Task<ActionResult> AddRole([FromBody]AddRoleModel.Requset requset)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(await _userService.AddRoleAsync(requset));
        }
        [HttpGet("refreshToken")]
        public async Task<ActionResult> RefreshToken()
        {
            var refreshtoken = Request.Cookies["refeshToken"];
            var response =await _userService.RefreshTokenAsync(refreshtoken);
            if(!response.Success)
                return BadRequest(response);
            setRefreshTokenInCookie(response.Data.RefreshToken,response.Data.RefreshTokenExpon);
            return Ok(response);
        }
        [HttpPost("revokeToken")]
        public async Task<ActionResult> RevokeToken(string? refeshToken)
        {
            var token = refeshToken ?? Request.Cookies["refreshToken"];
            if(string.IsNullOrEmpty(token))
                return BadRequest("Token is required");
            var response = await _userService.RevokeTokenAsync(token);
            if (!response)
                return BadRequest("Token is invalid");
            return Ok("Done");
        }
        private void setRefreshTokenInCookie(string refreshToken, DateTime expOn)
        {
            var cookiesOpions = new CookieOptions
            {
                HttpOnly = true,
                Expires = expOn.ToLocalTime()
            };
            Response.Cookies.Append("refreshToken", refreshToken ,cookiesOpions);
        }
    
    }
}
