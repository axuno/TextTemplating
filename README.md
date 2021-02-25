<img src="https://raw.githubusercontent.com/axuno/Axuno.TextTemplating/main/TextTemplating.png" width="64" alt="Logo">

# Axuno.TextTemplating

[![build + test](https://github.com/axuno/Axuno.TextTemplating/workflows/build%20%2B%20test/badge.svg)](https://github.com/axuno/Axuno.TextTemplating/actions?query=workflow%3A%22build+%2B+test%22)

Text templating is used to dynamically render contents based on a template and a model.

* It is based on the [Scriban library](https://github.com/lunet-io/scriban), a language that supports conditional logics, loops and much more.
* Template content can be localized.
* You can define layout templates to be used as the layout while rendering other templates.
* You can pass arbitrary objects to the template context (beside the model) for advanced scenarios.

The library is a modified version of [Volo.Abp.TextTemplating](https://github.com/abpframework/abp/tree/dev/framework/src/Volo.Abp.TextTemplating/Volo/Abp/TextTemplating) 4.1
Modifications to the source code were made by axuno in 2020/21. Changes focused on:

* decouple Volo.Abp.TextTemplating from all dependencies of the Abp Framework
* replace the dependency Volo.Abp.VirtualFileSystem with a modified forked version of it ([Axuno.VirtualFileSystem](https://github.com/axuno/Axuno.VirtualFileSystem))
* use Microsoft DependencyInjection instead of [AutoFac](https://autofac.org/)
* use .Net resource files for inline localization instead JSON files
* change of namespaces

### Getting started
* [![NuGet](https://img.shields.io/nuget/v/Axuno.TextTemplating.svg)](https://www.nuget.org/packages/Axuno.TextTemplating/) Install the NuGet package
* Run the demo program included in the repo
* Read the [Scriban language](https://github.com/lunet-io/scriban) docs
* Read the [Text Templating wiki](https://github.com/axuno/Axuno.TextTemplating/wiki)
