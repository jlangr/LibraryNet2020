using System.Threading.Tasks;
using LibraryNet2020.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LibraryNet2020.Models;
using LibraryNet2020.Services;
using LibraryNet2020.ViewModels;

namespace LibraryNet2020.Controllers
{
    public class PatronsController : Controller
    {
        private readonly LibraryContext context;
        private readonly PatronsService patronsService;

        public PatronsController(LibraryContext context)
        {
            this.context = context;
            patronsService = new PatronsService(context);
        }

        // GET: Patrons
        public async Task<IActionResult> Index()
        {
            return View(await context.Patrons.ToListAsync());
        }

        // GET: Patrons/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var patron = await context.Patrons.FindById(id);
            if (patron == null)
                return BadRequest();
            
            var patronView = new PatronViewModel(patron)
            {
                Holdings = patronsService.HoldingsForPatron(id)
            };
            
            return View(patronView);
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
            
            context.Add(patron);
            await context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Patrons/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            return this.ViewIf(await context.Patrons.FindDirect(id));
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
                    context.Update(patron);
                    await context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!context.Patrons.Exists(patron.Id)) return NotFound();
                    throw;
                }
            }
            return View(patron);
        }

        // GET: Patrons/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            return this.ViewIf(await context.Patrons.FindById(id));
        }

        // POST: Patrons/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            context.Patrons.Delete(id, context);
            return RedirectToAction(nameof(Index));
        }
    }
}
