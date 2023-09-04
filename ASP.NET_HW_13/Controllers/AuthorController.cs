using ASP.NET_HW_13.Data;
using ASP.NET_HW_13.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ASP.NET_HW_13.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorController : ControllerBase {
        private readonly DataContext _context;

        public AuthorController(DataContext context, IDbInitializer initializer) {
            _context = context;
            initializer.Initialize();
        }

        // GET: api/Author
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Author>>> GetAuthors() {
            if (_context.Authors == null) {
                return NotFound();
            }

            return await _context.Authors.Include(a => a.Books).ToListAsync();
        }

        // GET: api/Author/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Author>> GetAuthor(int id) {
            if (_context.Authors == null) {
                return NotFound();
            }

            var author = await _context.Authors.FindAsync(id);

            if (author == null) {
                return NotFound();
            }

            return author;
        }

        // PUT: api/Author/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id:int}")]
        public async Task<IActionResult> PutAuthor(int id, Author author) {
            if (id != author.Id) {
                return BadRequest();
            }

            _context.Entry(author).State = EntityState.Modified;

            try {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) {
                if (!AuthorExists(id)) {
                    return NotFound();
                }
                else {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Author
        [HttpPost]
        public async Task<ActionResult<Author>> PostAuthor(Author author) {
            if (_context.Authors == null) {
                return Problem("Entity set 'DataContext.Authors'  is null.");
            }

            _context.Authors.Add(author);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAuthor", new { id = author.Id }, author);
        }

        // DELETE: api/Author/5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteAuthor(int id) {
            if (_context.Authors == null) {
                return NotFound();
            }

            var author = await _context.Authors.FindAsync(id);
            if (author == null) {
                return NotFound();
            }

            _context.Authors.Remove(author);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AuthorExists(int id) {
            return (_context.Authors?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}