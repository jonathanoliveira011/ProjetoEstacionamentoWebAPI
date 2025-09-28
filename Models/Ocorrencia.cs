using System.ComponentModel.DataAnnotations;

namespace ProjetoEstacionamentoWebAPI.Models
{
    public class Ocorrencia
    {
        [Key]
        public int ocoid { get; set; }
        public string ocodescricao { get; set; }

    }
}
