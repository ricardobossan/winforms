// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.ComponentModel;
using System.ComponentModel.Design;
using System.ComponentModel.Design.Serialization;
using System.Windows.Forms.Design.Behavior;
using Moq;

namespace System.Windows.Forms.Design.Tests;
public class StandardMenuStripVerbTests : IDisposable
{
    private DesignerActionService? _designerActionService;
    private DesignerActionUIService? _designerActionUIService;
    private BehaviorService? _behaviorService;
    private readonly Mock<IDesignerHost> _designerHostMock = new();
    private readonly Mock<IServiceProvider> _serviceProviderMock = new();
    private readonly Mock<ISelectionService> _mockSelectionService = new();
    private Mock<ISite>? _siteMock;
    private readonly Mock<IComponentChangeService> _componentChangeServiceMock = new();
    private readonly Mock<DesignerTransaction> _mockTransaction = new(MockBehavior.Loose);
    private readonly ParentControlDesigner _parentControlDesigner = new();
    private readonly ToolStripDesigner _designer = new();
    private MenuStrip _menuStrip = new();

    public void Dispose()
    {
        _menuStrip?.Dispose();
        _parentControlDesigner.Dispose();
        _designerActionService?.Dispose();
        _designerActionUIService?.Dispose();
        _behaviorService!.Dispose();
        _designer.Dispose();
    }

    private MenuStrip MockMinimalControl()
    {
        _mockSelectionService.Setup(s => s.GetComponentSelected(_menuStrip)).Returns(true);
        _siteMock = CreateMockSiteWithDesignerHost(_designerHostMock.Object);
        _siteMock.Setup(s => s.GetService(typeof(ISelectionService))).Returns(_mockSelectionService.Object);
        _siteMock.Setup(s => s.GetService(typeof(IDesignerHost))).Returns(_designerHostMock.Object);
        _siteMock.Setup(s => s.GetService(typeof(IComponentChangeService))).Returns(_componentChangeServiceMock.Object);
        _behaviorService = new(_serviceProviderMock.Object, new DesignerFrame(_siteMock.Object));
        _siteMock.Setup(s => s.GetService(typeof(BehaviorService))).Returns(_behaviorService);
        _siteMock.Setup(s => s.GetService(typeof(ToolStripAdornerWindowService))).Returns(null!);
        _designerActionService = new(_siteMock.Object);
        _siteMock.Setup(s => s.GetService(typeof(DesignerActionService))).Returns(_designerActionService);
        Mock<INameCreationService> nameCreationServiceMock = new();
        nameCreationServiceMock.Setup(n => n.IsValidName(It.IsAny<string>())).Returns(true);
        _siteMock.Setup(s => s.GetService(typeof(INameCreationService))).Returns(nameCreationServiceMock.Object);
        _siteMock.Setup(s => s.GetService(typeof(IDesignerHost))).Returns(_designerHostMock.Object);
        _designerActionUIService = new(_siteMock.Object);
        _siteMock.Setup(s => s.GetService(typeof(DesignerActionUIService))).Returns(_designerActionUIService);

        _serviceProviderMock.Setup(s => s.GetService(typeof(IDesignerHost))).Returns(_designerHostMock.Object);
        _serviceProviderMock.Setup(s => s.GetService(typeof(IComponentChangeService))).Returns(new Mock<IComponentChangeService>().Object);
        _designerHostMock.Setup(h => h.RootComponent).Returns(_menuStrip);
        _designerHostMock.Setup(h => h.CreateTransaction(It.IsAny<string>())).Returns(_mockTransaction.Object);
        _designerHostMock.Setup(h => h.GetService(typeof(IComponentChangeService))).Returns(_componentChangeServiceMock.Object);
        _designerHostMock.Setup(h => h.AddService(typeof(ToolStripKeyboardHandlingService), It.IsAny<object>()));
        _designerHostMock.Setup(h => h.AddService(typeof(ISupportInSituService), It.IsAny<object>()));
        _designerHostMock.Setup(h => h.AddService(typeof(DesignerActionService), It.IsAny<object>()));
        _designerHostMock.Setup(h => h.GetDesigner(_menuStrip)).Returns(_parentControlDesigner);
        _designerHostMock.Setup(h => h.AddService(typeof(DesignerActionUIService), It.IsAny<object>()));
        Mock<ToolStripMenuItem> toolStripMenuItemMock = new();
        //ToolStrip toolStrip = toolStripMenuItemMock.Object.GetCurrentParent();
        toolStripMenuItemMock.Object.DropDown = new ToolStripDropDownMenu();

        string[] menuItemImageNames =
        [
            "file", "new", "open", "save", "print", "printPreview", "cut", "copy", "paste", "undo", "redo", "delete", "selectAll", "edit", "exit", "saveAs", "saveAll", "tool", "tools"
        ];

        foreach (string item in menuItemImageNames)
        {
        _designerHostMock.Setup(h => h.CreateComponent(typeof(ToolStripMenuItem), item + "ToolStripMenuItem")).Returns(toolStripMenuItemMock.Object);
        }

        _designerHostMock.Setup(h => h.CreateComponent(typeof(ToolStripSeparator), "toolStripSeparator")).Returns(new Mock<ToolStripSeparator>().Object);

toolStripMenuItemMock.Object.DropDown.Items.Add(new ToolStripMenuItem("New", null, null, "newToolStripMenuItem"));
        /*
        _designerHostMock.Setup(h => h.CreateComponent(typeof(ToolStripMenuItem), "fileToolStripMenuItem")).Returns(toolStripMenuItemMock.Object);
        _designerHostMock.Setup(h => h.CreateComponent(typeof(ToolStripMenuItem), "newToolStripMenuItem")).Returns(toolStripMenuItemMock.Object);
        _designerHostMock.Setup(h => h.CreateComponent(typeof(ToolStripMenuItem), "openToolStripMenuItem")).Returns(toolStripMenuItemMock.Object);
        _designerHostMock.Setup(h => h.CreateComponent(typeof(ToolStripMenuItem), "saveToolStripMenuItem")).Returns(toolStripMenuItemMock.Object);
        _designerHostMock.Setup(h => h.CreateComponent(typeof(ToolStripMenuItem), "saveAsToolStripMenuItem")).Returns(toolStripMenuItemMock.Object);
        _designerHostMock.Setup(h => h.CreateComponent(typeof(ToolStripMenuItem), "printToolStripMenuItem")).Returns(toolStripMenuItemMock.Object);
        _designerHostMock.Setup(h => h.CreateComponent(typeof(ToolStripMenuItem), "printPreviewToolStripMenuItem")).Returns(toolStripMenuItemMock.Object);
        _designerHostMock.Setup(h => h.CreateComponent(typeof(ToolStripMenuItem), "exitToolStripMenuItem")).Returns(toolStripMenuItemMock.Object);
        _designerHostMock.Setup(h => h.CreateComponent(typeof(ToolStripMenuItem), "editToolStripMenuItem")).Returns(toolStripMenuItemMock.Object);
        _designerHostMock.Setup(h => h.CreateComponent(typeof(ToolStripMenuItem), "deleteToolStripMenuItem")).Returns(toolStripMenuItemMock.Object);
        _designerHostMock.Setup(h => h.CreateComponent(typeof(ToolStripMenuItem), "undoToolStripMenuItem")).Returns(toolStripMenuItemMock.Object);
        _designerHostMock.Setup(h => h.CreateComponent(typeof(ToolStripMenuItem), "redoToolStripMenuItem")).Returns(toolStripMenuItemMock.Object);
        */

        _menuStrip.Site = _siteMock.Object;
        return _menuStrip;
    }

