using System;
using System.IO;
using System.Threading.Tasks;
using Axuno.TextTemplating.VirtualFiles;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Primitives;
using NUnit.Framework;

namespace Axuno.TextTemplating.Tests.VirtualFiles;

[TestFixture]
public class LocalizedTemplateContentReaderFactoryTests
{
    private readonly ITemplateDefinitionManager _templateDefinitionManager;

    public LocalizedTemplateContentReaderFactoryTests()
    {
        IServiceProvider services = ServiceSetup.GetTextTemplatingServiceProvider();
        _templateDefinitionManager = services.GetRequiredService<ITemplateDefinitionManager>();
            
    }

    [Test]
    public async Task Create_Should_Work_With_PhysicalFileProvider()
    {
        var localizedTemplateContentReaderFactory = new LocalizedTemplateContentReaderFactory(
            new TestPhysicalVirtualFileProvider(
                new PhysicalFileProvider(Path.Combine(DirectoryLocator.GetTargetProjectPath(typeof(ServiceSetup)), "Templates"))));

        var reader = await localizedTemplateContentReaderFactory.CreateAsync(_templateDefinitionManager.Get(Templates.WelcomeEmail)!);

        Assert.That(reader.GetContent("en"), Is.EqualTo("Welcome {{model.name}} to Axuno.TextTemplating!"));
        Assert.That(reader.GetContent("de"), Is.EqualTo("Willkommen, {{model.name}}, bei Axuno.TextTemplating!"));
    }

    private class TestPhysicalVirtualFileProvider : IFileProvider
    {
        private readonly PhysicalFileProvider _physicalFileProvider;

        public TestPhysicalVirtualFileProvider(PhysicalFileProvider physicalFileProvider)
        {
            _physicalFileProvider = physicalFileProvider;
        }

        public IFileInfo GetFileInfo(string subPath)
        {
            return _physicalFileProvider.GetFileInfo(subPath);
        }

        public IDirectoryContents GetDirectoryContents(string subPath)
        {
            return _physicalFileProvider.GetDirectoryContents(subPath);
        }

        public IChangeToken Watch(string filter)
        {
            return _physicalFileProvider.Watch(filter);
        }
    }
}
