using Libanon.Models;
using Libanon.Repository;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Libanon.Controllers
{
    public class BooksController : Controller
    {
        

        readonly IBookRepository BookRepository;
        readonly IUserRepository UserRepository;

        public BooksController(IBookRepository BookRepository, IUserRepository UserRepository)
        {
            this.BookRepository = BookRepository;
            this.UserRepository = UserRepository;
        }
        public ActionResult Index()
        {
            List<Book> ListBooks = BookRepository.GetAll().ToList();
            return View(ListBooks);
        }
        [HttpPost]
        public ActionResult Index(string UserName)
        {
            List<Book> ListBooks = BookRepository.Search(UserName).ToList<Book>();
            return View(ListBooks);
        }

        public ActionResult Create()
        {
            ViewData["Owner"] = new User();
            return View();
        }

        [HttpPost]
        public ActionResult Create(Book Book, User Owner)
        {
            Book.State = BookState.Waiting;
            User TargetOwner = UserRepository.Get(Owner);
            if (TargetOwner != null)
            {
                Book.CurrentOwnerId = TargetOwner.UserId;
                BookRepository.Add(Book);
            }
            else
            {
                UserRepository.AddOwner(Owner, Book);
            }

            string title = "Bạn đã thêm một quyển sách";
            string mailbody = "Xin chào " + Owner.Name;
            mailbody += "<br /><br />Bạn đã thêm thành công một quyển sách tên: " + Book.Title;
            mailbody += "<br /><br />Click vào link bên dưới nếu quyển sách của bạn đã sẵn sàng trên kệ.";
            mailbody += "<br /><a href = '" + string.Format($"{Request.Url.Scheme}://{Request.Url.Authority}/Books/ConfirmBookshelf/{Book.BookId}") + "'>Click vào đây nếu bạn đã sẵn sàng.</a>";
            BookRepository.SendEmail(Owner.Email,title, mailbody);
            
            return View("Index");
        }

        public ActionResult ConfirmBookshelf(int Id)
        {
            Book Book = BookRepository.Get(Id);
            if (Book.State != BookState.BookShelf)
            {   
                Book.State = BookState.BookShelf; 
                BookRepository.Update(Book);
            }
            return View(Book);
        }

        public ActionResult Edit(int id)
        {
            Book Book = BookRepository.Get(id);
            
            return View(Book);
        }
        [HttpPost]
        public ActionResult Edit(Book Book)
        {
            BookRepository.Update(Book);
            return RedirectToAction("Index");
        }

        public ActionResult RequireBorrow(int id)
        {
            Book Book = BookRepository.Get(id);
            
            ViewData["Borrower"] = new User();
            return View(Book);
        }
        [HttpPost]
        public ActionResult RequireBorrow(Book Book, User Borrower)
        {
            User TargetUser = UserRepository.Get(Borrower);
            if(TargetUser == null)
            {
                UserRepository.AddBorrower(Borrower);
            }

            //string title = "Bạn đã mượn quyển sách";
            //string mailbody = "Xin chào " + Borrower.Name;
            //mailbody += "<br /><br />Bạn đã mượn một quyển sách tên: " + Book.Title;
            //mailbody += "<br /><br />Click vào link bên dưới nếu bạn thật sự muốn mượn.";
            //mailbody += "<br /><a href = '" + string.Format($"{Request.Url.Scheme}://{Request.Url.Authority}/Books/ConfirmBookshelf/{Book.BookId}") + "'>Click vào đây nếu bạn đã sẵn sàng.</a>";
            //BookRepository.SendEmail(Owner.Email, title, mailbody);

            return RedirectToAction("Index");
        }

        public ActionResult MailRequireBorrower()
        {
            
            return View();
        }

    }
}