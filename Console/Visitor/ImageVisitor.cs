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
    public class ImageVisitor
    {
        public static Item GetImages(IDocument document, Item message)
        {
            var images = document.GetElementsByTagName("img");
            var listOfImages = new List<ItemImage>();

            foreach (var image in images)
            {
                if (!string.IsNullOrEmpty(image.GetAttribute("alt")))
                {
                    if (!string.IsNullOrEmpty(image.GetAttribute("src")))
                    {
                        var src = image.GetAttribute("src");
                        if (!src.Contains("http") || !src.Contains(message.WebsiteUrl))
                        {
                            if (!src.StartsWith("/"))
                            {
                                src = "/" + src;
                            }

                            src = message.WebsiteUrl + src;
                        }
                        var img = new ItemImage
                        {
                            Url = image.GetAttribute("src"),
                            Alt = image.GetAttribute("alt"),
                            Title = image.GetAttribute("title"),
                            Annotation = null,
                            Placeholder = null,
                            Width = image.GetAttribute("width"),
                            Height = image.GetAttribute("height"),
                            ItemId = message.Id
                        };
                        listOfImages.Add(img);
                    }
                }
            }

            message.Images = listOfImages;

            return message;
        }
    }
}
