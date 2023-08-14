using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProdutosAPI.Context;
using ProdutosAPI.Migrations;
using ProdutosAPI.Models;

namespace ProdutosAPI.Controllers
{
    /// <summary>
    /// O controlador dos métodos HTTP para a entidade de Categoria.
    /// </summary>
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

        /// <summary>
        /// Lista todas as categorias existentes na base de dados.
        /// </summary>
        /// <returns>Uma lista de categorias.</returns>
        /// <response code="200">Retorna todas as categorias encontradas.</response>
        /// <response code="404">Caso não existam categorias na base de dados.</response>
        [HttpGet(Name = "GetAllCategories")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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

        /// <summary>
        /// Retorna uma categoria, que corresponde ao ID fornecido.
        /// </summary>
        /// <param name="id">Número de identificação da categoria.</param>
        /// <returns>Uma categoria.</returns>
        /// <response code="200">Retorna a categoria requisitada.</response>
        /// <response code="400">Caso o valor do ID fornecido não seja válido.</response>
        /// <response code="404">A categoria com o ID fornecido não existe.</response>
        [HttpGet("{id}", Name = "GetOneCategory")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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

        /// <summary>
        /// Cadastra uma categoria com o nome fornecido.
        /// </summary>
        /// <param name="categoriaDTO">Uma categoria, contendo um nome para cadastro.</param>
        /// <returns>O link para a nova categoria.</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /Categoria
        ///     {
        ///        "nome": "Sucos"
        ///     }
        ///
        /// </remarks>
        /// <response code="201">Retorna a categoria recém-criada.</response>
        /// <response code="400">Caso a categoria tenha dados faltando.</response>
        [HttpPost(Name = "PostCategory")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
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

        /// <summary>
        /// Atualiza a categoria que corresponde ao ID, utilizando o nome fornecido.
        /// </summary>
        /// <param name="id">Número de identificação da categoria.</param>
        /// <param name="categoriaDTO">Uma categoria, contendo um nome para a atualização.</param>
        /// <remarks>
        /// Sample request:
        ///
        ///     PUT /Categoria
        ///     {
        ///        "nome": "Sucos"
        ///     }
        ///
        /// </remarks>
        /// <response code="204">A categoria foi alterada com sucesso.</response>
        /// <response code="400">Caso a categoria tenha dados faltando.</response>
        /// <response code="404">A categoria com o ID fornecido não existe.</response>
        [HttpPut("{id}", Name = "PutCategory")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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
