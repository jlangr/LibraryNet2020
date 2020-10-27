using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibraryNet2020.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LibraryNet2020.Models;

namespace LibraryNet2020.Controllers
{
    public class PatronsController : Controller
    {
        private readonly LibraryContext _context;

        public PatronsController(LibraryContext context)
        {
            _context = context;
        }

        // GET: Patrons
        public async Task<IActionResult> Index()
        {
            return View(await _context.Patrons.ToListAsync());
        }

        // GET: Patrons/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            return this.ViewIf(await _context.Patrons.FindById(id));
        }

        // GET: Patrons/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Patrons/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Id,Balance")] Patron patron)
        {
            if (!ModelState.IsValid)
                return View(patron);
            
            {
                _context.Add(patron);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            // return View(patron);
        }

        // GET: Patrons/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            return this.ViewIf(await _context.Patrons.FindDirect(id));
        }

        // POST: Patrons/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Name,Id,Balance")] Patron patron)
        {
            if (id != patron.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(patron);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Patrons.Exists(patron.Id)) return NotFound();
                    throw;
                }
            }
            return View(patron);
        }

        // GET: Patrons/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            return this.ViewIf(await _context.Patrons.FindById(id));
        }

        // POST: Patrons/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            _context.Patrons.Delete(id, _context);
            return RedirectToAction(nameof(Index));
        }
    }
}
