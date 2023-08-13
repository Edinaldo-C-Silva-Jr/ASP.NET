using System.ComponentModel.DataAnnotations;

namespace ProdutosAPI.Models
{
    /// <summary>
    /// Classe que representa um produto com todos os campos. Usada para criar a tabela do banco de dados.
    /// </summary>
    public class Produto
    {
        /// <summary>
        /// O valor de identificação do produto.
        /// </summary>
        [Key]
        public int Id { get; set; }

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
