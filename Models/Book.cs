using System.ComponentModel.DataAnnotations;

namespace LibraryManagement.Models
{
    public class Book
    {
        public int BookId { get; set; }
        public string Title { get; set; }

        [Display(Name = "Author")]
        public int AuthorId { get; set; }
        [Display(Name = "Library Branch")]
        public int LibraryBranchId { get; set; }
        public Author? Author { get; set; }=new Author();
        public LibraryBranch? Branch { get; set; } = new LibraryBranch();
    }
}

