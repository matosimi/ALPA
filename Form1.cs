namespace AlterLinePictureAproximator
{
    using AtariMapMaker;
    using System.CodeDom;
    using System.Drawing;
    using System.Numerics;
    using System.Text.Json.Serialization.Metadata;

    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public byte[] atariPalRgb = new byte[256 * 3];
        //my colors:    1e,16,26,76,00
        //              14,16,48,b6,00
        public float[] atariPalLab = new float[256 * 3];
        public Color[,] customColors;
        public byte[] line0 = new byte[5] { 0x1e, 0x16, 0x26, 0x76, 0x00 };
        public byte[] line1 = new byte[5] { 0x14, 0x16, 0x48, 0xb6, 0x00 };
        public Color[,] cline0 = new Color[4, 2];
        public Color[,] cline1 = new Color[4, 2];
        public int[,,] diff_matrix;
        public AtariColorPicker atariColorPicker = new AtariColorPicker();
        public int[,] mask;
        public int[,,] bitmap;

        private void Form1_Load(object sender, EventArgs e)
        {
            /*Vector4 rgb = new Vector4(30/255f, 60/255f, 90/255f, 0);
            Vector4 lab = Conversion.RGBToLab(rgb);
            Vector4 rgb2 = Conversion.LabToRGB(lab);*/
            LoadPalette();
            CreatePal(false);
            comboBoxDistance.SelectedIndex = 0;
            comboBoxDither.SelectedIndex = 0;
        }

        private Color AverageColor(Color c1, Color c2, bool simple)
        {
            if (!simple)
            {
                int r2 = (c1.R * c1.R + c2.R * c2.R) / 2;
                int g2 = (c1.G * c1.G + c2.G * c2.G) / 2;
                int b2 = (c1.B * c1.B + c2.B * c2.B) / 2;
                return Color.FromArgb(Clamp256((int)Math.Sqrt(r2)), Clamp256((int)Math.Sqrt(g2)), Clamp256((int)Math.Sqrt(b2)));
            }

            int r = (c1.R + c2.R) / 2;
            int g = (c1.G + c2.G) / 2;
            int b = (c1.B + c2.B) / 2;
            return Color.FromArgb(r, g, b);
        }
        private Color AverageColor(int c1, int c2, bool simple)
        {
            if (!simple)
            {
                double r2 = (Math.Pow((int)atariPalRgb[c1 * 3], 2) + Math.Pow((int)atariPalRgb[c2 * 3], 2)) / 2;
                double g2 = (Math.Pow((int)atariPalRgb[c1 * 3 + 1], 2) + Math.Pow((int)atariPalRgb[c2 * 3 + 1], 2)) / 2;
                double b2 = (Math.Pow((int)atariPalRgb[c1 * 3 + 2], 2) + Math.Pow((int)atariPalRgb[c2 * 3 + 2], 2)) / 2;
                return Color.FromArgb(Clamp256((int)Math.Sqrt(r2)), Clamp256((int)Math.Sqrt(g2)), Clamp256((int)Math.Sqrt(b2)));
            }
            int r = ((int)atariPalRgb[c1 * 3] + (int)atariPalRgb[c2 * 3]) / 2;
            int g = ((int)atariPalRgb[c1 * 3 + 1] + (int)atariPalRgb[c2 * 3 + 1]) / 2;
            int b = ((int)atariPalRgb[c1 * 3 + 2] + (int)atariPalRgb[c2 * 3 + 2]) / 2;
            return Color.FromArgb(r, g, b);
        }

        private void CreatePal(bool simple)
        {
            Bitmap p = new Bitmap(128, 200);
            Graphics g = Graphics.FromImage(p);
            customColors = new Color[16, 2];

            for (int z = 0; z < 2; z++)
            {
                List<byte> l0 = line0.ToList();
                List<byte> l1 = line1.ToList();
                l0.RemoveAt(3 - z);
                l1.RemoveAt(3 - z);

                for (int a = 0; a < 4; a++)
                {
                    for (int b = 0; b < 4; b++)
                    {
                        customColors[a * 4 + b, z] = AverageColor(l0[a], l1[b], simple);
                        g.FillRectangle(new SolidBrush(customColors[a * 4 + b, z]), new Rectangle(b * 16 + (65 * z), a * 16, 16, 16));
                    }
                    cline0[a, z] = AverageColor(l0[a], l0[a], simple);
                    cline1[a, z] = AverageColor(l1[a], l1[a], simple);
                    g.FillRectangle(new SolidBrush(cline0[a, z]), new Rectangle(a * 16 + 65 * z, 65, 16, 16));
                    g.FillRectangle(new SolidBrush(cline1[a, z]), new Rectangle(a * 16 + 65 * z, 65 + 16, 16, 16));
                }
            }
            pictureBoxPalette.Image = p;
        }
        private void LoadPalette()
        {
            atariPalRgb = File.ReadAllBytes("altirraPAL.pal");
            AtariPalette.Load(atariPalRgb);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DoConvert(checkBoxSimpleAvg.Checked, comboBoxDistance.SelectedIndex, checkBoxUseDither.Checked, comboBoxDither.SelectedIndex);
        }

        public static int Clamp(int value, int min, int max)
        {
            return (value < min) ? min : (value > max) ? max : value;
        }

        public static int Clamp256(int value)
        {
            return (value < 0) ? 0 : (value > 255) ? 255 : value;
        }
        private void DoConvert(bool simple, int distanceMethod, bool dither, int ditherMethod = 0)
        {
            Bitmap b = (Bitmap)pictureBoxSource.Image;
            Bitmap t = new Bitmap(b.Width * 2, b.Height);
            Bitmap a = new Bitmap(b.Width * 2, b.Height);
            int[,,] delta_matrix = new int[b.Width + 1, b.Height + 1, 3];
            diff_matrix = new int[b.Width + 1, b.Height + 1, 2];
            bitmap = new int[b.Width, b.Height, 2];
            for (int z = 0; z < 2; z++)
            {
                delta_matrix.Initialize();
                for (int y = 0; y < pictureBoxSource.Image.Height / 2; y++)
                {
                    for (int x = 1; x < pictureBoxSource.Image.Width - 1; x++)
                    {
                        Color src = AverageColor(b.GetPixel(x, y * 2), b.GetPixel(x, y * 2 + 1), simple);
                        int[] channel = new int[3]{src.R + delta_matrix[x,y,0],
                                               src.G + delta_matrix[x,y,1],
                                               src.B + delta_matrix[x,y,2]};
                        Color srcPlusDelta = Color.FromArgb(Clamp256(channel[0]), Clamp256(channel[1]), Clamp256(channel[2]));
                        int index = FindCustomClosest2(dither ? srcPlusDelta : src, distanceMethod, z == 0 ? false : true);   //use "src" to disable error diffusion
                        diff_matrix[x, y, z] = (int)Distance(srcPlusDelta, customColors[index, z], distanceMethod);
                        // 1/2*|# 1|
                        //     |1 0|

                        // 1/4*|- # 2|
                        //     |1 1 0|

                        // 1/16*|- # 7|
                        //      |3 5 1|
                        int[] delta = new int[3] { src.R - customColors[index,z].R + (channel[0] - Clamp256(channel[0])),
                                               src.G - customColors[index,z].G + (channel[1] - Clamp256(channel[1])),
                                               src.B - customColors[index,z].B + (channel[2] - Clamp256(channel[2]))};
                        for (int i = 0; i < 3; i++)
                        {
                            switch (ditherMethod)   //chess, sierra, F-S
                            {
                                case 0: //chess
                                    if ((x + y) % 2 == 0)
                                    {
                                        delta_matrix[x + 1, y, i] = delta[i] / 2;
                                        delta_matrix[x, y + 1, i] = delta[i] / 2;
                                    }
                                    break;
                                case 1: //sierra
                                    delta_matrix[x + 1, y, i] = delta[i] / 2;
                                    delta_matrix[x + 1, y + 1, i] = delta[i] / 4;
                                    delta_matrix[x, y + 1, i] = delta[i] / 4;
                                    break;
                                case 2: //f-s
                                    delta_matrix[x + 1, y, i] = (int)float.Floor((delta[i] / 16.0f) * 7);
                                    delta_matrix[x - 1, y + 1, i] = (int)float.Floor((delta[i] / 16.0f) * 3);
                                    delta_matrix[x, y + 1, i] = (int)float.Floor((delta[i] / 16.0f) * 5);
                                    delta_matrix[x + 1, y + 1, i] = (int)float.Floor((delta[i] / 16.0f) * 1);
                                    break;
                                default:
                                    throw new NotImplementedException();
                            }

                        }

                        Color p1 = cline0[index / 4, z];
                        Color p2 = cline1[index % 4, z];
                        bitmap[x, y * 2, z] = index / 4;
                        bitmap[x, y * 2 + 1, z] = index % 4;

                        a.SetPixel(x * 2, y * 2, p1);
                        a.SetPixel(x * 2 + 1, y * 2, p1);
                        a.SetPixel(x * 2, y * 2 + 1, p2);
                        a.SetPixel(x * 2 + 1, y * 2 + 1, p2);

                        t.SetPixel(x * 2, y * 2, customColors[index, z]);
                        t.SetPixel(x * 2 + 1, y * 2, customColors[index, z]);
                        t.SetPixel(x * 2, y * 2 + 1, customColors[index, z]);
                        t.SetPixel(x * 2 + 1, y * 2 + 1, customColors[index, z]);
                    }
                }
                if (z == 0)
                {
                    pictureBoxAprox.Image = new Bitmap(t);
                    pictureBoxAprox.Size = t.Size;
                    pictureBoxAtariAprox.Image = new Bitmap(a);
                    pictureBoxAtariAprox.Size = a.Size;
                }
                else
                {
                    pictureBoxAproxInverse.Image = new Bitmap(t);
                    pictureBoxAproxInverse.Size = t.Size;
                    pictureBoxAtariAproxInverse.Image = new Bitmap(a);
                    pictureBoxAtariAproxInverse.Size = a.Size;
                }
            }
        }

        private double Distance(Color col1, Color col2, int distanceMethod)
        {
            double dist = double.MaxValue;
            switch (distanceMethod)
            {
                case 0:
                    dist = Difference(col1, col2);
                    break;
                case 1:
                    dist = RGBEuclidianDistance(col1, col2);
                    break;
                case 2:
                    dist = RGByuvDistance(col1, col2);
                    break;
                default:
                    throw new NotImplementedException();
                    break;
            }
            return dist;
        }
        private int FindCustomClosest2(Color color, int distanceMethod, bool inverse)
        {
            int index = 0;
            double maxdist = double.MaxValue;
            for (int i = 0; i < 16; i++)
            {
                double dist = Distance(color, customColors[i, inverse ? 1 : 0], distanceMethod);
                if (dist < maxdist)
                {
                    maxdist = dist;
                    index = i;
                }
            }
            return index;
        }

        private static double Difference(Color col1, Color col2)
        {
            int dr = col2.R - col1.R;
            int dg = col2.G - col1.G;
            int db = col2.B - col1.B;
            return Math.Abs(dr) + Math.Abs(dg) + Math.Abs(db);
        }

        private float RGByuvDistance(Color col1, Color col2)
        {
            uint DISTANCE_MAX = 0xffffffff;

            int dr = col2.R - col1.R;
            int dg = col2.G - col1.G;
            int db = col2.B - col1.B;

            float dy = 0.299f * dr + 0.587f * dg + 0.114f * db;
            float du = (db - dy) * 0.565f;
            float dv = (dr - dy) * 0.713f;

            float d = dy * dy + du * du + dv * dv;

            if (d > (float)DISTANCE_MAX)

                d = (float)DISTANCE_MAX;
            return d;
        }

        private float RGBEuclidianDistance(Color col1, Color col2)
        {
            uint DISTANCE_MAX = 0xffffffff;

            int dr = col2.R - col1.R;
            int dg = col2.G - col1.G;
            int db = col2.B - col1.B;

            float d = dr * dr + dg * dg + db * db;

            if (d > (float)DISTANCE_MAX)

                d = (float)DISTANCE_MAX;
            return d;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            MixIt();
        }

        private void MixIt()
        {
            Bitmap b = new Bitmap(pictureBoxAtariAprox.Image); //non inverse
            Bitmap m = new Bitmap(pictureBoxAtariAprox.Image);
            Graphics g = Graphics.FromImage(b);
            Graphics gm = Graphics.FromImage(m);
            int charHeight = b.Height / 8;
            mask = new int[32, charHeight];
            double[,,] chardiff = new double[32, charHeight, 2];
            for (int y = 0; y < charHeight; y++)
                for (int x = 0; x < 32; x++)
                {
                    for (int z = 0; z < 2; z++)
                        for (int cy = 0; cy < 4; cy++)
                            for (int cx = 0; cx < 4; cx++)
                                chardiff[x, y, z] += Math.Pow(diff_matrix[x * 4 + cx, y * 4 + cy, z], 2);
                    if (chardiff[x, y, 0] > chardiff[x, y, 1])
                    {
                        g.DrawImage(pictureBoxAtariAproxInverse.Image, x * 8, y * 8, new Rectangle(x * 8, y * 8, 8, 8), GraphicsUnit.Pixel);
                        gm.FillRectangle(new SolidBrush(Color.Black), new Rectangle(x * 8, y * 8, 8, 8));
                        mask[x, y] = 1;
                    }
                    else
                    {
                        gm.FillRectangle(new SolidBrush(Color.White), new Rectangle(x * 8, y * 8, 8, 8));
                        mask[x, y] = 0;
                    }
                }
            pictureBoxAtariMix.Image = b;
            pictureBoxAtariMix.Size = b.Size;
            pictureBoxCharMask.Image = m;
            pictureBoxCharMask.Size = m.Size;
        }

        private void checkBoxSimple_CheckedChanged(object sender, EventArgs e)
        {
            CreatePal(checkBoxSimpleAvg.Checked);
        }

        private void checkBoxUseDither_CheckedChanged(object sender, EventArgs e)
        {
            comboBoxDither.Enabled = checkBoxUseDither.Checked;
        }

        private void buttonOpen_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if (!checkBoxAutoscale.Checked)
                {
                    pictureBoxSource.Image = new Bitmap(Bitmap.FromFile(openFileDialog1.FileName));
                }
                else
                {
                    Bitmap bmp = (Bitmap)Bitmap.FromFile(openFileDialog1.FileName);
                    float ratio = bmp.Width / 256f;
                    int height = (int)(bmp.Height / ratio);
                    Bitmap scaled = new Bitmap(256, 192);
                    Graphics g = Graphics.FromImage(scaled);
                    g.DrawImage(bmp, new Rectangle(0, 0, 256, height));

                    Bitmap input = new Bitmap(128, 192);
                    g = Graphics.FromImage(input);
                    g.DrawImage(scaled, new Rectangle(0, 0, 128, 192));
                    pictureBoxSource.Image = input;
                    g.Dispose();
                    bmp.Dispose();
                    scaled.Dispose();

                }
                CreatePal(checkBoxSimpleAvg.Checked);
            }
        }

        private void pictureBoxPalette_MouseDown(object sender, MouseEventArgs e)
        {
            int[] remap = { 0, 1, 2, 4, 0, 1, 3, 4 };
            int[] joint = { 0, 1, 0, 1, 0, 1, 0, 1 };   //defines which colors are joined(constant) between dlilines
            if (e.Y > 64)
            {
                int line = (e.Y - 65) / 16;
                int index = (e.X / 16) % 8;
                Color col = atariColorPicker.Pick(line == 0 ? line0[remap[index]] : line1[remap[index]]);
                if (atariColorPicker.PickedNewColor)
                    if (line == 0)
                    {
                        line0[remap[index]] = atariColorPicker.PickedColorIndex;
                        if (joint[index] == 1)
                            line1[remap[index]] = atariColorPicker.PickedColorIndex;
                    }
                    else
                    {
                        line1[remap[index]] = atariColorPicker.PickedColorIndex;
                        if (joint[index] == 1)
                            line0[remap[index]] = atariColorPicker.PickedColorIndex;

                    }
                CreatePal(checkBoxSimpleAvg.Checked);
            }
        }

        private void AtariExport()
        {
            byte[] vram = new byte[32 * 24];
            int height = mask.Length / 32;
            for (int y = 0; y < height; y++)
                for (int x = 0; x < 32; x++)
                    vram[x + y * 32] = (byte)(x + (y % 4) * 32 + 128 * mask[x, y]);

            for (int y = height; y < 24; y++)
                for (int x = 0; x < 32; x++)
                    vram[x + y * 32] = (byte)(x + (y % 4) * 32);

            int pointer = 0;
            byte[] remap = { 1, 2, 3, 0 };
            byte[] charsets = new byte[32 * 24 * 8];
            for (int y = 0; y < height; y++)
                for (int x = 0; x < 32; x++)
                {
                    int picnum = mask[x, y];
                    for (int i = 0; i < 8; i++) //8 bytes of a char
                    {
                        byte multiplyer = 64;
                        for (int b = 0; b < 4; b++)
                        {
                            charsets[pointer] += (byte)(multiplyer * remap[bitmap[x * 4 + b, y * 8 + i, picnum]]);
                            multiplyer >>= 2;
                        }
                        if (x == 0) charsets[pointer] &= 0x3f;
                        else if (x == 31)
                            charsets[pointer] &= 0xfc;
                        pointer++;
                    }
                }

            byte[] colors = new byte[8] { line0[1], line0[4], line0[0], line1[0], line0[2], line1[2], line0[3], line1[3] };

            File.WriteAllBytes("vram.dat", vram);
            File.WriteAllBytes("font.fnt", charsets);
            File.WriteAllBytes("colors.dat", colors);

            //012c $4000 font
            //$1930 $6000 vram
            //$1c30 colors
            byte[] xex = File.ReadAllBytes("alterline2.xex");
            Array.Copy(charsets, 0, xex, 0x012c, charsets.Length);
            Array.Copy(vram, 0, xex, 0x1930, vram.Length);
            Array.Copy(colors, 0, xex, 0x1c30, colors.Length);
            File.WriteAllBytes("output.xex", xex);
        }

        private void buttonXex_Click(object sender, EventArgs e)
        {
            AtariExport();
        }
    }
}