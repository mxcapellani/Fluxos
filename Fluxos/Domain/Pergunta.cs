using System.Text.Json.Serialization;

namespace Fluxos.Domain
{
    public class Pergunta
    {
        public int Id { get; set; } // Identificador único da pergunta (chave primária no banco)
        public string PerguntaTexto { get; set; } // Texto da pergunta
        public List<Opcao> Opcoes { get; set; } = new(); // Lista de opções relacionadas
    }
}
