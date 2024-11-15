// BookController.cs
using Microsoft.AspNetCore.Mvc;
using LibraryManagement.Models;
using LibraryManagement.ViewModels;
using Microsoft.EntityFrameworkCore;
using LibraryManagement.Data;
using Microsoft.AspNetCore.Authorization;

namespace LibraryManagement.Controllers
{
    [Authorize]
    public class BookController : Controller
    {
        private readonly AppDbContext _context;

        // Constructor to inject the database context
        public BookController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Details()
        {
            var books = _context.Books.ToList();
            var bookViewModels = new List<BookViewModel>();
            foreach (var book in books)
            {
                book.Author = _context.Authors.FirstOrDefault(e => e.AuthorId == book.AuthorId);
                book.Branch = _context.LibraryBranches.FirstOrDefault(e => e.LibraryBranchId == book.LibraryBranchId);
                var BookViewModel = new BookViewModel()
                {
                    Book = book
                };
                bookViewModels.Add(BookViewModel);
            }
            _context.SaveChanges();
            return View(bookViewModels);
        }

        public IActionResult Edit(int id)
        {
            var book = _context.Books.FirstOrDefault(e => e.BookId == id);
            if (book == null) return RedirectToAction(nameof(Details));

            var author = _context.Authors.FirstOrDefault(e => e.AuthorId == book.AuthorId);
            var branch = _context.LibraryBranches.FirstOrDefault(e => e.LibraryBranchId == book.LibraryBranchId);
            book.Author = author;
            book.Branch = branch;
            var bookViewModel = new BookViewModel()
            {
                Book = book,
                Authors = _context.Authors.ToList(),
                Branches = _context.LibraryBranches.ToList()
            };
            _context.SaveChanges();
            return View(bookViewModel);
        }

        [HttpPost]
        public IActionResult Edit(BookViewModel bookViewModel)
        {
            if (bookViewModel == null) return RedirectToAction(nameof(Details));

            var book = _context.Books.FirstOrDefault(e => e.BookId == bookViewModel.Book.BookId);
            if (book == null) return RedirectToAction(nameof(Details));
            book.Title = bookViewModel.Book.Title;
            book.AuthorId = bookViewModel.Book.AuthorId;
            book.LibraryBranchId = bookViewModel.Book.LibraryBranchId;
            _context.SaveChanges();
            return RedirectToAction(nameof(Details));
        }

        public IActionResult Delete(int id)
        {
            if (id == null) return RedirectToAction(nameof(Details));
            var book = _context.Books.FirstOrDefault(e => e.BookId == id);
            _context.Books.Remove(book);
            _context.SaveChanges();
            return RedirectToAction(nameof(Details));
        }

        public IActionResult Add()
        {
            var bookViewModel = new BookViewModel()
            {
                Book = new Book(),
                Authors = _context.Authors,
                Branches = _context.LibraryBranches
            };
            return View(bookViewModel);
        }
        [HttpPost]
        public IActionResult Add(BookViewModel bookViewModel)
        {
            if (bookViewModel == null) return RedirectToAction(nameof(Details));

            var maxId = 0;
            if (_context.Books.Any())
            {
                maxId = _context.Books.Max(e => e.BookId);
            }
            var book = new Book()
            {
                BookId = maxId + 1,
                Title = bookViewModel.Book.Title,
                AuthorId = bookViewModel.Book.AuthorId,
                LibraryBranchId = bookViewModel.Book.LibraryBranchId,
                Author = _context.Authors.FirstOrDefault(e=>e.AuthorId == bookViewModel.Book.AuthorId),
                Branch = _context.LibraryBranches.FirstOrDefault(e=>e.LibraryBranchId==bookViewModel.Book.LibraryBranchId)
            };
            _context.Books.Add(book);
            _context.SaveChanges();

            return RedirectToAction(nameof(Details));
        }
    }
}
