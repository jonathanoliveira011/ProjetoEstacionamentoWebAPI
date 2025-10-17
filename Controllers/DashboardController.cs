using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.CodeModifier.CodeChange;
using ProjetoEstacionamentoWebAPI.Data;
using ProjetoEstacionamentoWebAPI.Models;
using RestSharp;

namespace ProjetoEstacionamentoWebAPI.Controllers
{
    [ApiController]
    [Route("dashboard")]
    public class DashboardController : ControllerBase
    {

        private readonly AppDbContext _context;

        public DashboardController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("dados")]
        public IActionResult dadosDashboard(int idpessoa)
        {
            var dadosDash = new
            {
                totalOcorrencias = _context.tblpessoaocorrencia.Count(u => u.pesid == idpessoa),
                veiculosCadastrados = _context.tblpessoaveiculo.Count(),
                usuariosCadastrados = _context.tblusuario.Count()
            };
            return Ok(dadosDash);
        }

        [HttpPost("enviamsg")]
        public IActionResult EnviaMsg(string telefone, string descricaoMensagem)
        {
            var url = "https://api.ultramsg.com/instance112404/messages/chat";
            var client = new RestClient(url);

            var request = new RestRequest(url, RestSharp.Method.Post);
            request.AddHeader("content-type", "application/x-www-form-urlencoded");
            request.AddParameter("token", "so7z1s2xejuaak5c");
            request.AddParameter("to", "+55" + telefone);
            request.AddParameter("body", descricaoMensagem);


            RestResponse response = client.Execute(request);

            if (response.IsSuccessful)
            {
                return Ok("Mensagem enviada com sucesso!");
            }
            else
            {
                return BadRequest(new ErroRetorno { Sucess = false, Mensagem = "Falha ao enviar a mensagem." } );
            }
        }
    }
}
