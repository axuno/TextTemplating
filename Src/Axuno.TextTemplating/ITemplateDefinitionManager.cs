using System.Collections.Generic;

namespace Axuno.TextTemplating;

public interface ITemplateDefinitionManager
{
        
    TemplateDefinition? Get(string name);

    IReadOnlyList<TemplateDefinition> GetAll();
}
