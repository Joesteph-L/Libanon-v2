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

        public ActionResult Edit(int id)
        {
            Book Book = BookRepository.Get(id);

            return View(Book);
        }
        [HttpPost]
        public ActionResult Edit(Book Book)
        {

            return RedirectToAction("Check","OTP",Book);
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

        public ActionResult MailRequireBook(int IdBook, int IdBorrower)
        {

            Book Book = BookRepository.Get(IdBook);
            BorrowerTemp BorrowerTemp = BorrowerRepository.Get(IdBorrower);
            if (Book.ConfirmOwner == true)
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

        public ActionResult MailAcceptBook(int IdBook, int IdBorrower)
        {
            Book Book = BookRepository.Get(IdBook);
            BorrowerTemp BorrowerTemp = BorrowerRepository.Get(IdBorrower);

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

        public ActionResult ReturnBook(int Id)
        {
            Book Book = BookRepository.Get(Id);
            string title = "Yêu cầu trả sách";
            string mailbody = "Xin chào " + Book.CurrentOwner.Name;
            mailbody += "<br /><br />Sách của bạn sẽ được trả lại bởi: " + Book.CurrentBorrower.Name;
            mailbody += "<br /><br />Vui lòng liên hệ số: " + Book.CurrentBorrower.Phone ;
            mailbody += "<br /><br />Click vào link bên dưới nếu bạn đã nhận.";
            mailbody += "<br /><a href = '" + string.Format($"{Request.Url.Scheme}://{Request.Url.Authority}/Books/MailReturn?Id={Book.BookId}&Email=Owner") + "'>Click vào đây nếu bạn đã nhận được sách.</a>";
            BookRepository.SendEmail(Book.CurrentOwner.Email, title, mailbody);

            //User Owner = UserRepository.Get(Book.CurrentOwner);

            title = "Yêu cầu trả sách";
            mailbody = "Xin chào " + Book.CurrentBorrower.Name;
            mailbody += "<br /><br />Bạn  đã yêu cầu trả quyển sách: " + Book.Title;
            mailbody += "<br /><br />Dưới đây là thông tin người sở hưu sách.";
            mailbody += "<br /><br />Tên: " + Book.CurrentOwner.Name + "<br /><br />Sđt: " + Book.CurrentOwner.Phone + "<br /><br />Email: " + Book.CurrentOwner.Email;
            mailbody += "<br /><a href = '" + string.Format($"{Request.Url.Scheme}://{Request.Url.Authority}/Books/MailReturn?Id={Book.BookId}&Email=Borrower") + "'>Click vào đây nếu bạn đã trả sách.</a>";
            BookRepository.SendEmail(Book.CurrentBorrower.Email, title, mailbody);
            return RedirectToAction("Index");
        }

        public ActionResult MailReturn(int Id, EmailUser Email)
        {
            Book Book = BookRepository.Get(Id);
            if (Email == EmailUser.Borrower)
            {
                Book.State = BookState.Waiting;
                BookRepository.Update(Book);
            }
            else if (Email == EmailUser.Owner)
            {
                if (Book.State == BookState.Waiting)
                {
                    Book.State = BookState.BookShelf;
                    Book.ConfirmOwner = false;
                    Book.CurrentBorrowerId = null;
                    BookRepository.Update(Book);
                    return View("SuccessBook");
                }
                return View("VerifyEmail");
            }
            else
            {
                return View("Error");
            }
            return View("SuccessBook");
        }





    }
}