using System;
using System.Threading.Tasks;
using Axuno.TextTemplating.VirtualFiles;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace Axuno.TextTemplating.Tests.VirtualFiles;

[TestFixture]
public class VirtualFileTemplateContributorTests
{
    private readonly IServiceProvider _services;
    private readonly ITemplateDefinitionManager _templateDefinitionManager;
    private readonly VirtualFileTemplateContentContributor _virtualFileTemplateContentContributor;

    public VirtualFileTemplateContributorTests()
    {
        _services = ServiceSetup.GetTextTemplatingServiceProvider();
        _templateDefinitionManager = _services.GetRequiredService<ITemplateDefinitionManager>();
        _virtualFileTemplateContentContributor = _services.GetRequiredService<VirtualFileTemplateContentContributor>();
    }

    [Test]
    public async Task Should_Get_Localized_Content_By_Culture()
    {
        Assert.AreEqual("Welcome {{model.name}} to Axuno.TextTemplating!",
            await _virtualFileTemplateContentContributor.GetAsync(
                new TemplateContentContributorContext(_templateDefinitionManager.Get(Templates.WelcomeEmail)!,
                    _services,
                    "en")));
            
        Assert.AreEqual("Willkommen, {{model.name}}, bei Axuno.TextTemplating!",
            await _virtualFileTemplateContentContributor.GetAsync(
                new TemplateContentContributorContext(_templateDefinitionManager.Get(Templates.WelcomeEmail)!,
                    _services,
                    "de")));
    }

    [Test]
    public async Task Should_Get_Non_Localized_Template_Content()
    {
        Assert.AreEqual("{{ L \"HelloText\" model.name}}, {{ L \"HowAreYou\" }}",
            await _virtualFileTemplateContentContributor.GetAsync(
                new TemplateContentContributorContext(_templateDefinitionManager.Get(Templates.SayHelloEmail)!,
                    _services,
                    null)));
    }
}