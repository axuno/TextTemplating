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
        /// <summary>
        /// Gets the <see cref="ITemplateContentProvider"/>.
        /// </summary>
        protected readonly ITemplateContentProvider TemplateContentProvider;
        /// <summary>
        /// Gets the <see cref="ITemplateDefinitionProvider"/>.
        /// </summary>
        protected readonly ITemplateDefinitionManager TemplateDefinitionManager;
        /// <summary>
        /// Gets the <see cref="IStringLocalizerFactory"/>.
        /// </summary>
        protected readonly IStringLocalizerFactory StringLocalizerFactory;

        /// <summary>
        /// Creates a new instance of a <see cref="TemplateRenderer"/> class.
        /// </summary>
        /// <param name="templateContentProvider"></param>
        /// <param name="templateDefinitionManager"></param>
        /// <param name="stringLocalizerFactory"></param>
        public TemplateRenderer(
            ITemplateContentProvider templateContentProvider,
            ITemplateDefinitionManager templateDefinitionManager,
            IStringLocalizerFactory stringLocalizerFactory)
        {
            TemplateContentProvider = templateContentProvider;
            TemplateDefinitionManager = templateDefinitionManager;
            StringLocalizerFactory = stringLocalizerFactory;
        }

        /// <summary>
        /// Renders a text template.
        /// </summary>
        /// <param name="templateName">The template name</param>
        /// <param name="model">An optional model object that is used in the template.</param>
        /// <param name="cultureName">Culture name. Uses the <see cref="CultureInfo.CurrentUICulture"/> if not specified</param>
        /// <param name="globalContext">A dictionary which can be used to import global objects to the template</param>
        /// <returns>Returns the rendered text template.</returns>
        public virtual async Task<string> RenderAsync(
             string templateName,
             object? model = null,
             string? cultureName = null,
             Dictionary<string, object>? globalContext = null)
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

        /// <summary>
        /// Renders a text template.
        /// </summary>
        /// <param name="templateName">The template name</param>
        /// <param name="model">An optional model object that is used in the template.</param>
        /// <param name="globalContext">A dictionary which can be used to import global objects to the template</param>
        /// <returns>Returns the rendered text template.</returns>
        protected virtual async Task<string> RenderInternalAsync(
            string templateName,
            Dictionary<string, object> globalContext,
            object? model = null)
        {
            var templateDefinition = TemplateDefinitionManager.Get(templateName);
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

        /// <summary>
        /// Renders a text template.
        /// </summary>
        /// <param name="templateDefinition">The template definition</param>
        /// <param name="globalContext">A dictionary which can be used to import global objects to the template</param>
        /// <param name="model">An optional model object that is used in the template.</param>
        /// <returns>Returns the rendered text template.</returns>
        protected virtual async Task<string> RenderSingleTemplateAsync(
            TemplateDefinition templateDefinition,
            Dictionary<string, object> globalContext,
            object? model = null)
        {
            var rawTemplateContent = await TemplateContentProvider
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

        /// <summary>
        /// Renders a text template with Scriban.
        /// </summary>
        /// <param name="templateDefinition">The template definition</param>
        /// <param name="templateContent">The template content</param>
        /// <param name="globalContext">A dictionary which can be used to import global objects to the template</param>
        /// <param name="model">An optional model object that is used in the template.</param>
        /// <returns>Returns the rendered text template.</returns>
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

        /// <summary>
        /// Creates a new Scriban <see cref="TemplateContext"/>.
        /// </summary>
        /// <param name="templateDefinition">The <see cref="TemplateDefinition"/>.</param>
        /// <param name="globalContext">The global context.</param>
        /// <param name="model">The model to render the template.</param>
        /// <returns></returns>
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
            return StringLocalizerFactory.Create(templateDefinition.LocalizationResource ?? typeof(object));
        }
    }
}
