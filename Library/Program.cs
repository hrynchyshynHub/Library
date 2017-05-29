using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.IO;

namespace Library
{
   
    public class Program
    {
    

        static void Main(string[] args)
        {
           
            string FileName = "library.xml";
            bool b = true;

            while (b)
            {
                switch (Menu())
                {
                    case 1:
                        Console.Clear();
                        ShowDepartaments(loadLibraryFromXml(FileName));
                        break;
                    case 2:
                        Console.Clear();
                        InsertDepartament(loadLibraryFromXml(FileName), FileName);
                        break;
                    case 3:
                        Console.Clear();
                        Console.WriteLine("Type departament id:");
                        int depId;
                        int.TryParse(Console.ReadLine(),  out depId);
                        UpdateDepartament(loadLibraryFromXml(FileName), depId,FileName);
                        break;
                    case 4:
                        Console.Clear();
                        ShowDepartaments(loadLibraryFromXml(FileName));
                        Console.WriteLine("Select departament id");
                        int departamentId;
                        int.TryParse(Console.ReadLine(), out departamentId);
                        DeleteDepartament(loadLibraryFromXml(FileName), departamentId,FileName);
                        break;
                    case 5:
                        Console.Clear();
                        loadLibraryFromXml(FileName).SortDepartamentsByBooks();
                        break;
                    case 6:
                        Console.Clear();
                        Library l = loadLibraryFromXml(FileName);
                        l.ShowAuthors();
                        l.ShowAllBooksSortedByPages();
                        break;
                    case 7:
                        Console.WriteLine("Library have: " + loadLibraryFromXml(FileName).countBooks() + " books");
                        break;
                    case 8:
                        createBook();
                        break;
                    case 9:
                        Console.Clear();
                        Departament d = loadLibraryFromXml(FileName).Departaments.ToArray().OrderByDescending(x => x.books.Count).FirstOrDefault();
                        Console.WriteLine(d.DepName);
                        Console.WriteLine("max book is " + d.books.Count);
                        break;
                    case 10:
                        Book book = loadLibraryFromXml(FileName).showBooksThereHaveLessPages();
                        Console.WriteLine(book.ToString());
                        break;
                    case 11:
                        Console.Clear();
                        b = false;
                        break;
                }
            }
                                

                

        }


        static int Menu()
        {   int choice;
            Console.WriteLine("-----------Menu----------");
            Console.WriteLine("1 - Show Deparrtaments");
            Console.WriteLine("2 - Add departament");
            Console.WriteLine("3 - Edit Departament");
            Console.WriteLine("4 - Delete Departament");
            Console.WriteLine("5 - Sort Departaments by Book");
            Console.WriteLine("6 - Show Authors and Books");
            Console.WriteLine("7 - Count books");
            Console.WriteLine("8 - Create book");
            Console.WriteLine("9 - Show Departamrnt with max books");
            Console.WriteLine("10 -show book with min pages in all libreary");
            Console.WriteLine("11 - exit");
            Console.WriteLine("Enter number(0-9): "); 
            int.TryParse(Console.ReadLine(), out choice);
            return choice;
        }


        public static void createBook()
        {

            Console.Clear();
            Console.WriteLine("Enter title of your book");
            string bookName = Console.ReadLine();
            Console.WriteLine("Enter pages");
            int pages;
            int.TryParse(Console.ReadLine(), out pages);
            Library library = loadLibraryFromXml("library.xml");
            library.ShowAuthors();
            Console.WriteLine("chose author id: ");
            int authorId = Convert.ToInt32(Console.ReadLine());
            Author a = Author.findAuthorById(authorId);
            Console.WriteLine("chose Departament");
            library.SortDepartamentsByBooks();
            int depId = Convert.ToInt32(Console.ReadLine());
            Departament d = library.FindDepartamentById(depId);
            Book b = new Book();
            b.author = a;
            b.AuthorId = authorId;
            b.Pages = pages;
            b.Title = bookName;
            b.departament = d;
            library.Departaments.Find(x => x.Id == depId).books.Add(b);
            XmlSerializer ser = new XmlSerializer(typeof(Library));
            StreamWriter sw = new StreamWriter("library.xml");
            ser.Serialize(sw, library);
            sw.Close();
        }

