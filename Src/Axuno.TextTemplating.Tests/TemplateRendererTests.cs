using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace Axuno.TextTemplating.Tests;

[TestFixture]
public class TemplateRendererTests
{
    private readonly ServiceProvider _services;
    private readonly ITemplateRenderer _renderer;
        
    public TemplateRendererTests()
    {
        _services = ServiceSetup.GetTextTemplatingServiceProvider();
        _renderer = _services.GetRequiredService<ITemplateRenderer>();
    }

    [OneTimeTearDown]
    public void CleanUp()
    {
        _services.Dispose();
    }

    [Test]
    public async Task Should_Get_Rendered_With_CultureSpecific_Template()
    {
        Assert.That(await _renderer.RenderAsync(
            Templates.WelcomeEmail,
            model: new
            {
                name = "John"
            },
            cultureName: "en"
        ), Is.EqualTo("Welcome John to Axuno.TextTemplating!"));

        Assert.That(await _renderer.RenderAsync(
            Templates.WelcomeEmail,
            model: new
            {
                name = "John"
            },
            cultureName: "de"
        ), Is.EqualTo("Willkommen, John, bei Axuno.TextTemplating!"));
    }

    [Test]
    public async Task Should_Use_Fallback_Culture_CultureSpecific_Template()
    {
        //"en-US" falls back to "en" since "en-US" doesn't exists and "en" is the fallback culture
        Assert.That(await _renderer.RenderAsync(
            Templates.WelcomeEmail,
            model: new
            {
                name = "John"
            },
            cultureName: "en_US"
        ), Is.EqualTo("Welcome John to Axuno.TextTemplating!"));

        //"es" falls back to "en" since "es" doesn't exists and "en" is the fallback culture
        Assert.That(await _renderer.RenderAsync(
            Templates.WelcomeEmail,
            model: new
            {
                name = "John"
            },
            cultureName: "es"
        ), Is.EqualTo("Welcome John to Axuno.TextTemplating!"));
    }

    [Test]
    public async Task Should_Get_Rendered_Localized_Template_Content_With_Strongly_Typed_Model()
    {
        Assert.That(await _renderer.RenderAsync(
            Templates.WelcomeEmail,
            model: new WelcomeEmailModel("John"),
            cultureName: "en"
        ), Is.EqualTo("Welcome John to Axuno.TextTemplating!"));
    }
        
    [Test]
    public async Task Should_Get_Rendered_Localized_Template_Content_With_Dictionary_Model()
    {
        Assert.That(await _renderer.RenderAsync(
            Templates.WelcomeEmail,
            model: new Dictionary<string, object> { { "name", "John" } },
            cultureName: "en"
        ), Is.EqualTo("Welcome John to Axuno.TextTemplating!"));
    }

    [Test]
    public async Task Should_Get_Rendered_Inline_Localized_Template()
    {
        Assert.That(await _renderer.RenderAsync(
            Templates.SayHelloEmail,
            new SayHelloEmailModel("John"),
            cultureName: "en"
        ), Is.EqualTo("*BEGIN*Hello John, how are you?*END*"));

        Assert.That(await _renderer.RenderAsync(
            Templates.SayHelloEmail,
            new SayHelloEmailModel("John"),
            cultureName: "fr"
        ), Is.EqualTo("*BEGIN*Bonjour John, comment ça va?*END*"));

        Assert.That(await _renderer.RenderAsync(
            Templates.SayHelloEmail,
            new SayHelloEmailModel("John"),
            cultureName: "de"
        ), Is.EqualTo("*BEGIN*Hallo John, wie geht es Dir?*END*"));
    }

    [Test]
    public async Task Should_Get_Localized_Numbers()
    {
        Assert.That(await _renderer.RenderAsync(
            Templates.ShowDecimalNumber,
            new Dictionary<string, decimal>(new List<KeyValuePair<string, decimal>> {new KeyValuePair<string, decimal>("amount", 123.45M)}),
            cultureName: "en"
        ), Is.EqualTo("*BEGIN*123.45*END*"));

        Assert.That(await _renderer.RenderAsync(
            Templates.ShowDecimalNumber,
            new Dictionary<string, decimal>(new List<KeyValuePair<string, decimal>> {new KeyValuePair<string, decimal>("amount", 123.45M)}),
            cultureName: "de"
        ), Is.EqualTo("*BEGIN*123,45*END*"));
    }

    private class WelcomeEmailModel
    {
        public string Name { get; set; }

        public WelcomeEmailModel(string name)
        {
            Name = name;
        }
    }

    private class SayHelloEmailModel
    {
        public string Name { get; set; }

        public SayHelloEmailModel(string name)
        {
            Name = name;
        }
    }
}
