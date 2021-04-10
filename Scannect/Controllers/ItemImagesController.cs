using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Scannect.Models;

namespace Scannect.Controllers
{
    public class ItemImagesController : Controller
    {
        private readonly ScannectContext _context;

        public ItemImagesController(ScannectContext context)
        {
            _context = context;
        }

        // GET: ItemImages
        public async Task<IActionResult> Index()
        {
            var scannectContext = _context.ItemImages.Include(i => i.Item);
            return View(await scannectContext.ToListAsync());
        }

        // GET: ItemImages/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var itemImage = await _context.ItemImages
                .Include(i => i.Item)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (itemImage == null)
            {
                return NotFound();
            }

            return View(itemImage);
        }

        // GET: ItemImages/Create
        public IActionResult Create()
        {
            ViewData["ItemId"] = new SelectList(_context.Items, "Id", "Id");
            return View();
        }

        // POST: ItemImages/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Url,Alt,Title,Annotation,Placeholder,Width,Height,ItemId")] ItemImage itemImage)
        {
            if (ModelState.IsValid)
            {
                _context.Add(itemImage);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ItemId"] = new SelectList(_context.Items, "Id", "Id", itemImage.ItemId);
            return View(itemImage);
        }

        // GET: ItemImages/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var itemImage = await _context.ItemImages.FindAsync(id);
            if (itemImage == null)
            {
                return NotFound();
            }
            ViewData["ItemId"] = new SelectList(_context.Items, "Id", "Id", itemImage.ItemId);
            return View(itemImage);
        }

        // POST: ItemImages/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Url,Alt,Title,Annotation,Placeholder,Width,Height,ItemId")] ItemImage itemImage)
        {
            if (id != itemImage.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(itemImage);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ItemImageExists(itemImage.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ItemId"] = new SelectList(_context.Items, "Id", "Id", itemImage.ItemId);
            return View(itemImage);
        }

        // GET: ItemImages/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var itemImage = await _context.ItemImages
                .Include(i => i.Item)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (itemImage == null)
            {
                return NotFound();
            }

            return View(itemImage);
        }

        // POST: ItemImages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var itemImage = await _context.ItemImages.FindAsync(id);
            _context.ItemImages.Remove(itemImage);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ItemImageExists(int id)
        {
            return _context.ItemImages.Any(e => e.Id == id);
        }
    }
}
