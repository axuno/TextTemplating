using System;
using System.Collections.Generic;
using System.Text;
using Axuno.TextTemplating.Tests.Localization;

namespace Axuno.TextTemplating.Tests
{
    public class TestTemplateDefinitionProvider : TemplateDefinitionProvider
    {
        public override void Define(ITemplateDefinitionContext context)
        {
            context.Add(
                new TemplateDefinition(
                    Templates.WelcomeEmail,
                    typeof(TestLocalizationResource),
                    defaultCultureName: "en"
                ).WithVirtualFilePath("/WelcomeEmail", false)
            );

            context.Add(
                new TemplateDefinition(
                    Templates.SayHelloEmail,
                    localizationResource: typeof(TestLocalizationResource),
                    layout: Templates.TestTemplateLayout
                ).WithVirtualFilePath("/SayHelloEmail.tpl", true)
            );

            context.Add(
                new TemplateDefinition(
                    Templates.TestTemplateLayout,
                    typeof(TestLocalizationResource),
                    isLayout: true
                ).WithVirtualFilePath("/TestTemplateLayout.tpl", true)
            );
            
            context.Add(
                new TemplateDefinition(
                    Templates.ShowDecimalNumber,
                    localizationResource: typeof(TestLocalizationResource),
                    layout: Templates.TestTemplateLayout
                ).WithVirtualFilePath("/ShowDecimalNumber.tpl", true)
            );
        }
    }
}
