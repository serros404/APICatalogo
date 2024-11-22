using APICatalogo.Context;
using APICatalogo.Models;
using APICatalogo.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CategoriasController : ControllerBase
    {

        private readonly AppDbContext _context;

        public CategoriasController(AppDbContext context)
        {
            _context = context;

        }

        [HttpGet("UsandoFromServices/{nome}")]
        public ActionResult<string> GetSaudacaoFromServices([FromServices] IMeuServico meuServico,
                                                          string nome)
        {
            return meuServico.Saudacao(nome);
        }

        [HttpGet("SemFromServices/{nome}")]
        public ActionResult<string> GetSemFromServices(IMeuServico meuServico,
                                                       string nome)
        {
            return meuServico.Saudacao(nome);
        }

        [HttpGet("produtos")]
        public async Task<ActionResult<IEnumerable<Categoria>>> GetCategoriasProdutosAsync()
        {
            try
            {
                // Use ToListAsync para realizar a consulta de forma assíncrona
                var categoria = await _context.Categorias
                    .Include(p => p.Produtos)
                    .Where(c => c.CategoriaId <= 10)
                    .AsNoTracking()
                    .ToListAsync();

                return categoria; // Retorne a lista de categoria
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Ocorreu um problema ao tratar a sua solicitação.");
            }
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<Categoria>>> GetAsync()
        {
            try
            {
                var categoria = await _context.Categorias.AsNoTracking().ToListAsync();
                return categoria;
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Ocorreu um problema ao tratar a sua solicitação.");
            }

        }

        [HttpGet("{id:int}", Name = "ObterCategoria")]
        public async Task<ActionResult<Categoria>> GetAsync(int id)
        {
            try
            {
                var categoria = await _context.Categorias
                    .FirstOrDefaultAsync(p => p.CategoriaId == id);
                if (categoria == null)
                {
                    return NotFound($"Categoria Id = {id} não encontrada!");
                }
                return Ok(categoria);
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Ocorreu um problema ao tratar a sua solicitação.");
            }

        }

        [HttpPost]
        public async Task<ActionResult> PostAsync(Categoria categoria)
        {
            try
            {
                if (categoria is null)
                {
                    return BadRequest();
                }

                _context.Categorias.Add(categoria);
                await _context.SaveChangesAsync();

                return new CreatedAtRouteResult("ObterCategoria",
                    new { id = categoria.CategoriaId }, $"Categoria {categoria.CategoriaId} criada com sucesso!");
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Ocorreu um problema ao tratar a sua solicitação.");

            }

        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> PutAsync(int id, Categoria categoria)
        {
            try
            {
                if (id != categoria.CategoriaId)
                {
                    return BadRequest();
                }
                _context.Entry(categoria).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return Ok($"Categoria Id = {id} atualizada com sucesso!");
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
                // Obtendo a categoria de forma assíncrona
                var categoria = await _context.Categorias
                    .FirstOrDefaultAsync(p => p.CategoriaId == id);

                if (categoria == null)
                {
                    return NotFound($"Categoria com Id = {id} não localizada...");
                }

                // Removendo a categoria do contexto
                _context.Categorias.Remove(categoria);

                // Salvando as alterações de forma assíncrona
                await _context.SaveChangesAsync();

                return Ok($"Categoria Id = {id} deletada com sucesso!");
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                "Ocorreu um problema ao tratar a sua solicitação.");
            }
        }

    }
}
