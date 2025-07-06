namespace Gerenciador.Model.Responses
{
    public class CompradorResponse
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public string Documento { get; set; }
        public string Cidade { get; set; }
        public string Estado { get; set; }
        public DateTime DataDeCriacao { get; set; }
    }
}
