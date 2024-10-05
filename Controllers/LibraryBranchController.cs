using Microsoft.AspNetCore.Mvc;
using LibraryManagement.Models;
using LibraryManagement.Data;

namespace LibraryManagement.Controllers
{
    public class LibraryBranchController : Controller
    {
        private readonly AppDbContext _context;

        public LibraryBranchController(AppDbContext appDbContext)
        {
            _context = appDbContext;
        }
        public IActionResult Details()
        {
            var branches = _context.LibraryBranches.ToList();
            return View(branches);
        }

        public IActionResult Edit(int id)
        {
            var libraryBranch = _context.LibraryBranches.FirstOrDefault(e => e.LibraryBranchId == id);
            return View(libraryBranch);
        }

        [HttpPost]
        public IActionResult Edit(LibraryBranch libraryBranch)
        {
            if (ModelState.IsValid)
            {
                var libraryToUpdate = _context.LibraryBranches.FirstOrDefault(e => e.LibraryBranchId == libraryBranch.LibraryBranchId);
                if (libraryToUpdate != null)
                {
                    libraryToUpdate.BranchName = libraryBranch.BranchName;
                    _context.SaveChanges();
                }
            }
            return RedirectToAction(nameof(Details));
        }

        public IActionResult Delete(int id)
        {
            if (id != null)
            {
                var libraryToUpdate = _context.LibraryBranches.FirstOrDefault(e => e.LibraryBranchId == id);
                if (libraryToUpdate != null)
                {
                    // remove books under that library branch first
                    var booksToDelete = _context.Books.Where(e=>e.LibraryBranchId ==id).ToList();
                    if (booksToDelete.Any())
                    {
                        _context.RemoveRange(booksToDelete);
                    }

                    _context.LibraryBranches.Remove(libraryToUpdate);
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
        public IActionResult Add(string branchName)
        {
            if (branchName == null) return View();
            var maxId = 0;
            if (_context.LibraryBranches.Any()) {
                maxId=_context.LibraryBranches.Max(e => e.LibraryBranchId);
            }
            var libraryBranch = new LibraryBranch
            {
                LibraryBranchId = maxId + 1,
                BranchName = branchName
            };
            _context.LibraryBranches.Add(libraryBranch);
            _context.SaveChanges();

            return RedirectToAction(nameof(Details));
        }
    }
}
