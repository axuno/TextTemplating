using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace Axuno.TextTemplating.Tests;

[TestFixture]
public class TemplateRendererTests
{
    private readonly IServiceProvider _services;
    private readonly ITemplateRenderer _renderer;
        
    public TemplateRendererTests()
    {
        _services = ServiceSetup.GetTextTemplatingServiceProvider();
        _renderer = _services.GetRequiredService<ITemplateRenderer>();
    }

    [Test]
    public async Task Should_Get_Rendered_With_CultureSpecific_Template()
    {
        Assert.AreEqual("Welcome John to Axuno.TextTemplating!", await _renderer.RenderAsync(
            Templates.WelcomeEmail,
            model: new
            {
                name = "John"
            },
            cultureName: "en"
        ));

        Assert.AreEqual("Willkommen, John, bei Axuno.TextTemplating!", await _renderer.RenderAsync(
            Templates.WelcomeEmail,
            model: new
            {
                name = "John"
            },
            cultureName: "de"
        ));
    }

    [Test]
    public async Task Should_Use_Fallback_Culture_CultureSpecific_Template()
    {
        //"en-US" falls back to "en" since "en-US" doesn't exists and "en" is the fallback culture
        Assert.AreEqual("Welcome John to Axuno.TextTemplating!", await _renderer.RenderAsync(
            Templates.WelcomeEmail,
            model: new
            {
                name = "John"
            },
            cultureName: "en_US"
        ));
            
        //"es" falls back to "en" since "es" doesn't exists and "en" is the fallback culture
        Assert.AreEqual("Welcome John to Axuno.TextTemplating!", await _renderer.RenderAsync(
            Templates.WelcomeEmail,
            model: new
            {
                name = "John"
            },
            cultureName: "es"
        ));
    }

    [Test]
    public async Task Should_Get_Rendered_Localized_Template_Content_With_Strongly_Typed_Model()
    {
        Assert.AreEqual("Welcome John to Axuno.TextTemplating!", await _renderer.RenderAsync(
            Templates.WelcomeEmail,
            model: new WelcomeEmailModel("John"),
            cultureName: "en"
        ));
    }
        
    [Test]
    public async Task Should_Get_Rendered_Localized_Template_Content_With_Dictionary_Model()
    {
        Assert.AreEqual("Welcome John to Axuno.TextTemplating!", await _renderer.RenderAsync(
            Templates.WelcomeEmail,
            model: new Dictionary<string, object> { { "name", "John" } },
            cultureName: "en"
        ));
    }

    [Test]
    public async Task Should_Get_Rendered_Inline_Localized_Template()
    {
        Assert.AreEqual("*BEGIN*Hello John, how are you?*END*", await _renderer.RenderAsync(
            Templates.SayHelloEmail,
            new SayHelloEmailModel("John"),
            cultureName: "en"
        ));

        Assert.AreEqual("*BEGIN*Bonjour John, comment ça va?*END*", await _renderer.RenderAsync(
            Templates.SayHelloEmail,
            new SayHelloEmailModel("John"),
            cultureName: "fr"
        ));
            
        Assert.AreEqual("*BEGIN*Hallo John, wie geht es Dir?*END*", await _renderer.RenderAsync(
            Templates.SayHelloEmail,
            new SayHelloEmailModel("John"),
            cultureName: "de"
        ));
    }

    [Test]
    public async Task Should_Get_Localized_Numbers()
    {
        Assert.AreEqual("*BEGIN*123.45*END*", await _renderer.RenderAsync(
            Templates.ShowDecimalNumber,
            new Dictionary<string, decimal>(new List<KeyValuePair<string, decimal>> {new KeyValuePair<string, decimal>("amount", 123.45M)}),
            cultureName: "en"
        ));
            
        Assert.AreEqual("*BEGIN*123,45*END*", await _renderer.RenderAsync(
            Templates.ShowDecimalNumber,
            new Dictionary<string, decimal>(new List<KeyValuePair<string, decimal>> {new KeyValuePair<string, decimal>("amount", 123.45M)}),
            cultureName: "de"
        ));
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