using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace GiveMeBooks
{
    public class BooksContext : DbContext
    {
        public DbSet<Book> DbBooks { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=GiveMeBooksDB;Trusted_Connection=True;MultipleActiveResultSets=true;");
        }

    }

    public class Book
    {
        public int BookId { get; set; }
        public string ISBN { get; set; }
        public string Title { get; set; }
        public string Authors { get; set; }
        public string Publisher { get; set; }
        public string ImgUrl { get; set; }
    }
}
