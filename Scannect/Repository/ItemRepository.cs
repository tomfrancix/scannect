﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Scannect.Models;

namespace Scannect.Repository
{
    public class ItemRepository
    {

        public static  List<Item> GetSearchResults(string input, ScannectContext context)
        {
            var inputList = input.ToLower().Split(" ");
            var compoundInputList = new List<string>();
            var wordsInList = "";
            // Get all the combinations of input words.
            foreach (var word in inputList)
            {
                
                compoundInputList.Add(word);
                wordsInList += " " + word;
                compoundInputList.Add(wordsInList);
                foreach (var otherWord in inputList)
                {
                    if (otherWord != word)
                    {
                        var phrase = word + " " + otherWord;

                        if (!compoundInputList.Contains(phrase))
                        {
                            compoundInputList.Add(phrase);
                        }
                    }
                }
            }

            // Sort combination phrases in order of length.
            var phrases = compoundInputList.OrderBy(x => x.Length).Reverse().Take(10);

            var results = new List<Item>();
            foreach (var phrase in phrases)
            {
                var items = context.Items
                    .Where(i => i.Title.ToLower().Contains(phrase) || 
                                i.Snippet.ToLower().Contains(phrase) || 
                                i.Tags.Any(s => s.Title.ToLower().Contains(phrase)))
                    .Include(i => i.Images)
                    .Include(t => t.Tags).Where(t => t.Title.ToLower().Contains(phrase))
                    .ToListAsync().Result;

                foreach (var item in items.Where(item => !results.Contains(item)))
                {
                    results.Add(item);
                }
            }

            return results;
        }
    }
}
