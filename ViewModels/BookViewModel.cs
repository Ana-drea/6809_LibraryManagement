using LibraryManagement.Models;

namespace LibraryManagement.ViewModels
{
    public class BookViewModel
    {
        public Book Book { get; set; } = new Book();
        public IEnumerable<Author>? Authors { get; set; } = new List<Author>();
        public IEnumerable<LibraryBranch>? Branches { get; set; } = new List<LibraryBranch>();
    }
}
