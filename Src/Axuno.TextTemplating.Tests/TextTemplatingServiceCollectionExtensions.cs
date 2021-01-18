using System;
using System.Globalization;
using Axuno.TextTemplating;
using Axuno.TextTemplating.VirtualFiles;
using Axuno.VirtualFileSystem;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;

namespace Axuno.TextTemplating.Tests
{
    public static class TextTemplatingServiceCollectionExtensions
    {
        public static IServiceCollection AddTextTemplatingModule(this IServiceCollection services, Action<VirtualFileSystemOptions>? virtualFileSystemOptions = null, Action<LocalizationOptions>? localizationOptions = null)
        {
            services.Configure<VirtualFileSystemOptions>(virtualFileSystemOptions ?? (options => { }));
            services.Configure<LocalizationOptions>(localizationOptions ?? (options => { }));

            services.Configure<TextTemplatingOptions>(options =>
            {
                // Don't forget to add content and definition providers as services
                options.DefinitionProviders.Add(typeof(TestTemplateDefinitionProvider));
                options.ContentContributors.Add(typeof(VirtualFileTemplateContentContributor));
            });

            services.AddSingleton<ILoggerFactory>(b => new NullLoggerFactory()); 
            services.AddTransient<TestTemplateDefinitionProvider>();
            services.AddSingleton<IFileProvider, VirtualFileProvider>();
            services.AddSingleton<IDynamicFileProvider, DynamicFileProvider>();
            services.AddTransient<VirtualFileTemplateContentContributor>();
            services.AddSingleton<ILocalizedTemplateContentReaderFactory, LocalizedTemplateContentReaderFactory>();
           
            services.AddSingleton<ITemplateDefinitionManager, TemplateDefinitionManager>();
            services.AddTransient<ITemplateContentProvider, TemplateContentProvider>();
            services.AddTransient<ITemplateRenderer, TemplateRenderer>();

            services.TryAddSingleton<IStringLocalizerFactory>(s => new ResourceManagerStringLocalizerFactory(s.GetRequiredService<IOptions<LocalizationOptions>>(), NullLoggerFactory.Instance));
            services.TryAddSingleton(typeof(IStringLocalizer<>), typeof(StringLocalizer<>));
            
            return services;
        }
    }
}
