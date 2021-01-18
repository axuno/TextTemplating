﻿using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using Axuno.TextTemplating.Tests.Localization;
using Axuno.VirtualFileSystem;

namespace Axuno.TextTemplating.Tests
{
    class ServiceSetup
    {
        public static ServiceProvider GetTextTemplatingServiceProvider()
        {
            return new ServiceCollection()
                .AddLocalization(o => { })
                .AddTextTemplatingModule(vfs =>
                    {
                        // The complete Templates folder is embedded in the project file
                        vfs.FileSets.AddEmbedded<ServiceSetup>("Axuno.TextTemplating.Tests.Templates");
                        vfs.FileSets.AddPhysical(Path.Combine(DirectoryLocator.GetTargetProjectPath(typeof(ServiceSetup)), "Templates"));
                    },
                    locOpt =>
                    {
                    })
                .BuildServiceProvider();
        }
    }
}
