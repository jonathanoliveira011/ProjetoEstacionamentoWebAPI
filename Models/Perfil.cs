using System.ComponentModel.DataAnnotations;

namespace ProjetoEstacionamentoWebAPI.Models
{
    public class Perfil
    {
        [Key]
        public int pflid { get; set; }
        public string pflnome { get; set; }

    }
}
