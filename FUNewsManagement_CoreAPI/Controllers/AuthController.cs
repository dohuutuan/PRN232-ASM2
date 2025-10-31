
using FUNewsManagement_CoreAPI.DTOs;
using FUNewsManagement_CoreAPI.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FUNewsManagement_CoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO req)
        {
            try
            {
                var result = await _authService.Login(req);
                return Ok(new APIResponse<LoginResponseDTO>
                {
                    StatusCode = 200,
                    Message = "Login successful",
                    Data = result
                });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new APIResponse<object>
                {
                    StatusCode = 401,
                    Message = ex.Message,
                });
            }
        }
        //[HttpPost("refresh-token")]
        //public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequestDTO req)
        //{
        //    try
        //    {
        //        var result = await _authService.RefreshToken(req);
        //        return Ok(new ApiResponse<LoginResponseDTO>
        //        {
        //            Success = true,
        //            Message = "Token refreshed successfully",
        //            Data = result
        //        });
        //    }
        //    catch (UnauthorizedAccessException ex)
        //    {
        //        return Unauthorized(new ApiResponse<object>
        //        {
        //            Success = false,
        //            Message = ex.Message,
        //        });
        //    }
        //}

    }
}
