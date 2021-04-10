using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ScannectConsole.Parser;

namespace ScannectConsole.Visitor
{
    public class DownloadVisitor
    {
        /// <summary>
        /// This downloads the HTML from the given URL.
        /// </summary>
        /// <param name="url"> The URL to scrape.</param>
        public static void DownloadHtml(string url)
        {
            var html = "";
            using (var client = new WebClient())
            {
                try
                {
                    Thread.Sleep(2000);

                    // Download the HTML.
                    html = client.DownloadString(url);
                }
                catch
                {
                    Console.WriteLine("This page was too long to parse.");
                    Program.StartTool("");
                }
            }

            if (!string.IsNullOrEmpty(html))
            {
                ParseHtml.Execute(html, url);
            }
        }
    }
}
