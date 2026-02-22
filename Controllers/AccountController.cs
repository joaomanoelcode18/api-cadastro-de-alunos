using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AlunoWeb.Services;
using AlunoWeb.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Identity;


namespace AlunoWeb
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IAuthenticate _authenticate;

        public AccountController(IConfiguration configuration, IAuthenticate authenticate)
        {
            _configuration = configuration ??
                throw new ArgumentNullException(nameof(configuration));
            _authenticate = authenticate ??
                throw new ArgumentNullException(nameof(authenticate));
        }
        [HttpPost("CreateUser")]
        public async Task<ActionResult<UserToken>> CreateUser([FromBody] RegisterModel model)
        {
            if (model.Password != model.ConfirmPassword)
            {
                ModelState.AddModelError("ConfirmPassword", "As senhas não conferem.");
                return BadRequest(ModelState);
            }
            var result = await _authenticate.RegisterUser(model.Email, model.Password);
            if (result.Succeeded)
            {
                return Ok($"Usuário {model.Email} criado com sucesso!");
            }
            else
            {
                ModelState.AddModelError("CreateUser", "Registro inválido.");
                return BadRequest(ModelState);
            }
        }
        [HttpPost("LoginUser")]
public async Task<IActionResult> LoginUser([FromBody] LoginModel model)
{
    var user = await _authenticate.Authenticate(model.Email, model.Password);

    if (user == null)
        return Unauthorized("Login inválido");

    var token = GenerateToken(user.Email);

    return Ok(new
    {
        token = token.Token,
        expiration = token.Expiration,
        email = user.Email
    });
}

        private UserToken GenerateToken(string email)
{
    var claims = new[]
    {
        new Claim(JwtRegisteredClaimNames.Email, email),
        new Claim("meuToken", "token do meu app"),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
    };

    var key = new SymmetricSecurityKey(
        Encoding.UTF8.GetBytes(_configuration["Jwt:Key"])
    );

    var creds = new SigningCredentials(
        key, SecurityAlgorithms.HmacSha256
    );

    var expiration = DateTime.UtcNow.AddMinutes(20);

    var token = new JwtSecurityToken(
        issuer: _configuration["Jwt:Issuer"],
        audience: _configuration["Jwt:Audience"],
        claims: claims,
        expires: expiration,
        signingCredentials: creds
    );

    return new UserToken
    {
        Token = new JwtSecurityTokenHandler().WriteToken(token),
        Expiration = expiration
    };
}

    }

}
