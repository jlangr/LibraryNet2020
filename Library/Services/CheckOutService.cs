using System.Collections.Generic;
using LibraryNet2020.Controllers.Validations;
using LibraryNet2020.Models;
using LibraryNet2020.NonPersistentModels;
using LibraryNet2020.Util;
using LibraryNet2020.ViewModels;
using static LibraryNet2020.Controllers.Validations.Constants;
using Validator = LibraryNet2020.Controllers.Validations.Validator;

namespace LibraryNet2020.Services
{
    public class CheckOutService
    {
        private PipelineValidator pipelineValidator = new PipelineValidator();

        public virtual IEnumerable<string> ErrorMessages => pipelineValidator.ErrorMessages;
        
        public virtual bool Checkout(LibraryContext context, CheckOutViewModel checkout)
        {
            pipelineValidator.Validate(new List<Validator>
            {
                new PatronRetrievalValidator(context, checkout.PatronId),
                new BarcodeValidator(context, checkout.Barcode),
                new HoldingRetrievalValidator(context, checkout.Barcode),
                new HoldingAvailableValidator(context)
            });
            if (!pipelineValidator.IsValid())
                return false;

            var holding = (Holding) pipelineValidator.Data[HoldingKey];
            // TODO determine policy material, which in turn comes from from Isbn lookup on creation 
            // Currently Holding creation in controller does not accept ISBN
            // TODO manage thru holdingservice
            holding.CheckOut(TimeService.Now, checkout.PatronId, new BookCheckoutPolicy());
            context.SaveChanges();
            return true;
        }
    }
}