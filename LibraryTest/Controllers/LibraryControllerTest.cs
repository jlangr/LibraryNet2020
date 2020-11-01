using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace LibraryTest
{
    public class LibraryControllerTest
    {
        protected ModelErrorCollection ControllerErrors(ViewResult viewResult, string modelKey)
        {
            return viewResult.ViewData.ModelState[modelKey].Errors;
        }
    }
}