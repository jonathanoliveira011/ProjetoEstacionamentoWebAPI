using Microsoft.AspNetCore.Mvc;
using ProjetoEstacionamentoWebAPI.Data;
using ProjetoEstacionamentoWebAPI.Models;
using System.Threading.Tasks;
using System.Linq;

namespace ProjetoEstacionamentoWebAPI.Controllers
{
    [ApiController]
    [Route("user")]
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

        public class AlteraSenhaRequest
        {
            public int UserId { get; set; }
            public string NovaSenha { get; set; }
        }

        [HttpPost("login")]
        public async Task<IActionResult> login([FromBody] LoginRequest request)
        {
            var usuario = _context.tblusuario.FirstOrDefault(u => u.usrnome == request.Username);

            if (usuario == null)
                return NotFound(new ErroRetorno { Sucess = false, Mensagem = "Usuário não encontrado." });

            if (usuario.usrsenha != request.Password)
                return BadRequest(new ErroRetorno { Sucess = false, Mensagem = "Senha incorreta." });

            if (usuario.usrstatus != 1)
                return BadRequest(new ErroRetorno { Sucess = false, Mensagem = "Usuário inativo." });

            if (usuario.usrsenha_temporaria)
            {
                return Ok(new
                {
                    Success = true,
                    Mensagem = "Senha temporária. Redefina sua senha.",
                    SenhaTemporaria = true,
                    UserId = usuario.pesid
                });
            }

            return Ok(new
            {
                Success = true,
                Mensagem = "Login realizado com sucesso.",
                SenhaTemporaria = false,
                UserId = usuario.pesid
            });
        }

        [HttpPost("redefinir-senha")]
        public async Task<IActionResult> RedefinirSenha([FromBody] dynamic request)
        {
            int userId = request.UserId;
            string novaSenha = request.NovaSenha;
            var usuario = _context.tblusuario.FirstOrDefault(u => u.pesid == userId);
            if (usuario == null)
                return NotFound(new ErroRetorno { Sucess = false, Mensagem = "Usuário não encontrado." });
            usuario.usrsenha = novaSenha;
            usuario.usrsenha_temporaria = false;
            await _context.SaveChangesAsync();
            return Ok(new
            {
                Success = true,
                Mensagem = "Senha redefinida com sucesso."
            });
        }

        [HttpPost("altera-senha-temporaria")]
        public async Task<IActionResult> alteraSenhaTemporaria([FromBody] AlteraSenhaRequest request)
        {
            int userId = request.UserId;
            string novaSenha = request.NovaSenha;
            var usuario = _context.tblusuario.FirstOrDefault(u => u.pesid == userId);
            if (usuario == null)
                return NotFound(new ErroRetorno { Sucess = false, Mensagem = "Usuário não encontrado." });
            usuario.usrsenha = novaSenha;
            usuario.usrsenha_temporaria = false;
            await _context.SaveChangesAsync();
            return Ok(new
            {
                Success = true,
                Mensagem = "Senha redefinida com sucesso."
            });
        }
    }
}