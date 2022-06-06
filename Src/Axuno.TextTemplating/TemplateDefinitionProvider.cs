
namespace Axuno.TextTemplating;

/// <summary>
/// Template definition provider abstract class.
/// </summary>
/// <remarks>
/// Inject derived classes as transient.
/// </remarks>
public abstract class TemplateDefinitionProvider : ITemplateDefinitionProvider
{
    public virtual void PreDefine(ITemplateDefinitionContext context)
    {
    }

    public abstract void Define(ITemplateDefinitionContext context);

    public virtual void PostDefine(ITemplateDefinitionContext context)
    {
    }
}
