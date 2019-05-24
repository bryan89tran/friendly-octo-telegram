using Newtonsoft.Json;
using System;
using System.Web;
using System.Net;

namespace GiveMeBooks
{
    class Program
    {
        static void Main(string[] args)
        {

            while (true)
            {
                Console.WriteLine("Please enter something to search");
                var search = Console.ReadLine();
                rootObject bookResults = QueryGoogle(search);


                Console.WriteLine("Would you like to add to the database? Y/N");
                var isAddDatabase = Console.ReadLine();

                if (isAddDatabase.ToLower() == "y")
                {
                    AddToDatabse(bookResults);
                }

                Console.WriteLine("Do you want to list everything in thase datatase?");
                var isListDatabase = Console.ReadLine();

                if (isListDatabase.ToLower() == "y")
                {
                    GetAllItemDB();
                }

                Console.WriteLine("Start over?");
                var restart = Console.ReadLine();

                if (restart.ToLower() != "y")
                {
                    return;
                }

            }
        }

        public static rootObject QueryGoogle(string query)
        {
            var client = new WebClient();
            client.Headers.Add("Content-Type:application/json");
            client.Headers.Add("Accept:application/json");

            var urlString = "https://www.googleapis.com/books/v1/volumes?q=" + HttpUtility.UrlEncode(query);

            var result = client.DownloadString(urlString);
            rootObject bookResults = JsonConvert.DeserializeObject<rootObject>(result);

            Console.WriteLine(String.Format("{0, 20} {1, 20} {2, 20} {3, 20} {4, 20}", "Title", "Author", "Publisher", "ISBN", "HasImage"));
            foreach (var item in bookResults.Items)
            {
                var book = FilterBookData(item);
                DisplayBooks(book);
            }

            return bookResults;

        }

        public static void GetAllItemDB()
        {
            using (var db = new BooksContext())
            {

                Console.WriteLine();
                Console.WriteLine("All Books in database:");

                Console.WriteLine(String.Format("{0, 20} {1, 20} {2, 20} {3, 20} {4, 20}", "Title", "Author", "Publisher", "ISBN", "HasImage"));
                foreach (var book in db.DbBooks)
                {
                    
                    Console.WriteLine(String.Format("{0, 20} {1, 20} {2, 20} {3, 20} {4, 20}", book.Title.Truncate(15), book.Authors.Truncate(15), book.Publisher.Truncate(15), book.ISBN, book.ImgUrl.Truncate(15)));
                }

            }
        }

        public static void AddToDatabse(rootObject rootObject)
        {
            using (var db = new BooksContext())
            {
                int count = 0;
                foreach (var item in rootObject.Items)
                {
                    var book = FilterBookData(item);
                    db.DbBooks.Add(book);
                    db.SaveChanges();
                    count++;
                    Console.WriteLine("{0} books saved to database", count);
                }
            }

        }

        public static Book FilterBookData(Item item)
        {
            string ISBN = "Not Found";
            try
            {
                foreach (var ISBNType in item.VolumeInfo.IndustryIdentifiers)
                {
                    if (ISBNType.Type == "ISBN_13")
                    {
                        ISBN = ISBNType.Identifier;
                    }
                    else if (ISBNType.Type == "ISBN_10")
                    {
                        ISBN = ISBNType.Identifier;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"No ISBN {e}");
            }

            Book book = new Book
            {
                Title = item.VolumeInfo.Title,
                Authors = string.Join(",", item.VolumeInfo.Authors),
                Publisher = item.VolumeInfo.Publisher,
                ImgUrl = item.VolumeInfo.ImageLinks.SmallThumbnail,
                ISBN = ISBN
            };

            return book;
        }

        public static void DisplayBooks(Book book)
        {
            bool isBookImg = !string.IsNullOrEmpty(book.ImgUrl) ? true : false;

            Console.WriteLine(String.Format("{0, 20} {1, 20} {2, 20} {3, 20} {4, 20}", book.Title.Truncate(15), book.Authors.Truncate(15), book.Publisher.Truncate(15), book.ISBN, isBookImg));
            Console.WriteLine();
        }


    }

    public static class StringExt
    {
        public static string Truncate(this string value, int maxLength)
        {
            if (string.IsNullOrEmpty(value)) return value;
            return value.Length <= maxLength ? value : value.Substring(0, maxLength);
        }
    }
}
