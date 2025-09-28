using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjetoEstacionamentoWebAPI.Models
{
    public class Pessoa
    {
        [Key]
        public int pesid { get; set; }
        public string pesrm { get; set; } = string.Empty;
        public string pesnome { get; set; }
        public string? pesemail { get; set; }
        public string pescpf { get; set; }
        public string pestelefone { get; set; }
        [ForeignKey("Curso")]
        public int curid { get; set; }
        public string pesturma { get; set; }
        public char pesobservacao { get; set; }
        public byte[]? pesfoto { get; set; }
    }
}