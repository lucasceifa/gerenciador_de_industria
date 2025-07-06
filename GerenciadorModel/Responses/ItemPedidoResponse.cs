namespace Gerenciador.Model.Responses
{
    public class ItemPedidoResponse
    {
        public Guid Id { get; set; }
        public Guid CarneId { get; set; }
        public decimal Preco { get; set; }
        public int Moeda { get; set; }
    }
}
