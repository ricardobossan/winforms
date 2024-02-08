using System.Drawing;

namespace System.Windows.Forms.Tests;

public abstract class AbstractButtonBaseTests
{
    protected abstract ButtonBase CreateButton();

    [WinFormsFact]
    public virtual void ButtonBase_Click_RaisesClickEvent()
    {
        using var button = (Button)CreateButton();
        bool clickEventRaised = false;
        button.Click += (sender, e) => clickEventRaised = true;
        button.PerformClick();

        Assert.True(clickEventRaised);
    }

    [WinFormsTheory]
    [InlineData(-1)]
    [InlineData(0)]
    [InlineData(1)]
    public virtual unsafe void Button_Flat_ValidBorder(int borderSize)
    {
        using var control = CreateButton();
        control.FlatStyle = FlatStyle.Flat;

        Assert.Throws<NotSupportedException>(() => control.FlatAppearance.BorderColor = Color.Transparent);

        if (borderSize < 0)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => control.FlatAppearance.BorderSize = borderSize);
        }
        else
        {
            control.FlatAppearance.BorderSize = borderSize;
            Assert.Equal(borderSize, control.FlatAppearance.BorderSize);
        }
    }

    [WinFormsTheory]
    [InlineData(255, 0, 0)]
    [InlineData(0, 255, 0)]
    [InlineData(0, 0, 255)]
    public virtual unsafe void Button_Flat_ProperColor(int red, int green, int blue)
    {
        Color expectedColor = Color.FromArgb(red, green, blue);

        using var control = CreateButton();
        control.FlatStyle = FlatStyle.Flat;
        control.BackColor = expectedColor;

        control.FlatAppearance.CheckedBackColor = expectedColor;
        control.FlatAppearance.BorderColor = expectedColor;

        Assert.Equal(expectedColor, control.BackColor);
        Assert.Equal(expectedColor, control.FlatAppearance.BorderColor);
        Assert.Equal(expectedColor, control.FlatAppearance.CheckedBackColor);
    }
}
