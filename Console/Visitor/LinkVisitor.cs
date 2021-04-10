using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using AngleSharp.Css.Dom;
using AngleSharp.Dom;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Scannect.Controllers;
using Scannect.Models;
using Scannect.Repository;
using ScannectConsole.Repository;
using ScannectConsole.S3;

namespace ScannectConsole.Visitor
{
    public class LinkVisitor
    {
        /// <summary>
        /// This method collects links from the page and saves them to S3 bucket.
        /// </summary>
        /// <param name="document"> The HTML document.</param>
        public static void GetNextUrl(IDocument document)
        {
            // Get all the anchor tags on the page.
            var links = document.GetElementsByTagName("a");

            // Initialise a new list of links.
            var urls = new List<string>();

            // Loop through the anchor tags.
            foreach (var a in links)
            {
                // Get their href.
                var href = a.GetAttribute("href");

                // TODO: Don't continue if the anchor element has visibility:hidden, or display:none! It's a trap!

                // Do not continue without an href.
                if (string.IsNullOrEmpty(href)) continue;

                // Only continue for regular links, not relative links ( for now ).
                if (!href.StartsWith("https://") && !href.StartsWith("www.")) continue;

                // Don't entertain short links.
                if (href.Length > 20)
                {
                    // If the link is NOT blacklisted...
                    if (!IsBlackListedWebsite(href))
                    {
                        // If the link is NOT already in the database...
                        if (!CheckRepository.ItemExistsByUrl(href))
                        {
                            // If the link has NOT already been added to the list...
                            if (!urls.Contains(href))
                            {
                                // Then add the URL.
                                urls.Add(href);
                            }
                        }
                    }
                }
            }

            foreach (var url in urls)
            {
                var tempUrl = url;
                if (url.Contains("http"))
                {
                    tempUrl = url.Substring(4);
                    if (tempUrl.Contains("http"))
                    {
                        tempUrl = tempUrl.Split("http")[0];
                    }

                    tempUrl = "http" + tempUrl;
                }

                if (IsSiteSpecificallySound(url))
                {
                    SaveS3.SaveNewS3Link(tempUrl);
                }
            }

            Console.WriteLine("Saved " + urls.Count + " links to S3 bucket.");
        }

        /// <summary>
        /// Checks to see if the link is blacklisted.
        /// </summary>
        /// <param name="href"></param>
        /// <returns></returns>
        public static bool IsBlackListedWebsite(string href)
        {
            // If its not set, check for links in the bucket.
            var sourceBucket = "C:\\S3\\wd-nskater-blacklisted";
            var s3Object = Directory.GetFiles(sourceBucket).ToList();

            // Go through all the links
            return (from obj 
                in s3Object 
                let keep = false 
                select File.ReadAllLines(obj))
                            .Any(linkLines => linkLines
                            .Any(link => href.Contains(link)));
        }

        public static bool IsSiteSpecificallySound(string url)
        {
            var blackListedWords = new List<string>()
            {
                "checkout",
                "basket",
                "payment",
                "cart",
                "sitemap"
            };

            foreach (var word in blackListedWords)
            {
                if (url.Contains(word))
                {
                    return false;
                }
            }

            var originalUrl = url;

            if (url.Contains("http://"))
            {
                url = url.Substring(7);
            }
            if (url.Contains("https://"))
            {
                url = url.Substring(8);
            }
            if (url.Contains("www."))
            {
                url = url.Substring(4);
            }
            if (url.Contains("/"))
            { 
                url = url.Substring(0, url.IndexOf("/", StringComparison.Ordinal));
            }

            var domain = url;
            if (url.Split(".").Length == 4 || url.Split(".").Length == 3)
            {
                domain = url.Split(".")[1];
            }
            if (url.Split(".").Length == 2)
            {
                domain = url.Split(".")[0];
            }


            switch (domain)
            {
                case "wikipedia":
                    if (!originalUrl.Contains("wikipedia.org/wiki/"))
                    {
                        return false;
                    }
                    if (!originalUrl.Contains("index.php"))
                    {
                        return false;
                    }
                    break;
                case "surveymonkey":
                case "d-nb":
                case "facebook":
                case "google":
                case "twitter":
                case "linkedin":
                case "pluralsight":
                case "udemy":
                case "gov":
                case "mediawiki":
                case "wikidata":
                case "loc":
                    return false;
                case "default":
                    return true;
            }

            return true;
        }
    }
}
