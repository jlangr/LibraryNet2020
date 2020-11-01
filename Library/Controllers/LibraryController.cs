using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace LibraryNet2020.Controllers
{
    public class LibraryController: Controller
    {
        protected void AddModelErrors(IEnumerable<string> errorMessages, string modelKey)
        {
            foreach (var message in errorMessages)
                ModelState.AddModelError(modelKey, message);
        }
    }
}