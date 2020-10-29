using System.Threading.Tasks;
using LibraryNet2020.ControllerHelpers;
using LibraryNet2020.Models;

namespace LibraryNet2020.Controllers
{
    public class HoldingsService
    {
        private readonly LibraryContext context;

        public HoldingsService(LibraryContext context)
        {
            this.context = context;
        }

        public async Task Add(Holding holding)
        {
            if (holding.CopyNumber == 0)
                holding.CopyNumber = HoldingsControllerUtil.NextAvailableCopyNumber(context, holding.Classification);
            context.Add(holding);
            await context.SaveChangesAsync();
        }
    }
}