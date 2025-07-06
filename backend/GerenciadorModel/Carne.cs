using GerenciadorModel.Enums;
using System.ComponentModel.DataAnnotations;

namespace GerenciadorModel
{
    public class Carne : Entidade
    {
        [Required]
        public string Nome { get; set; }

        [Required]
        public string Descricao { get; set; }

        [Required]
        public ITipoCarne Tipo { get; set; }
    }
}
