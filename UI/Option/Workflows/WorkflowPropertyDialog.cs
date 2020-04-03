using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using Aperture;
using Captain.Common;
using Captain.UI;
using Ookii.Dialogs.Wpf;
using static Captain.Application.Application;
using DisplayName = Captain.Common.DisplayName;
using Region = Captain.Common.Region;

namespace Captain.Application {
  /// <inheritdoc />
  /// <summary>
  ///   Displays an interface for creating and modifying workflows
  /// </summary>
  internal partial class WorkflowPropertyDialog : Window {
    /// <summary>
    ///   Workflow to be edited
    /// </summary>
    internal WorkflowBase Workflow { get; set; }

    /// <inheritdoc />
    /// <summary>
    ///   Class constructor
    /// </summary>
    /// <param name="workflow">Workflow to be edited</param>
    public WorkflowPropertyDialog(WorkflowBase workflow = null) {
      InitializeComponent();

      Tag = Size;
      this.tabStrip.ExtendTabs = false;

      Workflow = workflow ?? new Workflow();
      InitializeGeneralPage();
    }

    #region Common window logic

    /// <summary>
    ///    Updates the title text
    /// </summary>
    private void UpdateTitle() {
      Text = String.IsNullOrWhiteSpace(this.nameTextBox.Text)
        ? String.Format(Resources.WorkflowPropertyDialog_UnnamedWorkflowTitleFormat, this.tabStrip.SelectedTab.Text)
        : String.Format(Resources.WorkflowPropertyDialog_NamedWorkflowTitleFormat, this.nameTextBox.Text,
                        this.tabStrip.SelectedTab.Text);
    }

    /// <summary>
    ///   Triggered when the tab page changed
    /// </summary>
    /// <param name="sender">Sender object</param>
    /// <param name="eventArgs">Event arguments</param>
    private void OnSelectedTabChanged(object sender, EventArgs eventArgs) {
      UpdateTitle();

      switch (this.tabStrip.SelectedTab.Name) {
        case nameof(this.generalPage):
          InitializeGeneralPage();
          AnimateResize((Size) Tag); // restore original size
          break;

        case nameof(this.handlersPage):
          InitializeHandlersPage();

          // resize width so that it's 175% the current width
          AnimateResize(new Size((int) (Width * 1.75), Height));
          break;

        default:
          AnimateResize((Size) Tag); // restore original size
          break;
      }
    }

    #region Drag and drop logic

    /// <inheritdoc />
    /// <summary>Raises the <see cref="E:System.Windows.Forms.Control.DragOver" /> event.</summary>
    /// <param name="eventArgs">A <see cref="T:System.Windows.Forms.DragEventArgs" /> that contains the event data. </param>
    protected override void OnDragOver(DragEventArgs eventArgs) {
      eventArgs.Effect = DragDropEffects.None;
      DropTargetHelper.DragOver(new Point(eventArgs.X, eventArgs.Y), eventArgs.Effect);
      base.OnDragOver(eventArgs);
    }

    /// <inheritdoc />
    /// <summary>Raises the <see cref="E:System.Windows.Forms.Control.DragEnter" /> event.</summary>
    /// <param name="eventArgs">A <see cref="T:System.Windows.Forms.DragEventArgs" /> that contains the event data. </param>
    protected override void OnDragEnter(DragEventArgs eventArgs) {
      eventArgs.Effect = DragDropEffects.None;
      DropTargetHelper.DragEnter(this, eventArgs.Data, new Point(eventArgs.X, eventArgs.Y), eventArgs.Effect);
      base.OnDragEnter(eventArgs);
    }

    /// <inheritdoc />
    /// <summary>Raises the <see cref="E:System.Windows.Forms.Control.DragLeave" /> event.</summary>
    /// <param name="eventArgs">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
    protected override void OnDragLeave(EventArgs eventArgs) {
      DropTargetHelper.DragLeave(this);
      base.OnDragLeave(eventArgs);
    }

