using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjetoEstacionamentoWebAPI.Models
{
    public class PessoaOcorrencia
    {

        [Key]
        public int pocid { get; set; }
        [ForeignKey("Pessoa")]
        public int pesid { get; set; }
        [ForeignKey("Veiculo")]
        public int pveid { get; set; }
        [ForeignKey("Ocorrencia")]
        public int ocoid { get; set; }
        public DateTime pocdatahora { get; set; }
        public string pocobservacao { get; set; }

    }
}
