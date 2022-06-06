using System.Collections.Generic;

namespace Axuno.TextTemplating;

public interface ITemplateDefinitionContext
{
    IReadOnlyList<TemplateDefinition> GetAll(string name);

    TemplateDefinition? Get(string name);

    void Add(params TemplateDefinition[] definitions);
}
