using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProdutosAPI.Context;
using ProdutosAPI.Models;

namespace ProdutosAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProdutoController : ControllerBase
    {
        private readonly ProdutoAPIContext _context;

        public ProdutoController(ProdutoAPIContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult> GetTodosProdutos() 
        {
            return Ok(await _context.Produtos.ToListAsync());
        }

        [HttpGet("{id}", Name = "Get")]
        public async Task<ActionResult> GetProduto(int id)
        {
            Produto produto = await _context.Produtos.FindAsync(id);

            if (produto == null)
            {
                return NotFound();
            }

            return Ok(produto);
        }

        [HttpPost]
        public async Task<ActionResult> PostProduto(ProdutoDTO produtoDTO)
        {
            Produto produto = new Produto() 
            { Nome = produtoDTO.Nome, Categoria = produtoDTO.Categoria, Preco = produtoDTO.Preco, Quantidade = produtoDTO.Quantidade };

            _context.Produtos.Add(produto);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetProduto), new { id = produto.Id }, produto);
        }

        [HttpPut]
        public async Task<ActionResult> PutProduto(int id, ProdutoDTO produtoDTO)
        {
            Produto produtoToChange = _context.Produtos.Find(id);

            if (produtoToChange == null || produtoDTO == null)
            {
                return NotFound();
            }

            produtoToChange.Nome = produtoDTO.Nome;
            produtoToChange.Categoria = produtoDTO.Categoria;
            produtoToChange.Preco = produtoDTO.Preco;
            produtoToChange.Quantidade = produtoDTO.Quantidade;

            _context.Entry(produtoToChange).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch
            {
                return NoContent();
            }

            return NoContent();
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteProduto(int id)
        {
            Produto produto = _context.Produtos.Find(id);

            if (produto == null)
            {
                return NotFound();
            }

            _context.Produtos.Remove(produto);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
