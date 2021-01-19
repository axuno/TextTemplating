
namespace Axuno.TextTemplating
{
    /// <summary>
    /// Class for options applied to text templating.
    /// </summary>
    public class TextTemplatingOptions
    {
        /// <summary>
        /// CTOR.
        /// </summary>
        public TextTemplatingOptions()
        {
            DefinitionProviders = new TypeList<ITemplateDefinitionProvider>();
            ContentContributors = new TypeList<ITemplateContentContributor>();
        }
        
        /// <summary>
        /// Gets a <see cref="ITypeList"/> of <see cref="ITemplateDefinitionProvider"/>s.
        /// </summary>
        public ITypeList<ITemplateDefinitionProvider> DefinitionProviders { get; }
        /// <summary>
        /// Gets a <see cref="ITypeList"/> of <see cref="ITemplateContentContributor"/>s.
        /// </summary>
        public ITypeList<ITemplateContentContributor> ContentContributors { get; }
    }
}