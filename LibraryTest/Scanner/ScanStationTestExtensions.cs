using System;
using LibraryNet2020.Models;
using LibraryNet2020.NonPersistentModels;
using LibraryNet2020.Scanner;
using LibraryNet2020.Util;
using Moq;

namespace LibraryTest.Scanner
{
    public static class ScanStationTestExtensions
    {
        public static void ScanNewMaterial(this ScanStation scanner, string barcode,
            Mock<IClassificationService> serviceMock)
        {
            var classification = Holding.ClassificationFromBarcode(barcode);
            var isbn = "x";
            var material = new Material
            {
                Author = "Long",
                CheckoutPolicy = CheckoutPolicies.BookCheckoutPolicy,
                Title = "A Book",
                Year = "2001",
                Classification = classification
            };
            serviceMock.Setup(service => service.Classification(isbn)).Returns(classification);
            serviceMock.Setup(service => service.Retrieve(classification)).Returns(material);
            scanner.AddNewHolding(isbn);
        }

        public static void CheckOut(this ScanStation scanner, string barcode, int patronId, DateTime dateTime)
        {
            TimeService.NextTime = dateTime;
            scanner.AcceptLibraryCard(patronId);
            TimeService.NextTime = dateTime;
            scanner.AcceptBarcode(barcode);
        }

        public static void CheckIn(this ScanStation scanner, string barcode, DateTime dateTime)
        {
            TimeService.NextTime = dateTime;
            scanner.AcceptBarcode(barcode);
        }
    }
}