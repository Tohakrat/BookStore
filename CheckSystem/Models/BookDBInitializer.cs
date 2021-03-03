using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace CheckSystem.Models
{
    public class BookDbInitializer : CreateDatabaseIfNotExists<BookContext>
    {
        protected override void Seed(BookContext db)
        {
            db.Books.Add(new Book { Name = "Война и мир", Author = db.Authors.Find(0), Price = 220 });
            db.Books.Add(new Book { Name = "Отцы и дети", Author = db.Authors.Find(1), Price = 180 });
            db.Books.Add(new Book { Name = "Чайка", Author = db.Authors.Find(2), Price = 150 });

            db.Purchases.Add(new Purchase { Person = "Ленин", Date = DateTime.Now,  Address = "Горького 5" ,BookId=1});
            db.Purchases.Add(new Purchase { Person = "Stalin", Date = DateTime.Now, Address = "Odoevskogo 5", BookId = 2 });
            db.Purchases.Add(new Purchase { Person = "Hytler", Date = DateTime.Now, Address = "NordStrasse 5", BookId = 0 });

            db.Authors.Add(new Author { Id = 1, Name = "Pushkin", Age = 38 });
            db.Authors.Add(new Author { Id = 2, Name = "lermontov", Age = 39 });
            db.Authors.Add(new Author { Id = 3, Name = "gogol", Age = 40 });
            db.Authors.Add(new Author { Id = 4, Name = "tolstoy", Age = 41 });


            base.Seed(db);
        }
    }
}