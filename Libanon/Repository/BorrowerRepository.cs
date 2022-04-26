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
        public BorrowerTemp Add(BorrowerTemp BorrowerTemp)
        {
            if (BorrowerTemp == null)
            {
                throw new ArgumentNullException("NewBook");
            }

            _DbContext.Borrowers.Add(BorrowerTemp);
            _DbContext.SaveChanges();

            BorrowerTemp TargetBorrower = _DbContext.Borrowers.
               Where<BorrowerTemp>(u => u.Name == BorrowerTemp.Name && u.Email == BorrowerTemp.Email && u.Phone == BorrowerTemp.Phone).
               FirstOrDefault();

            return BorrowerTemp;
        }

        public bool Delete(int Id)
        {
            BorrowerTemp BorrowerTemp = _DbContext.Borrowers.Where<BorrowerTemp>(b => b.BorrowerTempId == Id).
                FirstOrDefault();
            _DbContext.Borrowers.Remove(BorrowerTemp);
            _DbContext.SaveChanges();
            return true;
        }

        public BorrowerTemp Get(int Id)
        {
            BorrowerTemp BorrowerTemp = _DbContext.Borrowers.Where<BorrowerTemp>(b => b.BorrowerTempId == Id).
                FirstOrDefault(); ;

            return BorrowerTemp;
        }

        public bool Update(BorrowerTemp BorrowerTemp)
        {
           
                if (BorrowerTemp == null)
                {
                    throw new ArgumentNullException("Book");
                }
                var NewBorrowerTemp = _DbContext.Borrowers.Where<BorrowerTemp>(s => s.BorrowerTempId == BorrowerTemp.BorrowerTempId).FirstOrDefault();

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