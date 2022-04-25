using Libanon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace Libanon.Repository
{
    public class BookRepository : IBookRepository
    {
        ManageDbContext _DbContext;
        public BookRepository()
        {
            _DbContext = new ManageDbContext();
        }

        public IEnumerable<Book> GetAll()
        {
            List<Book> ListBooks = _DbContext.Books.ToList();
            return ListBooks;
        }

        public Book Add(Book NewBook)
        {
            if(NewBook == null)
            {
                throw new ArgumentNullException("NewBook");
            }
            _DbContext.Books.Add(NewBook);
            _DbContext.SaveChanges();

            return NewBook;
        }



        public Book Get(int id)
        {
            return _DbContext.Books.Where(s => s.BookId == id).FirstOrDefault();
        }



        public bool Update(Book TargetBook)
        {
            if (TargetBook == null)
            {
                throw new ArgumentNullException("Book");
            }
            var BookUpdate = _DbContext.Books.Where(s => s.BookId == TargetBook.BookId).FirstOrDefault();

            BookUpdate.Title = TargetBook.Title;
            BookUpdate.ImageUrl = TargetBook.ImageUrl;
            BookUpdate.Author = TargetBook.Author;
            BookUpdate.ReleaseDate = TargetBook.ReleaseDate;
            BookUpdate.State = TargetBook.State;
            BookUpdate.CurrentISBN.ISBNCode = TargetBook.CurrentISBN.ISBNCode;
            BookUpdate.CurrentBorrowerId = TargetBook.CurrentBorrowerId;
            _DbContext.SaveChanges();
            return true;
        }

        

        public IEnumerable<Book> Search(string UserName)
        {
            if (UserName == null)
            {
                throw new ArgumentNullException("UserName");
            }

            List<Book> ListBooks = _DbContext.Books.Where(b => b.CurrentOwner.Name == UserName).ToList();
            return ListBooks;
        }

        public void SendEmail(string Email, string EmailTitle, string EmailContent)
        {
            var FromMail = new MailAddress("libanon.ln@gmail.com");
            var FromEmailPassword = "Nguyen001";
            var ToEmail = new MailAddress(Email);
            var EmailSubject = EmailTitle;
            var EmailBody = EmailContent;
            var Message = new MailMessage(FromMail.Address, ToEmail.Address, EmailSubject, EmailBody);
            Message.IsBodyHtml = true;

            var smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com";
            smtp.Port = 587;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new NetworkCredential(FromMail.Address, FromEmailPassword);
            smtp.EnableSsl = true;
            smtp.Send(Message);

        }
    }
}