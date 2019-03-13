using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Web.Http;
using System.Web.Mvc;
using WikiQuote.Models;

namespace WikiQuote.Controllers.Api
{
    public class QuoteController : ApiController
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

        public Quote GetQuote()
        {
            Uri uri = new Uri("https://en.wikiquote.org/wiki/Main_Page");
            string html;
            Quote newQuote = new Quote();


            //has todays quote been recieved if so grab it from the db and serve from API else scrape the webpage.
            var yesterday = DateTime.Today.AddDays(-1);
            var today = _context.Quotes.Where(a => a.Date > yesterday).FirstOrDefault();

            if (today == null)
            { 
                //creates web client to retrieve page HTML
                using (WebClient client = new WebClient())
                {
                    html = client.DownloadString(uri);
                }

                //new up html document object
                var doc = new HtmlDocument();
                //loads document using html string
                doc.LoadHtml(html);

                //selects note where quote is
                var test = doc.DocumentNode.SelectNodes("//*//div/div/table/tbody/tr/td/table/tbody/tr/td/table/tbody");

                string quote = "";

                //appends html into string variable
                foreach (var item in test)
                {
                    quote = quote + item.InnerHtml.ToString();
                }


                //strips all html tags
                quote = Regex.Replace(quote, "<.*?>", string.Empty);

                //finds quotee and adds it into string
                Regex r = new Regex(@"~(.+?)~");
                MatchCollection mc = r.Matches(quote);

                string quotee = mc[0].Groups[1].Value;

                quote = Regex.Replace(quote, "~.*?~", string.Empty);



                // loads object with quote
                newQuote.Content = quote;
                newQuote.Quotee = quotee;
                newQuote.Date = DateTime.Now;

                //add in db if theres no additions today

                _context.Quotes.Add(newQuote);
                _context.SaveChanges();
            } else
            {
                newQuote.Content = today.Content;
                newQuote.Quotee = today.Quotee;
                newQuote.Date = today.Date;
            }




            return newQuote;
        }
        
    }
}
