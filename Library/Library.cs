using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Library
{
   
    [XmlRoot]
    public class Library:ICountingBooks
    {
        [XmlIgnore]
        private static int id;

        [XmlAttribute("id")]
        public int Id { get; set; }
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlArray("authors")]
        [XmlArrayItem("author")]
        public  List<Author> Authors { get; set; }

        [XmlArray("departaments")]
        [XmlArrayItem("departament")]
        public List<Departament>Departaments { get; set; }

        public Library()
        {
            id++;
            this.Id = id;
        }
        public int countBooks()
        {
            int sum = 0;
            Departaments.ToList().ForEach(delegate (Departament d)
            {
                sum += d.books.Count;
            });
            return sum;
        }
    
        public  Author FindAuthorById(int idAuthor)
        {
            var author = Authors.Find(x => x.Id == idAuthor);
            return author;
        }

        public Departament FindDepartamentById(int depID)
        {
            return Departaments.FirstOrDefault(x => x.Id == depID);        
        }

        public void SortDepartamentsByBooks()
        {
            var sortedDepartaments = this.Departaments.OrderByDescending(x => x.books.Count).ToList();
            Console.WriteLine();
            sortedDepartaments.ForEach(delegate(Departament d)
            {
                Console.WriteLine("----------------Departament id = "+d.Id+"--------------");
                Console.WriteLine("Name:" + d.DepName);
                Console.WriteLine("Count books:"+ d.books.Count);
            });
        }
        public Book showBooksThereHaveLessPages()
        {
            List<Book> allBooks = new List<Book>();
            foreach (Departament d in Departaments) {
                allBooks.AddRange(d.books);
            }

            return allBooks.OrderBy(x => x.Pages).FirstOrDefault();
           
        }
        public void ShowAuthors()
        {
            Console.WriteLine("-------------All Authors------------");
            Authors.ForEach(delegate (Author a)
            {
                Console.WriteLine(a.ToString());
            });
            Console.WriteLine("------------------------------------");
        }
        public  void SortAuthorByBook()
        {
            
            var sortedAuthor = Authors.OrderByDescending(x => x.books.Count).ToList();
            sortedAuthor.ForEach(delegate (Author a) {
                Console.WriteLine(a.Name + a.Surname + " have been write " + a.books.Count);
            });
        }
        public void ShowAllBooksSortedByPages()
        {
            Console.WriteLine("-------------All Books------------");
            Departaments.ForEach(delegate (Departament d) {
                d.books.OrderByDescending(x=>x.Pages).ToList().ForEach(delegate (Book b) {
                    Console.WriteLine(b.ToString());
                });
            });
            Console.WriteLine("----------------------------------");
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Library name = " + this.Name);
            sb.Append("All departaments in this library\n");
            foreach(Departament d  in Departaments)
            {
                Console.WriteLine(d.ToString());
            }
            sb.Append("--------Authors---------");
            foreach(Author a in Authors)
            {
                Console.WriteLine(a.ToString());
            }
            return sb.ToString();
        }


    }

    public class Departament
    {   [XmlIgnore]
        private static int id;

        [XmlAttribute("id")]
        public int Id{ get; set; }
        [XmlAttribute("name")]
        public string DepName { get; set; }
        [XmlAttribute("library_id")]
        public int Lib_id { get; set; }
        [XmlIgnore]
        public Library library { get; set; }
        [XmlArray("books")]
        [XmlArrayItem("book")]
        public List<Book> books { get; set; }

        public Departament() {
            id++;
            this.Id = id;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Departament name " + DepName+"\n");
            sb.Append("-------Departaments books---------");
            foreach(Book b in books)
            {
                Console.WriteLine(b.ToString());
            }
            return sb.ToString();

        }

    }
    public class Book
    {
        [XmlIgnore]
        private static int id;

        [XmlAttribute("id")]
        public  int Id { get; set; }
        [XmlAttribute("title")]
        public string Title { get; set; }
        [XmlAttribute("pages")]
        public int Pages { get; set; }
        [XmlAttribute("author_id")]
        public int AuthorId{ get; set; }
        [XmlIgnore]
        public Departament departament;
        [XmlIgnore]
        public Author author { get; set; }

        public Book()
        {
            id++;
            this.Id = id;
            author = Author.findAuthorById(AuthorId);

        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("------------------------#" + Id + "--------------------------------------\n");
            sb.Append("Book title: " + Title+ "   pages: " + Pages);
            sb.Append(" AuthorId: " + AuthorId);
            return sb.ToString();
        }

    }
    public class Author
    {
        [XmlIgnore]
        private static int id;
        [XmlIgnore]
        public static List<Author> all = new List<Author>();

        [XmlAttribute("id")]
        public int Id{ get; set;  }
        [XmlAttribute("name")]
        public string Name { get; set; }
        [XmlAttribute("surname")]
        public string Surname { get; set; }
        [XmlIgnore]
        public List<Book> books { get; set; }
       

        public Author()
        {
            id++;
            this.Id = id;
            
         }
        public static Author findAuthorById(int id)
        {
            Author a = all.FirstOrDefault(x => x.Id == id);
            return a;
        }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("id: " + Id + " ");
            sb.Append("Name:" + Name + "   Surname:" + Surname);
            return sb.ToString();
        }   


    }
  
}
