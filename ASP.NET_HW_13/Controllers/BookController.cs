using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ASP.NET_HW_13.Data;
using ASP.NET_HW_13.Models;

namespace ASP.NET_HW_13.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase {
        private readonly DataContext _context;

        public BookController(DataContext context, IDbInitializer initializer) {
            _context = context;
            initializer.Initialize();
        }

        // GET: api/Book
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Book>>> GetBooks(int? pageSize, int? page) {
            if (_context.Books == null) {
                return NotFound();
            }

            IQueryable<Book> books =  _context.Books.Include(b => b.Author);

            if (pageSize is null || page is null) {
                return await books.ToListAsync();
            }

            return await books.OrderBy(b => b.Id).Skip(((int)page - 1) * (int)pageSize).Take((int)pageSize).ToListAsync();
        }

        
        // GET: api/Book/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Book>> GetBook(int id) {
            if (_context.Books == null) {
                return NotFound();
            }

            var book = await _context.Books.FindAsync(id);
            
            if (book == null) {
                return NotFound();
            }

            return book;
        }

        // PUT: api/Book/5
        [HttpPut("{id:int}")]
        public async Task<IActionResult> PutBook(int id, Book book) {
            if (id != book.Id) {
                return BadRequest();
            }

            _context.Entry(book).State = EntityState.Modified;

            try {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) {
                if (!BookExists(id)) {
                    return NotFound();
                }
                else {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Book
        [HttpPost]
        public async Task<ActionResult<Book>> PostBook(Book book) {
            if (_context.Books == null) {
                return Problem("Entity set 'DataContext.Books'  is null.");
            }

            _context.Books.Add(book);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBook", new { id = book.Id }, book);
        }

        // DELETE: api/Book/5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteBook(int id) {
            if (_context.Books == null) {
                return NotFound();
            }

            var book = await _context.Books.FindAsync(id);
            if (book == null) {
                return NotFound();
            }

            _context.Books.Remove(book);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BookExists(int id) {
            return (_context.Books?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}