using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain.Entities
{
    public class BookAuthors
    {
        [ForeignKey(nameof(Book))]
        public int BookID { get; set; }
        [ForeignKey(nameof(Author))]
        public int AuthorID { get; set; }

        public Book Book { get; set; }
        public Author Author { get; set; }
    }
}