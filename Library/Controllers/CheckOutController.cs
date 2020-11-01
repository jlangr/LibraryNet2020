using System.Collections.Generic;
using LibraryNet2020.Models;
using LibraryNet2020.Services;
using LibraryNet2020.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace LibraryNet2020.Controllers
{
    // TODO test
    public class CheckOutController : LibraryController
    {
        public const string ModelKey = "CheckOut";
        private readonly LibraryContext context;
        private readonly CheckOutService checkOutService;

        public CheckOutController(LibraryContext context, CheckOutService checkOutService)
        {
            this.context = context;
            this.checkOutService = checkOutService;
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
            if (!checkOutService.Checkout(context, checkout))
            {
                AddModelErrors(checkOutService.ErrorMessages, ModelKey);
                return View(checkout);
            }
            return RedirectToAction("Index");
        }
    }
}