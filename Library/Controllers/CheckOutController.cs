using System.Collections.Generic;
using LibraryNet2020.Models;
using LibraryNet2020.Services;
using LibraryNet2020.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace LibraryNet2020.Controllers
{
    // TODO test
    public class CheckOutController : Controller
    {
        private const string ModelKey = "CheckOut";
        private readonly LibraryContext context;

        public CheckOutController(LibraryContext context)
        {
            this.context = context;
        }

        // GET: CheckOut
        public ActionResult Index()
        {
            var model = new CheckOutViewModel
                {BranchesViewList = new List<Branch>(context.AllBranchesIncludingVirtual())};
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(CheckOutViewModel checkout)
        {
            if (!ModelState.IsValid) return View(checkout);

            checkout.BranchesViewList = new List<Branch>(context.AllBranchesIncludingVirtual());
            var checkOutService = new CheckOutService();
            if (!checkOutService.Checkout(context, checkout))
            {
                foreach (var message in checkOutService.ErrorMessages)
                    ModelState.AddModelError(ModelKey, message);
                return View(checkout);
            }
            return RedirectToAction("Index");
        }
    }
}