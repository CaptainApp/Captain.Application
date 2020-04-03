using System.ComponentModel;
using System.Windows.Forms;

namespace Captain.Application {
  sealed partial class OptionsWindow {
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
      System.Windows.Forms.Panel _separator2;
      System.Windows.Forms.Panel _separator1;
      System.Windows.Forms.Panel taskToolBarPanel;
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OptionsWindow));
      this.panel1 = new System.Windows.Forms.Panel();
      this.createWorkflowLinkButton = new Captain.Application.LinkButton();
      this.startupTitleLabel = new System.Windows.Forms.Label();
      this.notificationsTitleLabel = new System.Windows.Forms.Label();
      this.tabStrip = new Captain.Application.TabStripControl();
      this.generalPage = new System.Windows.Forms.TabPage();
      this.upgradeToFullInstallPanel = new System.Windows.Forms.Panel();
      this.performInstallNoticeLabel = new System.Windows.Forms.Label();
      this.updateManagerUnavailableLabel = new System.Windows.Forms.Label();
      this.updatesTitleLabel = new System.Windows.Forms.Label();
      this.enableStatusPopupsCheckBox = new System.Windows.Forms.CheckBox();
      this.displayTrayIconCheckBox = new System.Windows.Forms.CheckBox();
      this.autoStartCheckBox = new System.Windows.Forms.CheckBox();
      this.updateOptionsPanel = new System.Windows.Forms.Panel();
      this.checkUpdatesRadioButton = new System.Windows.Forms.RadioButton();
      this.automaticUpdatesRadioButton = new System.Windows.Forms.RadioButton();
      this.disableUpdatesRadioButton = new System.Windows.Forms.RadioButton();
      this.workflowsPage = new System.Windows.Forms.TabPage();
      this.emptyWorkflowListLabel = new System.Windows.Forms.Label();
      this.workflowContainerPanel = new System.Windows.Forms.Panel();
      this.capturePage = new System.Windows.Forms.TabPage();
      this.shortcutsPage = new System.Windows.Forms.TabPage();
      this.advancedSettingsPage = new System.Windows.Forms.TabPage();
      this.helpTip = new System.Windows.Forms.ToolTip(this.components);
      _separator2 = new System.Windows.Forms.Panel();
      _separator1 = new System.Windows.Forms.Panel();
      taskToolBarPanel = new System.Windows.Forms.Panel();
      taskToolBarPanel.SuspendLayout();
      this.tabStrip.SuspendLayout();
      this.generalPage.SuspendLayout();
      this.upgradeToFullInstallPanel.SuspendLayout();
      this.updateOptionsPanel.SuspendLayout();
      this.workflowsPage.SuspendLayout();
      this.SuspendLayout();
      // 
      // _separator2
      // 
      _separator2.BackColor = System.Drawing.Color.Gainsboro;
      _separator2.Location = new System.Drawing.Point(76, 202);
      _separator2.Name = "_separator2";
      _separator2.Size = new System.Drawing.Size(304, 1);
      _separator2.TabIndex = 9;
      // 
      // _separator1
      // 
      _separator1.BackColor = System.Drawing.Color.Gainsboro;
      _separator1.Location = new System.Drawing.Point(76, 100);
      _separator1.Name = "_separator1";
      _separator1.Size = new System.Drawing.Size(304, 1);
      _separator1.TabIndex = 5;
      // 
      // taskToolBarPanel
      // 
      taskToolBarPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
      taskToolBarPanel.Controls.Add(this.panel1);
      taskToolBarPanel.Controls.Add(this.createWorkflowLinkButton);
      taskToolBarPanel.Dock = System.Windows.Forms.DockStyle.Top;
      taskToolBarPanel.Location = new System.Drawing.Point(0, 0);
      taskToolBarPanel.Name = "taskToolBarPanel";
      taskToolBarPanel.Size = new System.Drawing.Size(464, 33);
      taskToolBarPanel.TabIndex = 1;
      // 
      // panel1
      // 
      this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
      this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
      this.panel1.Location = new System.Drawing.Point(0, 32);
      this.panel1.Name = "panel1";
      this.panel1.Size = new System.Drawing.Size(464, 1);
      this.panel1.TabIndex = 1;
      // 
      // createWorkflowLinkButton
      // 
      this.createWorkflowLinkButton.Image = null;
      this.createWorkflowLinkButton.Location = new System.Drawing.Point(4, 4);
      this.createWorkflowLinkButton.Name = "createWorkflowLinkButton";
      this.createWorkflowLinkButton.Size = new System.Drawing.Size(128, 24);
      this.createWorkflowLinkButton.TabIndex = 0;
      this.createWorkflowLinkButton.Text = "Create workflow";
      this.createWorkflowLinkButton.TintColor = System.Drawing.Color.SeaGreen;
      this.createWorkflowLinkButton.Click += new System.EventHandler(this.OnCreateWorkflowButtonClicked);
      // 
      // startupTitleLabel
      // 
      this.startupTitleLabel.AutoSize = true;
      this.startupTitleLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
      this.startupTitleLabel.Location = new System.Drawing.Point(73, 17);
      this.startupTitleLabel.Name = "startupTitleLabel";
      this.startupTitleLabel.Size = new System.Drawing.Size(49, 15);
      this.startupTitleLabel.TabIndex = 1;
      this.startupTitleLabel.Text = "Startup";
      // 
      // notificationsTitleLabel
      // 
      this.notificationsTitleLabel.AutoSize = true;
      this.notificationsTitleLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
      this.notificationsTitleLabel.Location = new System.Drawing.Point(73, 115);
      this.notificationsTitleLabel.Name = "notificationsTitleLabel";
      this.notificationsTitleLabel.Size = new System.Drawing.Size(98, 15);
      this.notificationsTitleLabel.TabIndex = 3;
      this.notificationsTitleLabel.Text = "Status reporting";
      // 
      // tabStrip
      // 
      this.tabStrip.Controls.Add(this.generalPage);
      this.tabStrip.Controls.Add(this.workflowsPage);
      this.tabStrip.Controls.Add(this.capturePage);
      this.tabStrip.Controls.Add(this.shortcutsPage);
      this.tabStrip.Controls.Add(this.advancedSettingsPage);
      this.tabStrip.Dock = System.Windows.Forms.DockStyle.Fill;
      this.tabStrip.DrawMode = System.Windows.Forms.TabDrawMode.OwnerDrawFixed;
      this.tabStrip.ExtendTabs = true;
      this.tabStrip.HotTrack = true;
      this.tabStrip.ItemSize = new System.Drawing.Size(91, 32);
      this.tabStrip.Location = new System.Drawing.Point(0, 0);
      this.tabStrip.Margin = new System.Windows.Forms.Padding(0);
      this.tabStrip.Name = "tabStrip";
      this.tabStrip.Padding = new System.Drawing.Point(0, 0);
      this.tabStrip.SelectedIndex = 0;
      this.tabStrip.Size = new System.Drawing.Size(464, 381);
      this.tabStrip.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
      this.tabStrip.TabIndex = 0;
      this.tabStrip.Selecting += new System.Windows.Forms.TabControlCancelEventHandler(this.OnSelectingPage);
      // 
      // generalPage
      // 
      this.generalPage.BackColor = System.Drawing.Color.WhiteSmoke;
      this.generalPage.Controls.Add(this.upgradeToFullInstallPanel);
      this.generalPage.Controls.Add(this.updateManagerUnavailableLabel);
      this.generalPage.Controls.Add(this.updatesTitleLabel);
      this.generalPage.Controls.Add(_separator2);
      this.generalPage.Controls.Add(this.enableStatusPopupsCheckBox);
      this.generalPage.Controls.Add(this.notificationsTitleLabel);
      this.generalPage.Controls.Add(this.displayTrayIconCheckBox);
      this.generalPage.Controls.Add(this.startupTitleLabel);
      this.generalPage.Controls.Add(this.autoStartCheckBox);
      this.generalPage.Controls.Add(_separator1);
      this.generalPage.Controls.Add(this.updateOptionsPanel);
      this.generalPage.Location = new System.Drawing.Point(0, 33);
      this.generalPage.Name = "generalPage";
      this.generalPage.Padding = new System.Windows.Forms.Padding(3);
      this.generalPage.Size = new System.Drawing.Size(464, 348);
      this.generalPage.TabIndex = 0;
      this.generalPage.Text = "General";
      this.generalPage.Layout += new System.Windows.Forms.LayoutEventHandler(this.OnGeneralPageLayout);
      // 
      // upgradeToFullInstallPanel
      // 
      this.upgradeToFullInstallPanel.Controls.Add(this.performInstallNoticeLabel);
      this.upgradeToFullInstallPanel.Location = new System.Drawing.Point(76, 235);
      this.upgradeToFullInstallPanel.Name = "upgradeToFullInstallPanel";
      this.upgradeToFullInstallPanel.Size = new System.Drawing.Size(304, 98);
      this.upgradeToFullInstallPanel.TabIndex = 20;
      this.upgradeToFullInstallPanel.Visible = false;
      // 
      // performInstallNoticeLabel
      // 
      this.performInstallNoticeLabel.Location = new System.Drawing.Point(-3, 5);
      this.performInstallNoticeLabel.Margin = new System.Windows.Forms.Padding(0);
      this.performInstallNoticeLabel.Name = "performInstallNoticeLabel";
      this.performInstallNoticeLabel.Size = new System.Drawing.Size(304, 32);
      this.performInstallNoticeLabel.TabIndex = 0;
      this.performInstallNoticeLabel.Text = "Automatic updates are not available in the standalone version of {0}.";
      // 
      // updateManagerUnavailableLabel
      // 
      this.updateManagerUnavailableLabel.AutoSize = true;
      this.updateManagerUnavailableLabel.ForeColor = System.Drawing.SystemColors.GrayText;
      this.updateManagerUnavailableLabel.Location = new System.Drawing.Point(304, 216);
      this.updateManagerUnavailableLabel.Name = "updateManagerUnavailableLabel";
      this.updateManagerUnavailableLabel.Size = new System.Drawing.Size(76, 15);
      this.updateManagerUnavailableLabel.TabIndex = 14;
      this.updateManagerUnavailableLabel.Text = "Not available";
      this.helpTip.SetToolTip(this.updateManagerUnavailableLabel, "You can\'t change these options right now.\r\nThe update manager is currently unavai" +
        "lable, busy,\r\nor you are running the application in portable mode.");
      this.updateManagerUnavailableLabel.Visible = false;
      // 
      // updatesTitleLabel
      // 
      this.updatesTitleLabel.AutoSize = true;
      this.updatesTitleLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
      this.updatesTitleLabel.Location = new System.Drawing.Point(73, 216);
      this.updatesTitleLabel.Name = "updatesTitleLabel";
      this.updatesTitleLabel.Size = new System.Drawing.Size(116, 15);
      this.updatesTitleLabel.TabIndex = 7;
      this.updatesTitleLabel.Text = "Application updates";
      // 
      // enableStatusPopupsCheckBox
      // 
      this.enableStatusPopupsCheckBox.AutoSize = true;
      this.enableStatusPopupsCheckBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.enableStatusPopupsCheckBox.Location = new System.Drawing.Point(76, 142);
      this.enableStatusPopupsCheckBox.Name = "enableStatusPopupsCheckBox";
      this.enableStatusPopupsCheckBox.Size = new System.Drawing.Size(231, 20);
      this.enableStatusPopupsCheckBox.TabIndex = 2;
      this.enableStatusPopupsCheckBox.Text = "Show status tidbits around the mouse";
      this.enableStatusPopupsCheckBox.UseVisualStyleBackColor = true;
      // 
      // displayTrayIconCheckBox
      // 
      this.displayTrayIconCheckBox.AutoSize = true;
      this.displayTrayIconCheckBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.displayTrayIconCheckBox.Location = new System.Drawing.Point(76, 70);
      this.displayTrayIconCheckBox.Name = "displayTrayIconCheckBox";
      this.displayTrayIconCheckBox.Size = new System.Drawing.Size(198, 20);
      this.displayTrayIconCheckBox.TabIndex = 1;
      this.displayTrayIconCheckBox.Text = "Display &icon in notification area";
      this.helpTip.SetToolTip(this.displayTrayIconCheckBox, "When checked, the application will display an icon in the notification area at al" +
        "l times.\r\nIf disabled, the application can still be accessed by using one of the" +
        " configured keyboard shortcuts.");
      this.displayTrayIconCheckBox.UseVisualStyleBackColor = true;
      // 
      // autoStartCheckBox
      // 
      this.autoStartCheckBox.AutoSize = true;
      this.autoStartCheckBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.autoStartCheckBox.Location = new System.Drawing.Point(76, 44);
      this.autoStartCheckBox.Name = "autoStartCheckBox";
      this.autoStartCheckBox.Size = new System.Drawing.Size(207, 20);
      this.autoStartCheckBox.TabIndex = 0;
      this.autoStartCheckBox.Text = "Start when you &log into Windows";
      this.autoStartCheckBox.UseVisualStyleBackColor = true;
      // 
      // updateOptionsPanel
      // 
      this.updateOptionsPanel.Controls.Add(this.checkUpdatesRadioButton);
      this.updateOptionsPanel.Controls.Add(this.automaticUpdatesRadioButton);
      this.updateOptionsPanel.Controls.Add(this.disableUpdatesRadioButton);
      this.updateOptionsPanel.Location = new System.Drawing.Point(76, 235);
      this.updateOptionsPanel.Name = "updateOptionsPanel";
      this.updateOptionsPanel.Size = new System.Drawing.Size(304, 98);
      this.updateOptionsPanel.TabIndex = 15;
      this.updateOptionsPanel.Visible = false;
      // 
      // checkUpdatesRadioButton
      // 
      this.checkUpdatesRadioButton.AutoSize = true;
      this.checkUpdatesRadioButton.Enabled = false;
      this.checkUpdatesRadioButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.checkUpdatesRadioButton.Location = new System.Drawing.Point(0, 29);
      this.checkUpdatesRadioButton.Name = "checkUpdatesRadioButton";
      this.checkUpdatesRadioButton.Size = new System.Drawing.Size(304, 20);
      this.checkUpdatesRadioButton.TabIndex = 18;
      this.checkUpdatesRadioButton.TabStop = true;
      this.checkUpdatesRadioButton.Text = "Check for updates but let me choose to install them";
      this.helpTip.SetToolTip(this.checkUpdatesRadioButton, resources.GetString("checkUpdatesRadioButton.ToolTip"));
      this.checkUpdatesRadioButton.UseVisualStyleBackColor = true;
      // 
      // automaticUpdatesRadioButton
      // 
      this.automaticUpdatesRadioButton.AutoSize = true;
      this.automaticUpdatesRadioButton.Enabled = false;
      this.automaticUpdatesRadioButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.automaticUpdatesRadioButton.Location = new System.Drawing.Point(0, 3);
      this.automaticUpdatesRadioButton.Name = "automaticUpdatesRadioButton";
      this.automaticUpdatesRadioButton.Size = new System.Drawing.Size(182, 20);
      this.automaticUpdatesRadioButton.TabIndex = 16;
      this.automaticUpdatesRadioButton.TabStop = true;
      this.automaticUpdatesRadioButton.Text = "Install updates automatically";
      this.helpTip.SetToolTip(this.automaticUpdatesRadioButton, resources.GetString("automaticUpdatesRadioButton.ToolTip"));
      this.automaticUpdatesRadioButton.UseVisualStyleBackColor = true;
      // 
      // disableUpdatesRadioButton
      // 
      this.disableUpdatesRadioButton.AutoSize = true;
      this.disableUpdatesRadioButton.Enabled = false;
      this.disableUpdatesRadioButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.disableUpdatesRadioButton.Location = new System.Drawing.Point(0, 55);
      this.disableUpdatesRadioButton.Name = "disableUpdatesRadioButton";
      this.disableUpdatesRadioButton.Size = new System.Drawing.Size(157, 20);
      this.disableUpdatesRadioButton.TabIndex = 17;
      this.disableUpdatesRadioButton.TabStop = true;
      this.disableUpdatesRadioButton.Text = "Don\'t check for updates";
      this.helpTip.SetToolTip(this.disableUpdatesRadioButton, resources.GetString("disableUpdatesRadioButton.ToolTip"));
      this.disableUpdatesRadioButton.UseVisualStyleBackColor = true;
      // 
      // workflowsPage
      // 
      this.workflowsPage.BackColor = System.Drawing.Color.WhiteSmoke;
      this.workflowsPage.Controls.Add(this.emptyWorkflowListLabel);
      this.workflowsPage.Controls.Add(this.workflowContainerPanel);
      this.workflowsPage.Controls.Add(taskToolBarPanel);
      this.workflowsPage.Location = new System.Drawing.Point(0, 33);
      this.workflowsPage.Margin = new System.Windows.Forms.Padding(0);
      this.workflowsPage.Name = "workflowsPage";
      this.workflowsPage.Size = new System.Drawing.Size(464, 348);
      this.workflowsPage.TabIndex = 1;
      this.workflowsPage.Text = "Workflows";
      this.workflowsPage.Layout += new System.Windows.Forms.LayoutEventHandler(this.OnWorkflowsPageLayout);
      // 
      // emptyWorkflowListLabel
      // 
      this.emptyWorkflowListLabel.Dock = System.Windows.Forms.DockStyle.Fill;
      this.emptyWorkflowListLabel.ForeColor = System.Drawing.SystemColors.GrayText;
      this.emptyWorkflowListLabel.Location = new System.Drawing.Point(0, 33);
      this.emptyWorkflowListLabel.Name = "emptyWorkflowListLabel";
      this.emptyWorkflowListLabel.Size = new System.Drawing.Size(464, 315);
      this.emptyWorkflowListLabel.TabIndex = 3;
      this.emptyWorkflowListLabel.Text = "Create a workflow for capturing your screen.";
      this.emptyWorkflowListLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
      this.emptyWorkflowListLabel.Visible = false;
      // 
      // workflowContainerPanel
      // 
      this.workflowContainerPanel.AutoScroll = true;
      this.workflowContainerPanel.Dock = System.Windows.Forms.DockStyle.Fill;
      this.workflowContainerPanel.Location = new System.Drawing.Point(0, 33);
      this.workflowContainerPanel.Name = "workflowContainerPanel";
      this.workflowContainerPanel.Size = new System.Drawing.Size(464, 315);
      this.workflowContainerPanel.TabIndex = 2;
      // 
      // capturePage
      // 
      this.capturePage.BackColor = System.Drawing.Color.WhiteSmoke;
      this.capturePage.Location = new System.Drawing.Point(0, 33);
      this.capturePage.Name = "capturePage";
      this.capturePage.Padding = new System.Windows.Forms.Padding(3);
      this.capturePage.Size = new System.Drawing.Size(464, 348);
      this.capturePage.TabIndex = 3;
      this.capturePage.Text = "Capture";
      // 
      // shortcutsPage
      // 
      this.shortcutsPage.BackColor = System.Drawing.Color.WhiteSmoke;
      this.shortcutsPage.Location = new System.Drawing.Point(0, 33);
      this.shortcutsPage.Name = "shortcutsPage";
      this.shortcutsPage.Padding = new System.Windows.Forms.Padding(3);
      this.shortcutsPage.Size = new System.Drawing.Size(464, 348);
      this.shortcutsPage.TabIndex = 4;
      this.shortcutsPage.Text = "Shortcuts";
      // 
      // advancedSettingsPage
      // 
      this.advancedSettingsPage.Location = new System.Drawing.Point(0, 33);
      this.advancedSettingsPage.Name = "advancedSettingsPage";
      this.advancedSettingsPage.Padding = new System.Windows.Forms.Padding(3);
      this.advancedSettingsPage.Size = new System.Drawing.Size(464, 348);
      this.advancedSettingsPage.TabIndex = 5;
      this.advancedSettingsPage.Text = "Advanced";
      this.advancedSettingsPage.UseVisualStyleBackColor = true;
      // 
      // helpTip
      // 
      this.helpTip.AutoPopDelay = 32767;
      this.helpTip.InitialDelay = 500;
      this.helpTip.OwnerDraw = true;
      this.helpTip.ReshowDelay = 100;
      this.helpTip.ShowAlways = true;
      this.helpTip.Draw += new System.Windows.Forms.DrawToolTipEventHandler(this.OnHelpTipDraw);
      this.helpTip.Popup += new System.Windows.Forms.PopupEventHandler(this.OnHelpTipPopup);
      // 
      // OptionsWindow
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.BackColor = System.Drawing.Color.WhiteSmoke;
      this.ClientSize = new System.Drawing.Size(464, 381);
      this.Controls.Add(this.tabStrip);
      this.DoubleBuffered = true;
      this.Font = new System.Drawing.Font("Segoe UI", 9F);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
      this.KeyPreview = true;
      this.MaximizeBox = false;
      this.MinimumSize = new System.Drawing.Size(480, 420);
      this.Name = "OptionsWindow";
      this.Text = "{0} Options";
      taskToolBarPanel.ResumeLayout(false);
      this.tabStrip.ResumeLayout(false);
      this.generalPage.ResumeLayout(false);
      this.generalPage.PerformLayout();
      this.upgradeToFullInstallPanel.ResumeLayout(false);
      this.updateOptionsPanel.ResumeLayout(false);
      this.updateOptionsPanel.PerformLayout();
      this.workflowsPage.ResumeLayout(false);
      this.ResumeLayout(false);

    }

    #endregion

    private TabStripControl tabStrip;
    private TabPage generalPage;
    private TabPage capturePage;
    private TabPage shortcutsPage;
    private CheckBox autoStartCheckBox;
    private ToolTip helpTip;
    private CheckBox displayTrayIconCheckBox;
    private CheckBox enableStatusPopupsCheckBox;
    private Label startupTitleLabel;
    private Label notificationsTitleLabel;
    private Label updatesTitleLabel;
    private Label updateManagerUnavailableLabel;
    private Panel updateOptionsPanel;
    private RadioButton checkUpdatesRadioButton;
    private RadioButton automaticUpdatesRadioButton;
    private RadioButton disableUpdatesRadioButton;
    private Panel upgradeToFullInstallPanel;
    private Label performInstallNoticeLabel;
    private TabPage workflowsPage;
    private LinkButton createWorkflowLinkButton;
    private Panel workflowContainerPanel;
    private Panel panel1;
    private Label emptyWorkflowListLabel;
    private TabPage advancedSettingsPage;
  }
}