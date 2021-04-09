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
        public static Item GetMetaInfo(IDocument document, Item message)
        {
            var metas = document.GetElementsByTagName("meta");


            foreach (var meta in metas)
            {
                var property = meta.GetAttribute("property");
                if (string.IsNullOrEmpty(property))
                {
                    property = meta.GetAttribute("name");
                }

                var content = meta.GetAttribute("content");
                if (string.IsNullOrEmpty(content))
                {
                    content = meta.GetAttribute("value");
                }

                if (string.IsNullOrEmpty(property)) continue;
                if (string.IsNullOrEmpty(content)) continue;

                property = property.ToLower();
                content = content.ToLower();

                if (property.Contains("title"))
                {
                    if (!string.IsNullOrEmpty(message.Title)) continue;
                    message.Title = content;
                    continue;
                }

                if (property.Contains("description"))
                {
                    if (!string.IsNullOrEmpty(message.Snippet)) continue;
                    message.Snippet = content;
                    continue;
                }
            }

            return message;
        }
    }
}
