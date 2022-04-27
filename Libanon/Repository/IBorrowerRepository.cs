using Libanon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Libanon.Repository
{
    public interface IBorrowerRepository
    {
        Borrower Get(int Id);
        Borrower Add(Borrower BorrowerTemp);
        bool Update(Borrower BorrowerTemp);
        bool Delete(int Id);

    }
}
