using HotelListing.API.Contracts;
using HotelListing.API.Models.Users;
using Microsoft.AspNetCore.Mvc;

namespace HotelListing.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly IAuthManager _authManager;
    private readonly ILogger<AccountController> _logger;

    public AccountController(IAuthManager authManager, ILogger<AccountController> logger)
    {
        _authManager = authManager;
        _logger = logger;
    }

    // POST: api/Account/Login
    [HttpPost]
    [Route("login")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> Login([FromBody] LoginDto loginDto)
    {
        _logger.LogInformation($"Registration Attempt for {loginDto.Email}");
        try
        {
            var authResponse = await _authManager.Login(loginDto);

            if (authResponse == null) return Unauthorized();

            return Ok(authResponse);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                $"Something went wrong in the {nameof(Login)} - User Login attempt for {loginDto.Email}");
            return Problem($"Something went wrong in the {nameof(Login)}. Please contact support.", statusCode: 500);
        }
    }

    // POST: api/Account/Register
    [HttpPost]
    [Route("register")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> Register([FromBody] ApiUserDto apiUserDto)
    {
        _logger.LogInformation($"Registration Attempt for {apiUserDto.Email}");
        try
        {
            var errors = await _authManager.Register(apiUserDto);

            var identityErrors = errors.ToList();

            if (identityErrors.Any())
            {
                foreach (var error in identityErrors)
                    ModelState.AddModelError(error.Code, error.Description);

                return BadRequest(ModelState);
            }

            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                $"Something went wrong in the {nameof(Register)} - User Registration attempt for {apiUserDto.Email}");
            return Problem($"Something went wrong in the {nameof(Register)}. Please contact support.", statusCode: 500);
        }
    }

    // POST: api/Account/RefershToken
    [HttpPost]
    [Route("refreshToken")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> RefreshToken([FromBody] AuthResponseDto request)
    {
        var authResponse = await _authManager.VerifyRefreshToken(request);

        if (authResponse == null) return Unauthorized();

        return Ok(authResponse);
    }
}