    public static Mock<ISite> CreateMockSiteWithDesignerHost(object designerHost)
    {
        Mock<ISite> mockSite = new(MockBehavior.Loose);
        mockSite
            .Setup(s => s.GetService(typeof(IDesignerHost)))
            .Returns(designerHost);
        mockSite
            .Setup(s => s.GetService(typeof(IInheritanceService)))
            .Returns(null!);
        mockSite
            .Setup(s => s.GetService(typeof(IDictionaryService)))
            .Returns(null!);
        mockSite
            .Setup(s => s.GetService(typeof(IExtenderListService)))
            .Returns(null!);
        mockSite
            .Setup(s => s.GetService(typeof(ITypeDescriptorFilterService)))
            .Returns(null!);
        mockSite
            .Setup(s => s.GetService(typeof(AmbientProperties)))
            .Returns(null!);
        mockSite
            .Setup(s => s.GetService(typeof(DesignerActionService)))
            .Returns(null!);
        mockSite
            .Setup(s => s.GetService(typeof(IComponentChangeService)))
            .Returns(null!);
        mockSite
            .Setup(s => s.GetService(typeof(ToolStripKeyboardHandlingService)))
            .Returns(null!);
        mockSite
            .Setup(s => s.GetService(typeof(ISupportInSituService)))
            .Returns(null!);
        mockSite
            .Setup(s => s.GetService(typeof(INestedContainer)))
            .Returns(new Mock<IContainer>().Object);
        mockSite
            .Setup(s => s.GetService(typeof(ToolStripMenuItem)))
            .Returns(null!);

        Mock<IServiceProvider> mockServiceProvider = new(MockBehavior.Strict);

        mockSite
            .Setup(s => s.GetService(typeof(IServiceProvider)))
            .Returns(mockServiceProvider.Object);
        mockSite
            .Setup(s => s.GetService(typeof(ToolStripAdornerWindowService)))
            .Returns(null!);
        mockSite
            .Setup(s => s.GetService(typeof(DesignerOptionService)))
            .Returns(mockServiceProvider.Object);

        Mock<ISelectionService> mockSelectionService = new(MockBehavior.Strict);

        mockSite
            .Setup(s => s.GetService(typeof(ISelectionService)))
            .Returns(mockSelectionService.Object);
        mockSite
            .Setup(s => s.Container)
            .Returns(new ServiceContainer() as IContainer);
        mockSite
            .Setup(s => s.Name)
            .Returns("fileToolStripMenuItem");
        mockSite
            .Setup(s => s.DesignMode)
            .Returns(true);
        mockSite
            .Setup(s => s.GetService(typeof(UndoEngine)))
            .Returns(null!);

        return mockSite;
    }

    [Fact]
    public void StandardMenuStripVerb_Ctor_Default()
    {
        _menuStrip = MockMinimalControl();
        _designer.Initialize(_menuStrip);
        Mock<IContainer> containerMock = new();

        using MenuStrip menuStrip2 = MockMinimalControl();
        menuStrip2.Name = "standardMainMenuStrip";
        using MenuStrip menuStrip3 = MockMinimalControl();
        menuStrip3.Name = "fileToolStripMenuItem";
        IComponent[] components = [menuStrip2, menuStrip3];
        ComponentCollection componentCollection = new(components);
        //IContainer container = containerMock.Object;
        //container.Add(menuStrip2, "standardMainMenuStrip");
        //container.Add(menuStrip3, "fileToolStripMenuItem");
        containerMock.Setup(c => c.Components).Returns(componentCollection);
        _designerHostMock.Setup(d => d.Container).Returns(containerMock.Object);
        StandardMenuStripVerb standardMenuStripVerb = new(_designer);

        standardMenuStripVerb.Should().BeOfType<StandardMenuStripVerb>();

        standardMenuStripVerb.InsertItems();
    }
}
