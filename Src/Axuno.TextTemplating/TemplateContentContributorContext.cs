using System;


namespace Axuno.TextTemplating
{
    public class TemplateContentContributorContext
    {
        
        public TemplateDefinition TemplateDefinition { get; }
        
        public IServiceProvider ServiceProvider { get; }
        
        public string? Culture { get; }

        public TemplateContentContributorContext(
             TemplateDefinition templateDefinition,
             IServiceProvider serviceProvider, 
             string? culture)
        {
            TemplateDefinition = Check.NotNull(templateDefinition, nameof(templateDefinition));
            ServiceProvider = Check.NotNull(serviceProvider, nameof(serviceProvider));
            Culture = culture;
        }
    }
}