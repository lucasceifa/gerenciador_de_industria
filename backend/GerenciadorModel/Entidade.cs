using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GerenciadorModel
{
    public class Entidade
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public DateTime DataDeCriacao { get; set; }

        public Entidade()
        {
            Id = Guid.NewGuid();
            DataDeCriacao = DateTime.Now;
        }
    }
}
