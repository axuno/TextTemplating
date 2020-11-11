using System;
using System.Threading.Tasks;
using Axuno.TextTemplating;
using Axuno.VirtualFileSystem;


namespace TextTemplateDemo.Demos.Hello
{
    public class HelloDemo
    {
        private readonly ITemplateRenderer _templateRenderer;

        public HelloDemo(ITemplateRenderer templateRenderer)
        {
            _templateRenderer = templateRenderer;
        }

        public async Task RunAsync()
        {
            var result = await _templateRenderer.RenderAsync(
                "Hello", //the template name
                new HelloModel
                {
                    Name = "John"
                }
            );

            Console.WriteLine(result);
        }

        public async Task RunWithAnonymousModelAsync()
        {
            var result = await _templateRenderer.RenderAsync(
                "Hello", //the template name
                new
                {
                    Name = "John"
                }
            );

            Console.WriteLine(result);
        }
    }
}
