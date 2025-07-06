namespace Gerenciador.Model.Inputs
{
    public class PedidoInput
    {
        public Guid CompradorId { get; set; }
        public DateTime DataRealizada { get; set; }
        public List<ItemPedidoInput>? Itens { get; set; }
    }
}
