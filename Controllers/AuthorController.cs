using Microsoft.AspNetCore.Mvc;
using LibraryManagement.Models;
using Microsoft.EntityFrameworkCore;
using LibraryManagement.Data;

namespace LibraryManagement.Controllers
{
    public class AuthorController : Controller
    {
        private readonly AppDbContext _context;

        // Constructor to inject the database context
        public AuthorController(AppDbContext context)
        {

            _context = context;
        }
        public IActionResult Details()
        {
            var authors = _context.Authors.ToList();
            return View(authors);
        }

        public IActionResult Edit(int id)
        {
            var author = _context.Authors.FirstOrDefault(e => e.AuthorId == id);
            return View(author);
        }

        [HttpPost]
        public IActionResult Edit(Author author)
        {
            if (ModelState.IsValid)
            {
                var authorToUpdate = _context.Authors.FirstOrDefault(e => e.AuthorId == author.AuthorId);
                if (authorToUpdate != null)
                {
                    authorToUpdate.Name = author.Name;
                    _context.SaveChanges();
                }
            }
            return RedirectToAction(nameof(Details));
        }

        public IActionResult Delete(int id)
        {
            if (id != null)
            {
                var authorToUpdate = _context.Authors.FirstOrDefault(e => e.AuthorId == id);
                if (authorToUpdate != null)
                {
                    // remove books under that author first
                    var booksToDelete = _context.Books.Where(e=>e.AuthorId == id).ToList();
                    if (booksToDelete.Any())
                    {
                        _context.Books.RemoveRange(booksToDelete);
                    }

                    _context.Authors.Remove(authorToUpdate);
                    _context.SaveChanges();
                }
            }
            return RedirectToAction(nameof(Details));
        }

        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Add(string name)
        {
            if (name == null) return View();
            var maxId = 0;
            if (_context.Authors.Any())
            {
                maxId = _context.Authors.Max(e => e.AuthorId);
            }
            var author = new Author
            {
                AuthorId = maxId + 1,
                Name = name
            };
            _context.Authors.Add(author);
            _context.SaveChanges();

            return RedirectToAction(nameof(Details));
        }
    }

}

