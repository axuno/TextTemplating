using Axuno.TextTemplating;
using System;
using System.Collections.Generic;
using System.Text;
using TextTemplatingDemo.Localization;

namespace TextTemplatingDemo
{
    // inject as transient
    public class DemoTemplateDefinitionProvider : TemplateDefinitionProvider
    {
        public override void Define(ITemplateDefinitionContext context)
        {
            context.Add(
                new TemplateDefinition("Hello", typeof(DemoResource), defaultCultureName: "") //template name: "Hello"
                    .WithVirtualFilePath(
                        "/Demos/Hello/Hello.tpl", //template content path
                        isInlineLocalized: true
                    )
            );

            context.Add(
                new TemplateDefinition(
                    name: "PasswordReset",
                    localizationResource: typeof(DemoResource)
                ).WithVirtualFilePath(
                    "/Demos/PasswordReset/PasswordReset.tpl", //template content path
                    isInlineLocalized: true
                )
            );

            context.Add(
                new TemplateDefinition(
                    name: "WelcomeEmail",
                    defaultCultureName: "en",
                    layout: "EmailLayout" //Set the LAYOUT
                ).WithVirtualFilePath(
                    "/Demos/WelcomeEmail/Templates", //template content folder
                    isInlineLocalized: false
                )
            );

            context.Add(
                new TemplateDefinition(
                    "EmailLayout",
                    isLayout: true
                ).WithVirtualFilePath(
                    "/Demos/EmailLayout/EmailLayout.tpl", //template content path
                    isInlineLocalized: true
                )
            );

            context.Add(
                new TemplateDefinition("GlobalContextUsage")
                    .WithVirtualFilePath(
                        "/Demos/GlobalContext/GlobalContextUsage.tpl",
                        isInlineLocalized: true
                    )
            );

            context.Add(
                new TemplateDefinition("Custom")
                    .WithVirtualFilePath(
                        "/Demos/CustomRenderer/Custom.tpl",
                        isInlineLocalized: true
                    )
            );
        }
    }
}
