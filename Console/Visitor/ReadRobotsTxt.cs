using AngleSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ScannectConsole.Parser;
using ScannectConsole.Repository;

namespace ScannectConsole.Visitor
{
    public class RobotsVisitor
    {
        /// <summary>
        /// This hits the robots.txt file and checks for |  User-agent: *
        ///                                              |  Disallow:
        /// </summary>
        /// <param name="url"> The URL to hit.</param>
        /// <returns></returns>
        public static async Task<bool> AmAllowed(string url)
        {
            // We only want WWW. results.
            var domain = "";
            var html = "";

            // If URL has http(s):// then remove it!
            if (url.Contains("http"))
            {
                if (url.IndexOf("://", StringComparison.Ordinal) + 3 >= 0 &&
                    url.Length > url.IndexOf("://", StringComparison.Ordinal) + 3)
                    url = url.Substring(url.IndexOf("://", StringComparison.Ordinal) + 3);
            }

            if (CheckRepository.ItemExistsByUrl(url)) return false;

            // This should leave us with 'www.domain.co.uk'...
            domain += url.Split("/")[0];

            // Get the other part of the URL
            var postUrl = domain;
            try
            {
                var i = url.IndexOf(url.Split("/")[0], StringComparison.Ordinal) + domain.Length;
                if (i >= 0 && url.Length > i) postUrl = url.Substring(i);
            }
            catch
            {
                Console.WriteLine("...");
            }

            if (postUrl.Contains("http"))
            {
                postUrl = postUrl.Split("http")[0];
            }

            // Create the URL for robots.txt
            var robotsTxt = domain + "/robots.txt";

            Console.WriteLine("Checking " + robotsTxt + " file to see if its okay to scrape this URL...");

            // Create the complicated URI crap for the request.
            var server = "http://" + domain;
            var relativePath = "robots.txt";
            var serverUri = new Uri(server);
            var relativeUri = new Uri(relativePath, UriKind.Relative);
            var fullUri = new Uri(serverUri, relativeUri);

            // Wait 1 second
            Thread.Sleep(1000);

            // Create request object.
            var robotsRequest = (HttpWebRequest) WebRequest.Create(fullUri);
            robotsRequest.Method = "GET";

            // Get the response object
            var robotsResponse = robotsRequest.GetResponse();

            // Read the response.
            var reader =
                new StreamReader(robotsResponse.GetResponseStream() ?? throw new InvalidOperationException());
            html = reader.ReadToEnd();
            reader.Close();
            robotsResponse.Close();

            // If the robots.txt does not exist...
            if (string.IsNullOrEmpty(html))
            {
                Console.WriteLine("No response from '" + robotsTxt + "'...");
                return true;
            }

            // If we have a robots.txt file...
            return ParseRobotsTxt.Execute(html, postUrl).Result;

        }
    }
}
