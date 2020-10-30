using System.Linq;
using System.Threading.Tasks;
using LibraryNet2020.Controllers;
using LibraryNet2020.Models;
using Microsoft.AspNetCore.Mvc;
using Xunit;
using Xunit.Abstractions;

namespace LibraryTest
{
    [Collection("SharedLibraryContext")]
    public class PatronsControllerTest
    {
        // private readonly ITestOutputHelper output;
        private LibraryContext context;
        private PatronsController controller;

        public PatronsControllerTest(DbContextFixture fixture) // , ITestOutputHelper output)
        {
            fixture.Seed();
            // this.output = output;
            context = new LibraryContext(fixture.ContextOptions);
            controller = new PatronsController(context);
        }

        [Fact]
        public void Details_ReturnsNotFoundWhenNoPatronAdded()
        {
            var task = controller.Details(0);

            Assert.IsType<BadRequestResult>(task.Result);
        }
        
        [Fact]
        public async Task Create_PersistsPatron()
        {
            await controller.Create(new Patron {Name = "Jeff"});

            Assert.NotNull(context.Patrons.Single(patron => patron.Name == "Jeff"));
        }

        [Fact]
        public void Create_RedirectsToIndexWhenModelValid()
        {
            var task = controller.Create(new Patron {Name = "name"});

            Assert.Equal("Index", (task.Result as RedirectToActionResult).ActionName);
        }

        [Fact]
        public void Create_RendersPatronViewWhenPatronInvalid()
        {
            controller.ModelState.AddModelError("", "");
        
            var task = controller.Create(new Patron());
        
            Assert.IsType<Patron>((task.Result as ViewResult).Model);
        }

        // TODO all this
//             [Fact]
//             public void ReturnsListWithCheckedOutHolding()
//             {
//                 int holdingId1 = CreateCheckedOutHolding(patronId, checkoutController, 1);
//                 int holdingId2 = CreateCheckedOutHolding(patronId, checkoutController, 2);
//
//                 var view = (controller.Holdings(patronId) as ViewResult)?.Model as IEnumerable<Holding>;
//
//                 Assert.Equal(new List<int> { holdingId1, holdingId2 }, view.Select(h => h.Id));
//             }
//
//             private int CreateCheckedOutHolding(int id, CheckOutController controller, int copyNumber)
//             {
//                 var holdingId = holdingRepo.Create(new Holding { Classification = "X", CopyNumber = copyNumber, BranchId = branchId });
//                 var checkOutViewModel = new CheckOutViewModel { Barcode = $"X:{copyNumber}", PatronId = id };
//                 controller.Index(checkOutViewModel);
//                 return holdingId;
//             }
//         }
//         
//         public class Index: PatronsControllerTest
//         {
//             [Fact]
//             public void RetrievesViewOnAllPatrons()
//             {
//                 patronRepo.Create(new Patron { Name = "Alpha" }); 
//                 patronRepo.Create(new Patron { Name = "Beta" }); 
//
//                 var view = controller.Index();
//
//                 var patrons = (view as ViewResult)?.Model as IEnumerable<Patron>;
//                 Assert.Equal(new string[] { "Alpha", "Beta" }, patrons?.Select(p => p.Name));
//             }
//         }
//
    }
}