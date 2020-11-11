using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Axuno.TextTemplating;
using Axuno.VirtualFileSystem;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.DependencyInjection;
using TextTemplateDemo.Demos.Hello;
using TextTemplateDemo.Demos.PasswordReset;
using TextTemplateDemo.Demos.WelcomeEmail;
using TextTemplatingDemo.Demos.CustomRenderer;
using TextTemplatingDemo.TextTemplatingModule;

namespace TextTemplatingDemo
{
    public class Program
    {
        public static ServiceProvider Services;

        private static void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<ITemplateRenderer, CustomTemplateRenderer>();
            services.AddTextTemplatingModule(vfs =>
                {
                    vfs.FileSets.AddEmbedded<Program>(nameof(TextTemplatingDemo));
                    vfs.FileSets.AddPhysical(AssemblyDirectory);
                },
                locOpt =>
                {
                    locOpt.ResourcesPath = string.Empty;  // noop
                });
            services.AddSingleton(typeof(IStringLocalizer<>), typeof(StringLocalizer<>));
            
            Services = services.BuildServiceProvider();
        }

        static async Task Main(string[] args)
        {
            ConfigureServices(new ServiceCollection());
            
            // This will return the renderer registered as the last
            var renderer = Services.GetRequiredService<ITemplateRenderer>();

            Console.WriteLine("*** Hello Template from TemplateContentProider ***");
            var templateContentDemo = Services.GetRequiredService<ITemplateContentProvider>();
            Console.WriteLine(await templateContentDemo.GetContentAsync("Hello")); 
            Console.WriteLine();

            Console.WriteLine("*** Rendering Hello Template with class model ***");
            await new HelloDemo(renderer).RunAsync();
            Console.WriteLine("*** Rendering Hello Template with anonymous model ***");
            await new HelloDemo(renderer).RunWithAnonymousModelAsync();
            Console.WriteLine();

            Console.WriteLine("*** Rendering from Global context ***");
            await new TextTemplateDemo.Demos.GlobalContext.GlobalContextUsageDemo(renderer).RunAsync();
            Console.WriteLine();

            Console.WriteLine("*** Rendering Password Reset Template (localized inline) ***");
            Console.WriteLine("*** English ***");
            await new PasswordResetDemo(renderer).RunAsync("en");
            Console.WriteLine("*** German ***");
            await new PasswordResetDemo(renderer).RunAsync("de");
            Console.WriteLine();

            Console.WriteLine("*** Separate WelcomeEmail Templates from embedded resources, with Layout (localized) ***");
            Console.WriteLine();
            Console.WriteLine("*** English ***");
            await new WelcomeEmailDemo(renderer).RunAsync("en");
            Console.WriteLine();
            Console.WriteLine("*** Spanish ***");
            await new WelcomeEmailDemo(renderer).RunAsync("es");
            Console.WriteLine();

            Console.WriteLine("*** Custom renderer demo ***");
            await new CustomRendererDemo(Services).RunAsync();
            Console.WriteLine();
        }

        public static string AssemblyDirectory
        {
            get
            {
                var location = Assembly.GetExecutingAssembly().Location;
                UriBuilder uri = new UriBuilder(location);
                var path = Uri.UnescapeDataString(uri.Path);
                return Path.GetDirectoryName(path)!;
            }
        }
    }
}
