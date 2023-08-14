using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProdutosAPI.Context;
using ProdutosAPI.Models;
using System.Collections.Generic;

namespace ProdutosAPI.Controllers
{
    /// <summary>
    /// O controlador dos métodos HTTP para a entidade de Produto.
    /// </summary>
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
        /// <returns>Uma lista de produtos.</returns>
        /// <response code="200">Retorna todos os produtos encontrados.</response>
        /// <response code="404">Caso não exista nenhum produto na base de dados.</response>
        [HttpGet(Name = "GetAllProducts")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> GetTodosProdutos()
        {
            List<ProdutoDTO_GET> produtos = await _context.Produtos.Include(p => p.CategoriaPai).
                Select(p => new ProdutoDTO_GET() 
                {
                    Id = p.Id,
                    Nome = p.Nome,
                    Preco = p.Preco,
                    Quantidade = p.Quantidade,
                    CategoriaID = p.CategoriaID,
                    CategoriaNome = p.CategoriaPai.Nome
                }).ToListAsync();

            if (produtos.Count == 0)
            {
                return NotFound("Não há produtos cadastrados.");
            }

            return Ok(produtos);
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
            Produto produto = await _context.Produtos.Include(p => p.CategoriaPai).SingleOrDefaultAsync(p => p.Id == id);

            if (produto == null)
            {
                return NotFound("O produto com id " + id + " não existe.");
            }

            ProdutoDTO_GET produtoExibir = new ProdutoDTO_GET()
            { Id = produto.Id, Nome = produto.Nome, Preco = produto.Preco, Quantidade = produto.Quantidade, CategoriaID = produto.CategoriaID, CategoriaNome = produto.CategoriaPai.Nome };

            return Ok(produtoExibir);
        }

        /// <summary>
        /// Lista todos os produtos cuja categoria corresponde à categoria do ID fornecido.
        /// </summary>
        /// <param name="categoriaID">O número de identificação da categoria à qual os produtos pertencem.</param>
        /// <returns>Uma lista de produtos.</returns>
        /// <response code="200">Retorna todos os produtos encontrados.</response>
        /// <response code="400">Caso o valor do ID fornecido não seja válido.</response>
        /// <response code="404">Caso a categoria não exista, ou caso a categoria não tenha produtos cadastrados.</response>
        [HttpGet("Categoria/{categoriaID}", Name = "GetProductByCategory")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> GetProdutoCategoria(int categoriaID)
        {
            if (await _context.Categorias.FindAsync(categoriaID) == null)
            {
                return NotFound("A categoria com id " + categoriaID + " não existe");
            }

            List<ProdutoDTO_GET> produtos = await _context.Produtos.Include(p => p.CategoriaPai).
                Where(prod => prod.CategoriaID == categoriaID).
                Select(p => new ProdutoDTO_GET()
                {
                    Id = p.Id,
                    Nome = p.Nome,
                    Preco = p.Preco,
                    Quantidade = p.Quantidade,
                    CategoriaID = p.CategoriaID,
                    CategoriaNome = p.CategoriaPai.Nome
                }).ToListAsync();

            if (produtos.Count == 0)
            {
                return NotFound("Não há produtos cadastrados nesta categoria.");
            }

            return Ok(produtos);
        }

        /// <summary>
        /// Retorna todos os produtos com preço acima ou igual ao valor especificado.
        /// </summary>
        /// <param name="valorMinimo">O preço mínimo para os produtos que serão retornados.</param>
        /// <returns>Uma lista de produtos.</returns>
        /// <response code="200">Retorna todos os produtos encontrados.</response>
        /// <response code="400">Caso o valor fornecido não seja válido, ou seja um valor negativo.</response>
        /// <response code="404">Caso não haja produtos nesta faixa de preço.</response>
        [HttpGet("PrecoAcima/{valorMinimo}", Name = "GetProductAbovePrice")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> GetProdutoAcimaPreco(decimal valorMinimo)
        {
            if (valorMinimo < 0)
            {
                return BadRequest("O valor não pode ser negativo");
            }
            if (valorMinimo > 9999999999999999)
            {
                return BadRequest("O valor não pode ser acima de 9999999999999999");
            }

            List<ProdutoDTO_GET> produtos = await _context.Produtos.Include(p => p.CategoriaPai).
                Where(prod => prod.Preco >= valorMinimo).
                Select(p => new ProdutoDTO_GET()
                {
                    Id = p.Id,
                    Nome = p.Nome,
                    Preco = p.Preco,
                    Quantidade = p.Quantidade,
                    CategoriaID = p.CategoriaID,
                    CategoriaNome = p.CategoriaPai.Nome
                }).ToListAsync();

            if (produtos.Count == 0)
            {
                return NotFound("Não há produtos acima desta faixa de preço.");
            }

            return Ok(produtos);
        }

        /// <summary>
        /// Retorna todos os produtos com preço abaixo ou igual ao valor especificado.
        /// </summary>
        /// <param name="valorMaximo">O preço máximo para os produtos que serão retornados.</param>
        /// <returns>Uma lista de produtos.</returns>
        /// <response code="200">Retorna todos os produtos encontrados.</response>
        /// <response code="400">Caso o valor fornecido não seja válido, ou seja um valor negativo.</response>
        /// <response code="404">Caso não haja produtos nesta faixa de preço.</response>
        [HttpGet("PrecoAbaixo/{valorMaximo}", Name = "GetProductBelowPrice")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> GetProdutoAbaixoPreco(decimal valorMaximo)
        {
            if (valorMaximo < 0)
            {
                return BadRequest("O valor não pode ser negativo");
            }
            if (valorMaximo > 9999999999999999)
            {
                return BadRequest("O valor não pode ser acima de 9999999999999999");
            }

            List<ProdutoDTO_GET> produtos = await _context.Produtos.Include(p => p.CategoriaPai).
                Where(prod => prod.Preco <= valorMaximo).
                Select(p => new ProdutoDTO_GET()
                {
                    Id = p.Id,
                    Nome = p.Nome,
                    Preco = p.Preco,
                    Quantidade = p.Quantidade,
                    CategoriaID = p.CategoriaID,
                    CategoriaNome = p.CategoriaPai.Nome
                }).ToListAsync();

            if (produtos.Count == 0)
            {
                return NotFound("Não há produtos abaixo desta faixa de preço.");
            }

            return Ok(produtos);
        }

        /// <summary>
        /// Lista todos os produtos atualmente em estoque (quantidade maior que 0)
        /// </summary>
        /// <returns>Uma lista de produtos.</returns>
        /// <response code="200">Retorna todos os produtos encontrados.</response>
        /// <response code="404">Não há produtos atualmente em estoque..</response>
        [HttpGet("EmEstoque/", Name = "GetProductsInStock")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> GetProdutosEmEstoque()
        {
            List<ProdutoDTO_GET> produtos = await _context.Produtos.Include(p => p.CategoriaPai).
                Where(prod => prod.Quantidade > 0).
                Select(p => new ProdutoDTO_GET()
                {
                    Id = p.Id,
                    Nome = p.Nome,
                    Preco = p.Preco,
                    Quantidade = p.Quantidade,
                    CategoriaID = p.CategoriaID,
                    CategoriaNome = p.CategoriaPai.Nome
                }).ToListAsync();

            if (produtos.Count == 0)
            {
                return NotFound("Não há produtos atualmente em estoque.");
            }

            return Ok(produtos);
        }

        /// <summary>
        /// Lista todos os produtos atualmente esgotados (quantidade igual a 0)
        /// </summary>
        /// <returns>Uma lista de produtos.</returns>
        /// <response code="200">Retorna todos os produtos encontrados.</response>
        /// <response code="404">Não há produtos atualmente esgotados.</response>
        [HttpGet("Esgotado", Name = "GetProductsOutOfStock")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> GetProdutosEsgotados()
        {
            List<ProdutoDTO_GET> produtos = await _context.Produtos.Include(p => p.CategoriaPai).
                Where(prod => prod.Quantidade == 0).
                Select(p => new ProdutoDTO_GET()
                {
                    Id = p.Id,
                    Nome = p.Nome,
                    Preco = p.Preco,
                    Quantidade = p.Quantidade,
                    CategoriaID = p.CategoriaID,
                    CategoriaNome = p.CategoriaPai.Nome
                }).ToListAsync();

            if (produtos.Count == 0)
            {
                return NotFound("Não há produtos atualmente esgotados.");
            }

            return Ok(produtos);
        }

        /// <summary>
        /// Cadastra um produto com os dados fornecidos.
        /// </summary>
        /// <param name="produtoDTO">Um produto, com todos os dados necessários para o cadastro.</param>
        /// <returns>O link para o novo produto.</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /Produto
        ///     {
        ///        "nome": "Suco de Uva",
        ///        "preco": 5.49,
        ///        "quantidade": 100,
        ///        "categoria": 1
        ///     }
        ///
        /// </remarks>
        /// <response code="201">Retorna o produto recém-criado.</response>
        /// <response code="400">Caso o produto tenha dados faltando. Caso o preço ou a quantidade sejam negativos. Caso o preço seja um valor muito grande.</response>
        /// <response code="404">Caso a categoria com o ID fornecido não exista.</response>
        [HttpPost(Name = "PostProduct")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> PostProduto(ProdutoDTO_POST produtoDTO)
        {
            if (produtoDTO == null)
            {
                return BadRequest("O produto fornecido não possui todos os dados necessários.");
            }
            if (produtoDTO.Preco < 0)
            {
                return BadRequest("O preço deve ser um valor positivo, ou zero.");
            }
            if (produtoDTO.Preco > 9999999999999999)
            {
                return BadRequest("O preço não pode ser acima de 9999999999999999");
            }
            if (produtoDTO.Quantidade < 0)
            {
                return BadRequest("A quantidade deve ser um valor positivo, ou zero.");
            }
            if (_context.Categorias.Find(produtoDTO.CategoriaID) == null)
            {
                return NotFound("A categoria com id " + produtoDTO.CategoriaID + " não existe. " +
                    "A categoria deve ser cadastrada antes que possa ter produtos.");
            }

            Produto produto = new Produto()
            { Nome = produtoDTO.Nome, Preco = produtoDTO.Preco, Quantidade = produtoDTO.Quantidade, CategoriaID = produtoDTO.CategoriaID };

            _context.Produtos.Add(produto);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetProduto), new { id = produto.Id }, produto);
        }

        /// <summary>
        /// Atualiza o produto que corresponde ao ID, utilizando as novas informações fornecidas.
        /// </summary>
        /// <param name="id">Número de identificação do produto.</param>
        /// <param name="produtoDTO">Um produto, com todos os dados necessários para a atualização.</param>
        /// <remarks>
        /// Sample request:
        ///
        ///     PUT /Produto
        ///     {
        ///        "nome": "Suco de Morango",
        ///        "preco": 4.99,
        ///        "quantidade": 200,
        ///        "categoria": 1
        ///     }
        ///
        /// </remarks>
        /// <response code="204">O produto foi alterado com sucesso.</response>
        /// <response code="400">Caso o produto tenha dados faltando. Caso o preço ou a quantidade sejam negativos. Caso o preço seja um valor muito grande.</response>
        /// <response code="404">Caso a categoria com o ID fornecido não exista, ou o produto com o ID fornecido não exista.</response>
        [HttpPut("{id}", Name = "PutProduct")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> PutProduto(int id, ProdutoDTO_POST produtoDTO)
        {
            if (produtoDTO == null)
            {
                return BadRequest("O produto fornecido não possui todos os dados necessários.");
            }
            if (produtoDTO.Preco < 0)
            {
                return BadRequest("O preço deve ser um valor positivo, ou zero.");
            }
            if (produtoDTO.Preco > 9999999999999999)
            {
                return BadRequest("O preço não pode ser acima de 9999999999999999");
            }
            if (produtoDTO.Quantidade < 0)
            {
                return BadRequest("A quantidade deve ser um valor positivo, ou zero.");
            }
            if (_context.Categorias.Find(produtoDTO.CategoriaID) == null)
            {
                return NotFound("A categoria com id " + produtoDTO.CategoriaID + " não existe. " +
                    "A categoria deve ser cadastrada antes que possa ter produtos.");
            }

            Produto produtoToChange = await _context.Produtos.FindAsync(id);

            if (produtoToChange == null)
            {
                return NotFound("O produto com id " + id + " não existe.");
            }

            produtoToChange.Nome = produtoDTO.Nome;
            produtoToChange.Preco = produtoDTO.Preco;
            produtoToChange.Quantidade = produtoDTO.Quantidade;
            produtoToChange.CategoriaID = produtoDTO.CategoriaID;

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
            Produto produto = await _context.Produtos.FindAsync(id);

            if (produto == null)
            {
                return NotFound("O produto com id " + id + " não existe.");
            }

            _context.Produtos.Remove(produto);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
