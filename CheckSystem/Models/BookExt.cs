using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CheckSystem.Models
{
    public class BookExt
    {
        // ID книги
        public int Id { get; set; }
        // название книги
        public string Name { get; set; }
        // цена
        public int Price { get; set; }
        // автор книги
        public string Author { get; set; }        
    }
}