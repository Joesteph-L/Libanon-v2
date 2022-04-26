using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Libanon.Models
{
    public class BorrowerTemp
    {
        public int BorrowerTempId { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }

        public bool ConfirmBorrower { get; set; }

        public int? CurrentBookId { get; set; }
        public Book CurrentBook { get; set; }
    }
}