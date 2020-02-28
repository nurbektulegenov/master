using System.Collections.Generic;

namespace BookTestProject.Models {
    public class DataViewModel {
        public IEnumerable<BookViewModel> BooksViewModels { get; set; }
        public IEnumerable<AuthorViewModel> AuthorViewModels { get; set; }

    }
}