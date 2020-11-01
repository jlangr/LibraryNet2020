using System.Collections.Generic; using LibraryNet2020.Models;
using LibraryNet2020.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace LibraryNet2020.Controllers
{
    public class CheckInController: LibraryController
    {
        public const string ModelKey = "CheckIn";
        private readonly LibraryContext context;
        private readonly CheckInService checkInService;

        public CheckInController(LibraryContext context, CheckInService checkInService)
        {
            this.context = context;
            this.checkInService = checkInService;
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

            if (!checkInService.Checkin(context, checkin))
            {
                AddModelErrors(checkInService.ErrorMessages, ModelKey);
                return View(checkin);
            }

            // TODO this is broke (?)
            return RedirectToAction("Index");
        }
    }

}