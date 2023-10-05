using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OdontoControl.Core.Helpers
{
    public static class ManageImageProject
    {
        public async static Task<string> AddImage(IFormFile filePath, string projectFolder, string wwwrootFolder, Guid? id)
        {
            var fileFullPath = Path.Combine(projectFolder, wwwrootFolder, $"{id}-{filePath.FileName}");

            using (var stream = new FileStream(fileFullPath, FileMode.Create))
            {
                await filePath.CopyToAsync(stream);
            }

            return fileFullPath;
        }

        public static Task<bool> DeleteImage(string? filePath, string? projectFolder)
        {
            if (filePath == null || projectFolder == null) 
                return Task.FromResult(false);

            var fileFullPath = Path.Combine(projectFolder, filePath.Replace("~/", ""));

            if (File.Exists(fileFullPath))
            {
                File.Delete(fileFullPath);
                return Task.FromResult(true);
            }
            else
            {
                return Task.FromResult(false);
            }
        }
    }
}
