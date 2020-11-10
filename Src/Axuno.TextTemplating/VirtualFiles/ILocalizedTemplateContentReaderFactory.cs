using System.Threading.Tasks;

namespace Axuno.TextTemplating.VirtualFiles
{
    public interface ILocalizedTemplateContentReaderFactory
    {
        Task<ILocalizedTemplateContentReader> CreateAsync(TemplateDefinition templateDefinition);
    }
}