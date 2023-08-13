using System.ComponentModel.DataAnnotations;

namespace ProdutosAPI.Models
{
    /// <summary>
    /// Classe que representa um produto com os campos que serão usados para criá-lo/alterá-lo.
    /// </summary>
    public class ProdutoDTO
    {
        /// <summary>
        /// O nome do produto. Pode incluir a marca.
        /// </summary>
        [Required]
        public string Nome { get; set; } = string.Empty;

        /// <summary>
        /// O nome da categoria à qual o produto pertence.
        /// </summary>
        [Required]
        public string Categoria { get; set; } = string.Empty;

        /// <summary>
        /// O valor em reais do produto.
        /// </summary>
        [Required]
        public decimal Preco { get; set; }

        /// <summary>
        /// A quantidade atualmente disponível do produto.
        /// </summary>
        [Required]
        public int Quantidade { get; set; }
    }
}
