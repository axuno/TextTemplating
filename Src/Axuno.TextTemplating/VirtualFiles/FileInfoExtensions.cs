using Microsoft.Extensions.FileProviders;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Axuno.TextTemplating.VirtualFiles
{
    public static class FileInfoExtensions
    {
        /// <summary>
        /// Reads file content as string using the given <paramref name="encoding"/>.
        /// </summary>
        public static async Task<string> ReadAsStringAsync( this IFileInfo fileInfo, Encoding encoding)
        {
            Check.NotNull(fileInfo, nameof(fileInfo));

            await using var stream = fileInfo.CreateReadStream();
            using var streamReader = new StreamReader(stream, encoding, true);
            return await streamReader.ReadToEndAsync();
        }
    }
}
