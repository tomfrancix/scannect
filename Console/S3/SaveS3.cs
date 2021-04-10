using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AngleSharp;
using Microsoft.VisualStudio.Web.CodeGeneration.Contracts.Messaging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Scannect.Models;
using ScannectConsole.Model;

namespace ScannectConsole.S3
{
    class SaveS3
    {

        /// <summary>
        /// This saves the new object in the S3 bucket - To be serialized as a scooter model.
        /// </summary>
        /// <param name="message"> The message object.</param>
        /// <param name="itemSourceName"> The name of the item source.</param>
        public static void SaveNewS3Object(Item message, string itemSourceName)
        {
            var result = new SearchResult
            {
                Url = message.Url,
                WebsiteUrl = message.WebsiteUrl,
                Title = message.Title,
                Snippet = message.Snippet,
                Category = message.Category,
                Author = message.Author,
                Images = message.Images,
                Tags = message.Tags
            };

            // Serialize the JObject into a string.
            var line = JsonConvert.SerializeObject(result);

            // Add the new line to the manifest file.
            var dateString = DateTime.UtcNow.ToString("o");
            var date = dateString.Substring(0,
                dateString.IndexOf(dateString.Substring(dateString.Length - 5), StringComparison.Ordinal));

            date = date.Replace("-", "").Replace(":", "");

            var filePath = "C:\\S3\\wd-nskater-results\\result-" + itemSourceName + "-" + date + ".json";

            File.AppendAllText(filePath, line + Environment.NewLine);

            Console.WriteLine("A new item was saved to the bucket!");
        }

        /// <summary>
        /// This saves the new link in the S3 bucket - To be serialized as a scooter model.
        /// </summary>
        /// <param name="link"> The URL.</param>
        public static void SaveNewS3Link(string link)
        {
            // Get the datetime string.
            var dateString = DateTime.UtcNow.ToString("o");
            var date = dateString.Substring(0,
                dateString.IndexOf(dateString.Substring(dateString.Length - 5), StringComparison.Ordinal));
            date = date.Replace("-", "").Replace(":", "");

            var rand = new Random();

            // This is the new file path for the link.
            var filePath = "C:\\S3\\wd-nskater-links\\" + date + "-" + rand.Next(0, 100) + "-link-item.txt";

            File.AppendAllText(filePath, link);
        }
    }
}
