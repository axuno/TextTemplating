using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using NUnit.Framework;

namespace Axuno.TextTemplating.Tests
{
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
            Assert.Contains(typeof(TestTemplateDefinitionProvider), _options.DefinitionProviders.ToList());
        }
    }
}
