namespace ProdutosAPI.Models
{
    public class ProdutoDTO_GET
    {
        public int Id { get; set; }

        public string Nome { get; set; }

        public decimal Preco { get; set; }

        public int CategoriaID { get; set; }

        public string CategoriaNome { get; set; }
    }
}
