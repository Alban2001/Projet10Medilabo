using APIAuthenticationMediLabo.Models;
using APIAuthenticationMediLabo.Services;
using APIAuthenticationMediLabo.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace APIAuthenticationMediLabo.Controllers
{
    [Route("api/authentication")]
    public class AuthenticationController : ControllerBase
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly JwtService _jwtService;
        private readonly IConfiguration _config;

        public AuthenticationController(IConfiguration config, SignInManager<User> signInManager, UserManager<User> userManager, JwtService jwtService)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _jwtService = jwtService;
            _config = config;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDTO dto)
        {
            var user = new User
            {
                UserName = dto.Email,
                Email = dto.Email
            };

            var result = await _userManager.CreateAsync(user, dto.Password);

            if (!result.Succeeded)
                return BadRequest(result.Errors);

            return Ok();
        }

        [HttpPost("login")]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);

            if (user == null) return Unauthorized("Identifiant et mot de passe incorrect !");

            var result = await _signInManager.CheckPasswordSignInAsync(user, dto.Password, false);
            if (!result.Succeeded) return Unauthorized("Identifiant et mot de passe incorrect !");

            // Récupérer les rôles
            var roles = await _userManager.GetRolesAsync(user);

            if (roles != null && roles.Count > 0)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Email, dto.Email),
                    new Claim(ClaimTypes.NameIdentifier, user.Id)
                };
                foreach (var role in roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role));
                }
                var token = _jwtService.GenerateToken(_config["Jwt:Key"], claims);

                return Ok(token);
            }

            return Unauthorized();
        }
    }
}
