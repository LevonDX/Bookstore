using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookstoreCore.Models.BookViewModels
{
    public class BookInfoViewModel
    {
        public int BookID { get; set; }
        public string BookTitle { get; set; }
        public string Authors { get; set; }
    }
}