    /// <inheritdoc />
    /// <summary>Raises the <see cref="E:System.Windows.Forms.Control.DragDrop" /> event.</summary>
    /// <param name="eventArgs">A <see cref="T:System.Windows.Forms.DragEventArgs" /> that contains the event data. </param>
    protected override void OnDragDrop(DragEventArgs eventArgs) {
      eventArgs.Effect = DragDropEffects.None;
      DropTargetHelper.Drop(eventArgs.Data, new Point(eventArgs.X, eventArgs.Y), eventArgs.Effect);
      base.OnDragDrop(eventArgs);
    }

    #endregion

    #endregion

    #region General page logic

    private void InitializeGeneralPage() {
      if ((bool?) this.handlersPage.Tag ?? false) {
        // already initialized
        return;
      }

      Log.Trace("initializing General page");
      this.generalPage.Tag = true; // we're now initialized

      /* bind workflow properties to controls */
      // workflow name
      this.nameTextBox.DataBindings.Add("Text", Workflow, "Name");
      this.nameTextBox.TextChanged += delegate { UpdateTitle(); };

      // workflow type
      this.typeComboBox.DataSource = EnumAdapterHelper.GetComboBoxAdapter<WorkflowType>();
      this.typeComboBox.DataBindings.Add("SelectedValue", Workflow, "Type");
      this.typeComboBox.SelectedIndexChanged += delegate { UpdateEncoderComboBox(); };

      // screen region
      this.regionComboBox.DataSource = EnumAdapterHelper.GetComboBoxAdapter<RegionType>();
      this.regionComboBox.SelectedIndexChanged += delegate { UpdateRegion(); };

      // hotkey
      this.hotkeyBox.Hotkey = (Keys) Workflow.Hotkey;
      this.hotkeyBox.TextChanged += delegate { UpdateHotkey(); };

      // check boxes
      this.displayInApplicationMenuCheckBox.DataBindings.Add("Checked", Workflow, "ShowInApplicationMenu");
      this.allowOutsideDesktopCheckBox.DataBindings.Add("Checked", Workflow, "CrossContainerHotkey");

      // update window and field parameters
      UpdateTitle();
      UpdateEncoderComboBox();
      UpdateRegionLabel();

      // bind encoder combo box events
      this.encoderComboBox.SelectedIndexChanged += delegate { UpdateEncoder(); };
      if (Workflow.Codec.TypeName == null) {
        UpdateEncoder();
      }
    }

    /// <summary>
    ///   Updates the encoder combo box
    /// </summary>
    private void UpdateEncoderComboBox() {
      this.encoderComboBox.DataSource = (WorkflowType) this.typeComboBox.SelectedValue == WorkflowType.Still
        ? TypeAdapterHelper.GetComboBoxAdapter(Application.ExtensionManager.EnumerateObjects<StillImageCodec>())
        : TypeAdapterHelper.GetComboBoxAdapter(Application.ExtensionManager.EnumerateObjects<VideoCodec>());
    }

    /// <summary>
    ///   Updates this workflow's encoder
    /// </summary>
    private void UpdateEncoder() {
      if (Workflow.Codec.TypeName != this.encoderComboBox.SelectedValue.ToString()) {
        Workflow.Codec = (this.encoderComboBox.SelectedValue.ToString(), null);
      }
    }

    /// <summary>
    ///   Updates this workflow's region
    /// </summary>
    private async void UpdateRegion() {
      switch ((RegionType) this.regionComboBox.SelectedValue) {
        case RegionType.Fixed: {
          // create and unlock clipper component
          using (var clipper = new Clipper(Application.HudManager.GetContainer())) {
            await clipper.UnlockAsync(allowAdvancedSelection: false);

            if (!clipper.Area.Size.IsEmpty) {
              Workflow.Region = new Region {
                Type = RegionType.Fixed,
                Location = (clipper.Area.X, clipper.Area.Y),
                Size = (clipper.Area.Width, clipper.Area.Height)
              };

              break;
            }
          }

          // fall back to previous region
          this.regionComboBox.SelectedValue = Workflow.Region.Type;
          break;
        }

        default:
          Workflow.Region.Type = (RegionType) this.regionComboBox.SelectedValue;
          break;
      }

      UpdateRegionLabel();
    }

