namespace Gerenciador.Model.Responses
{
    public class PedidoResponse
    {
        public Guid Id { get; set; }
        public Guid CompradorId { get; set; }
        public DateTime DataDeCriacao { get; set; }
        public DateTime DataRealizada { get; set; }
        public List<ItemPedidoResponse> Itens { get; set; }
    }
}
