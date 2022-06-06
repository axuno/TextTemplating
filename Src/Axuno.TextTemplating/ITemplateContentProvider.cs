using System.Threading.Tasks;

namespace Axuno.TextTemplating;

public interface ITemplateContentProvider
{
    Task<string?> GetContentAsync(
        string templateName,
        string? cultureName = null,
        bool tryDefaults = true,
        bool useCurrentCultureIfCultureNameIsNull = true
    );

    Task<string?> GetContentAsync(
        TemplateDefinition templateDefinition,
        string? cultureName = null,
        bool tryDefaults = true,
        bool useCurrentCultureIfCultureNameIsNull = true
    );
}
