using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using Axuno.VirtualFileSystem;
using Microsoft.Extensions.FileProviders;

namespace Axuno.TextTemplating.VirtualFiles
{
    /// <summary>
    /// Localized template content reader factory
    /// </summary>
    /// <remarks>
    /// Inject as singleton
    /// </remarks>
    public class LocalizedTemplateContentReaderFactory : ILocalizedTemplateContentReaderFactory
    {
        /// <summary>
        /// Gets the <see cref="IFileProvider"/>.
        /// </summary>
        protected IFileProvider VirtualFileProvider { get; }
        /// <summary>
        /// Gets the reader cache.
        /// </summary>
        protected ConcurrentDictionary<string, ILocalizedTemplateContentReader> ReaderCache { get; }
        /// <summary>
        /// The <see cref="SemaphoreSlim"/> synchronization object.
        /// </summary>
        protected SemaphoreSlim SyncObj;

        /// <summary>
        /// CTOR.
        /// </summary>
        /// <param name="virtualFileProvider">The <see cref="IFileProvider"/> to use.</param>
        public LocalizedTemplateContentReaderFactory(IFileProvider virtualFileProvider)
        {
            VirtualFileProvider = virtualFileProvider;
            ReaderCache = new ConcurrentDictionary<string, ILocalizedTemplateContentReader>();
            SyncObj = new SemaphoreSlim(1, 1);
        }

        /// <summary>
        /// Gets the <see cref="ILocalizedTemplateContentReader"/> for the <see cref="TemplateDefinition"/>.
        /// </summary>
        /// <param name="templateDefinition">The <see cref="TemplateDefinition"/>.</param>
        /// <returns>The <see cref="ILocalizedTemplateContentReader"/> for the <see cref="TemplateDefinition"/>.</returns>
        public virtual async Task<ILocalizedTemplateContentReader> CreateAsync(TemplateDefinition templateDefinition)
        {
            if (ReaderCache.TryGetValue(templateDefinition.Name, out var reader))
            {
                return reader;
            }

            using (await SyncObj.LockAsync())
            {
                if (ReaderCache.TryGetValue(templateDefinition.Name, out reader))
                {
                    return reader;
                }

                reader = await CreateInternalAsync(templateDefinition);
                ReaderCache[templateDefinition.Name] = reader;
                return reader;
            }
        }

        /// <summary>
        /// Gets the <see cref="ILocalizedTemplateContentReader"/> for the <see cref="TemplateDefinition"/>.
        /// </summary>
        /// <param name="templateDefinition">The <see cref="TemplateDefinition"/>.</param>
        /// <returns>The <see cref="ILocalizedTemplateContentReader"/> for the <see cref="TemplateDefinition"/>.</returns>
        protected virtual async Task<ILocalizedTemplateContentReader> CreateInternalAsync(
            TemplateDefinition templateDefinition)
        {
            var virtualPath = templateDefinition.GetVirtualFilePath();
            if (virtualPath == null)
            {
                return NullLocalizedTemplateContentReader.Instance;
            }

            var fileInfo = VirtualFileProvider.GetFileInfo(virtualPath);
            
            if (!fileInfo.Exists)
            {
                var directoryContents = VirtualFileProvider.GetDirectoryContents(virtualPath);
                if (!directoryContents.Exists)
                {
                    throw new Exception("Could not find a file/folder at the location: " + virtualPath);
                }

                fileInfo = new VirtualDirectoryFileInfo(virtualPath, virtualPath, DateTimeOffset.UtcNow);
            }

            if (fileInfo.IsDirectory)
            {
                var folderReader = new VirtualFolderLocalizedTemplateContentReader();
                await folderReader.ReadContentsAsync(VirtualFileProvider, virtualPath);
                return folderReader;
            }
            else //File
            {
                var singleFileReader = new FileInfoLocalizedTemplateContentReader();
                await singleFileReader.ReadContentsAsync(fileInfo);
                return singleFileReader;
            }
        }
    }
}