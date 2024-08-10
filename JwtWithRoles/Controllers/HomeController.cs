using JwtWithRoles.Models;
using JwtWithRoles.Repository;
using JwtWithRoles.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JwtWithRoles.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        IUserRepository iUserRepository { get; }
        ITokenService iTokenService { get; }

        public HomeController(IUserRepository userRepository, ITokenService tokenService)
        {
            iUserRepository = userRepository;
            iTokenService = tokenService;
        }


        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        public IActionResult Authenticate([FromBody] Login model)
        {
            var user = iUserRepository.Get(model.Username, model.Password);

            if (user == null)
                return NotFound(new { message = "Usuário ou senha inválidos" });

            var token = iTokenService.GenerateToken(user);
            user.Password = "";

            return Ok(new
            {
                user = user,
                token = token
            });
        }

        [HttpGet]
        [Route("anonymous")]
        [AllowAnonymous]
        public IActionResult Anonymous()
        {
            return Ok("Anônimo");
        }

        [HttpGet]
        [Route("authenticated")]
        [Authorize]
        public IActionResult Authenticated()
        {
            return Ok(String.Format("Autenticado - {0}", User.Identity.Name));
        }

        [HttpGet]
        [Route("employee")]
        [Authorize(Roles = "employee,manager")]
        public IActionResult Employee()
        {
            return Ok("Funcionário");
        }

        [HttpGet]
        [Route("manager")]
        [Authorize(Roles = "manager")]
        public IActionResult Manager()
        {
            return Ok("Gerente");
        }
    }
}
