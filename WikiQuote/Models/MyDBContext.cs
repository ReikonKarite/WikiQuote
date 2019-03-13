using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace WikiQuote.Models
{
    public class MyDBContext : DbContext
    {
        public MyDBContext()
        {

        }
        public DbSet<Quote> Quotes { get; set; } // My domain models
    }
}