    /// <summary>
    ///   Displays currently selected region
    /// </summary>
    private void UpdateRegionLabel() {
      if (Workflow.Region.Type == RegionType.Fixed) {
        this.fixedRegionValue.AutoSize = true;
        this.fixedRegionValue.Image = Resources.TaskRegionFixed;
        this.fixedRegionValue.Text = $@"{Workflow.Region.Size.Width}×{Workflow.Region.Size.Height}";

        int autoWidth = this.fixedRegionValue.Width;
        this.fixedRegionValue.AutoSize = false;
        this.fixedRegionValue.Width = (int) (autoWidth + 1.25 * this.fixedRegionValue.Image.Width);
        this.fixedRegionValue.Visible = true;
      } else {
        this.fixedRegionValue.Visible = false;
      }
    }

    /// <summary>
    ///   Updates the workflow's hotkey value
    /// </summary>
    private void UpdateHotkey() {
      Workflow.Hotkey = (ulong) this.hotkeyBox.Hotkey;
    }

    /// <summary>
    ///   Calls the configuration UI for the selected codec
    /// </summary>
    /// <param name="sender">Sender object</param>
    /// <param name="eventArgs">Additional information associated with this event</param>
    private void OnEncoderConfigureLinkLabelClick(object sender, EventArgs eventArgs) {
      try {
        Type type = Type.GetType(Workflow.Codec.TypeName) ??
                    throw new InvalidOperationException("Could not resolve the specified type");

        // create an instance of the OptionProvider type associated with the extension
        using (var provider =
          Activator.CreateInstance(type.GetCustomAttribute<OptionProvider>().Type) as IOptionProvider) {
          if (provider is null) {
            throw new InvalidOperationException(
              "Could not create an instance of the option provider type for this extension");
          }

          Workflow.Codec = (Workflow.Codec.TypeName, provider.DisplayOptionUi(Workflow.Codec.Options));
        }
      } catch (Exception exception) {
        Log.Error($"could not call configuration UI for {Workflow.Codec.TypeName}: {exception}");

        new TaskDialog {
          WindowTitle = System.Windows.Forms.Application.ProductName,
          MainIcon = TaskDialogIcon.Error,
          AllowDialogCancellation = false,
          Width = 200,
          Buttons = {
            new TaskDialogButton(ButtonType.Ok) {Default = true}
          },
          // TODO: localize this
          Content = @"Could not display configuration UI for this encoder.",
          ExpandedInformation = exception.ToString(),
          ExpandFooterArea = true
        }.ShowDialog();
      }
    }

    #endregion

    #region Handlers page logic

    private void InitializeHandlersPage() {
      if ((bool?) this.handlersPage.Tag ?? false) {
        // already initialized
        return;
      }

      Log.Trace("initializing Handlers page");
      this.handlersPage.Tag = true; // we're now initialized

      UpdateAvailableHandlersPane();
    }

