using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WikiQuote.Models
{
    public class Quote
    {
        public int ID { get; set; }
        public string Content { get; set; }
        public string Quotee { get; set; }
        public DateTime Date { get; set; }
    }
}