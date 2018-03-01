using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Domain.Entities
{
    public class Author
    {
        public int ID { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Surname { get; set; }

        public List<BookAuthors> BookAuthors { get; set; }

        //[NotMapped]
        //public List<Book> Books
        //{
        //    get
        //    {
        //        return BookAuthors
        //            .Where(ba => ba.AuthorID == ID)
        //            .Select(ba => ba.Book)
        //            .ToList();
        //    }
        //}
    }
}