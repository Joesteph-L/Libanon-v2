using Libanon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Libanon.Repository
{
    public class BorrowerRepository : IBorrowerRepository
    {
        ManageDbContext _DbContext;
        public BorrowerRepository()
        {
            _DbContext = new ManageDbContext();
        }
        public Borrower Add(Borrower BorrowerTemp)
        {
            if (BorrowerTemp == null)
            {
                throw new ArgumentNullException("NewBook");
            }

            _DbContext.Borrowers.Add(BorrowerTemp);
            _DbContext.SaveChanges();

            Borrower TargetBorrower = _DbContext.Borrowers.
               Where<Borrower>(u => u.Name == BorrowerTemp.Name && u.Email == BorrowerTemp.Email && u.Phone == BorrowerTemp.Phone).
               FirstOrDefault();

            return BorrowerTemp;
        }

        public bool Delete(int Id)
        {
            Borrower BorrowerTemp = _DbContext.Borrowers.Where<Borrower>(b => b.BorrowerId == Id).
                FirstOrDefault();
            _DbContext.Borrowers.Remove(BorrowerTemp);
            _DbContext.SaveChanges();
            return true;
        }

        public Borrower Get(int Id)
        {
            Borrower BorrowerTemp = _DbContext.Borrowers.Where<Borrower>(b => b.BorrowerId == Id).
                FirstOrDefault(); ;

            return BorrowerTemp;
        }

        public bool Update(Borrower BorrowerTemp)
        {
           
                if (BorrowerTemp == null)
                {
                    throw new ArgumentNullException("Book");
                }
                var NewBorrowerTemp = _DbContext.Borrowers.Where<Borrower>(s => s.BorrowerId == BorrowerTemp.BorrowerId).FirstOrDefault();

                NewBorrowerTemp.Name = BorrowerTemp.Name;
                NewBorrowerTemp.Phone = BorrowerTemp.Phone;
                NewBorrowerTemp.Email = BorrowerTemp.Email;
                NewBorrowerTemp.Address = BorrowerTemp.Address;
                NewBorrowerTemp.ConfirmBorrower = BorrowerTemp.ConfirmBorrower;
                
                _DbContext.SaveChanges();
                return true;
           
        }
    }
}