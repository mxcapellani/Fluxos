namespace Fluxos.Domain
{
    public class Historico
    {
        public int Id { get; set; }
        public DateTime Data { get; set; }
        public string Telefone { get; set; }
        public int IdPergunta { get; set; }
        public int IdResposta { get; set; }
    }

}
