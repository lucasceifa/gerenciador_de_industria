namespace Gerenciador.Model.Responses
{
    public class CarneResponse
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public string? Descricao { get; set; }
        public int Tipo { get; set; }
        public DateTime DataDeCriacao { get; set; }
    }
}
