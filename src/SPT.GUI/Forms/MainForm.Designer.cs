using System.ComponentModel;
using System.Windows.Forms;
using System.Drawing;

namespace SPT.GUI
{
    partial class MainForm
    {
        private IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            ComponentResourceManager resources = new ComponentResourceManager(typeof(MainForm));
            this.mainToolStrip = new ToolStrip();
            this.fileToolStripDropDownButton = new ToolStripDropDownButton();
            this.openFileToolStripMenuItem = new ToolStripMenuItem();
            this.mainSplitContainer = new SplitContainer();
            this.renderingSplitContainer = new SplitContainer();
            this.sourceImgPictureBox = new PictureBox();
            this.previewImgPictureBox = new PictureBox();
            this.openImgFileDialog = new OpenFileDialog();
            this.closeFileToolStripMenuItem = new ToolStripMenuItem();
            this.exitToolStripMenuItem = new ToolStripMenuItem();
            this.mainToolStrip.SuspendLayout();
            ((ISupportInitialize)this.mainSplitContainer).BeginInit();
            this.mainSplitContainer.Panel2.SuspendLayout();
            this.mainSplitContainer.SuspendLayout();
            ((ISupportInitialize)this.renderingSplitContainer).BeginInit();
            this.renderingSplitContainer.Panel1.SuspendLayout();
            this.renderingSplitContainer.Panel2.SuspendLayout();
            this.renderingSplitContainer.SuspendLayout();
            ((ISupportInitialize)this.sourceImgPictureBox).BeginInit();
            ((ISupportInitialize)this.previewImgPictureBox).BeginInit();
            SuspendLayout();
            // 
            // mainToolStrip
            // 
            this.mainToolStrip.BackColor = SystemColors.ControlLightLight;
            this.mainToolStrip.Items.AddRange(new ToolStripItem[] { this.fileToolStripDropDownButton });
            this.mainToolStrip.Location = new Point(0, 0);
            this.mainToolStrip.Name = "mainToolStrip";
            this.mainToolStrip.Size = new Size(1008, 25);
            this.mainToolStrip.TabIndex = 0;
            this.mainToolStrip.Text = "toolStrip1";
            // 
            // fileToolStripDropDownButton
            // 
            this.fileToolStripDropDownButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
            this.fileToolStripDropDownButton.DropDownItems.AddRange(new ToolStripItem[] { this.openFileToolStripMenuItem, this.closeFileToolStripMenuItem, this.exitToolStripMenuItem });
            this.fileToolStripDropDownButton.Image = (Image)resources.GetObject("fileToolStripDropDownButton.Image");
            this.fileToolStripDropDownButton.ImageTransparentColor = Color.Magenta;
            this.fileToolStripDropDownButton.Name = "fileToolStripDropDownButton";
            this.fileToolStripDropDownButton.Size = new Size(38, 22);
            this.fileToolStripDropDownButton.Text = "File";
            // 
            // openFileToolStripMenuItem
            // 
            this.openFileToolStripMenuItem.Name = "openFileToolStripMenuItem";
            this.openFileToolStripMenuItem.Size = new Size(180, 22);
            this.openFileToolStripMenuItem.Text = "Open";
            this.openFileToolStripMenuItem.Click += OpenFileToolStripMenuItem_Click;
            // 
            // mainSplitContainer
            // 
            this.mainSplitContainer.Dock = DockStyle.Fill;
            this.mainSplitContainer.Location = new Point(0, 0);
            this.mainSplitContainer.Name = "mainSplitContainer";
            // 
            // mainSplitContainer.Panel2
            // 
            this.mainSplitContainer.Panel2.Controls.Add(this.renderingSplitContainer);
            this.mainSplitContainer.Size = new Size(1008, 729);
            this.mainSplitContainer.SplitterDistance = 336;
            this.mainSplitContainer.TabIndex = 0;
            // 
            // renderingSplitContainer
            // 
            this.renderingSplitContainer.Dock = DockStyle.Fill;
            this.renderingSplitContainer.Location = new Point(0, 0);
            this.renderingSplitContainer.Name = "renderingSplitContainer";
            this.renderingSplitContainer.Orientation = Orientation.Horizontal;
            // 
            // renderingSplitContainer.Panel1
            // 
            this.renderingSplitContainer.Panel1.Controls.Add(this.sourceImgPictureBox);
            // 
            // renderingSplitContainer.Panel2
            // 
            this.renderingSplitContainer.Panel2.Controls.Add(this.previewImgPictureBox);
            this.renderingSplitContainer.Size = new Size(668, 729);
            this.renderingSplitContainer.SplitterDistance = 228;
            this.renderingSplitContainer.TabIndex = 0;
            // 
            // sourceImgPictureBox
            // 
            this.sourceImgPictureBox.Dock = DockStyle.Fill;
            this.sourceImgPictureBox.Location = new Point(0, 0);
            this.sourceImgPictureBox.Name = "sourceImgPictureBox";
            this.sourceImgPictureBox.Size = new Size(668, 228);
            this.sourceImgPictureBox.TabIndex = 0;
            this.sourceImgPictureBox.TabStop = false;
            // 
            // previewImgPictureBox
            // 
            this.previewImgPictureBox.Dock = DockStyle.Fill;
            this.previewImgPictureBox.Location = new Point(0, 0);
            this.previewImgPictureBox.Name = "previewImgPictureBox";
            this.previewImgPictureBox.Size = new Size(668, 497);
            this.previewImgPictureBox.TabIndex = 0;
            this.previewImgPictureBox.TabStop = false;
            // 
            // openImgFileDialog
            // 
            this.openImgFileDialog.FileName = "openFileDialog1";
            this.openImgFileDialog.Filter = "PNG Files (*.png)|*.png";
            // 
            // closeFileToolStripMenuItem
            // 
            this.closeFileToolStripMenuItem.Name = "closeFileToolStripMenuItem";
            this.closeFileToolStripMenuItem.Size = new Size(180, 22);
            this.closeFileToolStripMenuItem.Text = "Close";
            this.closeFileToolStripMenuItem.Click += CloseFileToolStripMenuItem_Click;
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new Size(180, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += ExitToolStripMenuItem_Click;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(1008, 729);
            this.Controls.Add(this.mainToolStrip);
            this.Controls.Add(this.mainSplitContainer);
            this.Icon = (Icon)resources.GetObject("$this.Icon");
            this.Name = "MainForm";
            this.Text = "Star Pixel Tool";
            this.mainToolStrip.ResumeLayout(false);
            this.mainToolStrip.PerformLayout();
            this.mainSplitContainer.Panel2.ResumeLayout(false);
            ((ISupportInitialize)this.mainSplitContainer).EndInit();
            this.mainSplitContainer.ResumeLayout(false);
            this.renderingSplitContainer.Panel1.ResumeLayout(false);
            this.renderingSplitContainer.Panel2.ResumeLayout(false);
            ((ISupportInitialize)this.renderingSplitContainer).EndInit();
            this.renderingSplitContainer.ResumeLayout(false);
            ((ISupportInitialize)this.sourceImgPictureBox).EndInit();
            ((ISupportInitialize)this.previewImgPictureBox).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ToolStrip mainToolStrip;
        private ToolStripDropDownButton fileToolStripDropDownButton;
        private ToolStripMenuItem openFileToolStripMenuItem;
        private SplitContainer mainSplitContainer;
        private SplitContainer renderingSplitContainer;
        private PictureBox sourceImgPictureBox;
        private PictureBox previewImgPictureBox;
        private OpenFileDialog openImgFileDialog;
        private ToolStripMenuItem closeFileToolStripMenuItem;
        private ToolStripMenuItem exitToolStripMenuItem;
    }
}
