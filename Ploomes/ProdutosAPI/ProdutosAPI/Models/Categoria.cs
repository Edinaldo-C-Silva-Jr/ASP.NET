using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ProdutosAPI.Models
{
    /// <summary>
    /// Classe que representa uma categoria com todos os campos. Usada para criar a tabela e interagir com o banco de dados.
    /// </summary>
    public class Categoria
    {
        /// <summary>
        /// O valor de identificação da categoria.
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// O nome da categoria.
        /// </summary>
        [Required]
        public string Nome { get; set; } = string.Empty;

        // Reference Navigation
        [JsonIgnore]
        public ICollection<Produto>? ProdutosFilhos { get; set; }
    }
}
