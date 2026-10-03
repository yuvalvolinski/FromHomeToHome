using Microsoft.AspNetCore.Mvc;
using FromHomeToHome.Core.DTOs;
using FromHomeToHome.Backend.Services;

namespace FromHomeToHome.Backend.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly ILogger<AuthController> _logger;

    public AuthController(IUserService userService, ILogger<AuthController> logger)
    {
        _userService = userService;
        _logger = logger;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
    {
        if (registerDto == null)
        {
            _logger.LogWarning("Registration failed: Null DTO received.");
            return BadRequest("נתוני ההרשמה לא תקינים.");
        }

        if (string.IsNullOrWhiteSpace(registerDto.Username) || 
            string.IsNullOrWhiteSpace(registerDto.Email) || 
            string.IsNullOrWhiteSpace(registerDto.Password))
        {
            _logger.LogWarning("Registration failed: Missing required fields. Username={Username}, Email={Email}", 
                registerDto.Username, registerDto.Email);
            return BadRequest("נא למלא את כל השדות.");
        }

        try
        {
            _logger.LogInformation("Starting registration process for Username={Username}", registerDto.Username);

            bool userExists = await _userService.UserExistsAsync(registerDto.Username, registerDto.Email);
            if (userExists)
            {
                _logger.LogWarning("Registration conflict: Username '{Username}' or Email '{Email}' already exists.", 
                    registerDto.Username, registerDto.Email);
                return Conflict("שם המשתמש או כתובת האימייל כבר קיימים במערכת.");
            }

            int newUserId = await _userService.RegisterUserAsync(registerDto);

            _logger.LogInformation("Registration successful for Username={Username}, Assigned ID={UserId}", 
                registerDto.Username, newUserId);

            return CreatedAtAction(nameof(Register), new { id = newUserId }, new 
            { 
                UserId = newUserId, 
                registerDto.Username, 
                registerDto.Email 
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled server error occurred while registering Username={Username}", registerDto.Username);
            return StatusCode(StatusCodes.Status500InternalServerError, "אירעה שגיאה פנימית בשרת. נא לנסות שוב מאוחר יותר.");
        }
    }
}
