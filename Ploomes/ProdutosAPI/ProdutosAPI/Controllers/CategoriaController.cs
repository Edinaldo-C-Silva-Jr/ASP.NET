using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProdutosAPI.Context;
using ProdutosAPI.Migrations;
using ProdutosAPI.Models;

namespace ProdutosAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class CategoriaController : ControllerBase
    {
        private readonly ProdutoAPIContext _context;

        public CategoriaController(ProdutoAPIContext context)
        {
            _context = context;
        }

        [HttpGet(Name = "GetAllCategories")]
        public async Task<ActionResult> GetTodasCategorias()
        {
            List<CategoriaDTO_GET> categorias = await _context.Categorias.
                Select(c => new CategoriaDTO_GET() { Id = c.Id, Nome = c.Nome } ).ToListAsync();

            if(categorias.Count == 0)
            {
                return NotFound("Não há categorias cadastradas.");
            }
            return Ok(categorias);
        }

        [HttpGet("{id}", Name = "GetOneCategory")]
        public async Task<ActionResult> GetCategoria(int id)
        {
            Categoria categoria = await _context.Categorias.FindAsync(id);

            if(categoria == null)
            {
                return NotFound("A categoria com id " + id + " não existe");
            }

            CategoriaDTO_GET categoriaExibir = new CategoriaDTO_GET() { Id = categoria.Id, Nome = categoria.Nome };
            return Ok(categoriaExibir);
        }

        [HttpPost(Name = "PostCategory")]
        public async Task<ActionResult> PostCategoria (CategoriaDTO_POST categoriaDTO)
        {
            if (categoriaDTO == null)
            {
                return BadRequest("A categoria fornecida não possui todos os dados necessários.");
            }

            Categoria categoria = new Categoria() { Nome = categoriaDTO.Nome };
            _context.Categorias.Add(categoria);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetCategoria), new { id = categoria.Id }, categoria);
        }

        [HttpPut("{id}", Name = "PutCategory")]
        public async Task<ActionResult> PutCategoria(int id, CategoriaDTO_POST categoriaDTO)
        {
            Categoria categoriaUpdate = await _context.Categorias.FindAsync(id);

            if (categoriaUpdate == null)
            {
                return NotFound("A categoria com id " + id + " não existe");
            }
            if (categoriaDTO == null)
            {
                return BadRequest("A categoria fornecida não possui todos os dados necessários.");
            }

            categoriaUpdate.Nome = categoriaDTO.Nome;
            _context.Entry(categoriaUpdate).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
