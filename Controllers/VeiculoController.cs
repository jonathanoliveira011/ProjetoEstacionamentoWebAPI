using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProjetoEstacionamentoWebAPI.Data;
using ProjetoEstacionamentoWebAPI.Models;

namespace ProjetoEstacionamentoWebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class VeiculoController : Controller
    {
        private readonly AppDbContext _context;

        public VeiculoController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> buscaTodosVeiculos()
        {
            var veiculos = await _context.tblpessoaveiculo.ToListAsync();
            if(veiculos.Count > 0) return Ok(veiculos);

            return NotFound(new ErroRetorno { MensagemErro = "Nenhum veículo encontrado" });

        }

        [HttpGet("{id}")]
        public ActionResult<Veiculo> buscaVeiculoPorId(int id)
        {
            var pessoa = _context.tblpessoaveiculo.Find(id);
            if (pessoa == null)
            {
                return NotFound(new ErroRetorno { MensagemErro = "Veículo não encontrado." });
            }
            return Ok(pessoa);
        }

    }
}
