using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using AngleSharp;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.Web.CodeGeneration.Contracts.Messaging;
using Scannect.Models;
using Scannect.Repository;
using ScannectConsole.Repository;
using ScannectConsole.S3;
using ScannectConsole.Visitor;
using TextToAsciiArt;

namespace ScannectConsole
{
    public class Program
    {
        
        public static void Main(string[] args)
        {
            var writer = new ArtWriter();

            var data = writer.WriteString("NSKATER");

            Console.WriteLine(data);

            StartTool("");
        }

        /// <summary>
        /// The start command, which we return to at the end.
        /// </summary>
        public static void StartTool(string url)
        {
            while (true)
            {
                if (string.IsNullOrEmpty(url) || url == "")
                {
                    Console.WriteLine("Enter the URL you would like to scrape...");

                    url = Console.ReadLine();
                }

                if (!string.IsNullOrEmpty(url))
                {
                    if (CheckRepository.ItemExistsByUrl(url)) continue;

                    Console.WriteLine("Scraping URL: " + url);
                    DownloadHtml(url);
                }
                else
                {
                    continue;
                }

                break;
            }
        }

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
                    html = client.DownloadString(url);
                }
                catch
                {
                    Console.WriteLine("This page was too long to parse.");
                }
            }

            if (!string.IsNullOrEmpty(html))
            {
                ParseHtml(html, url);
            }
        }

        /// <summary>
        /// This parses the HTML we downloaded - Generically!
        /// </summary>
        /// <param name="html"> The downloaded HTML.</param>
        /// <param name="url"> The URL we downloaded it from.</param>
        public static async void ParseHtml(string html, string url)
        {
            var config = Configuration.Default;

            var context = BrowsingContext.New(config);

            var document = await context.OpenAsync(req => req.Content(html));

            Console.WriteLine("Parsing the source...");

            var composite = new Item();

            // Get the item URL
            composite.Url = url;

            // Get the item source name
            var itemSourceName = "";
            if (url.Contains("www."))
            {
                itemSourceName = url.Split("www.")[1].Split(".")[0];
            }
            else if (url.Contains("://"))
            {
                itemSourceName = url.Split("://")[1].Split(".")[0];
            }

            var webUrl = "www." + itemSourceName +
                         url.Substring(url.IndexOf(itemSourceName, StringComparison.Ordinal) + itemSourceName.Length).Split("/")[0];

            composite.WebsiteUrl = webUrl;

            // This will append information to the message object - if any is found!
            composite = MetaVisitor.GetMetaInfo(document, composite);

            // This will append information to the message object - if any is found!
            composite = ScriptVisitor.GetJsonInfo(document, composite);

            try
            {
                composite.Snippet = document.GetElementsByTagName("p")[0].TextContent;
            }
            catch
            {
                Console.WriteLine("Failed to get snippet!");
            }


            SaveS3.SaveNewS3Object(composite, itemSourceName);

            var nextUrl = LinkVisitor.GetNextUrl(document);

            if (string.IsNullOrEmpty(nextUrl))
            {
                nextUrl = "";
            }

            StartTool(nextUrl);

        }
    }
}
