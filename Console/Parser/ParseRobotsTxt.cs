using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AngleSharp;

namespace ScannectConsole.Parser
{
    public class ParseRobotsTxt
    {
        /// <summary>
        /// This method parses the robots.txt file for any webpage.
        /// </summary>
        /// <param name="html"> The robots.txt text</param>
        /// <param name="latterUrl"> The latter part of the URL (after domain).</param>
        /// <returns></returns>
        public static async Task<bool> Execute(string html, string latterUrl)
        {
            // Set up Anglesharp parsing
            var config = Configuration.Default;
            var context = BrowsingContext.New(config);

            // Open HTML as a DOM document.
            var document = await context.OpenAsync(req => req.Content(html));

            // Get the body tag.
            var body = document.GetElementsByTagName("body")[0];

            // Only continue if the body actually contains something.
            if (body?.TextContent == null)
            {
                Console.WriteLine("This websites 'robots.txt' file is empty... continuing...");
                return true;
            }

            // Get the full text.
            var fullText = body.TextContent;

            // Divided the text into an array by '#'s.
            var fullArray = fullText.Split("#").ToList();

            // Loop through items that contain 'User-agent: *', split them by ':',
            // get the 'Disallow' property, and see if it contains '/' or our latterUrl.
            var allowed = false;

            allowed = !(from item in fullArray
                where item.Contains("User-agent: *")
                from i in item.Split(":")
                where i.Contains("Disallow")
                where (i + 1).Contains("/")
                select i).Any(i => (i + 1).Contains(latterUrl));


            if (!fullArray.Any(item => item.Contains(latterUrl)))
            {
                return true;
            }


            // If we found the URL in robots.txt, we can't scrape it!
            Console.WriteLine("This URL is not permitted for scraping.");
            return false;
        }
    }
}
