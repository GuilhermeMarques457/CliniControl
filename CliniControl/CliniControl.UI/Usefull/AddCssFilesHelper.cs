using Microsoft.AspNetCore.Mvc;

namespace CliniControl.UI.Usefull
{
    public static class AddCssFilesHelper
    {
        public static void AddCssFiles(Controller controller, params string[] cssNames)
        {
            controller.ViewBag.CssFiles = new List<string>();

            foreach (string cssName in cssNames)
            {
                controller.ViewBag.CssFiles.Add(cssName);
            }
        }
    }
}
