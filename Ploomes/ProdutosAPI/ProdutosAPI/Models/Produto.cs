using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ProdutosAPI.Models
{
    /// <summary>
    /// Classe que representa um produto com todos os campos. Usada para criar a tabela e interagir com o banco de dados.
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
        /// O valor em reais do produto.
        /// </summary>
        [Required]
        public decimal Preco { get; set; }

        /// <summary>
        /// A quantidade do produto atualmente disponível em estoque.
        /// </summary>
        [Required]
        public int Quantidade { get; set; }

        /// <summary>
        /// O número de identificação da categoria à qual o produto pertence.
        /// </summary>
        // Foreign Key
        [Required]
        public int CategoriaID { get; set; }

        // Reference Navigation
        [JsonIgnore]
        public Categoria CategoriaPai { get; set; }
    }
}
