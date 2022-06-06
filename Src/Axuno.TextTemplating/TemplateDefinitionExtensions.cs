using Axuno.TextTemplating.VirtualFiles;

namespace Axuno.TextTemplating;

public static class TemplateDefinitionExtensions
{
    public static TemplateDefinition WithVirtualFilePath(
        this TemplateDefinition templateDefinition,
        string virtualPath,
        bool isInlineLocalized)
    {
        Check.NotNull(templateDefinition, nameof(templateDefinition));

        templateDefinition.IsInlineLocalized = isInlineLocalized;

        return templateDefinition.WithProperty(
            VirtualFileTemplateContentContributor.VirtualPathPropertyName,
            virtualPath
        );
    }

    public static string? GetVirtualFilePath(
        this TemplateDefinition templateDefinition)
    {
        Check.NotNull(templateDefinition, nameof(templateDefinition));

        return templateDefinition
            .Properties[VirtualFileTemplateContentContributor.VirtualPathPropertyName] as string;
    }
}