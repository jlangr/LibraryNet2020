using System.Collections.Generic;
using LibraryNet2020.Controllers.Validations;
using LibraryNet2020.Models;
using LibraryNet2020.NonPersistentModels;
using LibraryNet2020.Util;
using LibraryNet2020.ViewModels;
using Validator = LibraryNet2020.Controllers.Validations.Validator;

namespace LibraryNet2020.Services
{
    public class CheckOutService
    {
        private PipelineValidator pipelineValidator;

        public IList<string> ErrorMessages => pipelineValidator.ErrorMessages;
        
        public bool Checkout(LibraryContext context, CheckOutViewModel checkout)
        {
            checkout.BranchesViewList = new List<Branch>(context.AllBranchesIncludingVirtual());

            pipelineValidator = new PipelineValidator();
            pipelineValidator.Validate(new List<Validator>
            {
                new PatronRetrievalValidator(context, checkout.PatronId),
                new BarcodeValidator(context, checkout.Barcode),
                new HoldingRetrievalValidator(context, checkout.Barcode),
                new HoldingAvailableValidator(context)
            });
            if (!pipelineValidator.IsValid())
                return false;

            var holding = pipelineValidator.Data["Holding"] as Holding;
            // TODO determine policy material, which in turn comes from from Isbn lookup on creation 
            // Currently Holding creation in controller does not accept ISBN
            holding.CheckOut(TimeService.Now, checkout.PatronId, new BookCheckoutPolicy());
            context.SaveChanges();
            return true;
        }
    }
}