using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Axuno.TextTemplating.Localization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Axuno.TextTemplating;

/// <summary>
/// Template content provider
/// </summary>
/// <remarks>
/// Inject as transient
/// </remarks>
public class TemplateContentProvider : ITemplateContentProvider
{
    public IServiceScopeFactory ServiceScopeFactory { get; }
    public TextTemplatingOptions Options { get; }
    private readonly ITemplateDefinitionManager _templateDefinitionManager;

    public TemplateContentProvider(
        ITemplateDefinitionManager templateDefinitionManager,
        IServiceScopeFactory serviceScopeFactory,
        IOptions<TextTemplatingOptions> options)
    {
        ServiceScopeFactory = serviceScopeFactory;
        Options = options.Value;
        _templateDefinitionManager = templateDefinitionManager;
    }

    public virtual Task<string?> GetContentAsync(
        string templateName,
        string? cultureName = null,
        bool tryDefaults = true,
        bool useCurrentCultureIfCultureNameIsNull = true)
    {
        var template = _templateDefinitionManager.Get(templateName);
        return GetContentAsync(template, cultureName);
    }

    public virtual async Task<string?> GetContentAsync(
        TemplateDefinition? templateDefinition,
        string? cultureName = null,
        bool tryDefaults = true,
        bool useCurrentCultureIfCultureNameIsNull = true)
    {
        Check.NotNull(templateDefinition, nameof(templateDefinition));

        if (!Options.ContentContributors.Any())
        {
            throw new Exception(
                $"No template content contributor was registered. Use {nameof(TextTemplatingOptions)} to register contributors!"
            );
        }

        using var scope = ServiceScopeFactory.CreateScope();
        string? templateString = null;

        if (cultureName == null && useCurrentCultureIfCultureNameIsNull)
        {
            cultureName = CultureInfo.CurrentUICulture.Name;
        }

        var contributors = CreateTemplateContentContributors(scope.ServiceProvider);

        //Try to get from the requested culture
        templateString = await GetContentAsync(
            contributors,
            new TemplateContentContributorContext(
                templateDefinition!,
                scope.ServiceProvider,
                cultureName
            )
        );

        if (templateString != null)
        {
            return templateString;
        }

        if (!tryDefaults)
        {
            return null;
        }

        //Try to get from same culture without country code
        if (cultureName != null && cultureName.Contains('-')) //Example: "tr-TR"
        {
            templateString = await GetContentAsync(
                contributors,
                new TemplateContentContributorContext(
                    templateDefinition!,
                    scope.ServiceProvider,
                    CultureHelper.GetBaseCultureName(cultureName)
                )
            );

            if (templateString != null)
            {
                return templateString;
            }
        }

        if (templateDefinition!.IsInlineLocalized)
        {
            //Try to get culture independent content
            templateString = await GetContentAsync(
                contributors,
                new TemplateContentContributorContext(
                    templateDefinition,
                    scope.ServiceProvider,
                    null
                )
            );

            if (templateString != null)
            {
                return templateString;
            }
        }
        else
        {
            //Try to get from default culture
            if (templateDefinition.DefaultCultureName != null)
            {
                templateString = await GetContentAsync(
                    contributors,
                    new TemplateContentContributorContext(
                        templateDefinition,
                        scope.ServiceProvider,
                        templateDefinition.DefaultCultureName
                    )
                );

                if (templateString != null)
                {
                    return templateString;
                }
            }
        }

        //Not found
        return null;
    }

    protected virtual ITemplateContentContributor[] CreateTemplateContentContributors(IServiceProvider serviceProvider)
    {
        return Options.ContentContributors
            .Select(type => (ITemplateContentContributor)serviceProvider.GetRequiredService(type))
            .Reverse()
            .ToArray();
    }

    protected virtual async Task<string?> GetContentAsync(
        ITemplateContentContributor[] contributors,
        TemplateContentContributorContext context)
    {
        foreach (var contributor in contributors)
        {
            var templateString = await contributor.GetAsync(context);
            if (templateString != null)
            {
                return templateString;
            }
        }

        return null;
    }
}
