using System.Linq;
using System.Threading.Tasks;
using LibraryNet2020.Controllers;
using LibraryNet2020.Models;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace LibraryTest.Controllers
{
    [Collection("SharedLibraryContext")]
    public class PatronsControllerTest
    {
        private LibraryContext context;
        private PatronsController controller;

        public PatronsControllerTest(DbContextFixture fixture)
        {
            fixture.Seed();
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

            var result = Assert.IsType<RedirectToActionResult>(task.Result);

            Assert.Equal("Index", result.ActionName);
        }

        [Fact]
        public void Create_RendersPatronViewWhenPatronInvalid()
        {
            controller.ModelState.AddModelError("", "");
        
            var task = controller.Create(new Patron());
            
            var viewResult = Assert.IsType<ViewResult>(task.Result);
            Assert.IsType<Patron>(viewResult.Model);
        }
    }
}