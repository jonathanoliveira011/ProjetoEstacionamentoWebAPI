using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjetoEstacionamentoWebAPI.Data;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using System.IO;
using ProjetoEstacionamentoWebAPI.Models;
using DinkToPdf;
using DinkToPdf.Contracts;

namespace ProjetoEstacionamentoWebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RelatorioController : ControllerBase
    {
        private readonly AppDbContext _context;

        public RelatorioController(AppDbContext context)
        {
            _context = context;
        }

        public class RelatorioRequest
        {
            public string TipoRelatorio { get; set; }
            public string Placa { get; set; }
            public string DataInicio { get; set; }
            public string DataFinal { get; set; }
        }

        [HttpPost]
        public async Task<IActionResult> EmitirRelatorio([FromBody] RelatorioRequest request)
        {
            string conteudoHtml = string.Empty;

            switch (request.TipoRelatorio)
            {
                case "veiculos_cadastrados":
                    var veiculos = await _context.tblpessoaveiculo
                    .Join(_context.tblpessoa,
                        v => v.pesid,
                        p => p.pesid,
                        (v, p) => new {
                            v.veimarca,
                            v.veimodelo,
                            v.veiplaca,
                            pesnome = p.pesnome,
                            pesrm = p.pesrm
                        })
                        .ToListAsync();

                    var sbVeiculos = new StringBuilder();
                    sbVeiculos.Append("<html><body>");
                    sbVeiculos.Append("<h1>Relatório de Veículos Cadastrados</h1>");
                    sbVeiculos.Append("<table><thead><tr><th>Placa</th><th>Marca</th><th>Modelo</th><th>Nome do Proprietário</th><th>RM do Proprietário</th></tr></thead><tbody>");
                    foreach (var v in veiculos)
                    {
                        sbVeiculos.Append($"<tr><td>{v.veiplaca}</td><td>{v.veimarca}</td><td>{v.veimodelo}</td><td>{v.pesnome}</td><td>{v.pesrm}</td></tr>");
                    }
                    sbVeiculos.Append("</tbody></table></body></html>");
                    conteudoHtml = sbVeiculos.ToString();
                    break;

                case "ocorrencias_registradas":
                    var ocorrenciasQuery = _context.tblpessoaocorrencia
                        .Where(o => o.pocdatahora >= DateTime.Parse(request.DataInicio) &&
                                    o.pocdatahora <= DateTime.Parse(request.DataFinal).AddDays(1).AddSeconds(-1));

                    if (!string.IsNullOrEmpty(request.Placa))
                        ocorrenciasQuery = ocorrenciasQuery
                            .Join(_context.tblpessoaveiculo,
                                  o => o.pveid,
                                  v => v.pveid,
                                  (o, v) => new { o, v })
                            .Where(joined => joined.v.veiplaca == request.Placa)
                            .Select(joined => joined.o);

                    var ocorrencias = await ocorrenciasQuery
                        .Join(_context.tblpessoa,
                              o => o.pesid,
                              p => p.pesid,
                              (o, p) => new { o, p })
                        .Join(_context.tblpessoaveiculo,
                              op => op.o.pveid,
                              v => v.pveid,
                              (op, v) => new {
                                  op.p.pesnome,
                                  op.p.pesrm,
                                  v.veiplaca,
                                  op.o.pocdatahora,
                                  op.o.pocobservacao
                              })
                        .ToListAsync();

                    var sbOcorrencias = new StringBuilder();
                    sbOcorrencias.Append("<html><body>");
                    sbOcorrencias.Append("<h1>Relatório de Ocorrências Registradas</h1>");
                    sbOcorrencias.Append("<table><thead><tr><th>Nome do proprietário</th><th>RM do proprietário</th><th>Placa</th><th>Data e Hora</th><th>Ocorrência</th></tr></thead><tbody>");
                    foreach (var o in ocorrencias)
                    {
                        sbOcorrencias.Append($"<tr><td>{o.pesnome}</td><td>{o.pesrm}</td><td>{o.veiplaca}</td><td>{o.pocdatahora:dd/MM/yyyy HH:mm}</td><td>{o.pocobservacao}</td></tr>");
                    }
                    sbOcorrencias.Append("</tbody></table></body></html>");
                    conteudoHtml = sbOcorrencias.ToString();
                    break;

                case "pessoas_cadastradas":
                    var pessoas = await _context.tblpessoa
                        .Select(p => new {
                            p.pesrm,
                            p.pesnome,
                            p.pesemail,
                            p.pescpf,
                            p.pestelefone,
                            p.pesturma,
                            p.pesobservacao,
                            curperiodo = "", // ou valor padrão/campo relacionado se existir
                            curperiodicidade = "" // ou valor padrão/campo relacionado se existir
                        }).ToListAsync();

                    var sbPessoas = new StringBuilder();
                    sbPessoas.Append("<html><body>");
                    sbPessoas.Append("<h1>Relatório de Pessoas Cadastradas</h1>");
                    sbPessoas.Append("<table><thead><tr><th>Nome</th><th>RM</th><th>Email</th><th>CPF</th><th>Telefone</th><th>Turma</th><th>Observação</th><th>Período</th><th>Periodicidade</th></tr></thead><tbody>");
                    foreach (var p in pessoas)
                    {
                        sbPessoas.Append($"<tr><td>{p.pesnome}</td><td>{p.pesrm}</td><td>{p.pesemail}</td><td>{p.pescpf}</td><td>{p.pestelefone}</td><td>{p.pesturma}</td><td>{p.pesobservacao}</td><td>{p.curperiodo}</td><td>{p.curperiodicidade}</td></tr>");
                    }
                    sbPessoas.Append("</tbody></table></body></html>");
                    conteudoHtml = sbPessoas.ToString();
                    break;

                default:
                    return BadRequest(new { MensagemErro = "Relatório inválido." });
            }

            var nomeArquivo = "relatorio.pdf";
            var pastaRelatorios = Path.Combine(Directory.GetCurrentDirectory(), "Relatorios");
            if (!Directory.Exists(pastaRelatorios))
                Directory.CreateDirectory(pastaRelatorios);

            var caminhoArquivo = Path.Combine(pastaRelatorios, nomeArquivo);
            var converter = new SynchronizedConverter(new PdfTools());
            var doc = new HtmlToPdfDocument()
            {
                GlobalSettings = { PaperSize = PaperKind.A4 },
                Objects = { new ObjectSettings { HtmlContent = conteudoHtml } }
            };
            byte[] pdf = converter.Convert(doc);
            System.IO.File.WriteAllBytes(caminhoArquivo, pdf);
            var tipoMime = "application/pdf";

            var url = $"{Request.Scheme}://{Request.Host}/Relatorio/download/{nomeArquivo}";
            return Ok(new { Link = url });
        }

        [HttpGet("download/{nomeArquivo}")]
        public IActionResult DownloadRelatorio(string nomeArquivo)
        {
            var pastaRelatorios = Path.Combine(Directory.GetCurrentDirectory(), "Relatorios");
            var caminho = Path.Combine(pastaRelatorios, nomeArquivo);
            if (!System.IO.File.Exists(caminho))
                return NotFound(new ErroRetorno { MensagemErro = "Arquivo não encontrado." });

            var tipoMime = "application/pdf"; // ou "application/pdf" se for PDF
            return File(System.IO.File.ReadAllBytes(caminho), tipoMime, nomeArquivo);
        }
    }
}