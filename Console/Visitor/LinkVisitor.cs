using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using AngleSharp.Dom;
using Scannect.Controllers;
using Scannect.Models;
using Scannect.Repository;
using ScannectConsole.Repository;

namespace ScannectConsole.Visitor
{
    public class LinkVisitor
    {
        public static string GetNextUrl(IDocument document)
        {
            var links = document.GetElementsByTagName("a");
            var nextUrl = "";

            var urls = new List<string>();

            foreach (var a in links)
            {
                var href = a.GetAttribute("href");
                if (!string.IsNullOrEmpty(href))
                {
                    if (href.StartsWith("https://") || href.StartsWith("www."))
                    {
                        if (href.Length > 10)
                        {
                            if (!href.Contains("facebook.com"))
                            {
                                if (!CheckRepository.ItemExistsByUrl(href))
                                {
                                    urls.Add(href.Split("?")[0]);
                                }
                            }
                        }
                    }
                }
            }

            foreach (var url in urls)
            {
                Program.StartTool(url);
            }

            return nextUrl;
        }
    }
}
