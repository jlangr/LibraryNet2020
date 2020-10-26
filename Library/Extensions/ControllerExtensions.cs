using Microsoft.AspNetCore.Mvc;

namespace LibraryNet2020.Extensions
{
    public static class ControllerExtensions
    {
        public static IActionResult ViewIf<T>(this Controller controller, T entity)
        {
            if (entity == null) return controller.NotFound();
            return controller.View(entity);
        }
    }
}