    /// <summary>
    ///   Updates the left pane so that it shows all the handlers available
    /// </summary>
    private void UpdateAvailableHandlersPane() {
      this.handlerIconList.Images.Clear();
      this.handlerListView.Clear();
      this.handlerListView.Columns.Clear();

      this.handlerIconList.Images.Add(Resources.GenericExtensionIcon);
      this.handlerListView.Columns.AddRange(new[] {new ColumnHeader(), new ColumnHeader()});

      foreach (Type handler in Application.ExtensionManager.EnumerateObjects<Handler>()) {
        string displayName;

        try {
          displayName = handler.GetCustomAttributes(typeof(DisplayName), false)
                               .Cast<DisplayName>()
                               .OrderByDescending(
                                 dn => String.Equals(dn.LanguageCode,
                                                     CultureInfo.CurrentUICulture.TwoLetterISOLanguageName,
                                                     StringComparison.OrdinalIgnoreCase))
                               .First()
                               .Name;
        } catch {
          displayName = handler.Name;
        }

        string version;
        try {
          version = handler.Assembly.GetCustomAttributes(typeof(AssemblyFileVersionAttribute), false)
                           .Cast<AssemblyFileVersionAttribute>()
                           .First()
                           .Version;
        } catch {
          version = null;
        }

        string publisher;
        try {
          publisher = handler.Assembly.GetCustomAttributes(typeof(AssemblyCompanyAttribute), false)
                             .Cast<AssemblyCompanyAttribute>()
                             .First()
                             .Company;
        } catch {
          // TODO: localize this
          publisher = "Unknown publisher";
        }

        if (!String.IsNullOrWhiteSpace(version)) {
          publisher = " | " + publisher;
        } else {
          version = "";
        }

        int iconIndex = 0;
        try {
          // try to obtain a display icon associated with this handler
          IEnumerable<DisplayIcon> displayIcons = handler.GetCustomAttributes(typeof(DisplayIcon), false)
                                                         .Cast<DisplayIcon>();
          this.handlerIconList.Images.Add(displayIcons.First().Icon);
          iconIndex = this.handlerIconList.Images.Count - 1;
        } catch {
          /* failed to obtain an icon */
        }

        this.handlerListView.Items.Add(new ListViewItem(new[] {
          displayName,
          version + publisher
        }, iconIndex) {
          Tag = handler.FullName
        });
      }
    }

    /// <summary>
    ///   Paints the "Handlers" tab page
    /// </summary>
    /// <param name="sender">Sender object</param>
    /// <param name="eventArgs">Event arguments</param>
    private void OnHandlersPagePaint(object sender, PaintEventArgs eventArgs) {
      eventArgs.Graphics.DrawLine(new Pen(Color.FromArgb(0x20, 0, 0, 0)),
                                  this.handlerListView.Right,
                                  0,
                                  this.handlerListView.Right,
                                  this.handlersPage.Height);
    }

    #region Drag and drop logic for available handler list view

    /// <summary>
    ///   Triggered when a handler is dropped onto the handler list
    /// </summary>
    /// <param name="sender">Sender object</param>
    /// <param name="eventArgs">Event arguments</param>
    private void OnHandlerListDragDrop(object sender, DragEventArgs eventArgs) {
      eventArgs.Effect = DragDropEffects.None;
      DropTargetHelper.Drop(eventArgs.Data, new Point(eventArgs.X, eventArgs.Y), eventArgs.Effect);
    }

    /// <summary>
    ///   Triggered when a handler is dragged onto the handler list
    /// </summary>
    /// <param name="sender">Sender object</param>
    /// <param name="eventArgs">Event arguments</param>
    private void OnHandlerListDragEnter(object sender, DragEventArgs eventArgs) {
      eventArgs.Effect = DragDropEffects.None;
      DropTargetHelper.DragEnter(this, eventArgs.Data, new Point(eventArgs.X, eventArgs.Y), eventArgs.Effect);
    }

    /// <summary>
    ///   Triggered when the handler list drop target is left
    /// </summary>
    /// <param name="sender">Sender object</param>
    /// <param name="eventArgs">Event arguments</param>
    private void OnHandlerListDragLeave(object sender, EventArgs eventArgs) {
      DropTargetHelper.DragLeave(this);
    }

    /// <summary>
    ///   Triggered when an item is being dragged over the handler list
    /// </summary>
    /// <param name="sender">Sender object</param>
    /// <param name="eventArgs">Event arguments</param>
    private void OnHandlerListDragOver(object sender, DragEventArgs eventArgs) {
      eventArgs.Effect = DragDropEffects.None;
      DropTargetHelper.DragOver(new Point(eventArgs.X, eventArgs.Y), eventArgs.Effect);
    }

    #endregion


    #region Drag and drop logic for enabled handler panel

