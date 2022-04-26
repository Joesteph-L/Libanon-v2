using Libanon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Libanon.Repository
{
    public interface IUserRepository
    {
        IEnumerable<User> GetAll();
        User Get(int Id);
        User Get(User User);
        User Get(BorrowerTemp Borrower);
        User AddOwner(User NewOwner, Book NewBook);
        User AddBorrower(BorrowerTemp Borrower);
        
    }
}
