﻿using System;
using System.Globalization;

namespace Axuno.TextTemplating.Localization;

public class CultureSwitcher : IDisposable
{
    private readonly CultureInfo _originalCulture;
    private readonly CultureInfo _originalUiCulture;

    public CultureSwitcher(CultureInfo culture, CultureInfo uiCulture)
    {
        _originalCulture = CultureInfo.CurrentCulture;
        _originalUiCulture = CultureInfo.CurrentUICulture;
        SetCulture(culture, uiCulture);
    }

    private void SetCulture(CultureInfo culture, CultureInfo uiCulture)
    {
        CultureInfo.CurrentCulture = culture;
        CultureInfo.CurrentUICulture = uiCulture;
    }

    public void Dispose()
    {
        SetCulture(_originalCulture, _originalUiCulture);
    }
}