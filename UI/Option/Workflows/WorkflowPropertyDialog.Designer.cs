using System.ComponentModel;
using System.Windows.Forms;

namespace Captain.Application {
  partial class WorkflowPropertyDialog {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing) {
      if (disposing && (components != null)) {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent() {
      this.components = new System.ComponentModel.Container();
      System.Windows.Forms.Panel _separator1;
      System.Windows.Forms.Panel panel1;
      this.advancedPage = new System.Windows.Forms.TabPage();
      this.transformsPage = new System.Windows.Forms.TabPage();
      this.handlersPage = new System.Windows.Forms.TabPage();
      this.handlerDropTargetPanel = new System.Windows.Forms.Panel();
      this.label1 = new System.Windows.Forms.Label();
      this.handlerListView = new Captain.Application.ListViewEx();
      this.handlerIconList = new System.Windows.Forms.ImageList(this.components);
      this.handlerListViewPaddingPanel = new System.Windows.Forms.Panel();
      this.generalPage = new System.Windows.Forms.TabPage();
      this.hotkeyBox = new Captain.Application.HotkeyBox();
      this.fixedRegionValue = new System.Windows.Forms.Label();
      this.encoderConfigureLinkLabel = new Captain.Application.LinkLabel2();
      this.encoderComboBox = new System.Windows.Forms.ComboBox();
      this.encoderLabel = new System.Windows.Forms.Label();
      this.label6 = new System.Windows.Forms.Label();
      this.allowOutsideDesktopCheckBox = new System.Windows.Forms.CheckBox();
      this.displayInApplicationMenuCheckBox = new System.Windows.Forms.CheckBox();
      this.hotkeyLabel = new System.Windows.Forms.Label();
      this.triggersTitleLabel = new System.Windows.Forms.Label();
      this.regionComboBox = new System.Windows.Forms.ComboBox();
      this.regionLabel = new System.Windows.Forms.Label();
      this.typeComboBox = new System.Windows.Forms.ComboBox();
      this.typeLabel = new System.Windows.Forms.Label();
      this.nameTextBox = new System.Windows.Forms.TextBox();
      this.nameLabel = new System.Windows.Forms.Label();
      this.basicTitleLabel = new System.Windows.Forms.Label();
      this.tabStrip = new Captain.Application.TabStripControl();
      this.bottomPanel = new System.Windows.Forms.Panel();
      this.saveButton = new System.Windows.Forms.Button();
      this.exportLinkLabel = new Captain.Application.LinkLabel2();
      this.cancelButton = new System.Windows.Forms.Button();
      _separator1 = new System.Windows.Forms.Panel();
      panel1 = new System.Windows.Forms.Panel();
      this.handlersPage.SuspendLayout();
      this.handlerDropTargetPanel.SuspendLayout();
      this.generalPage.SuspendLayout();
      this.tabStrip.SuspendLayout();
      this.bottomPanel.SuspendLayout();
      this.SuspendLayout();
      // 
      // _separator1
      // 
      _separator1.BackColor = System.Drawing.Color.Gainsboro;
      _separator1.Location = new System.Drawing.Point(46, 174);
      _separator1.Name = "_separator1";
      _separator1.Size = new System.Drawing.Size(316, 1);
      _separator1.TabIndex = 10;
      // 
      // panel1
      // 
      panel1.BackColor = System.Drawing.Color.Gainsboro;
      panel1.Location = new System.Drawing.Point(46, 323);
      panel1.Name = "panel1";
      panel1.Size = new System.Drawing.Size(316, 1);
      panel1.TabIndex = 16;
      // 
      // advancedPage
      // 
      this.advancedPage.BackColor = System.Drawing.Color.WhiteSmoke;
      this.advancedPage.Location = new System.Drawing.Point(4, 32);
      this.advancedPage.Name = "advancedPage";
      this.advancedPage.Size = new System.Drawing.Size(396, 439);
      this.advancedPage.TabIndex = 3;
      this.advancedPage.Text = "Advanced";
      // 
      // transformsPage
      // 
      this.transformsPage.BackColor = System.Drawing.Color.WhiteSmoke;
      this.transformsPage.Location = new System.Drawing.Point(4, 32);
      this.transformsPage.Name = "transformsPage";
      this.transformsPage.Size = new System.Drawing.Size(396, 439);
      this.transformsPage.TabIndex = 2;
      this.transformsPage.Text = "Transforms";
      // 
      // handlersPage
      // 
      this.handlersPage.AllowDrop = true;
      this.handlersPage.AutoScroll = true;
      this.handlersPage.BackColor = System.Drawing.Color.WhiteSmoke;
      this.handlersPage.Controls.Add(this.handlerDropTargetPanel);
      this.handlersPage.Controls.Add(this.handlerListView);
      this.handlersPage.Controls.Add(this.handlerListViewPaddingPanel);
      this.handlersPage.Location = new System.Drawing.Point(4, 32);
      this.handlersPage.Name = "handlersPage";
      this.handlersPage.Size = new System.Drawing.Size(396, 439);
      this.handlersPage.TabIndex = 1;
      this.handlersPage.Text = "Handlers";
      this.handlersPage.Paint += new System.Windows.Forms.PaintEventHandler(this.OnHandlersPagePaint);
      // 
      // handlerDropTargetPanel
      // 
      this.handlerDropTargetPanel.AllowDrop = true;
      this.handlerDropTargetPanel.BackColor = System.Drawing.Color.Transparent;
      this.handlerDropTargetPanel.Controls.Add(this.label1);
      this.handlerDropTargetPanel.Dock = System.Windows.Forms.DockStyle.Fill;
      this.handlerDropTargetPanel.Location = new System.Drawing.Point(242, 0);
      this.handlerDropTargetPanel.Name = "handlerDropTargetPanel";
      this.handlerDropTargetPanel.Size = new System.Drawing.Size(154, 439);
      this.handlerDropTargetPanel.TabIndex = 4;
      this.handlerDropTargetPanel.DragDrop += new System.Windows.Forms.DragEventHandler(this.OnHandlerDragDrop);
      this.handlerDropTargetPanel.DragEnter += new System.Windows.Forms.DragEventHandler(this.OnHandlerDragEnter);
      this.handlerDropTargetPanel.DragOver += new System.Windows.Forms.DragEventHandler(this.OnHandlerDragOver);
      this.handlerDropTargetPanel.DragLeave += new System.EventHandler(this.OnHandlerDragLeave);
      // 
      // label1
      // 
      this.label1.AutoEllipsis = true;
      this.label1.BackColor = System.Drawing.Color.Transparent;
      this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.label1.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
      this.label1.Location = new System.Drawing.Point(0, 0);
      this.label1.Name = "label1";
      this.label1.Padding = new System.Windows.Forms.Padding(1, 0, 0, 0);
      this.label1.Size = new System.Drawing.Size(154, 439);
      this.label1.TabIndex = 0;
      this.label1.Text = "Drag some handlers from the left pane.";
      this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
      // 
      // handlerListView
      // 
      this.handlerListView.AllowDrop = true;
      this.handlerListView.BorderStyle = System.Windows.Forms.BorderStyle.None;
      this.handlerListView.Dock = System.Windows.Forms.DockStyle.Left;
      this.handlerListView.FullRowSelect = true;
      this.handlerListView.LabelWrap = false;
      this.handlerListView.LargeImageList = this.handlerIconList;
      this.handlerListView.Location = new System.Drawing.Point(2, 0);
      this.handlerListView.Name = "handlerListView";
      this.handlerListView.Size = new System.Drawing.Size(240, 439);
      this.handlerListView.TabIndex = 0;
      this.handlerListView.TileSize = new System.Drawing.Size(222, 40);
      this.handlerListView.UseCompatibleStateImageBehavior = false;
      this.handlerListView.View = System.Windows.Forms.View.Tile;
      this.handlerListView.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.OnHandlerItemDrag);
      this.handlerListView.DragDrop += new System.Windows.Forms.DragEventHandler(this.OnHandlerListDragDrop);
      this.handlerListView.DragEnter += new System.Windows.Forms.DragEventHandler(this.OnHandlerListDragEnter);
      this.handlerListView.DragOver += new System.Windows.Forms.DragEventHandler(this.OnHandlerListDragOver);
      this.handlerListView.DragLeave += new System.EventHandler(this.OnHandlerListDragLeave);
      // 
      // handlerIconList
      // 
      this.handlerIconList.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
      this.handlerIconList.ImageSize = new System.Drawing.Size(32, 32);
      this.handlerIconList.TransparentColor = System.Drawing.Color.Transparent;
      // 
      // handlerListViewPaddingPanel
      // 
      this.handlerListViewPaddingPanel.BackColor = System.Drawing.Color.White;
      this.handlerListViewPaddingPanel.Dock = System.Windows.Forms.DockStyle.Left;
      this.handlerListViewPaddingPanel.Location = new System.Drawing.Point(0, 0);
      this.handlerListViewPaddingPanel.Name = "handlerListViewPaddingPanel";
      this.handlerListViewPaddingPanel.Size = new System.Drawing.Size(2, 439);
      this.handlerListViewPaddingPanel.TabIndex = 3;
      // 
      // generalPage
      // 
      this.generalPage.BackColor = System.Drawing.Color.WhiteSmoke;
      this.generalPage.Controls.Add(this.hotkeyBox);
      this.generalPage.Controls.Add(this.fixedRegionValue);
      this.generalPage.Controls.Add(this.encoderConfigureLinkLabel);
      this.generalPage.Controls.Add(this.encoderComboBox);
      this.generalPage.Controls.Add(this.encoderLabel);
      this.generalPage.Controls.Add(this.label6);
      this.generalPage.Controls.Add(panel1);
      this.generalPage.Controls.Add(this.allowOutsideDesktopCheckBox);
      this.generalPage.Controls.Add(this.displayInApplicationMenuCheckBox);
      this.generalPage.Controls.Add(this.hotkeyLabel);
      this.generalPage.Controls.Add(this.triggersTitleLabel);
      this.generalPage.Controls.Add(_separator1);
      this.generalPage.Controls.Add(this.regionComboBox);
      this.generalPage.Controls.Add(this.regionLabel);
      this.generalPage.Controls.Add(this.typeComboBox);
      this.generalPage.Controls.Add(this.typeLabel);
      this.generalPage.Controls.Add(this.nameTextBox);
      this.generalPage.Controls.Add(this.nameLabel);
      this.generalPage.Controls.Add(this.basicTitleLabel);
      this.generalPage.Location = new System.Drawing.Point(4, 32);
      this.generalPage.Name = "generalPage";
      this.generalPage.Size = new System.Drawing.Size(396, 439);
      this.generalPage.TabIndex = 0;
      this.generalPage.Text = "General";
      // 
      // hotkeyBox
      // 
      this.hotkeyBox.BackColor = System.Drawing.SystemColors.Window;
      this.hotkeyBox.Font = new System.Drawing.Font("Segoe UI", 8F);
      this.hotkeyBox.Location = new System.Drawing.Point(165, 220);
      this.hotkeyBox.Name = "hotkeyBox";
      this.hotkeyBox.ReadOnly = true;
      this.hotkeyBox.Size = new System.Drawing.Size(197, 22);
      this.hotkeyBox.TabIndex = 22;
      // 
      // fixedRegionValue
      // 
      this.fixedRegionValue.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
      this.fixedRegionValue.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
      this.fixedRegionValue.Location = new System.Drawing.Point(165, 144);
      this.fixedRegionValue.Name = "fixedRegionValue";
      this.fixedRegionValue.Size = new System.Drawing.Size(197, 19);
      this.fixedRegionValue.TabIndex = 21;
      this.fixedRegionValue.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      this.fixedRegionValue.Visible = false;
      // 
      // encoderConfigureLinkLabel
      // 
      this.encoderConfigureLinkLabel.ForeColor = System.Drawing.SystemColors.HotTrack;
      this.encoderConfigureLinkLabel.HoverColor = System.Drawing.Color.Empty;
      this.encoderConfigureLinkLabel.Location = new System.Drawing.Point(165, 398);
      this.encoderConfigureLinkLabel.Name = "encoderConfigureLinkLabel";
      this.encoderConfigureLinkLabel.RegularColor = System.Drawing.Color.Empty;
      this.encoderConfigureLinkLabel.Size = new System.Drawing.Size(70, 16);
      this.encoderConfigureLinkLabel.TabIndex = 20;
      this.encoderConfigureLinkLabel.Text = "Configure…";
      this.encoderConfigureLinkLabel.Click += new System.EventHandler(this.OnEncoderConfigureLinkLabelClick);
      // 
      // encoderComboBox
      // 
      this.encoderComboBox.DisplayMember = "Display";
      this.encoderComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.encoderComboBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.encoderComboBox.FormattingEnabled = true;
      this.encoderComboBox.Location = new System.Drawing.Point(165, 369);
      this.encoderComboBox.Name = "encoderComboBox";
      this.encoderComboBox.Size = new System.Drawing.Size(197, 23);
      this.encoderComboBox.TabIndex = 19;
      this.encoderComboBox.ValueMember = "Value";
      // 
      // encoderLabel
      // 
      this.encoderLabel.AutoSize = true;
      this.encoderLabel.Location = new System.Drawing.Point(43, 372);
      this.encoderLabel.Name = "encoderLabel";
      this.encoderLabel.Size = new System.Drawing.Size(50, 15);
      this.encoderLabel.TabIndex = 18;
      this.encoderLabel.Text = "Encoder";
      // 
      // label6
      // 
      this.label6.AutoSize = true;
      this.label6.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
      this.label6.Location = new System.Drawing.Point(43, 341);
      this.label6.Name = "label6";
      this.label6.Size = new System.Drawing.Size(47, 15);
      this.label6.TabIndex = 17;
      this.label6.Text = "Format";
      // 
      // allowOutsideDesktopCheckBox
      // 
      this.allowOutsideDesktopCheckBox.AutoSize = true;
      this.allowOutsideDesktopCheckBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.allowOutsideDesktopCheckBox.Location = new System.Drawing.Point(46, 283);
      this.allowOutsideDesktopCheckBox.Name = "allowOutsideDesktopCheckBox";
      this.allowOutsideDesktopCheckBox.Size = new System.Drawing.Size(237, 20);
      this.allowOutsideDesktopCheckBox.TabIndex = 15;
      this.allowOutsideDesktopCheckBox.Text = "Allow shortcut key outside the desktop";
      this.allowOutsideDesktopCheckBox.UseVisualStyleBackColor = true;
      // 
      // displayInApplicationMenuCheckBox
      // 
      this.displayInApplicationMenuCheckBox.AutoSize = true;
      this.displayInApplicationMenuCheckBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.displayInApplicationMenuCheckBox.Location = new System.Drawing.Point(46, 254);
      this.displayInApplicationMenuCheckBox.Name = "displayInApplicationMenuCheckBox";
      this.displayInApplicationMenuCheckBox.Size = new System.Drawing.Size(179, 20);
      this.displayInApplicationMenuCheckBox.TabIndex = 14;
      this.displayInApplicationMenuCheckBox.Text = "Display in application menu";
      this.displayInApplicationMenuCheckBox.UseVisualStyleBackColor = true;
      // 
      // hotkeyLabel
      // 
      this.hotkeyLabel.AutoSize = true;
      this.hotkeyLabel.Location = new System.Drawing.Point(43, 223);
      this.hotkeyLabel.Name = "hotkeyLabel";
      this.hotkeyLabel.Size = new System.Drawing.Size(73, 15);
      this.hotkeyLabel.TabIndex = 12;
      this.hotkeyLabel.Text = "Shortcut key";
      // 
      // triggersTitleLabel
      // 
      this.triggersTitleLabel.AutoSize = true;
      this.triggersTitleLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
      this.triggersTitleLabel.Location = new System.Drawing.Point(43, 192);
      this.triggersTitleLabel.Name = "triggersTitleLabel";
      this.triggersTitleLabel.Size = new System.Drawing.Size(52, 15);
      this.triggersTitleLabel.TabIndex = 11;
      this.triggersTitleLabel.Text = "Triggers";
      // 
      // regionComboBox
      // 
      this.regionComboBox.DisplayMember = "Display";
      this.regionComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.regionComboBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.regionComboBox.FormattingEnabled = true;
      this.regionComboBox.Location = new System.Drawing.Point(165, 118);
      this.regionComboBox.Name = "regionComboBox";
      this.regionComboBox.Size = new System.Drawing.Size(197, 23);
      this.regionComboBox.TabIndex = 8;
      this.regionComboBox.ValueMember = "Value";
      // 
      // regionLabel
      // 
      this.regionLabel.AutoSize = true;
      this.regionLabel.Location = new System.Drawing.Point(43, 121);
      this.regionLabel.Name = "regionLabel";
      this.regionLabel.Size = new System.Drawing.Size(44, 15);
      this.regionLabel.TabIndex = 7;
      this.regionLabel.Text = "Region";
      // 
      // typeComboBox
      // 
      this.typeComboBox.DisplayMember = "Display";
      this.typeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.typeComboBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.typeComboBox.FormattingEnabled = true;
      this.typeComboBox.Location = new System.Drawing.Point(165, 89);
      this.typeComboBox.Name = "typeComboBox";
      this.typeComboBox.Size = new System.Drawing.Size(197, 23);
      this.typeComboBox.TabIndex = 6;
      this.typeComboBox.ValueMember = "Value";
      // 
      // typeLabel
      // 
      this.typeLabel.AutoSize = true;
      this.typeLabel.Location = new System.Drawing.Point(43, 92);
      this.typeLabel.Name = "typeLabel";
      this.typeLabel.Size = new System.Drawing.Size(32, 15);
      this.typeLabel.TabIndex = 5;
      this.typeLabel.Text = "Type";
      // 
      // nameTextBox
      // 
      this.nameTextBox.Location = new System.Drawing.Point(165, 60);
      this.nameTextBox.Name = "nameTextBox";
      this.nameTextBox.Size = new System.Drawing.Size(197, 23);
      this.nameTextBox.TabIndex = 4;
      // 
      // nameLabel
      // 
      this.nameLabel.AutoSize = true;
      this.nameLabel.Location = new System.Drawing.Point(43, 63);
      this.nameLabel.Name = "nameLabel";
      this.nameLabel.Size = new System.Drawing.Size(39, 15);
      this.nameLabel.TabIndex = 3;
      this.nameLabel.Text = "Name";
      // 
      // basicTitleLabel
      // 
      this.basicTitleLabel.AutoSize = true;
      this.basicTitleLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
      this.basicTitleLabel.Location = new System.Drawing.Point(43, 32);
      this.basicTitleLabel.Name = "basicTitleLabel";
      this.basicTitleLabel.Size = new System.Drawing.Size(35, 15);
      this.basicTitleLabel.TabIndex = 2;
      this.basicTitleLabel.Text = "Basic";
      // 
      // tabStrip
      // 
      this.tabStrip.Controls.Add(this.generalPage);
      this.tabStrip.Controls.Add(this.handlersPage);
      this.tabStrip.Controls.Add(this.transformsPage);
      this.tabStrip.Controls.Add(this.advancedPage);
      this.tabStrip.Dock = System.Windows.Forms.DockStyle.Fill;
      this.tabStrip.ExtendTabs = true;
      this.tabStrip.ItemSize = new System.Drawing.Size(101, 28);
      this.tabStrip.Location = new System.Drawing.Point(0, 0);
      this.tabStrip.Name = "tabStrip";
      this.tabStrip.SelectedIndex = 0;
      this.tabStrip.Size = new System.Drawing.Size(404, 475);
      this.tabStrip.TabIndex = 0;
      this.tabStrip.SelectedIndexChanged += new System.EventHandler(this.OnSelectedTabChanged);
      // 
      // bottomPanel
      // 
      this.bottomPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(231)))), ((int)(((byte)(231)))));
      this.bottomPanel.Controls.Add(this.saveButton);
      this.bottomPanel.Controls.Add(this.exportLinkLabel);
      this.bottomPanel.Controls.Add(this.cancelButton);
      this.bottomPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
      this.bottomPanel.Location = new System.Drawing.Point(0, 475);
      this.bottomPanel.Name = "bottomPanel";
      this.bottomPanel.Size = new System.Drawing.Size(404, 46);
      this.bottomPanel.TabIndex = 5;
      this.bottomPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.OnBottomPanelPaint);
      // 
      // saveButton
      // 
      this.saveButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.saveButton.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.saveButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.saveButton.Location = new System.Drawing.Point(235, 12);
      this.saveButton.Name = "saveButton";
      this.saveButton.Size = new System.Drawing.Size(75, 23);
      this.saveButton.TabIndex = 6;
      this.saveButton.Text = "&Save";
      this.saveButton.UseVisualStyleBackColor = true;
      this.saveButton.Click += new System.EventHandler(this.OnSaveButtonClick);
      // 
      // exportLinkLabel
      // 
      this.exportLinkLabel.ForeColor = System.Drawing.SystemColors.HotTrack;
      this.exportLinkLabel.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(103)))), ((int)(((byte)(155)))));
      this.exportLinkLabel.Location = new System.Drawing.Point(15, 15);
      this.exportLinkLabel.Name = "exportLinkLabel";
      this.exportLinkLabel.RegularColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(103)))), ((int)(((byte)(141)))));
      this.exportLinkLabel.Size = new System.Drawing.Size(50, 16);
      this.exportLinkLabel.TabIndex = 5;
      this.exportLinkLabel.Text = "Export…";
      this.exportLinkLabel.UseSystemColor = false;
      // 
      // cancelButton
      // 
      this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.cancelButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.cancelButton.Location = new System.Drawing.Point(316, 12);
      this.cancelButton.Name = "cancelButton";
      this.cancelButton.Size = new System.Drawing.Size(75, 23);
      this.cancelButton.TabIndex = 4;
      this.cancelButton.Text = "&Cancel";
      this.cancelButton.UseVisualStyleBackColor = true;
      // 
      // WorkflowPropertyDialog
      // 
      this.AcceptButton = this.saveButton;
      this.AllowDrop = true;
      this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.CancelButton = this.cancelButton;
      this.ClientSize = new System.Drawing.Size(404, 521);
      this.Controls.Add(this.tabStrip);
      this.Controls.Add(this.bottomPanel);
      this.DoubleBuffered = true;
      this.Font = new System.Drawing.Font("Segoe UI", 9F);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
      this.KeyPreview = true;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.MinimumSize = new System.Drawing.Size(420, 560);
      this.Name = "WorkflowPropertyDialog";
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      this.handlersPage.ResumeLayout(false);
      this.handlerDropTargetPanel.ResumeLayout(false);
      this.generalPage.ResumeLayout(false);
      this.generalPage.PerformLayout();
      this.tabStrip.ResumeLayout(false);
      this.bottomPanel.ResumeLayout(false);
      this.ResumeLayout(false);

    }

    #endregion

    private TabPage advancedPage;
    private TabPage transformsPage;
    private TabPage handlersPage;
    private TabPage generalPage;
    private TabStripControl tabStrip;
    private Panel bottomPanel;
    private Button saveButton;
    private LinkLabel2 exportLinkLabel;
    private Button cancelButton;
    private Label basicTitleLabel;
    private ComboBox typeComboBox;
    private Label typeLabel;
    private TextBox nameTextBox;
    private Label nameLabel;
    private ComboBox regionComboBox;
    private Label regionLabel;
    private Label hotkeyLabel;
    private Label triggersTitleLabel;
    private ComboBox encoderComboBox;
    private Label encoderLabel;
    private Label label6;
    private CheckBox allowOutsideDesktopCheckBox;
    private CheckBox displayInApplicationMenuCheckBox;
    private LinkLabel2 encoderConfigureLinkLabel;
    private Label fixedRegionValue;
    private HotkeyBox hotkeyBox;
    private ListViewEx handlerListView;
    private ImageList handlerIconList;
    private Panel handlerListViewPaddingPanel;
    private Panel handlerDropTargetPanel;
    private Label label1;
  }
}