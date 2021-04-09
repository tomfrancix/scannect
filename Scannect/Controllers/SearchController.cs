using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Routing;
using Newtonsoft.Json;
using Scannect.Models;
using Scannect.Repository;

namespace Scannect.Controllers
{
    public class SearchController : Controller
    {
        private readonly ScannectContext _context;

        public SearchController(ScannectContext context)
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult Execute(string input)
        { 
      
            return RedirectToAction("Index", "Item");
        }
    }
}
