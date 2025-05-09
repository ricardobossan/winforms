﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable disable

namespace System.Windows.Forms.Tests;

public class ScrollPropertiesTests
{
    public static IEnumerable<object[]> Ctor_ScrollableControl_TestData()
    {
        yield return new object[] { new ScrollableControl() };
        yield return new object[] { null };
    }

    [WinFormsTheory]
    [MemberData(nameof(Ctor_ScrollableControl_TestData))]
    public void ScrollProperties_Ctor_Control(ScrollableControl container)
    {
        SubScrollProperties properties = new(container);
        Assert.Same(container, properties.ParentControl);
        Assert.True(properties.Enabled);
        Assert.Equal(10, properties.LargeChange);
        Assert.Equal(1, properties.SmallChange);
        Assert.Equal(100, properties.Maximum);
        Assert.Equal(0, properties.Minimum);
        Assert.Equal(0, properties.Value);
        Assert.False(properties.Visible);
    }

    [WinFormsTheory]
    [BoolData]
    public void ScrollProperties_Enabled_Set_GetReturnsExpected(bool value)
    {
        using ScrollableControl container = new();
        SubScrollProperties properties = new(container)
        {
            Enabled = value
        };
        Assert.Equal(value, properties.Enabled);

        // Set same.
        properties.Enabled = value;
        Assert.Equal(value, properties.Enabled);

        // Set different.
        properties.Enabled = !value;
        Assert.Equal(!value, properties.Enabled);
    }

    [WinFormsTheory]
    [BoolData]
    public void ScrollProperties_Enabled_SetAutoScrollContainer_Nop(bool value)
    {
        using ScrollableControl container = new()
        {
            AutoScroll = true
        };
        SubScrollProperties properties = new(container)
        {
            Enabled = value
        };
        Assert.True(properties.Enabled);

        // Set same.
        properties.Enabled = value;
        Assert.True(properties.Enabled);

        // Set different.
        properties.Enabled = !value;
        Assert.True(properties.Enabled);
    }

    [WinFormsTheory]
    [BoolData]
    public void ScrollProperties_Enabled_SetNullContainer_GetReturnsExpected(bool value)
    {
        SubScrollProperties properties = new(null)
        {
            Enabled = value
        };
        Assert.Equal(value, properties.Enabled);

        // Set same.
        properties.Enabled = value;
        Assert.Equal(value, properties.Enabled);

        // Set different.
        properties.Enabled = !value;
        Assert.Equal(!value, properties.Enabled);
    }

    public static IEnumerable<object[]> LargeChange_Set_TestData()
    {
        yield return new object[] { 10 };
        yield return new object[] { 12 };
    }

    [WinFormsTheory]
    [MemberData(nameof(LargeChange_Set_TestData))]
    public void ScrollProperties_LargeChange_Set_GetReturnsExpected(int value)
    {
        using ScrollableControl container = new();
        SubScrollProperties properties = new(container)
        {
            LargeChange = value
        };
        Assert.Equal(value, properties.LargeChange);

        // Set same.
        properties.LargeChange = value;
        Assert.Equal(value, properties.LargeChange);
    }

    [WinFormsTheory]
    [MemberData(nameof(LargeChange_Set_TestData))]
    public void ScrollProperties_LargeChange_SetAutoScrollContainer_GetReturnsExpected(int value)
    {
        using ScrollableControl container = new()
        {
            AutoScroll = true
        };
        SubScrollProperties properties = new(container)
        {
            LargeChange = value
        };
        Assert.Equal(value, properties.LargeChange);

        // Set same.
        properties.LargeChange = value;
        Assert.Equal(value, properties.LargeChange);
    }

    [WinFormsTheory]
    [MemberData(nameof(LargeChange_Set_TestData))]
    public void ScrollProperties_LargeChange_SetNullContainer_GetReturnsExpected(int value)
    {
        SubScrollProperties properties = new(null)
        {
            LargeChange = value
        };
        Assert.Equal(value, properties.LargeChange);

        // Set same.
        properties.LargeChange = value;
        Assert.Equal(value, properties.LargeChange);
    }

    [WinFormsFact]
    public void ScrollProperties_LargeChange_SetNegative_ThrowsArgumentOutOfRangeException()
    {
        using ScrollableControl container = new();
        SubScrollProperties properties = new(container);
        Assert.Throws<ArgumentOutOfRangeException>("value", () => properties.LargeChange = -1);
        Assert.Equal(10, properties.LargeChange);
    }

    public static IEnumerable<object[]> SmallChange_Set_TestData()
    {
        yield return new object[] { 1, 1 };
        yield return new object[] { 8, 8 };
        yield return new object[] { 10, 10 };
        yield return new object[] { 12, 10 };
    }

