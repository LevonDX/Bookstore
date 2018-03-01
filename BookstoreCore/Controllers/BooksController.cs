using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookstoreCore.Models.BookViewModels;
using Domain;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BookstoreCore.Controllers
{
    public class BooksController : Controller
    {
        private readonly BookstoreDBContext _dbContext;

        public BooksController(BookstoreDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IActionResult Index()
        {
            var books = _dbContext
                .Books
                .Where(b => !b.Deleted)
                .Include(b => b.BookAuthors)
                    .ThenInclude(b => b.Author);

            List<BookInfoViewModel> model =
                new List<BookInfoViewModel>();

            foreach (Book item in books)
            {
                model.Add(new BookInfoViewModel()
                {
                    BookTitle = item.Title,
                    BookID = item.ID,
                    Authors = String.Join(',', item.BookAuthors.Select(b => b.Author.Name))
                });
            }

            return View(model);
        }

        public async Task<ActionResult> Delete(int bookID)
        {
            Book b = await _dbContext.Books.FindAsync(bookID);

            b.Deleted = true;

            await _dbContext.SaveChangesAsync();

            return RedirectToAction("Index", "Books");
        }

        [HttpGet]
        public ActionResult Add()
        {
            var authors = _dbContext.Authors;

            AddEditBookViewModel model = new AddEditBookViewModel()
            {
                Authors = GetSelectListItem(authors)
            };

            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Add(AddEditBookViewModel model)
        {
            if (ModelState.IsValid)
            {
                Book book = new Book()
                {
                    Title = model.Title,
                };

                BookAuthors bookAuthors = new BookAuthors()
                {
                    Book = book,
                    Author = await _dbContext.Authors.FindAsync(model.AuthorID)
                };

                await _dbContext.BookAuthors.AddAsync(bookAuthors);
                await _dbContext.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            return View();
        }

        public async Task<ActionResult> Edit(int bookID)
        {
            Book bookToEdit = await _dbContext
               .Books
               .Where(b => !b.Deleted && b.ID == bookID) 
               .Include(b => b.BookAuthors)
                    .ThenInclude(b => b.Author)
               .SingleAsync();

            AddEditBookViewModel model = new AddEditBookViewModel()
            {
                Id = bookID,
                Title = bookToEdit.Title,

                AuthorID = bookToEdit.BookAuthors
                    .Where(b => b.BookID == bookID)
                    .Select(ba => ba.Author).Single().ID,

                Authors = GetSelectListItem(_dbContext.Authors),
            };

            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(AddEditBookViewModel bookModel)
        {
            if(ModelState.IsValid)
            {
                Book book = await _dbContext.Books.FindAsync(bookModel.Id);
                book.Title = bookModel.Title;

                await _dbContext.SaveChangesAsync();

                return RedirectToAction("index");
            }

            return View();
        }

        [NonAction]
        private IEnumerable<SelectListItem> GetSelectListItem(
            IEnumerable<Author> authors)
        {
            List<SelectListItem> authorsToSelect = new List<SelectListItem>();

            foreach (Author item in authors)
            {
                authorsToSelect.Add(new SelectListItem()
                {
                    Value = item.ID.ToString(),
                    Text = item.Name
                });
            }

            return authorsToSelect;
        }
    }
}