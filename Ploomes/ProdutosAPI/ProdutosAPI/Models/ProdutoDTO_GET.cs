namespace ProdutosAPI.Models
{
    /// <summary>
    /// Classe que retorna os valores do produto a serem exibidos.
    /// </summary>
    public class ProdutoDTO_GET
    {
        /// <summary>
        /// O valor de identificação do produto.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// O nome do produto. Pode incluir a marca.
        /// </summary>
        public string Nome { get; set; } = string.Empty;

        /// <summary>
        /// O valor em reais do produto.
        /// </summary>
        public decimal Preco { get; set; }

        /// <summary>
        /// A quantidade do produto atualmente disponível em estoque.
        /// </summary>
        public int Quantidade { get; set; }

        /// <summary>
        /// O número de identificação da categoria à qual o produto pertence.
        /// </summary>
        public int CategoriaID { get; set; }

        /// <summary>
        /// O nome da categoria à qual o produto pertence.
        /// </summary>
        public string CategoriaNome { get; set; } = string.Empty;
    }
}
