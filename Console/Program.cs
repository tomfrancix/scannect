using System;
using System.Collections.Generic;
using System.Linq;
using ScannectConsole.S3;
using ScannectConsole.Visitor;
using TextToAsciiArt;

namespace ScannectConsole
{
    public class Program
    {
        private static List<string> listOfLinks = new List<string>();

        public static void Main(string[] args)
        {
            var writer = new ArtWriter();

            var data = writer.WriteString("SURFER");

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
                // Check if the url is set
                if (string.IsNullOrEmpty(url) || url == "")
                {
                    var s3Objects = CheckForLinks.LinksInS3(listOfLinks);

                    // If there is nothing in the bucket...
                    if (s3Objects.Count == 0)
                    {
                        // Get a seed URL from the user.
                        Console.WriteLine("Enter the URL you would like to scrape...");
                        listOfLinks = new List<string>();
                        url = Console.ReadLine();
                    }
                }

                // If we appended any URLs to the list...
                if (listOfLinks.Count > 0)
                {
                    // Loop through the list.
                    foreach (var link in listOfLinks.Where(link => !string.IsNullOrEmpty(link)))
                    {
                        try
                        {
                            if (!RobotsVisitor.AmAllowed(link).Result) continue;

                            // If we are allowed, go there and scrape it!
                            Console.WriteLine("Scraping URL: " + link);
                            DownloadVisitor.DownloadHtml(link);
                        }
                        catch
                        {
                            Console.WriteLine("Problem getting the robots.txt file.");
                        }
                    }
                }
                else // If the list of links is empty, we use the seed instead...
                {
                    // Do not continue without a URL.
                    if (string.IsNullOrEmpty(url)) return;

                    try
                    {
                        if (RobotsVisitor.AmAllowed(url).Result)
                        {
                            // If we are allowed, go there and scrape it!
                            Console.WriteLine("Scraping URL: " + url);
                            DownloadVisitor.DownloadHtml(url);
                        }
                    }
                    catch
                    {
                        Console.WriteLine("Problem getting the robots.txt file.");
                    }
                }

                url = "";
            }
        }
    }
}
