using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using LibraryNet2020.Controllers;
using LibraryNet2020.Models;
using LibraryNet2020.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace LibraryTest
{
    [Collection("SharedLibraryContext")]
    public class HoldingsControllerTest
    {
        private LibraryContext context;
        private HoldingsController controller;

        public HoldingsControllerTest(DbContextFixture fixture)
        {
            fixture.Seed();
            context = new LibraryContext(fixture.ContextOptions);
            controller = new HoldingsController(context);
        }

        [Fact]
        public async void Create_PersistsRetrievableHolding()
        {
            var id = controller.Create(new Holding { Classification = "ABC 123" });

            var task = controller.Details(id.Id);
            
            Assert.Equal("ABC 123", Holding(task.Result).Classification);
        }
        
        [Fact]
        public async void PopulatesViewModelWithBranchName()
        {
            var branchEntity = context.Branches.Add(new Branch {Name = "branch123"});
            var branchId = branchEntity.Entity.Id;
            
            controller.Create(new Holding { Classification = "AB123", CopyNumber = 1, BranchId = branchId });

            var viewResult = await controller.Index() as ViewResult;
            var holdingViewModels = viewResult.Model as IEnumerable<HoldingViewModel>;
            Assert.Collection(holdingViewModels, 
                holdingViewModel => Assert.Equal("branch123", holdingViewModel.BranchName));
        }

        private static Holding Holding(IActionResult result)
        {
            return (result as ViewResult).Model as Holding;
        }
    }
}