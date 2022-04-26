using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Libanon.Controllers
{
    public class OTPController : Controller
    {
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
        public ActionResult Index()
        {
            string RandomOTP = GenerateOTP(0, SaAllowedCharacters);


            return View();
        }
    }
}