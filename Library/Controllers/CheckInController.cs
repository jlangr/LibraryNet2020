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
        private readonly CheckInService checkInService;
        private readonly BranchesService branchesService;
        
        public CheckInController(LibraryContext _context, CheckInService checkInService, BranchesService branchesService)
        {
            this.checkInService = checkInService;
            this.branchesService = branchesService;
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

            if (!checkInService.Checkin(checkin))
            {
                AddModelErrors(checkInService.ErrorMessages, ModelKey);
                return View(checkin);
            }

            // TODO this is broke (?)
            return RedirectToAction("Index");
        }
    }

}