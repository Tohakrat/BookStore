using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CheckSystem.Models
{
    public class Book
    {
        // ID книги
        public int Id { get; set; }
        // название книги
        public string Name { get; set; }        
        // цена
        public int Price { get; set; }
        // автор книги
        public int? AuthorId { get; set; }
        public Author Author { get; set; }
    }
    

}