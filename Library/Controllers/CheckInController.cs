using System.Collections.Generic;
using LibraryNet2020.ControllerHelpers;
using LibraryNet2020.Models;
using LibraryNet2020.Util;
using LibraryNet2020.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace LibraryNet2020.Controllers
{
    public class CheckInController: Controller
    {
        public const string ModelKey = "CheckIn";
        private readonly LibraryContext context;
        private readonly BranchesService branchesService;
        private readonly HoldingsService holdingsService;

        public CheckInController(LibraryContext context)
        {
            this.context = context;
            branchesService = new BranchesService(context);
            holdingsService = new HoldingsService(context);
        }
        
        // GET: CheckIn
        public ActionResult Index()
        {
            var model = new CheckInViewModel
                {BranchesViewList = new List<Branch>(context.AllBranchesIncludingVirtual())};
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(CheckInViewModel checkin)
        {
            checkin.BranchesViewList = new List<Branch>(context.AllBranchesIncludingVirtual());

            // TODO move to service and use validators
            if (!Holding.IsBarcodeValid(checkin.Barcode))
            {
                ModelState.AddModelError(ModelKey, "Invalid holding barcode format.");
                return View(checkin);
            }

            var holding = holdingsService.FindByBarcode(checkin.Barcode);
            if (holding == null)
            {
                ModelState.AddModelError(ModelKey, "Invalid holding barcode.");
                return View(checkin);
            }
            if (!holding.IsCheckedOut)
            {
                ModelState.AddModelError(ModelKey, "Holding is already checked in.");
                return View(checkin);
            }

            holding.CheckIn(TimeService.Now, checkin.BranchId);
            context.SaveChanges();

            // TODO this is broke (?)
            return RedirectToAction("Index");
        }
    }
}