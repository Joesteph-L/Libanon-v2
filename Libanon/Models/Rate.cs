using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Libanon.Models
{
    public class Rate
    {
        public int RateId { get; set; }
        public int RateValue { get; set; }

        public int CurrentISBNId { get; set; }
        public virtual ISBN CurrentISBN { get; set; }
    }
}