    [WinFormsTheory]
    [MemberData(nameof(SmallChange_Set_TestData))]
    public void ScrollProperties_SmallChange_Set_GetReturnsExpected(int value, int expectedValue)
    {
        using ScrollableControl container = new();
        SubScrollProperties properties = new(container)
        {
            SmallChange = value
        };
        Assert.Equal(expectedValue, properties.SmallChange);

        // Set same.
        properties.SmallChange = value;
        Assert.Equal(expectedValue, properties.SmallChange);
    }

    [WinFormsTheory]
    [MemberData(nameof(SmallChange_Set_TestData))]
    public void ScrollProperties_SmallChange_SetAutoScrollContainer_GetReturnsExpected(int value, int expectedValue)
    {
        using ScrollableControl container = new()
        {
            AutoScroll = true
        };
        SubScrollProperties properties = new(container)
        {
            SmallChange = value
        };
        Assert.Equal(expectedValue, properties.SmallChange);

        // Set same.
        properties.SmallChange = value;
        Assert.Equal(expectedValue, properties.SmallChange);
    }

    [WinFormsTheory]
    [MemberData(nameof(SmallChange_Set_TestData))]
    public void ScrollProperties_SmallChange_SetNullContainer_ReturnsExpected(int value, int expectedValue)
    {
        SubScrollProperties properties = new(null)
        {
            SmallChange = value
        };
        Assert.Equal(expectedValue, properties.SmallChange);

        // Set same.
        properties.SmallChange = value;
        Assert.Equal(expectedValue, properties.SmallChange);
    }

    [WinFormsFact]
    public void ScrollProperties_SmallChange_SetNegative_ThrowsArgumentOutOfRangeException()
    {
        using ScrollableControl container = new();
        SubScrollProperties properties = new(container);
        Assert.Throws<ArgumentOutOfRangeException>("value", () => properties.SmallChange = -1);
        Assert.Equal(1, properties.SmallChange);
    }

    public static IEnumerable<object[]> Maximum_Set_TestData()
    {
        yield return new object[] { 0, 1 };
        yield return new object[] { 8, 9 };
        yield return new object[] { 10, 10 };
        yield return new object[] { 50, 10 };
    }

    [WinFormsTheory]
    [MemberData(nameof(Maximum_Set_TestData))]
    public void ScrollProperties_Maximum_Set_GetReturnsExpected(int value, int expectedLargeChange)
    {
        using ScrollableControl container = new();
        SubScrollProperties properties = new(container)
        {
            Maximum = value
        };
        Assert.Equal(value, properties.Maximum);
        Assert.Equal(0, properties.Minimum);
        Assert.Equal(0, properties.Value);
        Assert.Equal(expectedLargeChange, properties.LargeChange);
        Assert.Equal(1, properties.SmallChange);

        // Set value.
        properties.Maximum = value;
        Assert.Equal(value, properties.Maximum);
        Assert.Equal(0, properties.Minimum);
        Assert.Equal(0, properties.Value);
        Assert.Equal(expectedLargeChange, properties.LargeChange);
        Assert.Equal(1, properties.SmallChange);
    }

    [WinFormsTheory]
    [MemberData(nameof(Maximum_Set_TestData))]
    public void ScrollProperties_Maximum_SetNullContainer_GetReturnsExpected(int value, int expectedLargeChange)
    {
        SubScrollProperties properties = new(null)
        {
            Maximum = value
        };
        Assert.Equal(value, properties.Maximum);
        Assert.Equal(0, properties.Minimum);
        Assert.Equal(0, properties.Value);
        Assert.Equal(expectedLargeChange, properties.LargeChange);
        Assert.Equal(1, properties.SmallChange);

        // Set value.
        properties.Maximum = value;
        Assert.Equal(value, properties.Maximum);
        Assert.Equal(0, properties.Minimum);
        Assert.Equal(0, properties.Value);
        Assert.Equal(expectedLargeChange, properties.LargeChange);
        Assert.Equal(1, properties.SmallChange);
    }

    [WinFormsFact]
    public void ScrollProperties_Maximum_SetLessThanValueAndMinimum_SetsValueAndMinimum()
    {
        using ScrollableControl container = new();
        SubScrollProperties properties = new(container)
        {
            Value = 10,
            Minimum = 8,
            Maximum = 5
        };
        Assert.Equal(5, properties.Maximum);
        Assert.Equal(5, properties.Minimum);
        Assert.Equal(5, properties.Value);
        Assert.Equal(1, properties.LargeChange);
        Assert.Equal(1, properties.SmallChange);
    }

