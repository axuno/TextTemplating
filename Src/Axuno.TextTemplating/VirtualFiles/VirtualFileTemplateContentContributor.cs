using System.Threading.Tasks;
using Axuno.TextTemplating;
using Axuno.TextTemplating.VirtualFiles;


namespace Axuno.TextTemplating.VirtualFiles
{
    /// <summary>
    /// Virtual file template content contributor
    /// </summary>
    /// <remarks>
    /// Inject as transient
    /// </remarks>
    public class VirtualFileTemplateContentContributor : ITemplateContentContributor
    {
        public const string VirtualPathPropertyName = "VirtualPath";

        private readonly ILocalizedTemplateContentReaderFactory _localizedTemplateContentReaderFactory;

        public VirtualFileTemplateContentContributor(
            ILocalizedTemplateContentReaderFactory localizedTemplateContentReaderFactory)
        {
            _localizedTemplateContentReaderFactory = localizedTemplateContentReaderFactory;
        }

        public virtual async Task<string?> GetAsync(TemplateContentContributorContext context)
        {
            var localizedReader = await _localizedTemplateContentReaderFactory
                .CreateAsync(context.TemplateDefinition);

            return localizedReader.GetContent(
                context.Culture
            );
        }
    }
}