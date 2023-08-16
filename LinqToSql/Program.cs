using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Linq;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace LinqToSql
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            LibraryDataContext libraryDataContext= new LibraryDataContext(ConfigurationManager.ConnectionStrings["LibraryConnectionString"].ConnectionString);


            //1 - Вибрати всі книги, кількість сторінок в яких більше 200
            Console.WriteLine("1 - Вибрати всі книги, кількість сторінок в яких більше 200");
            GetBooksByPageCount(200, libraryDataContext);

            //2. Вибрати всі книги, ім’я яких починається на літеру ‘А’ або ‘а’
            Console.WriteLine("\n\n2. Вибрати всі книги, ім’я яких починається на літеру ‘А’ або ‘а’");
            GetBooksByNameStartLater('a', libraryDataContext);

            //3. Вибрати всі книги автора Arte Sacaze
            Console.WriteLine("\n\n3. Вибрати всі книги автора Arte Sacaze");
            GetBooksByAuthorName("Arte", "Sacaze", libraryDataContext);



            //4.Вибрати всі книги українських авторів відсортувавши по алфавіту
            Console.WriteLine("\n\n4. Вибрати всі книги українських авторів відсортувавши по алфавіту");
            GetBooksByAuthorCountry("Ukraine", libraryDataContext);


            //5. Вибрати всі книги, ім’я в яких складається менше ніж з 10-ти символів
            Console.WriteLine("\n\n5. Вибрати всі книги, ім’я в яких складається менше ніж з 10-ти символів");
            GetBooksByNameLeterCount(10, libraryDataContext);


            //6. Вибрати книгу з максимальною кількістю сторінок не американського автора
            Console.WriteLine("\n\n6. Вибрати книгу з максимальною кількістю сторінок не американського автора");
            GetBookByMaxPageExcludeCountry("USA", libraryDataContext);


            //7. Вибрати автора, який має найменше книг в базі даних
            Console.WriteLine("\n\n7. Вибрати автора, який має найменше книг в базі даних");
            GetFewestBooksAuthor(libraryDataContext);


            //8. Вибрати імена всіх авторів, крім американських, розташованих в алфавітному порядку
            Console.WriteLine("\n\n8. Вибрати імена всіх авторів, крім американських, розташованих в алфавітному порядку");
            GetAuthorsNamesExcludeCountry("USA", libraryDataContext);


            //9. Вибрати країну, авторів якої є найбільше в базі
            Console.WriteLine("\n\n9. Вибрати країну, авторів якої є найбільше в базі");
            GetCountryByMaxAuthors(libraryDataContext);

            Console.Read();
        }

        static void GetBooksByPageCount(int pageCount,LibraryDataContext dataContext)
        {
            var books = dataContext.Books.Where(n => n.PageCount > pageCount);
            PrintBooks(books);
        }

        static void GetBooksByNameStartLater(char leter, LibraryDataContext dataContext)
        {
            var books = dataContext.Books.Where(n => n.Name[0] == Char.ToLower(leter) || n.Name[0] == Char.ToUpper(leter));
            PrintBooks(books);
        }

        static void GetBooksByAuthorName(string name,string surname, LibraryDataContext dataContext)
        {
            var books = dataContext.Books.Where(n => n.Authors.Name == name && n.Authors.Surname == surname);
            PrintBooks(books);
        }

        static void GetBooksByAuthorCountry(string country, LibraryDataContext dataContext)
        {
            var books = dataContext.Books.Where(n => n.Authors.Countries.Name == country).OrderBy(n => n.Name);
            PrintBooks(books);
        }

        static void GetBooksByNameLeterCount(int leterCount, LibraryDataContext dataContext)
        {
            var books = dataContext.Books.Where(n => n.Name.Length < leterCount);
            PrintBooks(books);
        }

        static void GetBookByMaxPageExcludeCountry(string country, LibraryDataContext dataContext)
        {
            var book = dataContext.Books.Where(n => n.Authors.Countries.Name != country).OrderByDescending(n=>n.PageCount).First();
            PrintBook(book);
        }

        static void GetFewestBooksAuthor(LibraryDataContext dataContext)
        {
            var author = dataContext.Authors.OrderBy(n => n.Books.Count).Select(n => new { n.Surname, n.Name, n.Books.Count }).First();
            Console.WriteLine($"      {author.Name} {author.Surname}  - {author.Count} books");
        }

        static void GetAuthorsNamesExcludeCountry(string country, LibraryDataContext dataContext)
        {
            var authorsNames = dataContext.Authors.Where(n => n.Countries.Name != country).Select(n => n.Name).OrderBy(n => n);
            foreach (var name in authorsNames)
                Console.WriteLine($"     {name}");
        }

        static void GetCountryByMaxAuthors(LibraryDataContext dataContext)
        {
            var country = dataContext.Countries.OrderByDescending(n => n.Authors.Count).Select(n => new { n.Name, n.Authors.Count }).First();
            Console.WriteLine($"     {country.Name}  - {country.Count} authors");
        }

        static void PrintBook(Books book) => Console.WriteLine($"     \"{book.Name}\" {book.Authors.Name} {book.Authors.Surname}  [{book.Authors.Countries.Name}] - ({book.PageCount} pg.)");
        
        static void PrintBooks(IQueryable books)
        {
            foreach (Books book in books)
                PrintBook(book);
        }
    }
}
