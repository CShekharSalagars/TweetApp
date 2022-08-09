using com.tweetapp.Services.DTOS;
using com.tweetapp.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace com.tweetapp.API.Controllers
{
    [Authorize]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService authService;
        public AuthController(IAuthService _authService)
        {
            authService = _authService;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("tweets/register")]
        public async Task<IActionResult> Register(RegisterUserDto registerUserDto)
        {
            var serviceResponse = await authService.RegisterUser(registerUserDto);
            if (!serviceResponse.Success)
            {
                return BadRequest(serviceResponse);
            }
            return Ok(serviceResponse);
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("tweets/login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            var serviceResponse = await authService.Login(loginDto.userName, loginDto.password);
            if (!serviceResponse.Success)
            {
                return BadRequest(serviceResponse);
            }
            return Ok(serviceResponse);
        }

        [HttpPost]
        [Route("tweets/forgotPassword")]
        public async Task<IActionResult> ResetPassword(LoginDto loginDto)
        {
            var serviceResponse = await authService.ForgotPassword(loginDto.userName, loginDto.password);
            if (!serviceResponse.Success)
            {
                return BadRequest(serviceResponse);
            }
            return Ok(serviceResponse);
        }

        [HttpGet]
        [Route("tweet/user/{userName}")]
        public async Task<IActionResult> GetUser(string userName)
        {
            var serviceResponse = await authService.GetByName(userName);
            if (!serviceResponse.Success)
            {
                return BadRequest(serviceResponse);
            }
            return Ok(serviceResponse);
        }
    }
}
