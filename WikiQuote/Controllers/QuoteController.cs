using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using WikiQuote.Models;

namespace WikiQuote.Controllers
{
    public class QuoteController : Controller
    {
        private MyDBContext _context;


        public QuoteController()
        {
            _context = new MyDBContext();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }


        // GET: Quote
        public ActionResult Index()
        {
            return Content("yes");
        }
        public ActionResult GetQuote()
        {
            Uri uri = new Uri("https://en.wikiquote.org/wiki/Main_Page");
            string html;

            using (WebClient client = new WebClient())
            {
                html = client.DownloadString(uri);
            }


            var doc = new HtmlDocument();
            doc.LoadHtml(html);

            var test = doc.DocumentNode.SelectNodes("//*//div/div/table/tbody/tr/td/table/tbody/tr/td/table/tbody");

            string quote = "";

            foreach (var item in test)
            {
                quote = quote + item.InnerHtml.ToString();
            }

            quote = Regex.Replace(quote, "<.*?>", string.Empty);

            Regex r = new Regex(@"~(.+?)~");
            MatchCollection mc = r.Matches(quote);

            string quotee = mc[0].Groups[1].Value;

            quote = Regex.Replace(quote, "~.*?~", string.Empty);

            Quote newQuote = new Quote();

            newQuote.Content = quote;
            newQuote.Quotee = quotee;
            newQuote.Date = DateTime.Now;

            _context.Quotes.Add(newQuote);
            _context.SaveChanges();





            return Content(quote + quotee);
        }
    }
}