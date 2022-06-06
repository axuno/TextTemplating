using System.Threading.Tasks;

namespace Axuno.TextTemplating;

public interface ITemplateContentContributor
{
    Task<string?> GetAsync(TemplateContentContributorContext context);
}