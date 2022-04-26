using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Libanon.Models
{
    public enum EmailUser
    {
        Owner,
        Borrower
    }
    public class User
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }

        public virtual ICollection<Book> MyBooks { get; set; }
        public virtual ICollection<Book> BorrowBooks { get; set; }
    }
}