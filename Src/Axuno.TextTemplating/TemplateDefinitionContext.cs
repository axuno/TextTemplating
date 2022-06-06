using System.Collections.Generic;
using System.Collections.Immutable;

namespace Axuno.TextTemplating;

public class TemplateDefinitionContext : ITemplateDefinitionContext
{
    protected Dictionary<string, TemplateDefinition> Templates { get; }

    public TemplateDefinitionContext(Dictionary<string, TemplateDefinition> templates)
    {
        Templates = templates;
    }

    public IReadOnlyList<TemplateDefinition> GetAll(string name)
    {
        return Templates.Values.ToImmutableList();
    }

    public virtual TemplateDefinition? Get(string name)
    {
        return Templates[name];
    }

    public virtual IReadOnlyList<TemplateDefinition> GetAll()
    {
        return Templates.Values.ToImmutableList();
    }

    public virtual void Add(params TemplateDefinition[]? definitions)
    {
        if (definitions is null || definitions.Length == 0)
        {
            return;
        }

        foreach (var definition in definitions)
        {
            Templates[definition.Name] = definition;
        }
    }
}