    [WinFormsFact]
    public void ScrollProperties_Maximum_SetNegative_SetsValueAndMinimum()
    {
        using ScrollableControl container = new();
        SubScrollProperties properties = new(container)
        {
            Maximum = -1
        };
        Assert.Equal(-1, properties.Maximum);
        Assert.Equal(-1, properties.Minimum);
        Assert.Equal(-1, properties.Value);
        Assert.Equal(1, properties.LargeChange);
        Assert.Equal(1, properties.SmallChange);
    }

    [WinFormsTheory]
    [InlineData(0)]
    [InlineData(8)]
    public void ScrollProperties_Maximum_SetAutoScrollContainer_Nop(int value)
    {
        using ScrollableControl container = new()
        {
            AutoScroll = true
        };
        SubScrollProperties properties = new(container)
        {
            Maximum = value
        };
        Assert.Equal(100, properties.Maximum);
        Assert.Equal(0, properties.Minimum);
        Assert.Equal(0, properties.Value);
        Assert.Equal(10, properties.LargeChange);
        Assert.Equal(1, properties.SmallChange);
    }

    public static IEnumerable<object[]> Minimum_Set_TestData()
    {
        yield return new object[] { 0 };
        yield return new object[] { 5 };
    }

    [WinFormsTheory]
    [MemberData(nameof(Minimum_Set_TestData))]
    public void ScrollProperties_Minimum_Set_GetReturnsExpected(int value)
    {
        using ScrollableControl container = new();
        SubScrollProperties properties = new(container)
        {
            Value = 5,
            Minimum = value
        };
        Assert.Equal(100, properties.Maximum);
        Assert.Equal(value, properties.Minimum);
        Assert.Equal(5, properties.Value);
        Assert.Equal(10, properties.LargeChange);
        Assert.Equal(1, properties.SmallChange);

        // Set same.
        properties.Minimum = value;
        Assert.Equal(100, properties.Maximum);
        Assert.Equal(value, properties.Minimum);
        Assert.Equal(5, properties.Value);
        Assert.Equal(10, properties.LargeChange);
        Assert.Equal(1, properties.SmallChange);
    }

    [WinFormsFact]
    public void ScrollProperties_Minimum_SetGreaterThanValueAndMaximum_SetsValueAndMinimum()
    {
        using ScrollableControl container = new();
        SubScrollProperties properties = new(container)
        {
            Value = 10,
            Maximum = 8,
            Minimum = 12
        };
        Assert.Equal(12, properties.Maximum);
        Assert.Equal(12, properties.Minimum);
        Assert.Equal(12, properties.Value);
        Assert.Equal(1, properties.LargeChange);
        Assert.Equal(1, properties.SmallChange);
    }

    [WinFormsTheory]
    [InlineData(0)]
    [InlineData(8)]
    public void ScrollProperties_Minimum_SetAutoScrollContainer_Nop(int value)
    {
        using ScrollableControl container = new()
        {
            AutoScroll = true
        };
        SubScrollProperties properties = new(container)
        {
            Minimum = value
        };
        Assert.Equal(100, properties.Maximum);
        Assert.Equal(0, properties.Minimum);
        Assert.Equal(0, properties.Value);
        Assert.Equal(10, properties.LargeChange);
        Assert.Equal(1, properties.SmallChange);
    }

    [WinFormsTheory]
    [MemberData(nameof(Minimum_Set_TestData))]
    public void ScrollProperties_Minimum_SetNullContainer_GetReturnsExpected(int value)
    {
        SubScrollProperties properties = new(null)
        {
            Value = 5,
            Minimum = value
        };
        Assert.Equal(100, properties.Maximum);
        Assert.Equal(value, properties.Minimum);
        Assert.Equal(5, properties.Value);
        Assert.Equal(10, properties.LargeChange);
        Assert.Equal(1, properties.SmallChange);

        // Set same.
        properties.Minimum = value;
        Assert.Equal(100, properties.Maximum);
        Assert.Equal(value, properties.Minimum);
        Assert.Equal(5, properties.Value);
        Assert.Equal(10, properties.LargeChange);
        Assert.Equal(1, properties.SmallChange);
    }

    [WinFormsFact]
    public void ScrollProperties_Minimum_SetNegative_ThrowsArgumentOutOfRangeException()
    {
        using ScrollableControl container = new();
        SubScrollProperties properties = new(container);
        Assert.Throws<ArgumentOutOfRangeException>("value", () => properties.Minimum = -1);
        Assert.Equal(0, properties.Minimum);
    }