    /// <summary>
    ///   Triggered when a handler item is being dragged
    /// </summary>
    /// <param name="sender">Sender object</param>
    /// <param name="eventArgs">Event arguments</param>
    private void OnHandlerItemDrag(object sender, ItemDragEventArgs eventArgs) {
      // retrieve all the selected (and the dragging) item
      var items = new List<ListViewItem> {(ListViewItem) eventArgs.Item};

      // add remaining items
      foreach (ListViewItem item in this.handlerListView.SelectedItems) {
        if (!items.Contains(item)) {
          items.Add(item);
        }
      }

      // perform drag drop with a list of handler type names
      DragSourceHelper.DoDragDrop(this.handlerListView,
                                  this.handlerIconList.Images[items.First().ImageIndex] as Bitmap,
                                  Point.Empty,
                                  DragDropEffects.Copy,
                                  new KeyValuePair<string, object>(DataFormats.Serializable,
                                                                   items.Select(i => i.Tag.ToString()).ToList()));
    }

    /// <summary>
    ///   Triggered when an item has been dropped on the handler list
    /// </summary>
    /// <param name="sender">Sender object</param>
    /// <param name="eventArgs">Event arguments</param>
    private void OnHandlerDragDrop(object sender, DragEventArgs eventArgs) {
      DropTargetHelper.Drop(eventArgs.Data,
                            new Point(eventArgs.X, eventArgs.Y),
                            eventArgs.AllowedEffect & DragDropEffects.Copy);
    }

    /// <summary>
    ///   Triggered when a new handler is being dragged to the handler list
    /// </summary>
    /// <param name="sender">Sender object</param>
    /// <param name="eventArgs">Event arguments</param>
    private void OnHandlerDragEnter(object sender, DragEventArgs eventArgs) {
      // make sure we've got valid data
      if (eventArgs.Data.GetDataPresent(DataFormats.Serializable, true) &&
          eventArgs.Data.GetData(DataFormats.Serializable) is List<string> typeList) {
        // TODO: localize this
        string descriptionMessage = typeList.Count == 1
          ? "Add handler"
          : "Add %1";
        string descriptionInsert = typeList.Count == 1
          ? String.Empty
          : $"{typeList.Count} handlers";

        eventArgs.Effect = eventArgs.AllowedEffect & DragDropEffects.Copy;
        DropTargetHelper.DragEnter(this.handlerDropTargetPanel,
                                   eventArgs.Data,
                                   new Point(eventArgs.X, eventArgs.Y),
                                   eventArgs.Effect,
                                   descriptionMessage,
                                   descriptionInsert);
      } else {
        // no valid data received
        eventArgs.Effect = DragDropEffects.None;
        DropTargetHelper.DragEnter(this.handlerDropTargetPanel,
                                   eventArgs.Data,
                                   new Point(eventArgs.X, eventArgs.Y),
                                   eventArgs.Effect);
      }
    }

    /// <summary>
    ///   Triggered when the dragged item leaves the handler list region
    /// </summary>
    /// <param name="sender">Sender object</param>
    /// <param name="eventArgs">Event arguments</param>
    private void OnHandlerDragLeave(object sender, EventArgs eventArgs) {
      DropTargetHelper.DragLeave(this.handlerDropTargetPanel);
    }

    /// <summary>
    ///   Triggered when a handler item is being dragged over the handler list region
    /// </summary>
    /// <param name="sender">Sender object</param>
    /// <param name="eventArgs">Event arguments</param>
    private void OnHandlerDragOver(object sender, DragEventArgs eventArgs) {
      DropTargetHelper.DragOver(new Point(eventArgs.X, eventArgs.Y),
                                eventArgs.AllowedEffect & DragDropEffects.Copy);
    }

    #endregion

    #endregion

    #region Bottom panel logic

    /// <summary>
    ///   Paints a top border on the bottom panel
    /// </summary>
    /// <param name="sender">Sender object</param>
    /// <param name="eventArgs"></param>
    private void OnBottomPanelPaint(object sender, PaintEventArgs eventArgs) {
      eventArgs.Graphics.DrawLine(new Pen(Color.FromArgb(0x10, 0, 0, 0)), 0, 0, Width, 0);
    }

    /// <summary>
    ///   Triggered when the Save button gets clicked
    /// </summary>
    /// <param name="sender">Sender object</param>
    /// <param name="eventArgs">Event arguments</param>
    private void OnSaveButtonClick(object sender, EventArgs eventArgs) { }

    #endregion
  }
}