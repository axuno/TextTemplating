using System;
using System.Threading.Tasks;
using Axuno.TextTemplating;


namespace TextTemplateDemo.Demos.WelcomeEmail
{
    public class WelcomeEmailDemo
    {
        private readonly ITemplateRenderer _templateRenderer;

        public WelcomeEmailDemo(ITemplateRenderer templateRenderer)
        {
            _templateRenderer = templateRenderer;
        }

        public async Task RunAsync(string cultureName)
        {
            var result = await _templateRenderer.RenderAsync(
                "WelcomeEmail",
                cultureName: cultureName
            );

            Console.WriteLine(result);
        }
    }
}
