# Axuno.TextTemplating

Text templating is used to dynamically render contents based on a template and a model.

* It is based on the [Scriban library](https://github.com/lunet-io/scriban), so it supports conditional logics, loops and much more.
* Template content can be localized.
* You can define layout templates to be used as the layout while rendering other templates.
* You can pass arbitrary objects to the template context (beside the model) for advanced scenarios.

The library is a modified version of [Volo.Abp.TextTemplating](https://github.com/abpframework/abp/tree/dev/framework/src/Volo.Abp.TextTemplating/Volo/Abp/TextTemplating) 3.3.1
Modifications to the source code were made by axuno in 2020. Changes focused on:

* decouple Volo.Abp.TextTemplating from all dependencies of the Abp Framework
* replace the dependency Volo.Abp.VirtualFileSystem with a modified forked version of it ([Axuno.VirtualFileSystem](https://github.com/axuno/Axuno.VirtualFileSystem))
* use Microsoft DependencyInjection instead of [AutoFac](https://autofac.org/)
* use .Net resource files for inline localization instead JSON files
* change of namespaces
