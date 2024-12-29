namespace Fluxos.Domain
{
    public class Fluxo
    {
        public int id { get; set; }
        public int id_empresa { get; set; }
        public int ativo { get; set; }
        public string fluxo { get; set; }
        public string palavras { get; set; }
        public int id_pergunta { get; set; }
    }
}
