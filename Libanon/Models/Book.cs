using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Libanon.Models
{
    public enum BookState { BookShelf, Waiting, Borrowed }
    public class Book
    {
        public int BookId { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string ImageUrl { get; set; }
        public DateTime ReleaseDate { get; set; }
        public BookState State { get; set; }
        
        public bool ConfirmOwner { get; set; }


        public virtual ICollection<BorrowerTemp> Borrowers { get; set; }

        public int CurrentISBNId { get; set; }
        public virtual ISBN CurrentISBN { get; set; }

        public int CurrentOwnerId { get; set; }
        public virtual User CurrentOwner { get; set; }

        public int? CurrentBorrowerId { get; set; }
        public virtual User CurrentBorrower { get; set; }
    }
}