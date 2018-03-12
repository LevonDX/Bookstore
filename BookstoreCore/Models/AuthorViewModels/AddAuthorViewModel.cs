using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookstoreCore.Models.AuthorViewModels
{
    public class AddAuthorViewModel
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
    }
}
