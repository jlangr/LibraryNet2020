using System.Collections.Generic;
using LibraryNet2020.Controllers;
using LibraryNet2020.Controllers.Validations;
using LibraryNet2020.Models;
using LibraryNet2020.ViewModels;
using static LibraryNet2020.Controllers.Validations.Constants;
using Validator = LibraryNet2020.Controllers.Validations.Validator;

namespace LibraryNet2020.Services
{
    // TODO test
    public class CheckOutService
    {
        private readonly PipelineValidator pipelineValidator = new PipelineValidator();
        private readonly HoldingsService holdingsService;
        private readonly LibraryContext context;

        public CheckOutService() // needed for Moq
        {
        }

        public CheckOutService(LibraryContext context, HoldingsService holdingsService)
        {
            this.context = context;
            // holdingsService = new HoldingsService(context);
            this.holdingsService = holdingsService;
        }

        public virtual IEnumerable<string> ErrorMessages => pipelineValidator.ErrorMessages;
        
        public virtual bool Checkout(CheckOutViewModel checkout)
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
            holdingsService.CheckOut(holding, checkout.PatronId);
            return true;
        }
    }
}