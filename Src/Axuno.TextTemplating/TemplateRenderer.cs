using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Axuno.TextTemplating.Localization;
using Microsoft.Extensions.Localization;
using Scriban;
using Scriban.Runtime;

namespace Axuno.TextTemplating
{
    /// <summary>
    /// Template renderer.
    /// </summary>
    /// <remarks>
    /// Inject as transient.
    /// </remarks>
    public class TemplateRenderer : ITemplateRenderer
    {
        protected readonly ITemplateContentProvider _templateContentProvider;
        protected readonly ITemplateDefinitionManager _templateDefinitionManager;
        protected readonly IStringLocalizerFactory _stringLocalizerFactory;

        public TemplateRenderer(
            ITemplateContentProvider templateContentProvider,
            ITemplateDefinitionManager templateDefinitionManager,
            IStringLocalizerFactory stringLocalizerFactory)
        {
            _templateContentProvider = templateContentProvider;
            _templateDefinitionManager = templateDefinitionManager;
            _stringLocalizerFactory = stringLocalizerFactory;
        }

        public virtual async Task<string> RenderAsync(
             string templateName,
             object? model,
             string? cultureName,
             Dictionary<string, object>? globalContext)
        {
            globalContext ??= new Dictionary<string, object>();

            if (cultureName == null)
            {
                return await RenderInternalAsync(
                    templateName,
                    globalContext,
                    model
                );
            }

            using (new CultureSwitcher(new CultureInfo(cultureName), new CultureInfo(cultureName)))
            {
                return await RenderInternalAsync(
                    templateName,
                    globalContext,
                    model
                );
            }
        }

        protected virtual async Task<string> RenderInternalAsync(
            string templateName,
            Dictionary<string, object> globalContext,
            object? model = null)
        {
            var templateDefinition = _templateDefinitionManager.Get(templateName);
            if (templateDefinition is null) throw new Exception($"Template name '{templateName}' not found.");

            var renderedContent = await RenderSingleTemplateAsync(
                templateDefinition,
                globalContext,
                model
            );

            if (templateDefinition.Layout != null)
            {
                globalContext["content"] = renderedContent;
                renderedContent = await RenderInternalAsync(
                    templateDefinition.Layout,
                    globalContext
                );
            }

            return renderedContent;
        }

        protected virtual async Task<string> RenderSingleTemplateAsync(
            TemplateDefinition templateDefinition,
            Dictionary<string, object> globalContext,
            object? model = null)
        {
            var rawTemplateContent = await _templateContentProvider
                .GetContentAsync(
                    templateDefinition
                );

            return await RenderTemplateContentWithScribanAsync(
                templateDefinition,
                rawTemplateContent,
                globalContext,
                model
            );
        }

        protected virtual async Task<string> RenderTemplateContentWithScribanAsync(
            TemplateDefinition templateDefinition,
            string? templateContent,
            Dictionary<string, object> globalContext,
            object? model = null)
        {
            var context = CreateScribanTemplateContext(
                templateDefinition,
                globalContext,
                model
            );

            return await Template
                    .Parse(templateContent)
                    .RenderAsync(context);
        }

        protected virtual TemplateContext CreateScribanTemplateContext(
            TemplateDefinition templateDefinition,
            Dictionary<string, object> globalContext,
            object? model = null)
        {
            var context = new TemplateContext();

            var scriptObject = new ScriptObject();

            scriptObject.Import(globalContext);

            if (model != null)
            {
                scriptObject["model"] = model;
            }

            var localizer = GetLocalizer(templateDefinition);
            scriptObject.SetValue("L", new TemplateLocalizer(localizer), true);

            context.PushGlobal(scriptObject);

            return context;
        }

        private IStringLocalizer GetLocalizer(TemplateDefinition templateDefinition)
        {
            return _stringLocalizerFactory.Create(templateDefinition.LocalizationResource ?? typeof(object));
        }
    }
}
