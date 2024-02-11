using System.Drawing;

namespace System.Windows.Forms.Tests;

public abstract class AbstractButtonBaseTests
{
    protected abstract ButtonBase CreateButton();

    public virtual void ButtonBase_Click_RaisesClickEvent()
    {
        using var button = (Button)CreateButton();
        bool clickEventRaised = false;
        button.Click += (sender, e) => clickEventRaised = true;
        button.PerformClick();

        Assert.True(clickEventRaised);
    }

    public virtual void ButtonBase_FlatStyle_ValidFlatButtonBorder(int borderSize)
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

    public virtual void ButtonBase_FlatStyle_ProperFlatButtonColor(int red, int green, int blue)
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

    public virtual void ButtonBase_OverChangeRectangle_Get(Type controlType, Appearance appearance, FlatStyle flatStyle)
    {
        dynamic control = Activator.CreateInstance(controlType);

        if (control == null)
        {
            return;
        }

        control.Appearance = appearance;
        control.FlatStyle = flatStyle;

        Rectangle overChangeRectangle;

        // ButtonBase.Adapter prohibits this
        if (appearance == Appearance.Normal && (flatStyle != FlatStyle.Standard && flatStyle != FlatStyle.Popup &&
                                                flatStyle != FlatStyle.Flat))
        {
            Assert.ThrowsAny<Exception>(() => overChangeRectangle = control.OverChangeRectangle);

            return;
        }

        overChangeRectangle = control.OverChangeRectangle;

        if (control.FlatStyle == FlatStyle.Standard)
        {
            Assert.True(overChangeRectangle == new Rectangle(-1, -1, 1, 1));
        }

        if (control.Appearance == Appearance.Button)
        {
            if (control.FlatStyle != FlatStyle.Standard)
            {
                Assert.True(overChangeRectangle == control.ClientRectangle);
            }
        }
        else if (control.FlatStyle != FlatStyle.Standard)
        {
            Assert.True(overChangeRectangle == control.Adapter.CommonLayout().Layout().CheckBounds);
        }
    }
}
