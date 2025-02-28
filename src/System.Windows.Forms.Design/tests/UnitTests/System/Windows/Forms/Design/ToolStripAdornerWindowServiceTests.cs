﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable enable

using System.Drawing;
using Moq;
using System.Windows.Forms.Design.Behavior;
using System.ComponentModel;

namespace System.Windows.Forms.Design.Tests;

// TODO: (LOOK at the changes I made and instruct author regarding it) Test Dispose method for test class. See if _behaviorService is properly disposed of
// TODO: Remove STA method from every method, one by one, test if it really needs it
//  Need it:
//    - AdornerWindowPointToScreen_TranslatesPointCorrectly
//    - AdornerWindowToScreen_ReturnsCorrectScreenCoordinates
//    - ControlToAdornerWindow_TranslatesPointCorrectly
//    - ToolStripAdornerWindowGraphics_ReturnsGraphicsObject
// TODO: Ask Tanya if this is necessary since the test explorer doesn't show the pop up window when run from there and all tests pass
// TODO: (Not enough. Pop up window still shows when not running from test explorer) In those that do need STA method, try to remove with [WinformsFact] and see if ti is enough
// TODO: (This method is indeed unnecessary) Test removing unnecessary code: TestControl.CreateControl
// TODO: (Suggests assert it does not throw) Test if AdornerWindowPointToScreen with fix (8,31) is due to fail in different DPIs

public class ToolStripAdornerWindowServiceTests : IDisposable
{
    private readonly Mock<IServiceProvider> _serviceProviderMock;
    private readonly Mock<IOverlayService> _overlayServiceMock;
    private readonly BehaviorService _behaviorService;
    private readonly ToolStripAdornerWindowService _service;
    private bool _serviceDisposed;

    public ToolStripAdornerWindowServiceTests()
    {
        _serviceProviderMock = new();
        _overlayServiceMock = new();

        Mock<ISite> siteMock = new();
        siteMock.Setup(s => s.GetService(typeof(IOverlayService))).Returns(_overlayServiceMock.Object);

        _behaviorService = new(_serviceProviderMock.Object, new(siteMock.Object));

        _serviceProviderMock.Setup(sp => sp.GetService(typeof(IOverlayService))).Returns(_overlayServiceMock.Object);
        _serviceProviderMock.Setup(sp => sp.GetService(typeof(BehaviorService))).Returns(_behaviorService);

        using Control control = new();
        _service = new(_serviceProviderMock.Object, control);
        _behaviorService.Adorners.Add(_service.DropDownAdorner);
    }

    public void Dispose()
    {
        _behaviorService.Dispose();

        if (!_serviceDisposed)
        {
            _service.Dispose();
        }
    }

    private void RunInStaThread(Action action)
    {
        Exception? exception = null;
        Thread thread = new(() =>
        {
            try
            {
                action();
            }
            catch (Exception ex)
            {
                exception = ex;
            }
        });
        thread.SetApartmentState(ApartmentState.STA);
        thread.Start();
        thread.Join();
        if (exception is not null)
        {
            throw exception;
        }
    }

    private class TestControl : Control
    {
        private readonly Graphics _graphics;

        public TestControl(Graphics graphics) => _graphics = graphics;

        public Graphics TestGraphics => _graphics;

        public new Point PointToScreen(Point point) => new Point(point.X + 8, point.Y + 31);
    }

    [Fact]
    public void ProcessPaintMessage_InvokesInvalidateOnAdornerWindow()
    {
        Action action = () =>
        {
            Rectangle paintRect = new Rectangle(10, 10, 50, 50);
            _service.ProcessPaintMessage(paintRect);
        };

        action.Should().NotThrow();
    }

    [Fact]
    public void DropDownAdorner_ReturnsAdornerObject()
    {
        Adorner adorner = _service.DropDownAdorner;
        adorner.Should().NotBeNull();
    }

    [Fact]
    public void Dispose_DisposesResourcesCorrectly()
    {
        _service.Dispose();

        _overlayServiceMock.Verify(o => o.RemoveOverlay(It.IsAny<Control>()), Times.Once);
        _serviceProviderMock.Verify(sp => sp.GetService(typeof(BehaviorService)), Times.Once);
        _serviceProviderMock.Verify(sp => sp.GetService(typeof(IOverlayService)), Times.Exactly(2));
        _behaviorService.Adorners.Cast<Adorner>().Should().NotContain(_service.DropDownAdorner);

        if(_service.DropDownAdorner is null)
        {
            _serviceDisposed = true;
        }
    }

    [Fact]
    public void Invalidate_InvokesInvalidateOnAdornerWindow()
    {
        Action action() => _service.Invalidate;
        action().Should().NotThrow();
    }

    [Fact]
    public void InvalidateRegion_InvokesInvalidateOnAdornerWindow()
    {
        Action action = () =>
        {
            Region region = new(new Rectangle(10, 10, 50, 50));
            _service.Invalidate(region);
        };

        action.Should().NotThrow();
    }

    [Fact]
    public void AdornerWindowPointToScreen_TranslatesPointCorrectly() => RunInStaThread(() =>
    {
        Point point = new(10, 20);

        Point screenPoint = _service.AdornerWindowPointToScreen(point);

        Point expectedScreenPoint = new(18, 51);
        screenPoint.Should().Be(expectedScreenPoint);
    });

    [Fact]
    public void AdornerWindowToScreen_ReturnsCorrectScreenCoordinates() => RunInStaThread(() =>
    {
        TestControl control = new TestControl(Graphics.FromImage(new Bitmap(100, 100)));

        Point screenPoint = _service.AdornerWindowToScreen();

        Point expectedScreenPoint = new(8, 31);
        screenPoint.Should().Be(expectedScreenPoint);
    });

    [Fact]
    public void ControlToAdornerWindow_TranslatesPointCorrectly() => RunInStaThread(() =>
    {
        Control parentControl = new Control { Left = 10, Top = 20 };
        Control control = new Control { Parent = parentControl, Left = 30, Top = 40 };

        Point adornerWindowPoint = _service.ControlToAdornerWindow(control);

        Point expectedPoint = new(40, 60);
        adornerWindowPoint.Should().Be(expectedPoint);
    });

    [Fact]
    public void ToolStripAdornerWindowGraphics_ReturnsGraphicsObject() => RunInStaThread(() =>
    {
        using Graphics graphics = _service.ToolStripAdornerWindowGraphics;
        graphics.Should().NotBeNull();
    });
}
