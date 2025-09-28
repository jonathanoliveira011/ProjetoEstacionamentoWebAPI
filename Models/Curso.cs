using System.ComponentModel.DataAnnotations;
namespace ProjetoEstacionamentoWebAPI.Models
{
    public class Curso
    {
        [Key]
        public int curid { get; set; }
        public string curperiodo { get; set; }
        public string curperiodicidade { get; set; }

    }
}
