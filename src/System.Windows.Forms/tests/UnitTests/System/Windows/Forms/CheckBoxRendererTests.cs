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
    public void CheckBoxRenderer_DrawCheckBox(CheckBoxState cBState)
    {
        using Form form = new Form();
        using CheckBox control = new();
        form.Controls.Add(control);

        form.Handle.Should().NotBe(IntPtr.Zero);

        using EmfScope emf = new();
        DeviceContextState state = new(emf);
        using Graphics graphics = Graphics.FromHdc((IntPtr)emf.HDC);

        Point point = new(control.Location.X, control.Location.Y);
        Rectangle bounds = control.Bounds;

        CheckBoxRenderer.DrawCheckBox(graphics, point, bounds, control.Text, SystemFonts.DefaultFont, false, cBState);

        if (Application.RenderWithVisualStyles)
        {
            emf.Validate(
                state,
                Application.RenderWithVisualStyles
                    ? Validate.SkipType(ENHANCED_METAFILE_RECORD_TYPE.EMR_ALPHABLEND)
                    : Validate.Repeat(Validate.SkipType(ENHANCED_METAFILE_RECORD_TYPE.EMR_STRETCHDIBITS), 1)
            );
    }
    }

    [WinFormsTheory]
    [InlineData(CheckBoxState.CheckedNormal)]
    [InlineData(CheckBoxState.MixedNormal)]
    public void CheckBoxRenderer_DrawCheckBox_OverloadWithSizeAndText(CheckBoxState cBState)
    {
        using Form form = new Form();
        using CheckBox control = new();
        form.Controls.Add(control);

        form.Handle.Should().NotBe(IntPtr.Zero);

        using EmfScope emf = new();
        DeviceContextState state = new(emf);
        using Graphics graphics = Graphics.FromHdc((IntPtr)emf.HDC);

        Point point = new(control.Location.X, control.Location.Y);
        Rectangle bounds = control.Bounds;
        control.Text = "Text";

        CheckBoxRenderer.DrawCheckBox(graphics, point, bounds, control.Text, SystemFonts.DefaultFont, false, cBState);

        emf.Validate(
            state,
           Application.RenderWithVisualStyles
               ? Validate.SkipType(ENHANCED_METAFILE_RECORD_TYPE.EMR_ALPHABLEND)
               : Validate.Repeat(Validate.SkipType(ENHANCED_METAFILE_RECORD_TYPE.EMR_STRETCHDIBITS), 1),
            Validate.TextOut(
                    control.Text,
                    bounds: new Rectangle(41, 5, 20, 12),
                    State.FontFace(SystemFonts.DefaultFont.Name)
            )
        );
    }

    [WinFormsTheory]
    [InlineData(TextFormatFlags.Default, CheckBoxState.CheckedNormal)]
    [InlineData(TextFormatFlags.Default, CheckBoxState.MixedNormal)]
    [InlineData(TextFormatFlags.GlyphOverhangPadding, CheckBoxState.MixedHot)]
    [InlineData(TextFormatFlags.PreserveGraphicsTranslateTransform, CheckBoxState.CheckedPressed)]
    [InlineData(TextFormatFlags.TextBoxControl, CheckBoxState.UncheckedNormal)]
    public void CheckBoxRenderer_DrawCheckBox_VisualStyleOn_OverloadWithTextFormat(TextFormatFlags textFormat, CheckBoxState cBState)
    {
        using Form form = new Form();
        using CheckBox control = new();
        form.Controls.Add(control);

        form.Handle.Should().NotBe(IntPtr.Zero);

    [WinFormsFact]
    public unsafe void CaptureButton()
    {
        using CheckBox checkBox = (CheckBox)CreateButton();
        using EmfScope emf = new();
        DeviceContextState state = new(emf);
        using Graphics graphics = Graphics.FromHdc((IntPtr)emf.HDC);

        Point point = new(control.Location.X, control.Location.Y);
        Rectangle bounds = control.Bounds;
        control.Text = "Text";

        CheckBoxRenderer.DrawCheckBox(graphics, point, bounds, control.Text, SystemFonts.DefaultFont, textFormat, false, cBState);

        emf.Validate(
            state,
           Application.RenderWithVisualStyles
               ? Validate.SkipType(ENHANCED_METAFILE_RECORD_TYPE.EMR_ALPHABLEND)
               : Validate.Repeat(Validate.SkipType(ENHANCED_METAFILE_RECORD_TYPE.EMR_STRETCHDIBITS), 1),
            Validate.TextOut(
                    control.Text,
                    bounds: new Rectangle(3, 0, 20, 12),
                    State.FontFace(SystemFonts.DefaultFont.Name)
            )
        );
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
