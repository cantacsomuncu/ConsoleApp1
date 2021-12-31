using ConsoleApp1.Model;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace ConsoleApp1
{
    class Program
    {

        static void Main(string[] args)
        {
            ParseHtmlSplitTables();
        }


        public static void ParseHtmlSplitTables()
        {
            ContextDb contextDb = new ContextDb();

            //List<string> announcements = new List<string>();

            string[] result = new string[] { };
            WebClient webClient = new WebClient();
            string page = webClient.DownloadString("https://www.osym.gov.tr/TR,20810/2021.html");

            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(page);  


            foreach (HtmlNode table in doc.DocumentNode.SelectNodes("//*[@id=\"list\"]"))
            {
                foreach (HtmlNode row in table.SelectNodes("tr"))
                {
                    foreach (HtmlNode cell in row.SelectNodes("th|td"))
                    {
                        //announcements.Add(cell.InnerText.TrimStart().Replace("\r", "").Replace("\n", "").TrimEnd());
                        var duyuru = cell.InnerText.TrimStart().Replace("\r", "").Replace("\n", "").TrimEnd();
                        var duyurular = contextDb.Announcements.ToList();

                        var checkExist = duyurular.FirstOrDefault(x => x.Description == duyuru);

                        if (checkExist != null)
                            continue;

                        contextDb.Add(new Announcement()
                        {
                            Description = duyuru
                        });
                    }

                }
                contextDb.SaveChanges();
            }

        }    
    }
}
