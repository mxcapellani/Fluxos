namespace Fluxos.Domain
{
    public class Opcao
    {
        public int Id { get; set; } // Identificador único da opção
        public string Texto { get; set; } // Texto da opção
        public string FluxoDestino { get; set; } // Identifica o próximo fluxo relacionado à opção
        public int PerguntaId { get; set; } // Relaciona a opção com a pergunta (chave estrangeira)
    }
}
