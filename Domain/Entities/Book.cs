using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Linq;

namespace Domain.Entities
{
    public class Book
    {
        public int ID { get; set; }
        public string Title { get; set; }

        public List<BookAuthors> BookAuthors { get; set; }

        public bool Deleted { get; set; }
        //[NotMapped]
        //public List<Author> Authors
        //{
        //    get
        //    {
        //        return BookAuthors
        //            .Where(ba => ba.BookID == ID)
        //            .Select(b => b.Author)
        //            .ToList();
        //    }
        //}
    }
}
