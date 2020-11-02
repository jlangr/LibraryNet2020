using System.Collections.Generic;
using LibraryNet2020.ControllerHelpers;
using LibraryNet2020.Models;
using LibraryNet2020.Services;
using LibraryNet2020.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace LibraryNet2020.Controllers
{
    public class CheckInController: LibraryController
    {
        public const string ModelKey = "CheckIn";
        private readonly LibraryContext context;
        public CheckInService checkInService;
        private readonly BranchesService branchesService;
        
        public CheckInController(LibraryContext context)
        {
            this.context = context;
            checkInService = new CheckInService(context);
            branchesService = new BranchesService(context);
        }
        
        // GET: CheckIn
        public ActionResult Index()
        {
            var model = new CheckInViewModel
                {BranchesViewList = new List<Branch>(branchesService.AllBranchesIncludingVirtual())};
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(CheckInViewModel checkin)
        {
            checkin.BranchesViewList = new List<Branch>(branchesService.AllBranchesIncludingVirtual());

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