namespace Axuno.TextTemplating.VirtualFiles
{
    public interface ILocalizedTemplateContentReader
    {
        public string? GetContent(string? culture);
    }
}