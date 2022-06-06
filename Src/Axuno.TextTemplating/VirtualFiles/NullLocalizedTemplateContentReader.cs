namespace Axuno.TextTemplating.VirtualFiles;

public class NullLocalizedTemplateContentReader : ILocalizedTemplateContentReader
{
    public static NullLocalizedTemplateContentReader Instance { get; } = new NullLocalizedTemplateContentReader();

    private NullLocalizedTemplateContentReader()
    { }

    public string? GetContent(string? culture)
    {
        return null;
    }
}