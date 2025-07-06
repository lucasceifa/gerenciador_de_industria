using System.ComponentModel.DataAnnotations;

namespace GerenciadorModel
{
    public class Comprador : Entidade
    {
        [Required]
        public string Nome { get; set; }

        [Required]
        public string Documento { get; set; }


        [Required]
        public string Cidade { get; set; }


        [Required]
        public string Estado { get; set; }
    }
}
