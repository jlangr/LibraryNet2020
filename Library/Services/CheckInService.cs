using System.Collections.Generic;
using LibraryNet2020.Controllers;
using LibraryNet2020.Controllers.Validations;
using LibraryNet2020.Models;
using LibraryNet2020.ViewModels;
using static LibraryNet2020.Controllers.Validations.Constants;

namespace LibraryNet2020.Services
{
    public class CheckInService
    {
        private readonly PipelineValidator pipelineValidator = new PipelineValidator();
        public HoldingsService holdingsService;
        private readonly LibraryContext context;

        public CheckInService() // needed by Moq
        {
        }

        public CheckInService(LibraryContext context)
        {
            this.context = context;
            holdingsService = new HoldingsService(context);
        }

        public virtual IEnumerable<string> ErrorMessages => pipelineValidator.ErrorMessages;
        
        public virtual bool Checkin(CheckInViewModel checkin)
        {
            pipelineValidator.Validate(new List<Validator>
            {
                new BarcodeValidator(context, checkin.Barcode),
                new HoldingRetrievalValidator(context, checkin.Barcode), 
                new NotValidator(new HoldingAvailableValidator(context))
            });
            if (!pipelineValidator.IsValid())
                return false;

            var holding = (Holding) pipelineValidator.Data[HoldingKey];
            holdingsService.CheckIn(holding, checkin.BranchId);
            return true;
        }
    }
}