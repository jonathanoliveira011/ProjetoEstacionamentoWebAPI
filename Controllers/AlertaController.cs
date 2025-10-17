using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using ProjetoEstacionamentoWebAPI.Data;
using ProjetoEstacionamentoWebAPI.Models;
using System.Numerics;

namespace ProjetoEstacionamentoWebAPI.Controllers
{
    [ApiController]
    [Route("alerta")]
    public class AlertaController : ControllerBase
    {
        private readonly AppDbContext _context;
        DashboardController _dashboardController; 

        public AlertaController(AppDbContext context)
        {
            _context = context;
        }

        public class AlertaRequest
        {
            public string placa { get; set; }
            public int idOcorrencia { get; set; }
            public int idLogado { get; set; }
        }

        [HttpPost]
        public IActionResult EnviarAlerta([FromBody] AlertaRequest request)
        {
            string _placa = request.placa;
            int _idLogado = request.idLogado;
            int _idOcorrencia = request.idOcorrencia;
            _dashboardController = new DashboardController(_context);

            var veiculo = _context.tblpessoaveiculo.FirstOrDefault(v => v.veiplaca == _placa);
            var ocorrencia = _context.tblocorrencia.FirstOrDefault(o => o.ocoid == _idOcorrencia);

            if (veiculo == null || ocorrencia == null)
                return BadRequest(new { message = "Veículo ou Ocorrência não encontrado." });

            var pessoa = _context.tblpessoa.FirstOrDefault(p => p.pesid == veiculo.pesid);

            string mensagem = "Olá " + pessoa.pesnome + ", uma nova ocorrência foi registrada para o veículo de placa: " + veiculo.veiplaca + ".\nDescrição da ocorrência: " + ocorrencia.ocodescricao +  ".";
            var alertaEnviado = _dashboardController.EnviaMsg(pessoa.pestelefone, mensagem);

            if(alertaEnviado is BadRequestObjectResult)
            {
                return BadRequest(new ErroRetorno { Sucess = false, Mensagem = "Falha ao enviar o alerta." });
            } 
            else {
                var ocorrenciaAdd = _context.tblpessoaocorrencia.Add(new Models.PessoaOcorrencia
                {
                    pesid = pessoa.pesid,
                    pveid = veiculo.pveid,
                    ocoid = ocorrencia.ocoid,
                    pocdatahora = DateTime.Now,
                    pocobservacao = ocorrencia.ocodescricao
                });
                _context.SaveChanges();
            }

            return Ok(new { Mensagem = "Alerta enviado com sucesso!" });
        }
    }
}
