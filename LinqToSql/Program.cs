using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinqToSql
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            string connection ="Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Library;Integrated Security=True;Connect Timeout=30;Encrypt=False;Pooling = true";
            LibraryDataContext libraryDataContext= new LibraryDataContext(connection);


            //1 - Вибрати всі книги, кількість сторінок в яких більше 100
            Console.WriteLine("1 - Вибрати всі книги, кількість сторінок в яких більше 100");
            var books = libraryDataContext.Books.Where(n => n.PageCount > 100);
            PrintBook(books);

            //2. Вибрати всі книги, ім’я яких починається на літеру ‘А’ або ‘а’
            Console.WriteLine("\n\n2. Вибрати всі книги, ім’я яких починається на літеру ‘А’ або ‘а’");
            books = libraryDataContext.Books.Where(n => n.Name[0] == 'A' || n.Name[0] == 'a');
            PrintBook(books);

            //3. Вибрати всі книги автора William Shakespeare
            Console.WriteLine("\n\n3. Вибрати всі книги автора Roberto Backshell");
            books = libraryDataContext.Books.Where(n => (n.Authors.Name +  " " + n.Authors.Surname) == "Roberto Backshell");
            PrintBook(books);



            //4.Вибрати всі книги українських авторів відсортувавши по алфавіту
            Console.WriteLine("\n\n4. Вибрати всі книги китайських авторів відсортувавши по алфавіту");
            books = libraryDataContext.Books.Where(n => n.Authors.Countries.Name == "China").OrderBy(n=>n.Name);
            PrintBook(books);


            //5. Вибрати всі книги, ім’я в яких складається менше ніж з 10-ти символів
            Console.WriteLine("\n\n5. Вибрати всі книги, ім’я в яких складається менше ніж з 10-ти символів");
            books = libraryDataContext.Books.Where(n => n.Name.Length < 10);
            PrintBook(books);

            //6. Вибрати книгу з максимальною кількістю сторінок не американського автора
            Console.WriteLine("\n\n6. Вибрати книгу з максимальною кількістю сторінок не американського автора");
            int maxPages = libraryDataContext.Books.Where(n => n.Authors.Countries.Name != "China").Max(n=>n.PageCount);
            Books book = libraryDataContext.Books.FirstOrDefault(n => n.Authors.Countries.Name != "China" && n.PageCount == maxPages);
            Console.WriteLine($"     \"{book.Name}\" {book.Authors.Name} {book.Authors.Surname}  {book.Authors.Countries.Name} - {book.PageCount}");

            //7. Вибрати автора, який має найменше книг в базі даних
            Console.WriteLine("\n\n7. Вибрати автора, який має найменше книг в базі даних");
            int minBooksCount = libraryDataContext.Authors.Min(n => n.Books.Count());
            Authors author = libraryDataContext.Authors.FirstOrDefault(n => n.Books.Count() == minBooksCount);
            Console.WriteLine($"      {author.Name} {author.Surname}");

            //8. Вибрати імена всіх авторів, крім американських, розташованих в алфавітному порядку
            Console.WriteLine("\n\n8. Вибрати імена всіх авторів, крім американських, розташованих в алфавітному порядку");
            var authorsName = libraryDataContext.Authors.Where(n => n.Countries.Name!= "China").Select(n => n.Name).OrderBy(n=>n);
            foreach (var name in authorsName)
                Console.WriteLine($"{name}");

            //9. Вибрати країну, авторів якої є найбільше в базі
            Console.WriteLine("\n\n9. Вибрати країну, авторів якої є найбільше в базі");
            int cCount = libraryDataContext.Countries.Max(n=>n.Authors.Count());
            var country = libraryDataContext.Countries.FirstOrDefault(n=>n.Authors.Count() == cCount).Name;
            Console.WriteLine(country);

            Console.Read();
        }

        static void PrintBook(IQueryable books)
        {
            foreach (Books book in books)
                Console.WriteLine($"     \"{book.Name}\" {book.Authors.Name} {book.Authors.Surname}  {book.Authors.Countries.Name} - {book.PageCount}");
        }
    }
}
