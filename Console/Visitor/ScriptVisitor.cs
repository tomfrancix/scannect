using System.Linq;
using AngleSharp.Dom;
using Scannect.Models;

namespace ScannectConsole.Visitor
{
    public class ScriptVisitor
    {
        /// <summary>
        /// This visitor gets the scripts on the page and tries to append information to the message.
        /// </summary>
        /// <param name="document"> The HTML document.</param>
        /// <param name="composite"> The composite object</param>
        /// <returns></returns>
        public static Item GetJsonInfo(IDocument document, Item composite)
        {
            // Get all the script elements in the HTML
            var scripts = document.GetElementsByTagName("script");

            // Loop through each script.
            foreach (var script in scripts)
            {
                // Get the 'type' attribute.
                var type = script.GetAttribute("type");
                if (string.IsNullOrEmpty(type)) continue;

                // Only continue if the type is 'application/json'...
                if (!type.ToLower().Contains("json")) continue;

                // Get the serialized JSON string...
                var content = script.TextContent;

                // Split the string into a list.
                var properties = content.Split(',').ToList();

                // Loop through the list of properties.
                foreach (var property in properties)
                {
                    // Each list item should be divided into a key:value pair.
                    if (!property.Contains(":")) continue;

                    // Get the key...
                    var key = property.Split(':')[0].ToLower();

                    // Get the value...
                    var value = property.Split(':')[1];
                    value = value.Replace("\"", "").Replace("{", "").Replace("}", "");

                    // Append information to composite...
                    if (key.Contains("title"))
                    {
                        if (!string.IsNullOrEmpty(composite.Title)) continue;
                        composite.Title = value;
                        continue;
                    }

                    if (key.Contains("description"))
                    {
                        if (!string.IsNullOrEmpty(composite.Snippet)) continue;
                        composite.Snippet = value;
                        continue;
                    }
                }
            }

            return composite;
        }
    }
}
