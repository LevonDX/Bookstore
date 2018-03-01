using Domain.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BookstoreCore.Models.BookViewModels
{
    public class AddEditBookViewModel
    {
        public int? Id { get; set; }

        [Required(ErrorMessage ="Լրացրու")]
        [Display(Name = "Book Title")]
        public string Title { get; set; }
        
        public IEnumerable<SelectListItem> Authors { get; set; }

        public int AuthorID { get; set; }
    }
}
