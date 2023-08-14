using System.ComponentModel.DataAnnotations;

namespace ProdutosAPI.Models
{
    /// <summary>
    /// Classe que representa uma categoria com os campos que serão usados para criá-la/alterá-la.
    /// </summary>
    public class CategoriaDTO_POST
    {
        /// <summary>
        /// O nome da categoria.
        /// </summary>
        [Required]
        public string Nome { get; set; } = string.Empty;
    }
}
