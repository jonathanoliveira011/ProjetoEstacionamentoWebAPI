using Microsoft.EntityFrameworkCore;
using ProjetoEstacionamentoWebAPI.Models;

namespace ProjetoEstacionamentoWebAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Acesso> tblacesso { get; set; }
        public DbSet<Curso> tblcurso { get; set; }
        public DbSet<Estacionamento> tblestacionamento { get; set; }
        public DbSet<Ocorrencia> tblocorrencia { get; set; }
        public DbSet<Perfil> tblperfil { get; set; }
        public DbSet<Pessoa> tblpessoa { get; set; }
        public DbSet<PessoaDocumento> tblpessoadocumento { get; set; }
        public DbSet<PessoaOcorrencia> tblpessoaocorrencia { get; set; }
        public DbSet<Veiculo> tblpessoaveiculo { get; set; }
        public DbSet<Usuario> tblusuario { get; set; }

    }
}