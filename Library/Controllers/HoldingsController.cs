using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibraryNet2020.ControllerHelpers;
using LibraryNet2020.Extensions;
using LibraryNet2020.Models;
using LibraryNet2020.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryNet2020.Controllers
{
    public class HoldingsController : Controller
    {
        private readonly LibraryContext _context;

        public HoldingsController(LibraryContext context)
        {
            _context = context;
        }

        // GET: Holdings
        public async Task<IActionResult> Index()
        {
            var holdings = await _context.Holdings.ToListAsync();
            var holdingViewModels = holdings.Select(CreateHoldingViewModel);
            return View(holdingViewModels);
        }

        private HoldingViewModel CreateHoldingViewModel(Holding holding) =>
            new HoldingViewModel(holding)
            {
                BranchName = BranchesControllerUtil.BranchName(_context.Branches, holding.BranchId)
            };

        // GET: Holdings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            return this.ViewIf(await _context.Holdings.FindById(id));
        }

        // GET: Holdings/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Holdings/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind(
                "Id,Classification,CopyNumber,CheckOutTimestamp,LastCheckedIn,DueDate,BranchId,HeldByPatronId,CheckoutPolicyId")]
            Holding holding)
        {
            if (ModelState.IsValid)
            {
                _context.Add(holding);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(holding);
        }

        // GET: Holdings/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            return this.ViewIf(await _context.Holdings.FindDirect(id));
        }

        // POST: Holdings/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,
            [Bind(
                "Id,Classification,CopyNumber,CheckOutTimestamp,LastCheckedIn,DueDate,BranchId,HeldByPatronId,CheckoutPolicyId")]
            Holding holding)
        {
            if (id != holding.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(holding);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Holdings.Exists(holding.Id)) return NotFound();
                    throw;
                }
            }

            return View(holding);
        }

        // GET: Holdings/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            return this.ViewIf(await _context.Holdings.FindById(id));
        }

        // POST: Holdings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            _context.Holdings.Delete(id, _context);
            return RedirectToAction(nameof(Index));
        }
    }
}