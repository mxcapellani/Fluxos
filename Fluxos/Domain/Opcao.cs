namespace Fluxos.Domain
{
    public class Opcao
    {
        public int Id { get; set; } // Identificador único da opção
        public string Texto { get; set; } // Texto da opção
        public string Fluxo_Destino { get; set; } // Identifica o próximo fluxo relacionado à opção
        public int Pergunta_Id { get; set; } // Relaciona a opção com a pergunta (chave estrangeira)
        public int id_empresa { get; set; }
    }
}
