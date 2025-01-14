// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable enable

using System.ComponentModel;
using System.ComponentModel.Design;
using System.Diagnostics;
using Moq;

namespace System.Windows.Forms.Design.Tests;

public class ChangeToolStripParentVerbTests : IDisposable
{
    private readonly Mock<IDesignerHost> _mockDesignerHost = new();
    private readonly Mock<ISelectionService> _mockSelectionService = new();
    private readonly Mock<IComponentChangeService> _mockComponentChangeService = new();
    private readonly ToolStrip _toolStrip = new();
    private readonly ToolStripDesigner _designer = new();
    private readonly Mock<ISite> _mockSite = new();

    public ChangeToolStripParentVerbTests()
    {
        Trace.Listeners.Clear();
        _mockSite.Setup(s => s.GetService(typeof(IDesignerHost))).Returns(_mockDesignerHost.Object);
        _mockSite.Setup(s => s.GetService(typeof(ISelectionService))).Returns(_mockSelectionService.Object);
        _mockSite.Setup(s => s.GetService(typeof(IComponentChangeService))).Returns(_mockComponentChangeService.Object);
        _mockSite.Setup(s => s.GetService(typeof(DesignerActionUIService))).Returns(new DesignerActionUIService(_mockSite.Object));

        _toolStrip.Site = _mockSite.Object;

        _mockDesignerHost.Setup(d => d.GetService(typeof(IComponentChangeService))).Returns(_mockComponentChangeService.Object);

        _designer.Initialize(_toolStrip);
    }

    public void Dispose()
    {
        _toolStrip.Dispose();
        _designer.Dispose();
    }

    [Fact]
    public void ChangeTooLStripParentVerb_Ctor_Test()
    {
        Action action = () =>
        {
            ChangeToolStripParentVerb changeToolStripParentVerb = new(_designer);
        };

        // NOTE: Will note test property set because it's all private fields. So asserting that an exception is not thrown is acceptable.
        action.Should().NotThrow();
    }

    [Fact]
    public void ChangeParent_DoesNotThrow()
    {
        Action action = () =>
        {
            ChangeToolStripParentVerb changeToolStripParentVerb = new(_designer);
            changeToolStripParentVerb.ChangeParent();
        };

        action.Should().NotThrow();
    }

    [Fact]
    public void ChangeParent_RootComponentIsNotControl_DoesNotThrow()
    {
        Action action = () =>
        {
            _mockDesignerHost.Setup(h => h.RootComponent).Returns(_toolStrip);

            var mockParentControlDesigner = new Mock<ParentControlDesigner>();
// TODO: Must set ParentControlDesigner's

            _mockDesignerHost.Setup(s => s.GetDesigner(It.IsAny<Control>())).Returns(mockParentControlDesigner.Object);
            ChangeToolStripParentVerb changeToolStripParentVerb = new(_designer);

            changeToolStripParentVerb.TestAccessor().Dynamic._provider = _mockSite.Object;
            changeToolStripParentVerb.ChangeParent();
        };

        action.Should().NotThrow();
    }
}
