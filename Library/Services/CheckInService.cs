using System.Collections.Generic;
using LibraryNet2020.Controllers.Validations;
using LibraryNet2020.Models;
using LibraryNet2020.Util;
using LibraryNet2020.ViewModels;
using static LibraryNet2020.Controllers.Validations.Constants;

namespace LibraryNet2020.Controllers
{
    // TODO test
    public class CheckInService
    {
        private PipelineValidator pipelineValidator = new PipelineValidator();

        public virtual IEnumerable<string> ErrorMessages => pipelineValidator.ErrorMessages;
        
        public virtual bool Checkin(LibraryContext context, CheckInViewModel checkin)
        {
            pipelineValidator.Validate(new List<Validator>
            {
                new BarcodeValidator(context, checkin.Barcode),
                new HoldingRetrievalValidator(context, checkin.Barcode), 
                new NotValidator(new HoldingAvailableValidator(context))
            });
            if (!pipelineValidator.IsValid())
                return false;

            var holding = pipelineValidator.Data[HoldingKey] as Holding;

            // TODO  manage thru holdingservice
            holding.CheckIn(TimeService.Now, checkin.BranchId);
            context.SaveChanges();
            return true;
        }
    }
}