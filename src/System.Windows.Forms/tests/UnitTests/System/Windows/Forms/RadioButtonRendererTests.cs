// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Drawing;
using System.Windows.Forms.Metafiles;
using System.Windows.Forms.VisualStyles;

namespace System.Windows.Forms.Tests;

public class RadioButtonRendererTests : AbstractButtonBaseTests
{
    [WinFormsTheory]
    [InlineData(RadioButtonState.CheckedNormal)]
    [InlineData(RadioButtonState.CheckedPressed)]
    public void RadioButtonRenderer_DrawRadioButton(RadioButtonState state)
    {
        using Bitmap bitmap = new(100, 100);
        using Graphics graphics = Graphics.FromImage(bitmap);
        Point point = new Point(10, 20);

        RadioButtonRenderer.DrawRadioButton(graphics, point, state);
    }

    [WinFormsTheory]
    [InlineData(RadioButtonState.CheckedNormal)]
    [InlineData(RadioButtonState.CheckedPressed)]
    public void RadioButtonRenderer_DrawRadioButton_OverloadWithSizeAndText(RadioButtonState state)
    {
        using Bitmap bitmap = new(100, 100);
        using Graphics graphics = Graphics.FromImage(bitmap);
        Point point = new Point(10, 20);
        Rectangle bounds = new Rectangle(10, 20, 30, 40);

        RadioButtonRenderer.DrawRadioButton(graphics, point, bounds, "Text", SystemFonts.DefaultFont, false, state);
    }

    [WinFormsTheory]
    [InlineData(TextFormatFlags.Default, RadioButtonState.CheckedNormal)]
    [InlineData(TextFormatFlags.Default, RadioButtonState.CheckedPressed)]
    [InlineData(TextFormatFlags.PreserveGraphicsTranslateTransform, RadioButtonState.CheckedPressed)]
    [InlineData(TextFormatFlags.TextBoxControl, RadioButtonState.UncheckedNormal)]
    public void RadioButtonRenderer_DrawRadioButton_OverloadWithTextFormat(TextFormatFlags textFormat,
        RadioButtonState state)
    {
        using Bitmap bitmap = new(100, 100);
        using Graphics graphics = Graphics.FromImage(bitmap);
        Point point = new Point(10, 20);
        Rectangle bounds = new Rectangle(10, 20, 30, 40);

        RadioButtonRenderer.DrawRadioButton(graphics, point, bounds, "Text", SystemFonts.DefaultFont, textFormat, false,
            state);
    }

    [WinFormsFact]
    public unsafe void CaptureButton()
    {
        using RadioButton radioButton = (RadioButton)CreateButton();
        using EmfScope emf = new();
        radioButton.PrintToMetafile(emf);

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

        using RadioButton radioButton = (RadioButton)CreateButton();
        using EmfScope emf = new();
        DeviceContextState state = new(emf);
        Rectangle bounds = radioButton.Bounds;

        radioButton.PrintToMetafile(emf);

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

        using RadioButton radioButton = (RadioButton)CreateButton();
        radioButton.Text = "Hello";
        using EmfScope emf = new();
        DeviceContextState state = new(emf);
        Rectangle bounds = radioButton.Bounds;

        radioButton.PrintToMetafile(emf);

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
        using RadioButton radioButton = (Forms.RadioButton)CreateButton();
        form.Controls.Add(radioButton);

        using EmfScope emf = new();
        form.PrintToMetafile(emf);

        string details = emf.RecordsToString();
    }

    protected override ButtonBase CreateButton() => new RadioButton();
}
