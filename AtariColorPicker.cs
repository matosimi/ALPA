//Form na zobrazenie dialogu na vybratie farby z objektu palety AtariPalette

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Imaging;

namespace AtariMapMaker
{
    public partial class AtariColorPicker : Form
    {
        private byte selectedColorIndex;
        private byte oldColorIndex;
        private Color selectedColor;

        public AtariColorPicker()
        {
            InitializeComponent();
        }

        public byte PickedColorIndex { get { return selectedColorIndex; } }

        public bool PickedNewColor { get { return selectedColorIndex != oldColorIndex; } }

        public Color Pick(byte oldColorIndex)
        {
            this.oldColorIndex = oldColorIndex;
            this.selectedColorIndex = oldColorIndex;
            this.RenderPalette();
            this.DrawSelection(selectedColorIndex, selectedColorIndex);
            this.ShowDialog();
            return selectedColor;
        }

        private void RenderPalette()
        {
            Bitmap matrix = new Bitmap(128+16,256+16);
            Graphics gr = Graphics.FromImage(matrix);
            gr.Clear(this.BackColor);
            for (int y = 0; y < 16; y++)
            {
                gr.DrawString(String.Format("{0:X}", y), this.Font, new SolidBrush(this.ForeColor), 16 * 8 + 2, y * 16);
                for (int x = 0; x < 8; x++)
                    gr.FillRectangle(new SolidBrush(AtariPalette.GetColor(y * 16 + x * 2)), x * 16, y * 16, 16, 16);
            }
            gr.DrawRectangle(new Pen(new SolidBrush(Color.White)), (selectedColorIndex % 16) * 8, (selectedColorIndex / 16) * 16, 15, 15);
            
            for (int x = 0; x < 8; x++)
                gr.DrawString(String.Format("{0:X}", x * 2), this.Font, new SolidBrush(this.ForeColor), 16 * x, 16 * 16 + 2);
            if (pictureBox1.Image != null)
                pictureBox1.Image.Dispose();
            pictureBox1.Image = matrix;
            
            gr.Dispose();
            //matrix.Dispose();
        }

        private void DrawSelection(int oldColor, int newColor)
        {
            int w = pictureBox2.Width;
            int h = pictureBox2.Height;
            labelOldCol.Text = "$" + String.Format("{0:X2}", oldColor) + " - " + oldColor.ToString();
            labelNewCol.Text = "$" + String.Format("{0:X2}", newColor) + " - " + newColor.ToString();
            Bitmap clr = new Bitmap(w, h);
            Graphics gr = Graphics.FromImage(clr);
            gr.FillRectangle(new SolidBrush(AtariPalette.GetColor(oldColor)), 0, 0, w, h / 2);
            gr.FillRectangle(new SolidBrush(AtariPalette.GetColor(newColor)), 0, h / 2, w, h / 2);
            gr.Dispose();
            if (pictureBox2.Image != null)
                pictureBox2.Image.Dispose();
            pictureBox2.Image = clr;
        }

        private void PictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                selectedColorIndex = (byte)((e.X / 16)*2 + (e.Y / 16) * 16);
                selectedColor = AtariPalette.GetColor(selectedColorIndex);
                this.Close();
            }
        }

        private void PictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.X < 128 && e.Y < 256)
            {
                int index = (e.X / 16)*2 + (e.Y / 16) * 16;
                DrawSelection(selectedColorIndex, index);
            }
        }

        private void AtariColorPicker_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.selectedColorIndex = this.oldColorIndex;
                this.Close();
            }
        }
    }
}