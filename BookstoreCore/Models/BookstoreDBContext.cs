using Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public class BookstoreDBContext : IdentityDbContext<ApplicationUser>
    {
        public BookstoreDBContext(DbContextOptions<BookstoreDBContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<BookAuthors>()
                .HasKey(k => new { k.BookID, k.AuthorID });            
        }

        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<BookAuthors> BookAuthors { get; set; }
    }
}