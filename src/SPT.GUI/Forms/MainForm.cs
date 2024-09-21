using SPT.Core.Palettes.Serializers;
using SPT.Core.Palettes;
using SPT.GUI.Managers;

using System;
using System.IO;
using System.Windows.Forms;
using SPT.Core;
using System.Diagnostics;

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

        private void TransformButton_Click(object sender, EventArgs e)
        {
            // Settings
            SPTFileSettings fileSettings = SPTSettingsManager.GetFileSettings();
            SPTPalettesSettings palettesSettings = SPTSettingsManager.GetPalettesSettings();

            using FileStream inputFs = File.Open(fileSettings.InputFilename, FileMode.Open, FileAccess.Read);
            using FileStream outputFs = File.Open(fileSettings.OutputFilename, FileMode.OpenOrCreate, FileAccess.ReadWrite);

            // Palettes
            SPTPalette customPalette = null;
            if (!string.IsNullOrWhiteSpace(palettesSettings.DefinedPalette))
            {
                customPalette = SPTPaletteSerializer.Deserialize(Path.Combine(SPTDirectory.PalettesDirectory, palettesSettings.DefinedPalette));
            }

            // Pixelator
            using SPTPixelator pixalator = new(inputFs, outputFs)
            {
                PixelateFactor = pixelateFactor,
                PaletteSize = paletteSize,
                ColorTolerance = colorTolerance,
                CustomPalette = customPalette,
                Effects = [.. effects]
            };

            // Infos
            Stopwatch processStopwatch = new();
            processStopwatch.Start();

            pixalator.InitializePixelation();
            pixalator.ExportPixelatedImage();

            // Finish
            processStopwatch.Stop();
        }
    }
}
