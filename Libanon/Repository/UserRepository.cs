using Libanon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Libanon.Repository
{
    public class UserRepository : IUserRepository
    {
        ManageDbContext _DbContext;
        public UserRepository()
        {
            _DbContext = new ManageDbContext();
        }

        public User AddBorrower(BorrowerTemp Borrower)
        {
            if (Borrower == null)
            {
                throw new ArgumentNullException("NewBook");
            }
            User NewBorrower = new User()
            {
                Name = Borrower.Name,
                Phone = Borrower.Phone,
                Email = Borrower.Email,
                Address = Borrower.Address,
            };

            _DbContext.Users.Add(NewBorrower);
            _DbContext.SaveChanges();
            return NewBorrower;
        }

        public User AddOwner(User NewOwner, Book NewBook)
        {
            if (NewOwner == null)
            {
                throw new ArgumentNullException("NewBook");
            }
            User NewUser = new User()
            {
                Name = NewOwner.Name,
                Phone = NewOwner.Phone,
                Email = NewOwner.Email,
                Address = NewOwner.Address,
                MyBooks = new List<Book>()
                    {
                        new Book()
                        {
                            Title = NewBook.Title,
                            Author = NewBook.Author,
                            ImageUrl = NewBook.ImageUrl,
                            ReleaseDate = NewBook.ReleaseDate,
                            State = NewBook.State,
                            CurrentISBN = NewBook.CurrentISBN,
                        }
                    }
            };
            _DbContext.Users.Add(NewUser);
            _DbContext.SaveChanges();
            return NewOwner;
        }

        public User Get(User User)
        {
            User TargetUser = _DbContext.Users.
                Where<User>(u => u.Name == User.Name && u.Email == User.Email && u.Phone == User.Phone).
                FirstOrDefault();

            return TargetUser;
        }

        public User Get(BorrowerTemp User)
        {
            User TargetUser = _DbContext.Users.
                Where<User>(u => u.Name == User.Name && u.Email == User.Email && u.Phone == User.Phone).
                FirstOrDefault();
            return TargetUser;
        }

        public User Get(int Id)
        {
            User TargetUser = _DbContext.Users.
                Where<User>(u => u.UserId == Id).
                FirstOrDefault();
            return TargetUser;
        }

        public IEnumerable<User> GetAll()
        {
            throw new NotImplementedException();
        }

        
    }
}