using LibraryNet2020.Controllers;
using LibraryNet2020.Models;
using LibraryNet2020.Services;
using LibraryNet2020.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace LibraryTest
{
    [Collection("SharedLibraryContext")]
    public class CheckOutControllerTest
    {
        private LibraryContext context;
        private CheckOutController controller;
        private CheckOutService checkOutService;
        private Mock<CheckOutService> checkOutServiceMock = new Mock<CheckOutService>();
        
        public CheckOutControllerTest(DbContextFixture fixture)
        {
            fixture.Seed();
            context = new LibraryContext(fixture.ContextOptions);
            checkOutService = checkOutServiceMock.Object;           
            controller = new CheckOutController(context, checkOutService);
        }

        [Fact]
        public void Post_RedirectsToIndexOnSuccessfulCheckout()
        {
            var checkout = new CheckOutViewModel
            {
                Barcode = "QA123:1",
                PatronId = 1
            };
            checkOutServiceMock.Setup(
                s => s.Checkout(context, checkout)).Returns(true);

            var actionResult = controller.Index(checkout);

            Assert.Equal("Index", (actionResult as RedirectToActionResult).ActionName);
        }
    }
}