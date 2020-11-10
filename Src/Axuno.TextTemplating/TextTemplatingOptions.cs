
namespace Axuno.TextTemplating
{
    public class TextTemplatingOptions
    {
        public ITypeList<ITemplateDefinitionProvider> DefinitionProviders { get; }
        public ITypeList<ITemplateContentContributor> ContentContributors { get; }

        public TextTemplatingOptions()
        {
            DefinitionProviders = new TypeList<ITemplateDefinitionProvider>();
            ContentContributors = new TypeList<ITemplateContentContributor>();
        }
    }
}