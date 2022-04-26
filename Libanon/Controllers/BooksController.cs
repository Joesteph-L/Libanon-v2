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
        readonly IBorrowerRepository BorrowerRepository;

        public BooksController(IBookRepository BookRepository, IUserRepository UserRepository, BorrowerRepository BorrowerRepository)
        {
            this.BookRepository = BookRepository;
            this.UserRepository = UserRepository;
            this.BorrowerRepository = BorrowerRepository;
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
            if(Book.State != BookState.BookShelf)
            {
                return RedirectToAction("Index");
            }
            ViewData["Borrower"] = new BorrowerTemp();
            return View(Book);
        }
        [HttpPost]
        public ActionResult RequireBorrow(Book Book, BorrowerTemp Borrower)
        {

            Borrower.CurrentBookId = Book.BookId;

            
            BorrowerTemp TargetBorrower = BorrowerRepository.Add(Borrower);

            string title = "Bạn đã mượn sách";
            string mailbody = "Xin chào " + Borrower.Name;
            mailbody += "<br /><br />Bạn đã mượn một quyển sách tên: " + Book.Title;
            mailbody += "<br /><br />Click vào link bên dưới nếu bạn thật sự muốn mượn.";
            mailbody += "<br /><a href = '" + string.Format($"{Request.Url.Scheme}://{Request.Url.Authority}/Books/MailRequireBook?IdBorrower={TargetBorrower.BorrowerTempId}&IdBook={Book.BookId}") + "'>Click vào đây nếu bạn đã sẵn sàng.</a>";
            BookRepository.SendEmail(Borrower.Email, title, mailbody);

            User Owner = UserRepository.Get(Book.CurrentOwner);

            title = "Bạn có yêu cầu mượn sách";
            mailbody = "Xin chào " + Owner.Name;
            mailbody += "<br /><br />Bạn có yêu mượn một quyển sách tên: " + Book.Title;
            mailbody += "<br /><br />Click vào link bên dưới nếu bạn thật sự muốn cho mượn.";
            mailbody += "<br /><a href = '" + string.Format($"{Request.Url.Scheme}://{Request.Url.Authority}/Books/MailAcceptBook?IdBorrower={TargetBorrower.BorrowerTempId}&IdBook={Book.BookId}") + "'>Click vào đây nếu bạn đã sẵn sàng.</a>";
            BookRepository.SendEmail(Owner.Email, title, mailbody);

            return RedirectToAction("Index");
        }

        public ActionResult MailRequireBook(BorrowerTemp BorrowerTemp, Book Book)
        {

            //Book Book = BookRepository.Get(IdBook);
            //BorrowerTemp BorrowerTemp = BorrowerRepository.Get(IdBorrower);
            if(Book.ConfirmOwner == true)
            {
                User TargetBorrower = UserRepository.Get(BorrowerTemp);
                if(TargetBorrower == null)
                {
                    UserRepository.AddBorrower(BorrowerTemp);
                    TargetBorrower = UserRepository.Get(BorrowerTemp);
                }

                Book.State = BookState.Borrowed;
                Book.CurrentBorrowerId = TargetBorrower.UserId;
                BookRepository.Update(Book);
            }
            else
            {
                BorrowerTemp.ConfirmBorrower = true;
                BorrowerRepository.Update(BorrowerTemp);
            }


            
            return View("SuccessBook");
        }

        public ActionResult MailAcceptBook(BorrowerTemp BorrowerTemp, Book Book)
        {
            //Book Book = BookRepository.Get(IdBook);
            //BorrowerTemp BorrowerTemp = BorrowerRepository.Get(IdBorrower);

            if (BorrowerTemp.ConfirmBorrower == true)
            {
                User TargetBorrower = UserRepository.Get(BorrowerTemp);
                if (TargetBorrower == null)
                {
                    UserRepository.AddBorrower(BorrowerTemp);
                    TargetBorrower = UserRepository.Get(BorrowerTemp);
                }

                Book.State = BookState.Borrowed;
                Book.CurrentBorrowerId = TargetBorrower.UserId;
                BookRepository.Update(Book);
            }
            else
            {
                Book.ConfirmOwner = true;
                BookRepository.Update(Book);
            }

            return View("SuccessBook");
        }

        public ActionResult MailRejectBook(int IdBorrower)
        {
            BorrowerRepository.Delete(IdBorrower);
            return View("SuccessBook");
        }



    }
}