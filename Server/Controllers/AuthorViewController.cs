using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Data;
using Server.Models;

namespace Server.Controllers;

public class AuthorViewController : Controller {
    private readonly DataContext _context;

    public AuthorViewController(DataContext context) {
        _context = context;
    }

    // GET: AuthorView
    public async Task<IActionResult> Index() {
        return _context.Authors != null
            ? View(await _context.Authors.ToListAsync())
            : Problem("Entity set 'DataContext.Authors'  is null.");
    }

    // GET: AuthorView/Details/5
    public async Task<IActionResult> Details(int? id) {
        if (id == null || _context.Authors == null) {
            return NotFound();
        }

        var author = await _context.Authors
            .FirstOrDefaultAsync(m => m.Id == id);
        if (author == null) {
            return NotFound();
        }

        return View(author);
    }

    // GET: AuthorView/Create
    public IActionResult Create() {
        return View();
    }

    // POST: AuthorView/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,FirstName,LastName,BirthDate")] Author author) {
        if (ModelState.IsValid) {
            _context.Add(author);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        return View(author);
    }

    // GET: AuthorView/Edit/5
    public async Task<IActionResult> Edit(int? id) {
        if (id == null || _context.Authors == null) {
            return NotFound();
        }

        var author = await _context.Authors.FindAsync(id);
        if (author == null) {
            return NotFound();
        }

        return View(author);
    }

    // POST: AuthorView/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,LastName,BirthDate")] Author author) {
        if (id != author.Id) {
            return NotFound();
        }

        if (ModelState.IsValid) {
            try {
                _context.Update(author);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) {
                if (!AuthorExists(author.Id)) {
                    return NotFound();
                }
                else {
                    throw;
                }
            }

            return RedirectToAction(nameof(Index));
        }

        return View(author);
    }

    // GET: AuthorView/Delete/5
    public async Task<IActionResult> Delete(int? id) {
        if (id == null || _context.Authors == null) {
            return NotFound();
        }

        var author = await _context.Authors
            .FirstOrDefaultAsync(m => m.Id == id);
        if (author == null) {
            return NotFound();
        }

        return View(author);
    }

    // POST: AuthorView/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id) {
        if (_context.Authors == null) {
            return Problem("Entity set 'DataContext.Authors'  is null.");
        }

        var author = await _context.Authors.FindAsync(id);
        if (author != null) {
            _context.Authors.Remove(author);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool AuthorExists(int id) {
        return (_context.Authors?.Any(e => e.Id == id)).GetValueOrDefault();
    }
}