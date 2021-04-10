using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using AngleSharp;
using Newtonsoft.Json;
using Scannect.Models;
using ScannectConsole.Model;
using ScannectConsole.Repository;
using TextToAsciiArt;

namespace Commander
{
    class Program
    {
        public static void Main(string[] args)
        {
            var writer = new ArtWriter();

            var data = writer.WriteString("VORTEX");

            Console.WriteLine(data);

            Execute();
        }

        /// <summary>
        /// Checks for Apify search result payloads periodically and process them.
        /// </summary>
        /// <param name="options">The options.</param>
        /// <returns></returns>
        public static void Execute()
        {
            var sourceBucket = "C:\\S3\\wd-nskater-results";
            var s3Objects = Directory.GetFiles(sourceBucket).ToList();
            var numberOfResultsProcess = s3Objects.Count();
            var delay = 0;
            do
            {
                if (numberOfResultsProcess == 0)
                {
                    Console.WriteLine("No results currently in bucket");
                    delay = 10000;
                }

                Console.WriteLine($"Sleeping 10 second(s)...");

                Thread.Sleep(delay);

                s3Objects = Directory.GetFiles(sourceBucket).ToList();
                numberOfResultsProcess = s3Objects.Count();

            } while (numberOfResultsProcess == 0);

            if (numberOfResultsProcess > 0)
            {
                Console.WriteLine(" - Found " + numberOfResultsProcess + " objects in S3 bucket.");
                ProcessResults(s3Objects);
            }
        }

        /// <summary>
        /// This downloads the HTML from the given URL.
        /// </summary>
        /// <param name="url"> The URL to scrape.</param>
        public static void ProcessResults(List<string> s3Objects)
        {
            foreach (var obj in s3Objects)
            {
                var textContent = "";
                try
                {
                    var readText = File.ReadAllLines(obj);
                    foreach (var text in readText)
                    {
                        textContent += text;
                    }
                }
                catch
                {
                    Console.WriteLine("Failed to read file...");
                }

                var searchResult = new SearchResult();

                try
                {
                    searchResult = JsonConvert.DeserializeObject<SearchResult>(textContent);
                }
                catch
                {
                    Console.WriteLine("Failed to deserialize the JSON file.");
                }

                if (!string.IsNullOrEmpty(searchResult.Url))
                {
                    var item = new Item
                    {
                        Url = searchResult.Url,
                        WebsiteUrl = searchResult.WebsiteUrl,
                        Title = searchResult.Title,
                        Snippet = searchResult.Snippet,
                        DateCreated = DateTime.UtcNow,
                        Hits = 0,
                        Ranking = 0,
                        Category = searchResult.Category,
                        Author = searchResult.Author,
                        Images = searchResult.Images,
                        Tags = searchResult.Tags
                    };

                    ResultRepository.SaveItem(item);
                }

                File.Delete(obj);
            }

            Execute();
        }
    }
}
