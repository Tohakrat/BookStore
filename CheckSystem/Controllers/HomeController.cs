 using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CheckSystem.Models;
using System.Data.Entity;

namespace BookStore.Controllers
{
    public class PageInfo
    {
        public int PageNumber { get; set; } // номер текущей страницы
        public int PageSize { get; set; } // кол-во объектов на странице
        public int TotalItems { get; set; } // всего объектов
        public int TotalPages  // всего страниц
        {
            get { return (int)Math.Ceiling((decimal)TotalItems / PageSize); }
        }
    }
    public class IndexViewModel
    {
        public IEnumerable<Book> Books { get; set; }
        public PageInfo PageInfo { get; set; }
    }
    //Student student = db.Students.Find(id);
    public class HomeController : Controller
    {
        // создаем контекст данных
        BookContext db = new BookContext();

        public ActionResult Index()
        {
            // получаем из бд все объекты Book
            IEnumerable<Book> books = db.Books;
            // передаем все объекты в динамическое свойство Books в ViewBag
            ViewBag.Books = books;
            // возвращаем представление
            return View();
        }
        public ActionResult Purchases()
        {
            // получаем из бд все объекты Book
            IEnumerable<Purchase> purchases = db.Purchases;
            // передаем все объекты в динамическое свойство Books в ViewBag
            ViewBag.purchases = purchases;
            // возвращаем представление
            return View();
        }
        public ActionResult Purchase(int? id)
        {
            // получаем из бд все объекты Book
            ViewBag.purchase = db.Purchases.First(c => c.PurchaseId == id);
            
            // передаем все объекты в динамическое свойство Books в ViewBag
            
            // возвращаем представление
            return View(id);
        }
        public ActionResult Buy(int? id)
        {            
            ViewBag.BookId = id;
            return View();
            //ViewBag.purchase = db.Purchases.First(c => c.PurchaseId == id);                    
        }
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public string Buy([Bind(Include = "Person, Address, BookId")]Purchase purchase)
        {
            purchase.Date = DateTime.Now;           
            try
            {
                if (ModelState.IsValid)
                {
                    db.Purchases.Add(purchase);
                    db.SaveChanges();
                    return "Покупка совершена, " + purchase.Person + ", Поздравляем!";
                }
            }
            catch (Exception  dex )
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                //ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }
            //return View();
            return "Спасибо, " + purchase.Person + ", за покупку!";
        }

                
        public ActionResult AllBooks(int page = 1)
        {
            int pageSize = 3; // количество объектов на страницу
            IEnumerable<Book> boooksPerPages = db.Books.OrderBy(p => p.Id).Skip((page - 1) * pageSize).Take(pageSize);
            PageInfo pageInfo = new PageInfo { PageNumber = page, PageSize = pageSize, TotalItems = db.Books.Count() };
            IndexViewModel ivm = new IndexViewModel { PageInfo = pageInfo, Books = boooksPerPages };
            return View(ivm);
            //ViewBag.Title = "books";
            //ViewBag.Books = db.Books;
            /*   var BookExt = db.Books.Join(db.Authors, // второй набор
           p => p.AuthorId, // свойство-селектор объекта из первого набора
           c => c.Id, // свойство-селектор объекта из второго набора
           (p, c) => new // результат
           {
               Id = p.Id,
               Name = p.Name,
               Price = p.Price,
               Author = c.Name          
           });
            var BookExt = from p in db.Books
                         join c in db.Authors on p.AuthorId equals c.Id
                         select new { Id = p.Id, Name = p.Name, Price = p.Price, Author = c.Name };
            //ViewBag.Books1 = BookExt;*/
            //return View(db.Books);
            //ViewBag.purchase = db.Purchases.First(c => c.PurchaseId == id);                    
        }
        public ActionResult AllAuthors()
        {
            ViewBag.Authors = db.Authors;
            return View();
            //ViewBag.purchase = db.Purchases.First(c => c.PurchaseId == id);                    
        }
        public ActionResult NewBook()
        {
            IEnumerable<Author> Authors = db.Authors;
            ViewBag.Authors = Authors;
               //.Select(p => new { p.Id, p.Name });
            return View();
            //ViewBag.purchase = db.Purchases.First(c => c.PurchaseId == id);                    
        }

        [HttpPost]
        public string NewBook([Bind(Include = "Name, Price, AuthorId")] Book book)
        {           
            try
            {
                if (ModelState.IsValid)
                {
                    db.Books.Add(book);
                    db.SaveChanges();
                    return "Книга добавлена " + book.Name+ " ,  " +book.Price + " ,  " + book.AuthorId+ ", Поздравляем!";
                }
            }
            catch (Exception dex)
            {
                return "Exception " + dex.Message;
                //Log the error (uncomment dex variable name and add a line here to write a log.
                //ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }
            return "1" ;
        }
        [HttpGet]
        public ActionResult EditBook(int id)
        {
            IEnumerable<Author> Authors = db.Authors;
            ViewBag.Authors = Authors;
            ViewBag.Book = db.Books.Find(id);            
            return View();                              
        }
        [HttpPost]
        public string EditBook(Book book)
        {
            db.Entry(book).State = EntityState.Modified;
            db.SaveChanges();
            //return RedirectToAction("home\AllBooks");
            return "Книга изменена " + book.Name + " ,  " + book.Price + " ,  " + book.AuthorId + ", Поздравляем!";           
        }
        public ActionResult NewAuthor()
        {
            //IEnumerable<Author> Authors = db.Authors;
            //ViewBag.Authors = Authors;
            //.Select(p => new { p.Id, p.Name });
            return View();
            //ViewBag.purchase = db.Purchases.First(c => c.PurchaseId == id);                    
        }

        [HttpPost]
        public ActionResult NewAuthor(Author author)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Authors.Add(author);
                    db.SaveChanges();
                    //return "New author" + author.Name + " ,  " + author.Age  + ", Поздравляем!";
                    return RedirectPermanent("/Home/AllAuthors");
                }
                else { throw new ArgumentException("ModelState is not valid");
                    //return "An error ocured, we are sorry...";
                }
            }
            catch (Exception dex)
            {
                throw new ArgumentException(dex.ToString());
                //return "Exception " + dex.Message;
                //Log the error (uncomment dex variable name and add a line here to write a log.
                //ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }

        }
        
     



    [HttpGet]
        public ActionResult EditAuthor(int Id)
        {
            //IEnumerable<Author> Authors = db.Authors;
            //ViewBag.Authors = Authors;
            ViewBag.Author = db.Authors.Find(Id);
            return View();
        }
        [HttpPost]
        public string EditAuthor(Author author)
        {
            db.Entry(author).State = EntityState.Modified;
            db.SaveChanges();
            //return RedirectToAction("home\AllBooks");
            return "Данные автора изменены " + author.Name + " ,  " + author.Age + " ,  " + author.Id + ", Поздравляем!";
            //return View(/ Home / AllAuthors /);

        }
        
        public ActionResult DeleteAuthor(int Id)
        {
            ViewBag.Author = db.Authors.Find(Id);           
            return View();
        }
        [HttpPost]
        public RedirectResult DeleteAuthor(int Id, bool doing)
        {            
            ViewBag.Author = db.Authors.Find(Id);
            db.Entry(ViewBag.Author).State = EntityState.Deleted;
            db.SaveChanges();
            //return RedirectToAction("home\AllBooks");
            //return "Данные автора изменены " + author.Name + " ,  " + author.Age + " ,  " + author.Id + ", Поздравляем!";
            //return View("AllAuthors");
            return RedirectPermanent("/Home/AllAuthors");
        }
       
    }
}