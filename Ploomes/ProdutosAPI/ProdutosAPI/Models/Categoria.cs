using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ProdutosAPI.Models
{
    public class Categoria
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Nome { get; set; } = string.Empty;

        [JsonIgnore]
        public ICollection<Produto>? ProdutosFilhos { get; set; }
    }
}
