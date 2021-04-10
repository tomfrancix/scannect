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
    public class KeyWordVisitor
    {
        public static Item GetKeywords(IDocument document, Item message)
        {
            var listOfKeyWords = new List<string>();

            var h1s = document.GetElementsByTagName("h1");
            listOfKeyWords = h1s.Select(h1 => h1.TextContent).ToList();

            var h2s = document.GetElementsByTagName("h2");
            listOfKeyWords.AddRange(h2s.Select(h2 => h2.TextContent).ToList());

            var h3s = document.GetElementsByTagName("h3");
            listOfKeyWords.AddRange(h3s.Select(h3 => h3.TextContent).ToList());

            var h4s = document.GetElementsByTagName("h4");
            listOfKeyWords.AddRange(h4s.Select(h4 => h4.TextContent).ToList());

            var h5s = document.GetElementsByTagName("h5");
            listOfKeyWords.AddRange(h5s.Select(h5 => h5.TextContent).ToList());
            
            var h6s = document.GetElementsByTagName("h6");
            listOfKeyWords.AddRange(h6s.Select(h6 => h6.TextContent).ToList());

            var strongs = document.GetElementsByTagName("strong");
            listOfKeyWords.AddRange(strongs.Select(strong => strong.TextContent).ToList());

            var bs = document.GetElementsByTagName("b");
            listOfKeyWords.AddRange(bs.Select(b => b.TextContent).ToList());

            var listOfTags = new List<Tag>();
            var ranking = 0;
            foreach (var phrase in listOfKeyWords)
            {
                if (ranking == 20) break;

                if (!string.IsNullOrEmpty(phrase))
                {
                    var tag = new Tag
                    {
                        Title = phrase, 
                        Hits = "0", 
                        Ranking = ranking.ToString()
                    };
                    listOfTags.Add(tag);

                    ranking++;
                }
            }

            message.Tags = listOfTags;

            return message;
        }
    }
}
