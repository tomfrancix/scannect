using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using AngleSharp.Dom;
using Scannect.Models;

namespace ScannectConsole.Visitor
{
    public class MetaVisitor
    {
        /// <summary>
        /// This method get the meta info from the page.
        /// </summary>
        /// <param name="document"> The HTML document.</param>
        /// <param name="composite"> The composite item.</param>
        /// <returns></returns>
        public static Item GetMetaInfo(IDocument document, Item composite)
        {
            // Get all the meta tags.
            var metas = document.GetElementsByTagName("meta");

            // Loop through them.
            foreach (var meta in metas)
            {
                // Get their property, or name.
                var property = meta.GetAttribute("property");
                if (string.IsNullOrEmpty(property))
                {
                    property = meta.GetAttribute("name");
                }

                // Get their content, or value.
                var content = meta.GetAttribute("content");
                if (string.IsNullOrEmpty(content))
                {
                    content = meta.GetAttribute("value");
                }

                // Don't continue without property AND value.
                if (string.IsNullOrEmpty(property)) continue;
                if (string.IsNullOrEmpty(content)) continue;

                property = property.ToLower();

                // Check for properties for composite.
                if (property.Contains("title"))
                {
                    if (!string.IsNullOrEmpty(composite.Title)) continue;
                    composite.Title = content;
                    continue;
                }

                if (property.Contains("description"))
                {
                    if (!string.IsNullOrEmpty(composite.Snippet)) continue;
                    composite.Snippet = content;
                    continue;
                }
            }

            // Get all the title tags.
            var titles = document.GetElementsByTagName("title");

            foreach (var title in titles)
            {
                composite.Title = title.TextContent;
                break;
            }

            // Get all the paragraph tags.
            var snippetString = composite.Snippet ?? "";
            var paragraphs = document.GetElementsByTagName("title");
            foreach (var paragraph in paragraphs)
            {
                if (snippetString.Length < 200)
                {
                    snippetString += paragraph.TextContent;
                }
                else
                {
                    break;
                }
            }

            return composite;
        }
    }
}
