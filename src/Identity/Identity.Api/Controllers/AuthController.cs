using Application.Abstractions.Services;
using Core.Security.Dtos;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Api.Controllers
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

        [HttpPut]
        public async Task<IActionResult> SignInAsync([FromBody] UserSignInDto userSignInDto, [FromServices] IValidator<UserSignInDto> validator)
        {
            validator.ValidateAndThrow(userSignInDto);
            return Ok(await _authService.SignInAsync(userSignInDto, TimeSpan.FromDays(1)));
        }

        [HttpPut("[action]")]
        public async Task<IActionResult> RefreshTokenSignInAsync([FromBody] string refreshToken)
        {
            return Ok(await _authService.RefreshTokenSignInAsync(refreshToken, TimeSpan.FromDays(1)));
        }

        [HttpPost]
        public async Task<IActionResult> SignUpAsync([FromBody] UserSignUpDto userSignUpDto, [FromServices] IValidator<UserSignUpDto> validator)
        {
            validator.ValidateAndThrow(userSignUpDto);
            await _authService.SignUpAsync(userSignUpDto);
            return Created("", null);
        }
    }
}
