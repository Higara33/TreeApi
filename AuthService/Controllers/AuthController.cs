using AuthService.Repositories;
using AuthService.Services;
using Common.DTO;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.Controllers
{
    [ApiController]
    [Route("api/user/partner")]
    public class AuthController : ControllerBase
    {
        private readonly RefreshTokenRepository _tokenRepository;
        private readonly TokenService _tokenService;

        public AuthController(RefreshTokenRepository tokenRepository, TokenService tokenService)
        {
            _tokenRepository = tokenRepository;
            _tokenService = tokenService;
        }

        [HttpPost("RememberMe")]
        public async Task<IActionResult> RememberMe([FromBody] RemeberMeRequest request)
        {
            // TODO: что то вроде такого должно тут быть, если я правильно понял что этот метод делать должен
            // (есть какой то партнер сервис, который выдает и проверяет этот код)
            // var userId = await _partnerService.ValidateCodeAsync(request.Code);
            // if (userId == null)
            //   return Unauthorized();
            int userId = 123;

            var tokens = _tokenService.GenerateTokens(userId);
            await _tokenRepository.SaveAsync(userId, tokens.RefreshToken, tokens.RefreshTokenExpires);

            return Ok(new
            {
                accessToken = tokens.AccessToken
            });
        }
    }
}
