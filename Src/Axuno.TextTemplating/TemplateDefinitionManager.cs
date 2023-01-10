using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Axuno.TextTemplating;

/// <summary>
/// Temple definition manager
/// </summary>
/// <remarks>
/// Inject as singleton
/// </remarks>
public class TemplateDefinitionManager : ITemplateDefinitionManager
{
    protected Lazy<IDictionary<string, TemplateDefinition>> TemplateDefinitions { get; }

    protected TextTemplatingOptions Options { get; }

    protected IServiceProvider ServiceProvider { get; }

    public TemplateDefinitionManager(
        IOptions<TextTemplatingOptions> options,
        IServiceProvider serviceProvider)
    {
        ServiceProvider = serviceProvider;
        Options = options.Value;

        TemplateDefinitions =
            new Lazy<IDictionary<string, TemplateDefinition>>(CreateTextTemplateDefinitions, true);
    }

    public virtual IReadOnlyList<TemplateDefinition> GetAll()
    {
        return TemplateDefinitions.Value.Values.ToImmutableList();
    }

    public virtual TemplateDefinition? Get(string name)
    {
        return TemplateDefinitions.Value.Values.FirstOrDefault(tplDef => tplDef.Name.Equals(name));
    }

    protected virtual IDictionary<string, TemplateDefinition> CreateTextTemplateDefinitions()
    {
        var templates = new Dictionary<string, TemplateDefinition>();

        using var scope = ServiceProvider.CreateScope();
        var providers = Options
            .DefinitionProviders
            .Select(p => scope.ServiceProvider.GetRequiredService(p) as ITemplateDefinitionProvider)
            .Where(p => !(p is null))
            .ToList();

        var context = new TemplateDefinitionContext(templates);

        foreach (var provider in providers)
        {
            provider!.PreDefine(context);
        }

        foreach (var provider in providers)
        {
            provider!.Define(context);
        }

        foreach (var provider in providers)
        {
            provider!.PostDefine(context);
        }

        return templates;
    }
}
