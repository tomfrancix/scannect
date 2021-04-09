using System;
using System.Collections.Generic;
using System.Linq;
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
            var items = context.Items.Where(i => i.Title.Contains(input)).ToListAsync().Result;

            return items;
        }
    }
}
