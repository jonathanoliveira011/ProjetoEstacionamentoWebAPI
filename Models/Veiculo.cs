using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjetoEstacionamentoWebAPI.Models
{
    public class Veiculo
    {

        [Key]
        public int pveid { get; set; }

        [ForeignKey("Pessoa")]
        public int pesid { get; set; }
        public string veimarca { get; set; }
        public string veimodelo { get; set; }
        public string veiplaca { get; set; }

    }
}
