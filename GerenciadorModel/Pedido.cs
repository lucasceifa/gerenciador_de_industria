using System.ComponentModel.DataAnnotations;

namespace GerenciadorModel
{
    public class Pedido : Entidade
    {
        [Required]
        public Guid CompradorId { get; set; }

        [Required]
        public DateTime DataRealizada { get; set; }
    }
}
