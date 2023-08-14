using Microsoft.EntityFrameworkCore;
using ProdutosAPI.Models;

namespace ProdutosAPI.Context
{
    public class ProdutoAPIContext : DbContext
    {
        public ProdutoAPIContext(DbContextOptions<ProdutoAPIContext> options) : base(options)
        { }
        public DbSet<Produto> Produtos { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
    }
}