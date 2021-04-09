using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AngleSharp.Dom;
using Microsoft.VisualStudio.Web.CodeGeneration.Contracts.Messaging;
using Scannect.Models;

namespace ScannectConsole.Visitor
{
    public class ScriptVisitor
    {
        public static Item GetJsonInfo(IDocument document, Item message)
        {
            var scripts = document.GetElementsByTagName("script");

            foreach (var script in scripts)
            {
                var type = script.GetAttribute("type");
                if (string.IsNullOrEmpty(type)) continue;
                if (!type.ToLower().Contains("json")) continue;

                var content = script.TextContent;

                var properties = content.Split(',').ToList();

                foreach (var property in properties)
                {
                    if (!property.Contains(":")) continue;

                    var key = property.Split(':')[0].ToLower();

                    var value = property.Split(':')[1];
                    value = value.Replace("\"", "").Replace("{", "").Replace("}", "");

                    if (key.Contains("title"))
                    {
                        if (!string.IsNullOrEmpty(message.Title)) continue;
                        message.Title = value;
                        continue;
                    }

                    if (key.Contains("description"))
                    {
                        if (!string.IsNullOrEmpty(message.Snippet)) continue;
                        message.Snippet = value;
                        continue;
                    }
                }
            }

            return message;
        }
    }
}
