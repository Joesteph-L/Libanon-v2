using Libanon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Libanon.Repository
{
    public interface IBookRepository
    {
        IEnumerable<Book> GetAll();
        Book Get(int Id);
        Book Add(Book NewBook);

        IEnumerable<Book> Search(string UserName);
        bool Update(Book TargetBook);

        void SendEmail(string EmailId, string EmailTitle, string EmailContent);
    }
}