        public static Library loadLibraryFromXml(string filename)
        {   Library library = null;
            try
            {
                XmlSerializer xmlSer = new XmlSerializer(typeof(Library));
                StreamReader sr = new StreamReader(filename);
                library = (Library)xmlSer.Deserialize(sr);;
                sr.Close();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return library;
        }



        static void ShowDepartaments(Library libarary) {
            foreach(Departament d in libarary.Departaments)
            {
                Console.WriteLine("------------------departament id = " + d.Id+ "-------------------");
                Console.WriteLine("Departament: " + d.DepName);
                Console.WriteLine("Books:");
                if(d.books != null || d.books.Count > 0)
                {
                    foreach(Book b in d.books)
                    {
                    Console.WriteLine("title: " + b.Title + " pages: " + b.Pages);
                    }
                }
               
            }
        }


        public static Departament InsertDepartament(Library library, string filename)
        {
            Console.WriteLine("Enter name of departament");
            string depName = Console.ReadLine();
            var departament = new Departament();
            departament.Lib_id = library.Id;
            departament.DepName = depName;
            library.Departaments.Add(departament);
            XmlSerializer ser = new XmlSerializer(typeof(Library));
            StreamWriter sw = new StreamWriter(filename);
            ser.Serialize(sw, library);
            sw.Close();
            return departament;
        }

        public static void UpdateDepartament(Library libarary, int depID, string Filename)
        {
            Departament departament = libarary.FindDepartamentById(depID);
            Console.WriteLine("------current departament--------");
            Console.WriteLine(departament.ToString());
            Console.WriteLine("Enter new name of departament");
            string NewDepName = Console.ReadLine();
            departament.DepName = NewDepName;
            Console.WriteLine("Save all departaments books?(Yes/No)");
            string choice = Console.ReadLine();
            if (choice.ToLower().Trim().Equals("no")) {
                departament.books.Clear();
            }
            XmlSerializer serilaizer = new XmlSerializer(typeof(Library));
            StreamWriter sw = new StreamWriter(Filename);
            serilaizer.Serialize(sw, libarary);
            sw.Close();
            Console.WriteLine("Succesfull updeate");
        }
        public static void DeleteDepartament(Library library, int departamentId, string FileName)
        {
            Departament departament = library.FindDepartamentById(departamentId);
            library.Departaments.Remove(departament);
            XmlSerializer serializer = new XmlSerializer(typeof(Library));
            StreamWriter sw = new StreamWriter(FileName);
            serializer.Serialize(sw, library);
            sw.Close();
        }


        static void initXml()
        {
            Author a = new Author();
            a.Name = "Mukola";
            Library library = new Library
            {
              
                
                Name = "Shevchenko Libarary",
                Authors = new List<Author> {
                    new Author {Name = "Ivan" , Surname = "Hrynchsyhyn" },
                    new Author {Name = "Taras", Surname = "Shevchenko"},
                    new Author {Name = "Adolf" ,Surname = "Gitler"},
                    new Author {Name = "Bruce", Surname = "Ekkel"},
                    a
                },

                Departaments = new List<Departament>
               {
                        new Departament {DepName = "IT" ,
                            books = new List<Book> { new Book {Title = "Java", Pages = 600, author = a},
                                                     new Book {Title = "Php" , Pages = 3434, AuthorId = 1 } } },
                        new Departament {DepName = "Clissic" ,
                            books = new List<Book> { new Book {Title = "Romeo", Pages = 43, AuthorId = 2 },
                                                     new Book {Title = "Bukvar" , Pages = 4, AuthorId = 0 } } },
                        new Departament {DepName = "Ukraine" }
                }

            };
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(Library));
            StreamWriter sw = new StreamWriter("library.xml");
            xmlSerializer.Serialize(sw, library);
            sw.Close();
        }

    }
}
