using System;
using System.Collections.Generic;

namespace Axuno.TextTemplating;

public class TemplateDefinition
{
    /// <summary>
    /// CTOR.
    /// </summary>
    /// <param name="name"></param>
    /// <param name="localizationResource"></param>
    /// <param name="isLayout"></param>
    /// <param name="layout"></param>
    /// <param name="defaultCultureName"></param>
    public TemplateDefinition(
        string? name,
        Type? localizationResource = null,
        bool isLayout = false,
        string? layout = null,
        string? defaultCultureName = null)
    {
        Name = Check.NotNullOrWhiteSpace(name, nameof(name), MaxNameLength);
        LocalizationResource = localizationResource;
        IsLayout = isLayout;
        Layout = layout;
        DefaultCultureName = defaultCultureName;
        Properties = new Dictionary<string, object?>();
    }

    /// <summary>
    /// The maximum length allowed for a template name.
    /// </summary>
    public const int MaxNameLength = 128;

    /// <summary>
    /// Gets the template name.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Returns <see langword="true"/> if the template is a layout template, else <see langword="false"/>
    /// </summary>
    public bool IsLayout { get; }

    /// <summary>
    /// Gets or sets the layout to use for this template.
    /// </summary>
    public string? Layout { get; set; }

    /// <summary>
    /// If <see langword="true"/> the virtual path targets a template file,
    /// if <see langword="false"/> the virtual path targets a directory, which should contain template files with the culture name (e.g. "en.tpl").
    /// </summary>
    public bool IsInlineLocalized { get; set; }

    /// <summary>
    /// Gets or sets the localization resource type to use for this template.
    /// </summary>
    public Type? LocalizationResource { get; set; }

    /// <summary>
    /// Gets the default culture name.
    /// </summary>
    public string? DefaultCultureName { get; }

    /// <summary>
    /// Gets or sets a key-value of the <see cref="Properties"/> dictionary.
    /// </summary>
    /// <param name="name">The name of the property.</param>
    /// <returns>
    /// Returns the value in the <see cref="Properties"/> dictionary by given index,
    /// or <see langword="null"/> if the given index is not present in the <see cref="Properties"/> dictionary.
    /// </returns>
    public object? this[string name]
    {
        get => Properties.ContainsKey(name) ? Properties[name] : null;
        set => Properties[name] = value;
    }

    /// <summary>
    /// Can be used to get or set custom properties for this template.
    /// </summary>
    public Dictionary<string, object?> Properties { get; }

    /// <summary>
    /// Sets a property in the <see cref="Properties"/> dictionary.
    /// This is a shortcut for nested calls on this object.
    /// </summary>
    public virtual TemplateDefinition WithProperty(string key, object value)
    {
        Properties[key] = value;
        return this;
    }
}
