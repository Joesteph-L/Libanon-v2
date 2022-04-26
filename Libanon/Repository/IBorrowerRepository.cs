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
        BorrowerTemp Get(int Id);
        BorrowerTemp Add(BorrowerTemp BorrowerTemp);
        bool Update(BorrowerTemp BorrowerTemp);
        bool Delete(int Id);

    }
}
