using System;
using Axuno.TextTemplating;
using Axuno.TextTemplating.VirtualFiles;
using Axuno.VirtualFileSystem;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;

namespace TextTemplatingDemo.TextTemplatingModule;

public static class TextTemplatingServiceCollectionExtensions
{
    public static IServiceCollection AddTextTemplatingModule(this IServiceCollection services, Action<VirtualFileSystemOptions>? virtualFileSystemOptions = null, Action<LocalizationOptions>? localizationOptions = null)
    {
        services.Configure<VirtualFileSystemOptions>(virtualFileSystemOptions ?? (options => { }));
        services.Configure<LocalizationOptions>(localizationOptions ?? (options => { }));

        services.Configure<TextTemplatingOptions>(options =>
        {
            options.DefinitionProviders.Add(typeof(DemoTemplateDefinitionProvider));
            options.ContentContributors.Add(typeof(VirtualFileTemplateContentContributor));
        });

        services.AddTransient<DemoTemplateDefinitionProvider>();
        services.AddSingleton<IFileProvider, VirtualFileProvider>();
        services.AddSingleton<IDynamicFileProvider, DynamicFileProvider>();
        services.AddTransient<VirtualFileTemplateContentContributor>();
        services.AddSingleton<ILocalizedTemplateContentReaderFactory, LocalizedTemplateContentReaderFactory>();
        services.AddSingleton<IStringLocalizerFactory>(s => new ResourceManagerStringLocalizerFactory(s.GetRequiredService<IOptions<LocalizationOptions>>(), NullLoggerFactory.Instance));
        services.AddSingleton<ITemplateDefinitionManager, TemplateDefinitionManager>();
        services.AddTransient<ITemplateContentProvider, TemplateContentProvider>();
        services.AddTransient<ITemplateRenderer, TemplateRenderer>();

        return services;
    }
}