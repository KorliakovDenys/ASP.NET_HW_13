using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Server.Data;
using Server.Models;

namespace Server.Controllers;

public class BookViewController : Controller {
    private readonly DataContext _context;

    public BookViewController(DataContext context) {
        _context = context;
    }

    // GET: BookView
    public async Task<IActionResult> Index() {
        var dataContext = _context.Books.Include(b => b.Author);
        return View(await dataContext.ToListAsync());
    }

    // GET: BookView/Details/5
    public async Task<IActionResult> Details(int? id) {
        if (id == null || _context.Books == null) {
            return NotFound();
        }

        var book = await _context.Books
            .Include(b => b.Author)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (book == null) {
            return NotFound();
        }

        return View(book);
    }

    // GET: BookView/Create
    public IActionResult Create() {
        ViewData["AuthorId"] = new SelectList(_context.Authors, "Id", "FirstName");
        return View();
    }

    // POST: BookView/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Title,AuthorId,PublicationDate")] Book book) {
        if (ModelState.IsValid) {
            _context.Add(book);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        ViewData["AuthorId"] = new SelectList(_context.Authors, "Id", "FirstName", book.AuthorId);
        return View(book);
    }

    // GET: BookView/Edit/5
    public async Task<IActionResult> Edit(int? id) {
        if (id == null || _context.Books == null) {
            return NotFound();
        }

        var book = await _context.Books.FindAsync(id);
        if (book == null) {
            return NotFound();
        }

        ViewData["AuthorId"] = new SelectList(_context.Authors, "Id", "FirstName", book.AuthorId);
        return View(book);
    }

    // POST: BookView/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Title,AuthorId,PublicationDate")] Book book) {
        if (id != book.Id) {
            return NotFound();
        }

        if (ModelState.IsValid) {
            try {
                _context.Update(book);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) {
                if (!BookExists(book.Id)) {
                    return NotFound();
                }
                else {
                    throw;
                }
            }

            return RedirectToAction(nameof(Index));
        }

        ViewData["AuthorId"] = new SelectList(_context.Authors, "Id", "FirstName", book.AuthorId);
        return View(book);
    }

    // GET: BookView/Delete/5
    public async Task<IActionResult> Delete(int? id) {
        if (id == null || _context.Books == null) {
            return NotFound();
        }

        var book = await _context.Books
            .Include(b => b.Author)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (book == null) {
            return NotFound();
        }

        return View(book);
    }

    // POST: BookView/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id) {
        if (_context.Books == null) {
            return Problem("Entity set 'DataContext.Books'  is null.");
        }

        var book = await _context.Books.FindAsync(id);
        if (book != null) {
            _context.Books.Remove(book);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool BookExists(int id) {
        return (_context.Books?.Any(e => e.Id == id)).GetValueOrDefault();
    }
}