using System.ComponentModel.DataAnnotations;

namespace ProdutosAPI.Models
{
    public class CategoriaDTO_POST
    {
        [Required]
        public string Nome { get; set; }
    }
}
