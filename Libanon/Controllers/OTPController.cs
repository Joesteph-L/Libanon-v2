using Libanon.Repository;
using Libanon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Libanon.Controllers
{
    public class OTPController : Controller
    {
        readonly IBookRepository BookRepository;
        readonly IUserRepository UserRepository;

        public OTPController(IBookRepository BookRepository, IUserRepository UserRepository)
        {
            this.BookRepository = BookRepository;
            this.UserRepository = UserRepository;
        }

        string[] SaAllowedCharacters = { "1", "2", "3", "4", "5", "6", "7", "8", "9", "0" };
        public string GenerateOTP(int OTPLength, string[] SaAllowedCharacters)
        {
            string OTP = String.Empty;

            string TempChars = String.Empty;

            Random rand = new Random();

            for (int i = 0; i < OTPLength; i++)

            {

                int p = rand.Next(0, SaAllowedCharacters.Length);

                TempChars = SaAllowedCharacters[p];

                OTP += TempChars;

            }

            return OTP;
        }
        string RandomOTP;


        public ActionResult Index(int Id)
        {
            ViewBag.IdBook = Id;
            TempData["OTP"] = CreateOTP(Id);
            return View();
        }
        [HttpPost]
        public ActionResult Index(string OTP, int Id)
        {
            string RandomOTP = TempData["OTP"].ToString();
            if(OTP == RandomOTP)
            {
                return RedirectToAction("Edit/"+Id,"Books");
            }
            return RedirectToAction("Index");
        }
        public string CreateOTP(int Id)
        {
            RandomOTP = GenerateOTP(7, SaAllowedCharacters);
            SendOTP(Id, RandomOTP);
            return RandomOTP;
        }
        public void SendOTP(int Id, string OTP)
        {
            Book Book = BookRepository.Get(Id);
            string title = "Mã xác nhận quyền Update sách tại Libanon";
            string mailbody = "Xin chào " + Book.CurrentOwner.Name;
            mailbody += "<br /><br />Mã OTP của bạn là: " + OTP;
            mailbody += "<br /><br />Xác nhận OTP để có thể update sách tại Libanon.";
           
            BookRepository.SendEmail(Book.CurrentOwner.Email, title, mailbody);
        }
    }


}