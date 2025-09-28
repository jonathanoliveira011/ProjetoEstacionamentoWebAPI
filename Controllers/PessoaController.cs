using Microsoft.AspNetCore.Mvc;
using ProjetoEstacionamentoWebAPI.Data;
using ProjetoEstacionamentoWebAPI.Models;

namespace ProjetoEstacionamentoWebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PessoaController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PessoaController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Pessoa>> Get()
        {
            var pessoas = _context.tblpessoa.ToList();
            return Ok(pessoas);
        }

        [HttpGet("{id}")]
        public ActionResult<Pessoa> BuscaPessoaPorId(int id)
        {
            var pessoa = _context.tblpessoa.Find(id);
            if (pessoa == null)
            {
                return NotFound(new ErroRetorno { MensagemErro = "Pessoa não encontrada." });
            }
            return Ok(pessoa);
        }
    }
}