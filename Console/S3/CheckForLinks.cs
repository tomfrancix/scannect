using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScannectConsole.Model;

namespace ScannectConsole.S3
{
    public class CheckForLinks
    {
        public static List<string> LinksInS3(List<string> listOfLinks)
        {
            // If its not set, check for links in the bucket.
            var sourceBucket = "C:\\S3\\wd-nskater-links";
            var s3Object = Directory.GetFiles(sourceBucket).ToList();

            // Go through all the links
            foreach (var obj in s3Object)
            {
                // We will delete this before we continue.
                var keep = false;

                // Read the link
                var linkLines = File.ReadAllLines(obj);
                foreach (var link in linkLines)
                {
                    // If the link is not on our list...
                    if (!listOfLinks.Contains(link))
                    {
                        // We will keep it!
                        keep = true;

                        // And add it to the list.
                        listOfLinks.Add(link);
                    }

                    // Break out after first line.
                    break;
                }

                // If we are not keeping it..
                if (!keep)
                {
                    // Delete it.
                    File.Delete(obj);
                }
            }

            return s3Object;
        }
    }
}
