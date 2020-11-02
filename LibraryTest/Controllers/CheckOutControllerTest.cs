using System.Collections.Generic;
using System.Linq;
using LibraryNet2020.Controllers;
using LibraryNet2020.Models;
using LibraryNet2020.Services;
using LibraryNet2020.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using static LibraryNet2020.Controllers.CheckOutController;

namespace LibraryTest.Controllers
{
    [Collection("SharedLibraryContext")]
    public class CheckOutControllerTest: LibraryControllerTest
    {
        private readonly CheckOutController controller;
        private readonly Mock<CheckOutService> checkOutServiceMock = new Mock<CheckOutService>();
        private readonly CheckOutViewModel checkOutViewModel;

        public CheckOutControllerTest(DbContextFixture fixture)
        {
            fixture.Seed();
            var context = new LibraryContext(fixture.ContextOptions);
            controller = new CheckOutController(context)
            {
                checkOutService = checkOutServiceMock.Object
            };
            checkOutViewModel = new CheckOutViewModel
            {
                Barcode = "QA123:1",
                PatronId = 1
            };
        }

        [Fact]
        public void Post_RedirectsToIndexOnSuccessfulCheckout()
        {
            checkOutServiceMock.Setup(
                s => s.Checkout(checkOutViewModel)).Returns(true);

            var actionResult = Assert.IsType<RedirectToActionResult>(controller.Index(checkOutViewModel));

            Assert.Equal("Index", actionResult.ActionName);
        }
        
        [Fact]
        public void Post_SetsModelErrorsOnUnsuccessfulCheckin()
        {
            checkOutServiceMock.Setup(
                s => s.Checkout(checkOutViewModel)).Returns(false);
            checkOutServiceMock.Setup(
                s => s.ErrorMessages).Returns(new List<string> {"error"});

            var viewResult = Assert.IsType<ViewResult>(controller.Index(checkOutViewModel));

            Assert.Equal("error", 
                ControllerErrors(viewResult, ModelKey).First().ErrorMessage);
        }
    }
}