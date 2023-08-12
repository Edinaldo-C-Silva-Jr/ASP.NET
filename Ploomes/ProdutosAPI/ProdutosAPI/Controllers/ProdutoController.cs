using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProdutosAPI.Context;
using ProdutosAPI.Models;

namespace ProdutosAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class ProdutoController : ControllerBase
    {
        private readonly ProdutoAPIContext _context;

        public ProdutoController(ProdutoAPIContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Lista todos os produtos existentes na base de dados.
        /// </summary>
        /// <returns>Uma lista de produtos</returns>
        /// <response code="200">Retorna todos os produtos encontrados.</response>
        [HttpGet(Name = "GetAllProducts")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> GetTodosProdutos()
        {
            return Ok(await _context.Produtos.ToListAsync());
        }

        /// <summary>
        /// Retorna um produto, que corresponde ao ID fornecido.
        /// </summary>
        /// <param name="id">Número de identificação do produto.</param>
        /// <returns>Um produto.</returns>
        /// <response code="200">Retorna o produto requisitado.</response>
        /// <response code="404">O produto com o ID fornecido não existe.</response>
        [HttpGet("{id}", Name = "GetOneProduct")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> GetProduto(int id)
        {
            Produto produto = await _context.Produtos.FindAsync(id);

            if (produto == null)
            {
                return NotFound();
            }

            return Ok(produto);
        }

        /// <summary>
        /// Cadastra um produto com os dados fornecidos.
        /// </summary>
        /// <param name="produtoDTO">Um produto, com todos os dados necessários para o cadastro.</param>
        /// <returns>O link para o produto.</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /Produto
        ///     {
        ///        "nome": "Suco de Uva",
        ///        "categoria": "Sucos",
        ///        "preco": 5.49,
        ///        "quantidade": 100
        ///     }
        ///
        /// </remarks>
        /// <response code="201">Retorna o produto recém-criado.</response>
        /// <response code="400">Caso o produto seja nulo, ou tenha dados faltando.</response>
        [HttpPost(Name = "PostProduct")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> PostProduto(ProdutoDTO produtoDTO)
        {
            Produto produto = new Produto()
            { Nome = produtoDTO.Nome, Categoria = produtoDTO.Categoria, Preco = produtoDTO.Preco, Quantidade = produtoDTO.Quantidade };

            _context.Produtos.Add(produto);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetProduto), new { id = produto.Id }, produto);
        }

        /// <summary>
        /// Atualiza o produto que corresponde ao ID, utilizando as novas informações fornecidas.
        /// </summary>
        /// <param name="id">Número de identificação do produto.</param>
        /// <param name="produtoDTO">Um produto, com todos os dados necessários para o cadastro.</param>
        /// <remarks>
        /// Sample request:
        ///
        ///     PUT /Produto
        ///     {
        ///        "nome": "Suco de Uva",
        ///        "categoria": "Sucos",
        ///        "preco": 5.49,
        ///        "quantidade": 50
        ///     }
        ///
        /// </remarks>
        /// <response code="204">O produto foi alterado com sucesso.</response>
        /// <response code="400">Caso o produto seja nulo, ou tenha dados faltando.</response>
        /// <response code="404">O produto com o ID fornecido não existe.</response>
        [HttpPut("{id}", Name = "PutProduct")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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

        /// <summary>
        /// Deleta o produto que corresponde ao ID fornecido.
        /// </summary>
        /// <param name="id">Número de identificação do produto.</param>
        /// <response code="204">O produto foi alterado com sucesso.</response>
        /// <response code="404">O produto com o ID fornecido não existe.</response>
        [HttpDelete("{id}", Name = "DeleteProduct")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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
