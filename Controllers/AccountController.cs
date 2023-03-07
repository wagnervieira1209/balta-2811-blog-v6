using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using BlogV6.Data;
using BlogV6.Models;
using BlogV6.Services;
using BlogV6.ViewModels;
using BlogV6.ViewModels.Account;
using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SecureIdentity.Password;

namespace BlogV6.Controllers
{
    //[Authorize] // Valida autorização para todos os métodos
    [ApiController]
    public class AccountController : ControllerBase
    {
        #region Injeção de Dependência
        /*public readonly TokenService _tokenService;
        public AccountController(TokenService tokenService)
        {
            _tokenService = tokenService;
        }*/
        #endregion

        [HttpPost("v1/account")]
        public async Task<IActionResult> Post(
            [FromBody] CreateAccountViewModel model,
            [FromServices] BlogDataContext context,
            [FromServices] EmailService emailService
        )
        {
            if (!ModelState.IsValid)
                return BadRequest(new ResultViewModel<string>("Requisição inválida"));

            var user = new User()
            {
                Name = model.Name,
                Email = model.Email,
                Slug = model.Name.ToLower().Replace(' ', '-')
            };

            // Encriptando password para salvar no banco
            var password = PasswordGenerator.Generate(25, includeSpecialChars: true, upperCase: false);
            user.PasswordHash = PasswordHasher.Hash(password);

            try
            {
                await context.Users.AddAsync(user);
                await context.SaveChangesAsync();

                emailService.Send(
                    user.Name,
                    user.Email,
                    subject: "Seja Bem Vindo ao Curso Balta 2811",
                    body: $"Sua senha de acesso é <strong>{password}</strong>"
                );

                return Ok(new ResultViewModel<dynamic>(new
                {
                    user = user.Email,
                    password
                }));
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, new ResultViewModel<User>($"E1X01 - Usuário já inserido. Message {ex.Message}"));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<User>("E1X11 - Erro ao inserir usuário"));
            }
        }

        //[AllowAnonymous] // Permite chamar este método sem autorização
        [HttpPost("v1/Account/Login")]
        public async Task<IActionResult> Login(
            [FromBody] LoginViewModel model,
            [FromServices] BlogDataContext context,
            [FromServices] TokenService tokenService // Injeção de Dependência (Necessita adicionar AddScoped, Singleton)
        )
        {
            if (!ModelState.IsValid)
                return BadRequest(new ResultViewModel<string>("Requisição inválida"));

            var user = await context
                        .Users
                        .AsNoTracking()
                        .Include(x => x.Roles)
                        .FirstOrDefaultAsync(x => x.Email == model.Email);

            if (user == null)
                return StatusCode(401, new ResultViewModel<string>("Usuário não encontrado."));

            if (!PasswordHasher.Verify(user.PasswordHash, model.Password))
                return StatusCode(401, new ResultViewModel<string>("Usuário não encontrado."));

            try
            {
                var token = tokenService.GenerateToken(user);
                return Ok(new ResultViewModel<string>(token, error: null));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<string>("05Xa - Falha interna no servidor."));
            }
        }

        [Authorize]
        [HttpPost("v1/Account/upload-image")]
        public async Task<IActionResult> UploadImage(
            [FromBody] UploadImageViewModel model,
            [FromServices] BlogDataContext context
        )
        {
            var fileName = $"{Guid.NewGuid().ToString()}.jpg";
            var data = new Regex(pattern: @"^data:image\/[a-z]+;base64,")
                .Replace(input: model.Base64Image, replacement: "");
            var bytes = Convert.FromBase64String(data);

            try
            {
                await System.IO.File.WriteAllBytesAsync(path: $"wwwroot/images/{fileName}", bytes);
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<string>("Erro X0A1 - Falha interna do servidor"));
            }

            var user = await context
                        .Users
                        .FirstOrDefaultAsync(x => x.Email == User.Identity.Name);

            if (user == null)
                return StatusCode(401, new ResultViewModel<User>("Usuário não encontrado."));

            user.Image = $"https://www.localhost:5001/images/{fileName}";

            try
            {
                context.Users.Update(user);
                await context.SaveChangesAsync();
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<User>("Erro interno do servidor."));
            }

            return Ok(new ResultViewModel<string>("Imgem alterada com sucesso"));
        }

        /* SOMENTE PARA TESTAR ROLES
        
        [Authorize(Roles = "user")] // Valida e autoriza quando for usuário
        [HttpGet("v1/user")]
        public IActionResult GetUser() => Ok(User.Identity.Name);

        [Authorize(Roles = "author")] // Valida e autoriza quando for autor
        [HttpGet("v1/author")]
        public IActionResult GetAuthor() => Ok(User.Identity.Name);

        [Authorize(Roles = "admin")] // Valida e autoriza quando for admin
        [HttpGet("v1/admin")]
        public IActionResult GetAdmin() => Ok(User.Identity.Name);
        */
    }
}