using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using NUnit.Framework;

namespace Axuno.TextTemplating.Tests;

[TestFixture]
public class TextTemplatingOptionsTests
{
    private readonly TextTemplatingOptions _options;

    public TextTemplatingOptionsTests()
    {
        IServiceProvider services = ServiceSetup.GetTextTemplatingServiceProvider();
        _options = services.GetRequiredService<IOptions<TextTemplatingOptions>>().Value;
    }

    [Test]
    public void Should_Auto_Add_TemplateDefinitionProviders_To_Options()
    {
        Assert.That(_options.DefinitionProviders.ToList(), Does.Contain(typeof(TestTemplateDefinitionProvider)));
    }
}
