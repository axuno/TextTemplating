﻿using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;

namespace Axuno.TextTemplating;

public interface ITemplateRenderer
{
    /// <summary>
    /// Renders a text template.
    /// </summary>
    /// <param name="templateName">The template name</param>
    /// <param name="model">An optional model object that is used in the template.</param>
    /// <param name="cultureName">Culture name. Uses the <see cref="CultureInfo.CurrentUICulture"/> if not specified</param>
    /// <param name="globalContext">A dictionary which can be used to import global objects to the template</param>
    /// <returns>Returns the rendered text template.</returns>
    Task<string> RenderAsync(string templateName,
        object? model = null,
        string? cultureName = null,
        Dictionary<string, object>? globalContext = null);
}
