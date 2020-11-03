using System.Collections.Generic;
using System.Linq;
using LibraryNet2020.Controllers;
using LibraryNet2020.Models;
using LibraryNet2020.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace LibraryTest.Controllers
{
    [Collection("SharedLibraryContext")]
    public class HoldingsControllerTest: LibraryControllerTest
    {
        private readonly LibraryContext context;
        private readonly HoldingsController controller;

        public HoldingsControllerTest(DbContextFixture fixture)
        {
            fixture.Seed();
            context = new LibraryContext(fixture.ContextOptions);
            controller = new HoldingsController(context);
        }

        [Fact]
        public void Create_PersistsRetrievableHolding()
        {
            var result = controller.Create(new Holding { Classification = "ABC 123" });

            var task = controller.Details(result.Id);
            
            Assert.Equal("ABC 123", Holding(task.Result).Classification);
        }
        
        [Fact]
        public async void Create_PopulatesViewModelWithBranchName()
        {
            var branchEntity = await context.Branches.AddAsync(new Branch {Name = "branch123"});
            await context.SaveChangesAsync();
            var branchId = branchEntity.Entity.Id;
            
            await controller.Create(new Holding { Classification = "AB123", CopyNumber = 1, BranchId = branchId });

            var viewResult = await controller.Index() as ViewResult;
            var holdingViewModels = (IEnumerable<HoldingViewModel>)viewResult.Model;
            Assert.Collection(holdingViewModels, 
                holdingViewModel => Assert.Equal("branch123", holdingViewModel.BranchName));
        }

        [Fact]
        public void SetsModelErrorOnDuplicateBarcode()
        {
            controller.Create(new Holding { Classification = "AB123", CopyNumber = 1});
            
            var viewResult = Assert.IsType<ViewResult>(controller.Create(new Holding { Classification = "AB123", CopyNumber = 1 }).Result);
            
            Assert.Equal(HoldingsService.ErrorMessageDuplicateBarcode,
                ControllerErrors(viewResult, HoldingsController.ModelKey).First().ErrorMessage);
        }
        

        private static Holding Holding(IActionResult result)
        {
            return Assert.IsType<Holding>((Assert.IsType<ViewResult>(result)).Model);
        }
    }
}