using Microsoft.AspNetCore.Mvc;
using ProjetoEstacionamentoWebAPI.Data;
using ProjetoEstacionamentoWebAPI.Models;
using System.Threading.Tasks;
using System.Linq;

namespace ProjetoEstacionamentoWebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly AppDbContext _context;

        public LoginController(AppDbContext context)
        {
            _context = context;
        }

        public class LoginRequest
        {
            public string Username { get; set; }
            public string Password { get; set; }
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var usuario = _context.tblusuario.FirstOrDefault(u => u.usrnome == request.Username);

            if (usuario == null)
                return NotFound(new ErroRetorno { MensagemErro = "Usuário não encontrado." });

            if (usuario.usrsenha != request.Password)
                return BadRequest(new ErroRetorno { MensagemErro = "Senha incorreta." });

            if (usuario.usrstatus != 1)
                return BadRequest(new ErroRetorno { MensagemErro = "Usuário inativo." });

            if (usuario.usrsenha_temporaria)
            {
                return Ok(new
                {
                    Success = false,
                    Mensagem = "Senha temporária. Redefina sua senha.",
                    SenhaTemporaria = true,
                    UserId = usuario.pesid
                });
            }

            return Ok(new
            {
                Success = true,
                Mensagem = "Login realizado com sucesso.",
                UserId = usuario.pesid
            });
        }
    }
}