﻿using System;
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
            Assert.That(welcomeEmailTemplate?.Name, Is.EqualTo(Templates.WelcomeEmail));
            Assert.That(welcomeEmailTemplate?.IsInlineLocalized, Is.False);
            
            var sayHelloTemplate = _templateDefinitionManager.Get(Templates.SayHelloEmail);
            Assert.That(sayHelloTemplate?.Name, Is.EqualTo(Templates.SayHelloEmail));
            Assert.That(sayHelloTemplate?.IsInlineLocalized, Is.True);
        }

        [Test]
        public void Should_Get_Null_If_Template_Not_Found()
        {
            Assert.That(_templateDefinitionManager.Get("undefined-template"), Is.Null);
        }

        [Test]
        public void Should_Retrieve_All_Template_Definitions()
        {
            Assert.That(_templateDefinitionManager.GetAll().Count > 1);
        }
    }
}
