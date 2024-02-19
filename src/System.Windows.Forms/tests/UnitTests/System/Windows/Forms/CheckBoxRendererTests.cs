// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Drawing;
using System.Windows.Forms.Metafiles;
using System.Windows.Forms.VisualStyles;

namespace System.Windows.Forms.Tests;

public class CheckBoxRendererTests : AbstractButtonBaseTests
{
    [WinFormsTheory]
    [InlineData(CheckBoxState.CheckedNormal)]
    [InlineData(CheckBoxState.MixedNormal)]
    public void CheckBoxRenderer_DrawCheckBox(CheckBoxState state)
    {
        using Bitmap bitmap = new(100, 100);
        using Graphics graphics = Graphics.FromImage(bitmap);
        Point point = new(10, 20);

        CheckBoxRenderer.DrawCheckBox(graphics, point, state);
    }

    [WinFormsTheory]
    [InlineData(CheckBoxState.CheckedNormal)]
    [InlineData(CheckBoxState.MixedNormal)]
    public void CheckBoxRenderer_DrawCheckBox_OverloadWithSizeAndText(CheckBoxState state)
    {
        using Bitmap bitmap = new(100, 100);
        using Graphics graphics = Graphics.FromImage(bitmap);
        Point point = new(10, 20);
        Rectangle bounds = new(10, 20, 30, 40);

        CheckBoxRenderer.DrawCheckBox(graphics, point, bounds, "Text", SystemFonts.DefaultFont, false, state);
    }

    [WinFormsTheory]
    [InlineData(TextFormatFlags.Default, CheckBoxState.CheckedNormal)]
    [InlineData(TextFormatFlags.Default, CheckBoxState.MixedNormal)]
    [InlineData(TextFormatFlags.GlyphOverhangPadding, CheckBoxState.MixedHot)]
    [InlineData(TextFormatFlags.PreserveGraphicsTranslateTransform, CheckBoxState.CheckedPressed)]
    [InlineData(TextFormatFlags.TextBoxControl, CheckBoxState.UncheckedNormal)]
    public void CheckBoxRenderer_DrawCheckBox_OverloadWithTextFormat(TextFormatFlags textFormat, CheckBoxState state)
    {
        using Bitmap bitmap = new(100, 100);
        using Graphics graphics = Graphics.FromImage(bitmap);
        Point point = new(10, 20);
        Rectangle bounds = new(10, 20, 30, 40);

        CheckBoxRenderer.DrawCheckBox(graphics, point, bounds, "Text", SystemFonts.DefaultFont, textFormat, focused: false, state);
    }

    [WinFormsFact]
    public unsafe void CaptureButton()
    {
        using CheckBox checkBox = (CheckBox)CreateButton();
        using EmfScope emf = new();
        checkBox.PrintToMetafile(emf);

        List<ENHANCED_METAFILE_RECORD_TYPE> types = [];
        List<string> details = [];
        emf.Enumerate((ref EmfRecord record) =>
        {
            types.Add(record.Type);
            details.Add(record.ToString());
            return true;
        });
    }

    [WinFormsFact]
    public unsafe void Button_VisualStyles_off_Default_LineDrawing()
    {
        if (Application.RenderWithVisualStyles)
        {
            return;
        }

        using CheckBox checkBox = (CheckBox)CreateButton();
        using EmfScope emf = new();
        DeviceContextState state = new(emf);
        Rectangle bounds = checkBox.Bounds;

        checkBox.PrintToMetafile(emf);

        emf.Validate(
            state,
            Validate.Repeat(Validate.SkipType(ENHANCED_METAFILE_RECORD_TYPE.EMR_BITBLT), 1),
            Validate.LineTo(
                new(bounds.Right - 1, 0), new(0, 0),
                State.PenColor(SystemColors.ControlLightLight)),
            Validate.LineTo(
                new(0, 0), new(0, bounds.Bottom - 1),
                State.PenColor(SystemColors.ControlLightLight)),
            Validate.LineTo(
                new(0, bounds.Bottom - 1), new(bounds.Right - 1, bounds.Bottom - 1),
                State.PenColor(SystemColors.ControlDarkDark)),
            Validate.LineTo(
                new(bounds.Right - 1, bounds.Bottom - 1), new(bounds.Right - 1, -1),
                State.PenColor(SystemColors.ControlDarkDark)),
            Validate.LineTo(
                new(bounds.Right - 2, 1), new(1, 1),
                State.PenColor(SystemColors.Control)),
            Validate.LineTo(
                new(1, 1), new(1, bounds.Bottom - 2),
                State.PenColor(SystemColors.Control)),
            Validate.LineTo(
                new(1, bounds.Bottom - 2), new(bounds.Right - 2, bounds.Bottom - 2),
                State.PenColor(SystemColors.ControlDark)),
            Validate.LineTo(
                new(bounds.Right - 2, bounds.Bottom - 2), new(bounds.Right - 2, 0),
                State.PenColor(SystemColors.ControlDark)));
    }

    [WinFormsFact]
    public unsafe void Button_VisualStyles_off_Default_WithText_LineDrawing()
    {
        if (Application.RenderWithVisualStyles)
        {
            return;
        }

        using CheckBox checkBox = (CheckBox)CreateButton();
        checkBox.Text = "Hello";
        using EmfScope emf = new();
        DeviceContextState state = new(emf);
        Rectangle bounds = checkBox.Bounds;

        checkBox.PrintToMetafile(emf);

        emf.Validate(
            state,
            Validate.SkipType(ENHANCED_METAFILE_RECORD_TYPE.EMR_BITBLT),
            Validate.TextOut("Hello"),
            Validate.LineTo(
                new(bounds.Right - 1, 0), new(0, 0),
                State.PenColor(SystemColors.ControlLightLight)),
            Validate.LineTo(
                new(0, 0), new(0, bounds.Bottom - 1),
                State.PenColor(SystemColors.ControlLightLight)),
            Validate.LineTo(
                new(0, bounds.Bottom - 1), new(bounds.Right - 1, bounds.Bottom - 1),
                State.PenColor(SystemColors.ControlDarkDark)),
            Validate.LineTo(
                new(bounds.Right - 1, bounds.Bottom - 1), new(bounds.Right - 1, -1),
                State.PenColor(SystemColors.ControlDarkDark)),
            Validate.LineTo(
                new(bounds.Right - 2, 1), new(1, 1),
                State.PenColor(SystemColors.Control)),
            Validate.LineTo(
                new(1, 1), new(1, bounds.Bottom - 2),
                State.PenColor(SystemColors.Control)),
            Validate.LineTo(
                new(1, bounds.Bottom - 2), new(bounds.Right - 2, bounds.Bottom - 2),
                State.PenColor(SystemColors.ControlDark)),
            Validate.LineTo(
                new(bounds.Right - 2, bounds.Bottom - 2), new(bounds.Right - 2, 0),
                State.PenColor(SystemColors.ControlDark)));
    }

    [WinFormsFact]
    public unsafe void CaptureButtonOnForm()
    {
        using Form form = new();
        using CheckBox checkBox = (CheckBox)CreateButton();
        form.Controls.Add(checkBox);

        using EmfScope emf = new();
        form.PrintToMetafile(emf);

        string details = emf.RecordsToString();
    }

    protected override ButtonBase CreateButton() => new CheckBox();
}
