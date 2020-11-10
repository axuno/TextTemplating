using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.FileProviders;

namespace Axuno.TextTemplating.VirtualFiles
{
    public class FileInfoLocalizedTemplateContentReader : ILocalizedTemplateContentReader
    {
        private string? _content;

        public async Task ReadContentsAsync(IFileInfo fileInfo)
        {
            _content = await fileInfo.ReadAsStringAsync(Encoding.UTF8);
        }

        public string? GetContent(string? culture)
        {
            return culture == null ? _content : null;
        }
    }
}