namespace ProjetoEstacionamentoWebAPI.Models
{
    public class ErroRetorno
    {
        public string Mensagem { get; set; } = "Erro ao processar dados.";
        public bool Sucess { get; set; } = false;
        public bool SenhaTemporaria { get; set; } = false;

    }
}
