using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Axuno.TextTemplating.VirtualFiles;
using Microsoft.Extensions.FileProviders;

namespace Axuno.TextTemplating.VirtualFiles
{
    public class VirtualFolderLocalizedTemplateContentReader : ILocalizedTemplateContentReader
    {
        private readonly Dictionary<string, string> _dictionary = new Dictionary<string, string>();

        public async Task ReadContentsAsync(
            IFileProvider virtualFileProvider, 
            string virtualPath)
        {
            var directoryInfo = virtualFileProvider.GetFileInfo(virtualPath);
            if (!directoryInfo.IsDirectory)
            {
                throw new Exception("Given virtual path is not a folder: " + virtualPath);
            }

            foreach (var file in virtualFileProvider.GetDirectoryContents(virtualPath))
            {
                if (file.IsDirectory)
                {
                    continue;
                }

                _dictionary.Add(file.Name.RemovePostFix(".tpl"), await file.ReadAsStringAsync(Encoding.UTF8));
            }
        }

        public string? GetContent(string? cultureName)
        {
            return cultureName is null ? null : _dictionary[cultureName];
        }
    }
}