using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace Axuno.TextTemplating.Tests
{
    [TestFixture]
    public class TemplateDefinitionTests
    {
        private readonly ITemplateDefinitionManager _templateDefinitionManager;

        public TemplateDefinitionTests()
        {
            IServiceProvider services = ServiceSetup.GetTextTemplatingServiceProvider();
            _templateDefinitionManager = services.GetRequiredService<ITemplateDefinitionManager>();
        }

        [Test]
        public void Should_Retrieve_Template_Definition_By_Name()
        {
            var welcomeEmailTemplate = _templateDefinitionManager.Get(Templates.WelcomeEmail);
            Assert.AreEqual(Templates.WelcomeEmail, welcomeEmailTemplate?.Name);
            Assert.IsFalse(welcomeEmailTemplate?.IsInlineLocalized);
            
            var sayHelloTemplate = _templateDefinitionManager.Get(Templates.SayHelloEmail);
            Assert.AreEqual(Templates.SayHelloEmail, sayHelloTemplate?.Name);
            Assert.IsTrue(sayHelloTemplate?.IsInlineLocalized);
        }

        [Test]
        public void Should_Get_Null_If_Template_Not_Found()
        {
            Assert.IsNull(_templateDefinitionManager.Get("undefined-template"));
        }

        [Test]
        public void Should_Retrieve_All_Template_Definitions()
        {
            Assert.That(_templateDefinitionManager.GetAll().Count > 1);
        }
    }
}
