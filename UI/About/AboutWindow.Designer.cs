using System.ComponentModel;
using System.Windows.Forms;

namespace Captain.Application {
  sealed partial class AboutWindow {
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
      this.logoPictureBox = new System.Windows.Forms.PictureBox();
      this.versionLabel = new System.Windows.Forms.Label();
      this.bottomPanel = new System.Windows.Forms.Panel();
      this.licensingLinkLabel = new Captain.Application.LinkLabel2();
      this.closeButton = new System.Windows.Forms.Button();
      this.supportUriLinkLabel = new Captain.Application.LinkLabel2();
      this.toolTip = new System.Windows.Forms.ToolTip(this.components);
      this.updateStatusLabel = new Captain.Application.EllipsisProgressLabel();
      this.distributionLabel = new System.Windows.Forms.Label();
      this.nameLabel = new System.Windows.Forms.Label();
      ((System.ComponentModel.ISupportInitialize)(this.logoPictureBox)).BeginInit();
      this.bottomPanel.SuspendLayout();
      this.SuspendLayout();
      // 
      // logoPictureBox
      // 
      this.logoPictureBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
      this.logoPictureBox.Location = new System.Drawing.Point(56, 57);
      this.logoPictureBox.Name = "logoPictureBox";
      this.logoPictureBox.Size = new System.Drawing.Size(192, 35);
      this.logoPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
      this.logoPictureBox.TabIndex = 0;
      this.logoPictureBox.TabStop = false;
      // 
      // versionLabel
      // 
      this.versionLabel.Location = new System.Drawing.Point(62, 176);
      this.versionLabel.Name = "versionLabel";
      this.versionLabel.Size = new System.Drawing.Size(180, 15);
      this.versionLabel.TabIndex = 1;
      this.versionLabel.Tag = "Version";
      this.versionLabel.Paint += new System.Windows.Forms.PaintEventHandler(this.OnLabelPaint);
      // 
      // bottomPanel
      // 
      this.bottomPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(231)))), ((int)(((byte)(231)))));
      this.bottomPanel.Controls.Add(this.licensingLinkLabel);
      this.bottomPanel.Controls.Add(this.closeButton);
      this.bottomPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
      this.bottomPanel.Location = new System.Drawing.Point(0, 275);
      this.bottomPanel.Name = "bottomPanel";
      this.bottomPanel.Size = new System.Drawing.Size(304, 46);
      this.bottomPanel.TabIndex = 3;
      this.bottomPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.OnBottomPanelPaint);
      // 
      // licensingLinkLabel
      // 
      this.licensingLinkLabel.ForeColor = System.Drawing.SystemColors.HotTrack;
      this.licensingLinkLabel.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(103)))), ((int)(((byte)(155)))));
      this.licensingLinkLabel.Location = new System.Drawing.Point(15, 15);
      this.licensingLinkLabel.Name = "licensingLinkLabel";
      this.licensingLinkLabel.RegularColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(103)))), ((int)(((byte)(141)))));
      this.licensingLinkLabel.Size = new System.Drawing.Size(124, 16);
      this.licensingLinkLabel.TabIndex = 5;
      this.licensingLinkLabel.Text = "Licensing information";
      this.licensingLinkLabel.UseSystemColor = false;
      // 
      // closeButton
      // 
      this.closeButton.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.closeButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.closeButton.Location = new System.Drawing.Point(217, 12);
      this.closeButton.Name = "closeButton";
      this.closeButton.Size = new System.Drawing.Size(75, 23);
      this.closeButton.TabIndex = 4;
      this.closeButton.Text = "&Close";
      this.closeButton.UseVisualStyleBackColor = true;
      this.closeButton.Click += new System.EventHandler(this.OnCloseButtonClick);
      // 
      // supportUriLinkLabel
      // 
      this.supportUriLinkLabel.ForeColor = System.Drawing.SystemColors.HotTrack;
      this.supportUriLinkLabel.HoverColor = System.Drawing.Color.Empty;
      this.supportUriLinkLabel.Location = new System.Drawing.Point(141, 143);
      this.supportUriLinkLabel.Name = "supportUriLinkLabel";
      this.supportUriLinkLabel.RegularColor = System.Drawing.Color.Empty;
      this.supportUriLinkLabel.Size = new System.Drawing.Size(22, 16);
      this.supportUriLinkLabel.TabIndex = 4;
      this.supportUriLinkLabel.Text = "{0}";
      this.supportUriLinkLabel.Click += new System.EventHandler(this.OnSupportLinkClick);
      // 
      // updateStatusLabel
      // 
      this.updateStatusLabel.Animated = false;
      this.updateStatusLabel.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
      this.updateStatusLabel.Location = new System.Drawing.Point(12, 248);
      this.updateStatusLabel.Name = "updateStatusLabel";
      this.updateStatusLabel.Prefix = "";
      this.updateStatusLabel.ShowEllipsis = false;
      this.updateStatusLabel.Size = new System.Drawing.Size(280, 15);
      this.updateStatusLabel.Suffix = "";
      this.updateStatusLabel.TabIndex = 5;
      this.updateStatusLabel.Text = "...";
      // 
      // distributionLabel
      // 
      this.distributionLabel.Location = new System.Drawing.Point(62, 200);
      this.distributionLabel.Name = "distributionLabel";
      this.distributionLabel.Size = new System.Drawing.Size(180, 15);
      this.distributionLabel.TabIndex = 6;
      this.distributionLabel.Tag = "Dist. Type";
      this.distributionLabel.Paint += new System.Windows.Forms.PaintEventHandler(this.OnLabelPaint);
      // 
      // nameLabel
      // 
      this.nameLabel.Font = new System.Drawing.Font("Segoe UI", 12F);
      this.nameLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
      this.nameLabel.Location = new System.Drawing.Point(62, 108);
      this.nameLabel.Name = "nameLabel";
      this.nameLabel.Size = new System.Drawing.Size(181, 32);
      this.nameLabel.TabIndex = 7;
      this.nameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
      this.nameLabel.UseMnemonic = false;
      // 
      // AboutWindow
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.BackColor = System.Drawing.Color.WhiteSmoke;
      this.CancelButton = this.closeButton;
      this.ClientSize = new System.Drawing.Size(304, 321);
      this.Controls.Add(this.nameLabel);
      this.Controls.Add(this.distributionLabel);
      this.Controls.Add(this.updateStatusLabel);
      this.Controls.Add(this.supportUriLinkLabel);
      this.Controls.Add(this.bottomPanel);
      this.Controls.Add(this.versionLabel);
      this.Controls.Add(this.logoPictureBox);
      this.Font = new System.Drawing.Font("Segoe UI", 9F);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
      this.KeyPreview = true;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.MinimumSize = new System.Drawing.Size(320, 360);
      this.Name = "AboutWindow";
      this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
      this.Text = "About {0}";
      ((System.ComponentModel.ISupportInitialize)(this.logoPictureBox)).EndInit();
      this.bottomPanel.ResumeLayout(false);
      this.ResumeLayout(false);

    }

    #endregion

    private PictureBox logoPictureBox;
    private Label versionLabel;
    private Button closeButton;
    private LinkLabel2 supportUriLinkLabel;
    private LinkLabel2 licensingLinkLabel;
    private ToolTip toolTip;
    private EllipsisProgressLabel updateStatusLabel;
    private Label distributionLabel;
    private Panel bottomPanel;
    private Label nameLabel;
  }
}