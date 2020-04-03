using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace Captain.Application {
  /// <inheritdoc />
  /// <summary>
  ///   Displays information about the application
  /// </summary>
  internal sealed partial class AboutWindow : Window {
    /// <summary>
    ///   Class constructor
    /// </summary>
    public AboutWindow() {
      InitializeComponent();

      // set dialog icon and logo resource
      Icon = Resources.AppIcon;

      // format text labels
      Text = String.Format(Text, System.Windows.Forms.Application.ProductName);

      this.logoPictureBox.Image = Resources.Logo;

      this.nameLabel.Text = System.Windows.Forms.Application.ProductName;
      this.versionLabel.Text = Application.Version.ToString();
      this.distributionLabel.Text = Application.UpdateManager.Availability == UpdaterAvailability.NotSupported
        ? Resources.AboutWindow_DistType_Standalone
        : Resources.AboutWindow_DistType_Full;

      // set support URI text and center it!
      this.supportUriLinkLabel.Text = String.Format(this.supportUriLinkLabel.Text, Resources.AboutWindow_URI);
      this.supportUriLinkLabel.Left = (Width - this.supportUriLinkLabel.Width) / 2;

      // set tool tips
      this.toolTip.SetToolTip(this.versionLabel, this.versionLabel.Text);
      this.toolTip.SetToolTip(this.distributionLabel, Resources.AboutWindow_DistributionType);

      // bind updater events
      SetUpdateStatus(Application.UpdateManager.Status);
      Application.UpdateManager.OnUpdateStatusChanged += (um, us) => SetUpdateStatus(us);
      Application.UpdateManager.OnUpdateProgressChanged += (um, us, p) => SetUpdateStatus(us, p);
    }

    /// <summary>
    ///   Changes the update status label
    /// </summary>
    /// <param name="status">Update status</param>
    /// <param name="progress">Optional progress value</param>
    private void SetUpdateStatus(UpdateStatus status, int? progress = null) {
      this.updateStatusLabel.Suffix = "";

      switch (status) {
        case UpdateStatus.Idle:
          this.updateStatusLabel.Prefix = "";
          this.updateStatusLabel.Animated = false;
          break;

        case UpdateStatus.CheckingForUpdates:
          this.updateStatusLabel.Prefix = Resources.AboutWindow_UpdateStatusChecking;
          this.updateStatusLabel.Animated = true;
          break;

        case UpdateStatus.DownloadingUpdates:
          this.updateStatusLabel.Prefix = Resources.AboutWindow_UpdateStatusDownloading;
          break;

        case UpdateStatus.ApplyingUpdates:
          this.updateStatusLabel.Prefix = Resources.AboutWindow_UpdateStatusApplying;
          this.updateStatusLabel.Animated = true;
          break;

        case UpdateStatus.ReadyToRestart:
          this.updateStatusLabel.Prefix = String.Format(Resources.AboutWindow_UpdateStatusReadyToRestart,
            System.Windows.Forms.Application.ProductName);
          this.updateStatusLabel.Animated = false;
          break;
      }

      this.updateStatusLabel.Suffix = progress.HasValue ? $"({progress}%)" : "";
    }

    /// <summary>
    ///   Paints information labels
    /// </summary>
    /// <param name="sender">Control to be painted</param>
    /// <param name="eventArgs">Event arguments</param>
    private void OnLabelPaint(object sender, PaintEventArgs eventArgs) {
      var label = (Label) sender;
      eventArgs.Graphics.Clear(label.BackColor);

      // label parameters
      var labelRect = new Rectangle(0, 0, label.Width / 3, label.Height);
      const TextFormatFlags labelFlags = TextFormatFlags.EndEllipsis;

      // value parameters
      var valueRect = new Rectangle(label.Width / 3, 0, 2 * label.Width / 3, label.Height);
      const TextFormatFlags valueFlags = TextFormatFlags.EndEllipsis |
                                         TextFormatFlags.Right |
                                         TextFormatFlags.LeftAndRightPadding;

      // size of the label rectangle
      Size labelSize = TextRenderer.MeasureText(eventArgs.Graphics,
        (string) label.Tag,
        label.Font,
        labelRect.Size,
        labelFlags);

      // size of the value rectangle
      Size valueSize = TextRenderer.MeasureText(eventArgs.Graphics, label.Text, label.Font, valueRect.Size, valueFlags);

      // render label and value
      // TODO: don't hardcode color values, add support for high-contrast themes
      TextRenderer.DrawText(eventArgs.Graphics,
        (string) label.Tag,
        label.Font,
        labelRect,
        Color.FromArgb(0x666666),
        labelFlags);
      TextRenderer.DrawText(eventArgs.Graphics,
        label.Text,
        label.Font,
        valueRect,
        Color.FromArgb(0x333333),
        valueFlags);

      // draw separator
      eventArgs.Graphics.DrawLine(new Pen(Color.FromArgb(0x30, label.ForeColor)),
        labelSize.Width,
        1 + label.Height / 2,
        label.Width - valueSize.Width,
        1 + label.Height / 2);
    }

    /// <summary>
    ///   Triggered when the Close button gets clicked
    /// </summary>
    /// <param name="sender">Button object</param>
    /// <param name="eventArgs">Event arguments</param>
    private void OnCloseButtonClick(object sender, EventArgs eventArgs) {
      Close();
    }

    /// <summary>
    ///   Triggered when the support URI link label is clicked
    /// </summary>
    /// <param name="sender">Sender object</param>
    /// <param name="eventArgs">Event arguments</param>
    /// <remarks>
    ///   XXX: I'm not saying this is wrong, but it certainly *feels* wrong... idk it works at least
    /// </remarks>
    private void OnSupportLinkClick(object sender, EventArgs eventArgs) {
      Process.Start(this.supportUriLinkLabel.Text);
    }

    /// <summary>
    ///   Paints a top border on the bottom panel
    /// </summary>
    /// <param name="sender">Sender object</param>
    /// <param name="eventArgs"></param>
    private void OnBottomPanelPaint(object sender, PaintEventArgs eventArgs) {
      eventArgs.Graphics.DrawLine(new Pen(Color.FromArgb(0x10, 0, 0, 0)), 0, 0, Width, 0);
    }
  }
}