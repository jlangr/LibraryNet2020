using System.Collections.Generic;
using System.Linq;
using LibraryNet2020.Controllers;
using LibraryNet2020.Models;
using LibraryNet2020.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using static LibraryNet2020.Controllers.CheckInController;

namespace LibraryTest.Controllers
{
    [Collection("SharedLibraryContext")]
    public class CheckInControllerTest: LibraryControllerTest
    {
        private readonly LibraryContext context;
        private readonly CheckInController controller;
        private readonly Mock<CheckInService> checkInServiceMock = new Mock<CheckInService>();
        private readonly CheckInViewModel checkinViewModel;

        public CheckInControllerTest(DbContextFixture fixture)
        {
            fixture.Seed();
            context = new LibraryContext(fixture.ContextOptions);
            var checkInService = checkInServiceMock.Object;           
            controller = new CheckInController(context, checkInService);
            
            checkinViewModel = new CheckInViewModel
            {
                Barcode = "QA123:1",
                BranchId = 1
            };
        }
        
        [Fact]
        public void Post_RedirectsToIndexOnSuccessfulCheckout()
        {
            checkInServiceMock.Setup(
                s => s.Checkin(context, checkinViewModel)).Returns(true);

            var actionResult = controller.Index(checkinViewModel);

            Assert.Equal("Index", (actionResult as RedirectToActionResult).ActionName);
        }
        
        [Fact]
        public void Post_SetsModelErrorsOnUnsuccessfulCheckin()
        {
            checkInServiceMock.Setup(
                s => s.Checkin(context, checkinViewModel)).Returns(false);
            checkInServiceMock.Setup(
                s => s.ErrorMessages).Returns(new List<string> {"error"});

            var viewResult = controller.Index(checkinViewModel) as ViewResult;

            Assert.Equal("error", 
                ControllerErrors(viewResult, ModelKey).First().ErrorMessage);
        }
    }
}