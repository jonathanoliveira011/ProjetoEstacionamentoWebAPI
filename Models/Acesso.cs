using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjetoEstacionamentoWebAPI.Models
{
    public class Acesso
    {
        [Key]
        public int aceid { get; set; }
        [ForeignKey("Pessoa")]
        public int pesid { get; set; }
        public DateTime acedatahora { get; set; }
        public string acetipo { get; set; }
        [ForeignKey("Veiculo")]
        public int pveid { get; set; }
        [ForeignKey("Estacionamento")]
        public int estid { get; set; }

    }
}
