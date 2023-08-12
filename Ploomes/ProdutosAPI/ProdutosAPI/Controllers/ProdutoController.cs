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
            Produto item = await _context.Produtos.FindAsync(id);

            if (item == null)
            {
                return NotFound();
            }

            return Ok(item);
        }

        [HttpPost]
        public async Task<ActionResult> PostProduto(Produto produto)
        {
            _context.Produtos.Add(produto);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetProduto), new { id = produto.Id }, produto);
        }
    }
}
