using System.ComponentModel.DataAnnotations;

namespace ProdutosAPI.Models
{
    public class Produto
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Nome { get; set; }

        [Required]
        public string Categoria { get; set; }

        [Required]
        public decimal Preco { get; set; }

        public int Quantidade { get; set; }
    }
}
