using System;
using System.Globalization;
using System.Threading.Tasks;
using Axuno.TextTemplating;
using Axuno.TextTemplating.Localization;

namespace TextTemplateDemo.Demos.PasswordReset
{
    public class PasswordResetDemo
    {
        private readonly ITemplateRenderer _templateRenderer;

        public PasswordResetDemo(ITemplateRenderer templateRenderer)
        {
            _templateRenderer = templateRenderer;
        }

        public async Task RunAsync(string culture)
        {
            var result = await _templateRenderer.RenderAsync(
                "PasswordReset", //the template name
                new PasswordResetModel
                {
                    Link = "https://axuno.net"
                },
                culture
            );

            Console.WriteLine(result);
        }
    }
}
