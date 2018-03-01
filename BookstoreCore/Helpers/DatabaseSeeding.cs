using BookstoreCore.Models;
using Domain;
using Domain.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookstoreCore.Helpers
{
    public static class DatabaseSeeding
    {
        public static void SeedBooks(BookstoreDBContext dBContext)
        {
            if ((dBContext.GetService<IDatabaseCreator>() as RelationalDatabaseCreator).Exists())
                return;

            dBContext.Database.Migrate();

            if (dBContext.BookAuthors.Any())
                return;

            Author schildt = new Author() { Name = "Herbert", Surname = "Schildt" };

            List<BookAuthors> bookAuthor = new List<BookAuthors>()
            {
                new BookAuthors(){ Book=new Book() { Title = "C++: The Complete Reference" }, Author = schildt},
                new BookAuthors(){ Book=new Book() { Title = "C#: A Beginner's Guide" }, Author = schildt},
                new BookAuthors(){ Book=new Book() { Title = "Java: A Beginner's Guide" }, Author = schildt},
                new BookAuthors(){ Book=new Book() { Title = "Born to Code In C" }, Author = schildt},
            };

            dBContext.BookAuthors.AddRange(bookAuthor);
            dBContext.SaveChanges();
        }
    }
}