using System.ComponentModel.DataAnnotations;

namespace ProdutosAPI.Models
{
    public class Categoria
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Nome { get; set; } = string.Empty;

        public ICollection<Produto>? ProdutosFilhos { get; set; }
    }
}
