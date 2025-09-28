using System.ComponentModel.DataAnnotations;

namespace ProjetoEstacionamentoWebAPI.Models
{
    public class Estacionamento
    {
        [Key]
        public int estid { get; set; }
        public string estvaga { get; set; }
        public string esttipovaga { get; set; }

    }
}
