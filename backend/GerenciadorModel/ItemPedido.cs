using GerenciadorModel.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GerenciadorModel
{
    public class ItemPedido : Entidade
    {
        [Required]
        public Guid PedidoId { get; set; }

        [Required]
        public Guid CarneId { get; set; }

        [Required]
        public decimal Preco { get; set; }

        [Required]
        public IMoeda Moeda { get; set; }
    }
}
