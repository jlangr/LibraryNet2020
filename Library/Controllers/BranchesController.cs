using System.Threading.Tasks;
using LibraryNet2020.Extensions;
using LibraryNet2020.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryNet2020.Controllers
{
    public class BranchesController : Controller
    {
        private readonly LibraryContext context;

        public BranchesController(LibraryContext context)
        {
            this.context = context;
        }

        // GET: Branches
        public async Task<IActionResult> Index()
        {
            return View(await context.Branches.ToListAsync());
        }

        // GET: Branches/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            return this.ViewIf(await context.Branches.FirstByIdAsync(id));
        }


        // GET: Branches/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Branches/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] Branch branch)
        {
            if (!ModelState.IsValid) return View(branch);
            context.Add(branch);
            await context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Branches/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            return this.ViewIf(await context.Branches.FindByIdAsync(id));
        }

        // POST: Branches/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Branch branch)
        {
            if (id != branch.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    context.Update(branch);
                    await context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!context.Branches.Exists(branch.Id)) return NotFound();
                    throw;
                }
            }
            return View(branch);
        }

        // GET: Branches/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            return this.ViewIf(await context.Branches.FirstByIdAsync(id));
        }

        // POST: Branches/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            context.Branches.Delete(id, context);
            return RedirectToAction(nameof(Index));
        }
        
    }
}
