

using Microsoft.AspNetCore.Mvc;
using InternalBudgetTracker.DTOs;
using InternalBudgetTracker.Services;
using System.Threading.Tasks;

namespace InternalBudgetTracker.Controllers
{
    //[ApiController]
    //[Route("api/[controller]")]
    //public class UsersController : ControllerBase
    //{
    //    private readonly UserService _userService;

    //    public UsersController(UserService userService)
    //    {
    //        _userService = userService;
    //    }

    //    [HttpPost("register-manager")]

    //    public IActionResult RegisterManager(UserRegisterDTO model)
    //    {
    //        _userService.RegisterManager(model);
    //        return Ok("Manager registered successfully");
    //    }

    //    // 👨‍💻 Employee registration
    //    [HttpPost("register-employee")]
    //    public IActionResult RegisterEmployee(UserRegisterDTO model)
    //    {
    //        _userService.RegisterEmployee(model);
    //        return Ok("Employee registered successfully");
    //    }

    [ApiController]
    [Route("api/users")]
    public class UsersController : ControllerBase
    {
        private readonly UserService _userService;

        public UsersController(UserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register-employee")]
        public IActionResult RegisterEmployee(UserRegisterDTO dto)
            => Ok(new { message = _userService.RegisterEmployee(dto) });

        [HttpPost("register-manager")]
        public IActionResult RegisterManager(UserRegisterDTO dto)
            => Ok(new { message = _userService.RegisterManager(dto) });

        [HttpGet("verify")]
        public IActionResult Verify([FromQuery] string token)
        //=> Ok(new { message = _userService.VerifyUser(token) });
        {
            if (string.IsNullOrEmpty(token)) return BadRequest("Token is missing");
            var result=_userService.VerifyUser(token);
            return Ok(result);
                
        }

        [HttpPost("login")]
        public IActionResult Login(UserLoginDTO dto)
            => Ok(new { token = _userService.Login(dto) });
    }

}


