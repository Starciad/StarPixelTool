using SPT.GUI.Managers;

using System;
using System.IO;
using System.Windows.Forms;

namespace SPT.GUI
{
    public sealed partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void OpenFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.openImgFileDialog.ShowDialog() == DialogResult.OK)
            {
                string fileName = this.openImgFileDialog.FileName;

                SImageManager.Unload();
                SImageManager.Load(fileName, File.Open(fileName, FileMode.Open, FileAccess.Read, FileShare.Read));

                this.sourceImgPictureBox.Load(fileName);
            }
        }

        private void CloseFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SImageManager.Unload();
        }

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
