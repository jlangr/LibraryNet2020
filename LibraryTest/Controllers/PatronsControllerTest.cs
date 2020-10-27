using LibraryNet2020.Controllers;
using LibraryNet2020.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace LibraryTest
{
    public class DbContextFixture
    {
        public DbContextOptions<LibraryContext> ContextOptions { get; set; }

        public DbContextFixture()
        {
            ContextOptions = new DbContextOptionsBuilder<LibraryContext>()
                // .UseSqlite("Filename=Library.db")
                .UseSqlite("DataSource=:memory:")
                .Options;
            Seed();
        }

        private void Seed()
        {
            using var context = new LibraryContext(ContextOptions);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            // add materials here if necessary
            context.SaveChanges();
        }
    }

    public class PatronsControllerTest : IClassFixture<DbContextFixture>
    {
        private readonly DbContextFixture fixture;

        public PatronsControllerTest(DbContextFixture fixture)
        {
            this.fixture = fixture;
        }

        [Fact]
        public void Details_ReturnsNotFoundWhenNoPatronAdded()
        {
            using var context = new LibraryContext(fixture.ContextOptions);
            var controller = new PatronsController(context);

            var task = controller.Details(0);

            Assert.IsType<NotFoundResult>(task.Result);
        }

        [Fact]
        public void Create_PersistsRetrievablePatron()
        {
            using var context = new LibraryContext(fixture.ContextOptions);
            var controller = new PatronsController(context);

            var id = controller.Create(new Patron {Name = "Jeff"});

            var task = controller.Details(id.Id);
            Assert.Equal("Jeff", Patron(task.Result).Name);
        }

        private static Patron Patron(IActionResult result)
        {
            return (result as ViewResult).Model as Patron;
        }

        [Fact]
        public void Create_RedirectsToIndexWhenModelValid()
        {
            using var context = new LibraryContext(fixture.ContextOptions);
            var controller = new PatronsController(context);

            var task = controller.Create(new Patron {Name = "name"});

            Assert.Equal("Index", (task.Result as RedirectToActionResult).ActionName);
        }

        [Fact]
        public void Create_RendersPatronViewWhenPatronInvalid()
        {
            using var context = new LibraryContext(fixture.ContextOptions);
            var controller = new PatronsController(context);
            controller.ModelState.AddModelError("", "");
        
            var task = controller.Create(new Patron());
        
            Assert.IsType<Patron>((task.Result as ViewResult).Model);
        }

//
//             IRepository<Branch> branchRepo = new InMemoryRepository<Branch>();
//             CheckOutController checkoutController;
//             int patronId;
//             int branchId;
//
//             public HoldingsTest()
//             {
//                 CreateCheckoutController();
//                 CreatePatron();
//                 CreateBranch();
//             }
//             
//             private void CreateCheckoutController()
//             {
//                 checkoutController = new CheckOutController(branchRepo, holdingRepo, patronRepo);
//             }
//
//             private void CreatePatron()
//             {
//                 patronId = patronRepo.Create(new Patron());
//             }
//
//             private void CreateBranch()
//             {
//                 branchId = branchRepo.Create(new Branch());
//             }
//
//             [Fact]
//             public void ReturnsEmptyWhenPatronHasNotCheckedOutBooks()
//             {
//                 var view = (controller.Holdings(patronId) as ViewResult)?.Model as IEnumerable<Holding>;
//
//                 Assert.True(!view?.Any());
//             }
//
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