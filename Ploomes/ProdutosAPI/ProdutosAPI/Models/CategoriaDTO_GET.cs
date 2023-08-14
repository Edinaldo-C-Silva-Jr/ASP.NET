namespace ProdutosAPI.Models
{
    /// <summary>
    /// Classe que retorna os valores da categoria a serem exibidos.
    /// </summary>
    public class CategoriaDTO_GET
    {
        /// <summary>
        /// O valor de identificação da categoria.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// O nome da categoria.
        /// </summary>
        public string Nome { get; set; } = string.Empty;
    }
}
