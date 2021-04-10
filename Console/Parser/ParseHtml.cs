using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AngleSharp;
using Scannect.Models;
using ScannectConsole.S3;
using ScannectConsole.Visitor;

namespace ScannectConsole.Parser
{
    public class ParseHtml
    {

        /// <summary>
        /// This parses the HTML we downloaded - Generically!
        /// </summary>
        /// <param name="html"> The downloaded HTML.</param>
        /// <param name="url"> The URL we downloaded it from.</param>
        public static async void Execute(string html, string url)
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

            // This will append information to the message object - if any is found!
            composite = ImageVisitor.GetImages(document, composite);

            try
            {
                composite.Snippet = document.GetElementsByTagName("p")[0].TextContent;
            }
            catch
            {
                Console.WriteLine("Failed to get snippet!");
            }


            SaveS3.SaveNewS3Object(composite, itemSourceName);

            LinkVisitor.GetNextUrl(document);
        }
    }
}
