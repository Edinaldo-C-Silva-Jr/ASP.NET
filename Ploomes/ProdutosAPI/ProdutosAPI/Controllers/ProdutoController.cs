using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProdutosAPI.Context;
using ProdutosAPI.Models;
using System.Collections.Generic;

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
        /// <response code="400">Caso o valor do ID fornecido não seja válido.</response>
        /// <response code="404">O produto com o ID fornecido não existe.</response>
        [HttpGet("{id}", Name = "GetOneProduct")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> GetProduto(int id)
        {
            Produto produto = await _context.Produtos.FindAsync(id);

            if (produto == null)
            {
                return NotFound("O produto com id" + id + "não existe.");
            }

            return Ok(produto);
        }

        /// <summary>
        /// Lsita todos os produtos cuja categoria corresponde à categoria fornecida.
        /// </summary>
        /// <param name="categoria">O nome da categoria à qual os produtos pertencem.</param>
        /// <returns>Uma lista de produtos.</returns>
        /// <response code="200">Retorna todos os produtos encontrados.</response>
        /// <response code="400">Caso o nome da categoria fornecido não seja válido.</response>
        [HttpGet("Categoria/{categoria}", Name = "GetProductByCategory")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> GetProdutoCategoria(string categoria)
        {
            List<Produto> produtos = await _context.Produtos.ToListAsync();
            return Ok(produtos.Where(prod => prod.Categoria.ToLower() == categoria.ToLower()));
        }

        /// <summary>
        /// Retorna todos os produtos com preço acima do valor especificado.
        /// </summary>
        /// <param name="valor">O preço mínimo para os produtos que serão retornados.</param>
        /// <returns>Uma lista de produtos.</returns>
        /// <response code="200">Retorna todos os produtos encontrados.</response>
        /// <response code="400">Caso o valor fornecido não seja válido, ou seja um valor negativo.</response>
        [HttpGet("Preco/Acima/{valor}", Name = "GetProductAbovePrice")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> GetProdutoAcimaPreco(decimal valor)
        {
            if (valor < 0)
            {
                return BadRequest("O valor não pode ser negativo");
            }

            List<Produto> produtos = await _context.Produtos.ToListAsync();
            return Ok(produtos.Where(prod => prod.Preco >= valor));
        }

        /// <summary>
        /// Retorna todos os produtos com preço abaixo do valor especificado.
        /// </summary>
        /// <param name="valor">O preço máximo para os produtos que serão retornados.</param>
        /// <returns>Uma lista de produtos.</returns>
        /// <response code="200">Retorna todos os produtos encontrados.</response>
        /// <response code="400">Caso o valor fornecido não seja válido, ou seja um valor negativo.</response>
        [HttpGet("Preco/Abaixo/{valor}", Name = "GetProductBelowPrice")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> GetProdutoAbaixoPreco(decimal valor)
        {
            if (valor < 0)
            {
                return BadRequest("O valor não pode ser negativo");
            }

            List<Produto> produtos = await _context.Produtos.ToListAsync();
            return Ok(produtos.Where(prod => prod.Preco <= valor));
        }

        /// <summary>
        /// Lista todos os produtos atualmente em estoque (quantidade maior que 0)
        /// </summary>
        /// <returns>Uma lista de produtos.</returns>
        /// <response code="200">Retorna todos os produtos encontrados.</response>
        [HttpGet("/EmEstoque", Name = "GetProductsInStock")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> GetProdutosEmEstoque()
        {
            List<Produto> produtos = await _context.Produtos.ToListAsync();
            return Ok(produtos.Where(prod => prod.Quantidade > 0));
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

            if (produtoToChange == null)
            {
                return NotFound("O produto com id" + id + "não existe.");
            }

            if (produtoDTO == null)
            {
                return NotFound("O produto fornecido não possui todos os dados necessários.");
            }

            produtoToChange.Nome = produtoDTO.Nome;
            produtoToChange.Categoria = produtoDTO.Categoria;
            produtoToChange.Preco = produtoDTO.Preco;
            produtoToChange.Quantidade = produtoDTO.Quantidade;

            _context.Entry(produtoToChange).State = EntityState.Modified;
            await _context.SaveChangesAsync();
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
                return NotFound("O produto com id" + id + "não existe.");
            }

            _context.Produtos.Remove(produto);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
