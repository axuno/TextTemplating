using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Axuno.TextTemplating;
using TextTemplateDemo.Demos.Hello;

namespace TextTemplatingDemo.Demos.CustomRenderer
{
    class CustomRendererDemo
    {
        private readonly ITemplateRenderer _templateRenderer;

        public CustomRendererDemo(ServiceProvider? services)
        {
            if (services is null) throw new ArgumentNullException(nameof(services));
            _templateRenderer = services.GetServices<ITemplateRenderer>().First(s => s.GetType() == typeof(CustomTemplateRenderer));
        }

        public async Task RunAsync()
        {
            var result = await _templateRenderer.RenderAsync(
                "Custom", //the template name
                new 
                {
                    FirstName = "John",
                    LastName = "Specimen"
                }
            );

            Console.WriteLine(result);
        }
    }
}
