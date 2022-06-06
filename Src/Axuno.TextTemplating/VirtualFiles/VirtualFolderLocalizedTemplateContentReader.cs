using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.FileProviders;

namespace Axuno.TextTemplating.VirtualFiles;

public class VirtualFolderLocalizedTemplateContentReader : ILocalizedTemplateContentReader
{
    private readonly Dictionary<string, string> _dictionary = new Dictionary<string, string>();

    public async Task ReadContentsAsync(
        IFileProvider virtualFileProvider, 
        string virtualPath)
    {
        var directoryContents = virtualFileProvider.GetDirectoryContents(virtualPath);
        if (!directoryContents.Exists)
        {
            throw new Exception("Could not find a folder at the location: " + virtualPath);
        }

        foreach (var file in directoryContents)
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
        return cultureName is null ? null : _dictionary.ContainsKey(cultureName) ? _dictionary[cultureName] : null;
    }
}