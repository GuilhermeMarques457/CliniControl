using Microsoft.AspNetCore.Mvc;

namespace CliniControl.UI.Usefull
{
    public static class AddJsFilesHelper
    {
        public static void AddJsFiles(Controller controller, params string[] jsNames)
        {
            controller.ViewBag.JsFiles = new List<string>();

            foreach (string jsName in jsNames)
            {
                controller.ViewBag.JsFiles.Add(jsName);
            }
        }
    }
}
