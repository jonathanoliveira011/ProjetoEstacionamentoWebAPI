using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjetoEstacionamentoWebAPI.Models
{
    public class Usuario
    {
        [Key]
        [ForeignKey("Pessoa")]
        public int pesid { get; set; }
        public string usrnome { get; set; }
        public string usrsenha { get; set; }
        public int pflid { get; set; }
        public int usrstatus { get; set; }
        public bool usrsenha_temporaria { get; set; }

    }
}
