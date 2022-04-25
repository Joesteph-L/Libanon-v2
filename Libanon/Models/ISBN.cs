using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Libanon.Models
{
    public class ISBN
    {
        public int ISBNId { get; set; }
        public string ISBNCode { get; set; }

        public virtual ICollection<Book> Books { get; set; }
        public virtual ICollection<Rate> Rates { get; set; }
    }
}