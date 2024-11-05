using APICatalogo.Context;
using APICatalogo.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProdutosController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet] // api/produtos
        public async Task<ActionResult<IEnumerable<Produto>>> GetAsync()
        {
            try
            {
                var produtos = await _context.Produtos.Take(10).ToListAsync();
                if (produtos is null || !produtos.Any())
                {
                    return NotFound("Produtos não encontrados...");
                }
                return produtos;
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Ocorreu um problema ao tratar a sua solicitação.");
            }
        }

        [HttpGet("{id:int}", Name = "ObterProduto")]
        public async Task<ActionResult<Produto>> GetAsync(int id)
        {
            try
            {
                var produto = await _context.Produtos.FirstOrDefaultAsync(p => p.ProdutoId == id);
                if (produto is null)
                {
                    return NotFound($"Produto Id = {id} não encontrada!");
                }
                return produto;
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Ocorreu um problema ao tratar a sua solicitação.");
            }
        }

        [HttpPost]
        public async Task<ActionResult> PostAsync(Produto produto)
        {
            try
            {
                if (produto is null)
                    return BadRequest();

                _context.Produtos.Add(produto);
                await _context.SaveChangesAsync();

                return new CreatedAtRouteResult("ObterProduto",
                    new { id = produto.ProdutoId }, $"Produto {produto.ProdutoId} criada com sucesso! {produto}");
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Ocorreu um problema ao tratar a sua solicitação.");
            }
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> PutAsync(int id, Produto produto)
        {
            try
            {
                if (id != produto.ProdutoId)
                {
                    return BadRequest();
                }
                _context.Entry(produto).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return Ok($"Produto Id = {id} atualizada com sucesso!");
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Ocorreu um problema ao tratar a sua solicitação.");
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteAsync(int id)
        {
            try
            {
                var produto = await _context.Produtos.FirstOrDefaultAsync(p => p.ProdutoId == id); // Alterado para operação assíncrona
                if (produto is null)
                {
                    return NotFound($"Produto com Id = {id} não localizado...");
                }
                _context.Produtos.Remove(produto);
                await _context.SaveChangesAsync();

                return Ok($"Produto Id = {id} deletada com sucesso! \n {produto}");
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Ocorreu um problema ao tratar a sua solicitação.");
            }
        }
    }
}
