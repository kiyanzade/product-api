using Microsoft.AspNetCore.Mvc;
using ProductProject.Service.UserService;
using ProductProject.Service.UserService.Dto;
using Swashbuckle.AspNetCore.Annotations;

namespace ProductProject.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController :ControllerBase
    {
        private readonly IUserService _userService;

        public AuthController(IUserService userService)
        {
            _userService = userService;
        }


        [HttpPost("register")]
        [SwaggerOperation(
            Summary = "Register new user"
        )]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
               var result = await _userService.Register(dto);
               if (result.Succeeded)
               {
                   return Ok("User registered successfully");
               }
               foreach (var error in result.Errors)
               {
                   ModelState.AddModelError(string.Empty, error.Description);
               }
               return BadRequest(ModelState);
      
        }


        [HttpPost("login")]
        [SwaggerOperation(
            Summary = "Login User",
            Description = "Receive the token in the response and use it to authenticate swagger"
        )]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            try
            {
                var res = await _userService.Login(dto);
                return Ok(res);
            }
            catch (IOException ex)
            {
                return Unauthorized(ex.Message);
            }
           
        }

    }
}
