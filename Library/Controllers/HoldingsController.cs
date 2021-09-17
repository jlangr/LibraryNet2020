using System;
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
        public const string ModelKey = "Holdings";
        private readonly LibraryContext context;
        private readonly HoldingsService holdingsService;
        private readonly BranchesService branchesService;

        public HoldingsController(LibraryContext context)
        {
            this.context = context;
            holdingsService = new HoldingsService(context);
            branchesService = new BranchesService(context);
        }

        // GET: Holdings
        public async Task<IActionResult> Index()
        {
            var holdings = await context.Holdings.ToListAsync();
            return View(holdings.Select(HoldingViewModel));
        }

        private HoldingViewModel HoldingViewModel(Holding holding) =>
            new HoldingViewModel(holding)
            {
                BranchName = branchesService.BranchName(holding.BranchId)
            };

        // GET: Holdings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            return this.ViewIf(await context.Holdings.FirstByIdAsync(id));
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
            if (!ModelState.IsValid) return View(holding);

            try
            {
                holdingsService.Add(holding);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                ModelState.AddModelError(ModelKey, e.Message);
                return View(holding);
            }
        }

        // GET: Holdings/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            return this.ViewIf(await context.Holdings.FindByIdAsync(id));
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

            if (!ModelState.IsValid) return View(holding);

            try
            {
                context.Update(holding);
                await context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!context.Holdings.Exists(holding.Id)) return NotFound();
                throw;
            }
        }

        // GET: Holdings/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            return this.ViewIf(await context.Holdings.FirstByIdAsync(id));
        }

        // POST: Holdings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await Task.Run(() => context.Holdings.Delete(id, context));
            return RedirectToAction(nameof(Index));
        }
    }
}