    public static IEnumerable<object[]> Value_Set_TestData()
    {
        yield return new object[] { 0 };
        yield return new object[] { 5 };
        yield return new object[] { 100 };
    }

    [WinFormsTheory]
    [MemberData(nameof(Value_Set_TestData))]
    public void ScrollProperties_Value_Set_GetReturnsExpected(int value)
    {
        using ScrollableControl container = new();
        SubScrollProperties properties = new(container)
        {
            Value = value
        };
        Assert.Equal(100, properties.Maximum);
        Assert.Equal(0, properties.Minimum);
        Assert.Equal(value, properties.Value);
        Assert.Equal(10, properties.LargeChange);
        Assert.Equal(1, properties.SmallChange);

        // Set same.
        properties.Value = value;
        Assert.Equal(100, properties.Maximum);
        Assert.Equal(0, properties.Minimum);
        Assert.Equal(value, properties.Value);
        Assert.Equal(10, properties.LargeChange);
        Assert.Equal(1, properties.SmallChange);
    }

    [WinFormsTheory]
    [InlineData(0)]
    [InlineData(5)]
    [InlineData(100)]
    public void ScrollProperties_Value_SetAutoScrollContainer_GetReturnsExpected(int value)
    {
        using ScrollableControl container = new()
        {
            AutoScroll = true
        };
        SubScrollProperties properties = new(container)
        {
            Value = value
        };
        Assert.Equal(100, properties.Maximum);
        Assert.Equal(0, properties.Minimum);
        Assert.Equal(value, properties.Value);
        Assert.Equal(10, properties.LargeChange);
        Assert.Equal(1, properties.SmallChange);
    }

    [WinFormsTheory]
    [MemberData(nameof(Value_Set_TestData))]
    public void ScrollProperties_Value_SetNullContainer_GetReturnsExpected(int value)
    {
        SubScrollProperties properties = new(null)
        {
            Value = value
        };
        Assert.Equal(100, properties.Maximum);
        Assert.Equal(0, properties.Minimum);
        Assert.Equal(value, properties.Value);
        Assert.Equal(10, properties.LargeChange);
        Assert.Equal(1, properties.SmallChange);

        // Set same.
        properties.Value = value;
        Assert.Equal(100, properties.Maximum);
        Assert.Equal(0, properties.Minimum);
        Assert.Equal(value, properties.Value);
        Assert.Equal(10, properties.LargeChange);
        Assert.Equal(1, properties.SmallChange);
    }

    [WinFormsTheory]
    [InlineData(-1)]
    [InlineData(101)]
    public void ScrollProperties_Value_SetOutOfRange_ThrowsArgumentOutOfRangeException(int value)
    {
        using ScrollableControl container = new();
        SubScrollProperties properties = new(container);
        Assert.Throws<ArgumentOutOfRangeException>("value", () => properties.Value = value);
        Assert.Equal(0, properties.Value);
    }

    [WinFormsTheory]
    [BoolData]
    public void ScrollProperties_Visible_Set_GetReturnsExpected(bool value)
    {
        using ScrollableControl container = new();
        SubScrollProperties properties = new(container)
        {
            Visible = value
        };
        Assert.Equal(value, properties.Visible);

        // Set same.
        properties.Visible = value;
        Assert.Equal(value, properties.Visible);

        // Set different.
        properties.Visible = !value;
        Assert.Equal(!value, properties.Visible);
    }

    [WinFormsTheory]
    [BoolData]
    public void ScrollProperties_Visible_SetAutoScrollContainer_Nop(bool value)
    {
        using ScrollableControl container = new()
        {
            AutoScroll = true
        };
        SubScrollProperties properties = new(container)
        {
            Visible = value
        };
        Assert.False(properties.Visible);

        // Set same.
        properties.Visible = value;
        Assert.False(properties.Visible);

        // Set different.
        properties.Visible = !value;
        Assert.False(properties.Visible);
    }

    [WinFormsTheory]
    [BoolData]
    public void ScrollProperties_Visible_SetNullContainer_GetReturnsExpected(bool value)
    {
        SubScrollProperties properties = new(null)
        {
            Visible = value
        };
        Assert.Equal(value, properties.Visible);

        // Set same.
        properties.Visible = value;
        Assert.Equal(value, properties.Visible);

        // Set different.
        properties.Visible = !value;
        Assert.Equal(!value, properties.Visible);
    }

    private class SubScrollProperties : HScrollProperties
    {
        public SubScrollProperties(ScrollableControl container) : base(container)
        {
        }

        public new ScrollableControl ParentControl => base.ParentControl;
    }
}
