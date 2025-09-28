using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjetoEstacionamentoWebAPI.Models
{
    public class PessoaDocumento
    {
        [Key]
        public int pdoid { get; set; }
        [ForeignKey("Pessoa")]
        public int pesid { get; set; }
        public string pdodescricao { get; set; }
        public byte[] pdofoto { get; set; }

    }
}
