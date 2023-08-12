using System.ComponentModel.DataAnnotations;

namespace ProdutosAPI.Models
{
    public class ProdutoDTO
    {
        [Required]
        public string Nome { get; set; } = string.Empty;

        [Required]
        public string Categoria { get; set; } = string.Empty;

        [Required]
        public decimal Preco { get; set; }

        public int Quantidade { get; set; }
    }
}
