using System;
using System.Collections.Generic;
using Axuno.TextTemplating;
using Microsoft.Extensions.Localization;
using Scriban;
using Scriban.Parsing;
using Scriban.Runtime;
using Scriban.Syntax;

namespace TextTemplatingDemo.Demos.CustomRenderer;

/// <summary>
/// Custom template renderer.
/// </summary>
/// <remarks>
/// Inject as transient.
/// </remarks>
public class CustomTemplateRenderer : TemplateRenderer
{
    public CustomTemplateRenderer(
        ITemplateContentProvider templateContentProvider,
        ITemplateDefinitionManager templateDefinitionManager,
        IStringLocalizerFactory stringLocalizerFactory) : base(templateContentProvider, templateDefinitionManager, stringLocalizerFactory)
    { }

    /*
    protected override async Task<string> RenderTemplateContentWithScribanAsync(
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
    }*/

    protected override TemplateContext CreateScribanTemplateContext(
        TemplateDefinition templateDefinition,
        Dictionary<string, object> globalContext,
        object? model = null)
    {
        var baseContext = base.CreateScribanTemplateContext(templateDefinition, globalContext, model);
            
        // The ScriptObject is already registered as global
        baseContext.StrictVariables = true;  // Note: has no effect for child members
        baseContext.MemberRenamer = member => member.Name; // Use .NET object without changing the cases
        // will be called if the member could not be found:
        baseContext.TryGetMember = (TemplateContext context, SourceSpan span, object target, string member,
            out object value) =>
        {
            value = $"## Member '{member}' not found ##";
            return true;
        };
        // will be called if the variable could not be found
        baseContext. 
                TryGetVariable =
            (TemplateContext context, SourceSpan span, ScriptVariable variable, out object value) =>
            {
                value = $"## Variable '{variable}' not found ##";
                return true;
            };

        baseContext.BuiltinObject.SetValue(nameof(MyCustomFunction), new MyCustomFunction(), true);
        baseContext.BuiltinObject.Import("MySimpleFunction", new Func<string>(() => "Text from MySimpleFunction"));

        return baseContext;
    }
}