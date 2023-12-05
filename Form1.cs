namespace AlterLinePictureAproximator
{
    using AtariMapMaker;
    using System;
    using System.CodeDom;
    using System.Drawing;
    using System.Globalization;
    using System.Net.Security;
    using System.Numerics;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.Security;
    using System.Security.Cryptography.X509Certificates;
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
        public byte[,,] srcChannelMatrix;   //matrix containing RGB channels of source image
        public int[,] bit8map;
        public Color[] palette8 = new Color[256];
        public int[,] customP8Match = new int[256, 2];   //reset every custom palette change
        public double[,] customP8MatchDist = new double[256, 2];    ////reset every custom palette change
        public List<AlpaItem> alpaItems = new List<AlpaItem>();
        public Random rand = new Random();
        public int totalPixels;
        public byte[,,,] paletteMatch = new byte[256, 256, 256, 2];
        public class AlpaItem
        {
            public double Diff { get; set; }
            public double Ppdiff { get; set; }
            public byte[] Line0 { get; set; }
            public byte[] Line1 { get; set; }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            /*Vector4 rgb = new Vector4(30/255f, 60/255f, 90/255f, 0);
            Vector4 lab = Conversion.RGBToLab(rgb);
            Vector4 rgb2 = Conversion.LabToRGB(lab);*/
            LoadPalette();
            CreatePal(comboBoxAverMethod.SelectedIndex);
            comboBoxDistance.SelectedIndex = 0;
            comboBoxDither.SelectedIndex = 0;
            comboBoxAverMethod.SelectedIndex = 0;
            PictureToChannel(comboBoxAverMethod.SelectedIndex);
            ReduceColors();
        }


        // RGB -> YUV
        private byte[] RGB2YUV(byte R, byte G, byte B)
        {
            byte y = (byte)Clamp256(((66 * (R) + 129 * (G) + 25 * (B) + 128) >> 8) + 16);
            byte u = (byte)Clamp256(((-38 * (R) - 74 * (G) + 112 * (B) + 128) >> 8) + 128);
            byte v = (byte)Clamp256(((112 * (R) - 94 * (G) - 18 * (B) + 128) >> 8) + 128);
            return new byte[3] { y, u, v };
        }

        private byte[] YUV2RGB(byte y, byte u, byte v)
        {
            int C = y - 16;
            int D = u - 128;
            int E = v - 128;
            byte r = (byte)Clamp256((298 * C + 409 * E + 128) >> 8);
            byte g = (byte)Clamp256((298 * C - 100 * D - 208 * E + 128) >> 8);
            byte b = (byte)Clamp256((298 * C + 516 * D + 128) >> 8);
            return new byte[3] { r, g, b };
        }
    
        private Color AverageColor(Color c1, Color c2, int method)
        {
            switch (method)
            {
                case 0: //rgb simple
                    int r = (c1.R + c2.R) / 2;
                    int g = (c1.G + c2.G) / 2;
                    int b = (c1.B + c2.B) / 2;
                    return Color.FromArgb(r, g, b);
                case 1: //rgb euclid
                    int r2 = (c1.R * c1.R + c2.R * c2.R) / 2;
                    int g2 = (c1.G * c1.G + c2.G * c2.G) / 2;
                    int b2 = (c1.B * c1.B + c2.B * c2.B) / 2;
                    return Color.FromArgb(Clamp256((int)Math.Sqrt(r2)), Clamp256((int)Math.Sqrt(g2)), Clamp256((int)Math.Sqrt(b2)));
                case 2: //yuv euclid
                    byte[] z1 = RGB2YUV(c1.R, c1.G, c1.B);
                    byte[] z2 = RGB2YUV(c2.R, c2.G, c2.B);
                    int y = (z1[0] * z1[0] + z2[0] * z2[0]) / 2;
                    int u = (z1[1] * z1[1] + z2[1] * z2[1]) / 2;
                    int v = (z1[2] * z1[2] + z2[2] * z2[2]) / 2;
                    byte[] ave = YUV2RGB((byte)Clamp256((int)Math.Sqrt(y)), (byte)Clamp256((int)Math.Sqrt(u)), (byte)Clamp256((int)Math.Sqrt(v)));
                    return Color.FromArgb(ave[0], ave[1], ave[2]);
            }
            return Color.AliceBlue;
        }
        private Color AverageColor(int c1, int c2, int method)
        {

            switch (method)
            {
                case 0: //rgb simple
                    int r = ((int)atariPalRgb[c1 * 3] + (int)atariPalRgb[c2 * 3]) / 2;
                    int g = ((int)atariPalRgb[c1 * 3 + 1] + (int)atariPalRgb[c2 * 3 + 1]) / 2;
                    int b = ((int)atariPalRgb[c1 * 3 + 2] + (int)atariPalRgb[c2 * 3 + 2]) / 2;
                    return Color.FromArgb(r, g, b);
                case 1: //rgb euclid
                    double r2 = (atariPalRgb[c1 * 3] * atariPalRgb[c1 * 3] + atariPalRgb[c2 * 3] * atariPalRgb[c2 * 3]) / 2;
                    double g2 = (atariPalRgb[c1 * 3 + 1] * atariPalRgb[c1 * 3 + 1] + atariPalRgb[c2 * 3 + 1] * atariPalRgb[c2 * 3 + 1]) / 2;
                    double b2 = (atariPalRgb[c1 * 3 + 2] * atariPalRgb[c1 * 3 + 2] + atariPalRgb[c2 * 3 + 2] * atariPalRgb[c2 * 3 + 2]) / 2;
                    return Color.FromArgb(Clamp256((int)Math.Sqrt(r2)), Clamp256((int)Math.Sqrt(g2)), Clamp256((int)Math.Sqrt(b2)));
                case 2: //yuv euclid
                    byte[] z1 = RGB2YUV(atariPalRgb[c1 * 3], atariPalRgb[c1 * 3 + 1], atariPalRgb[c1 * 3 + 2]);
                    byte[] z2 = RGB2YUV(atariPalRgb[c2 * 3], atariPalRgb[c2 * 3 + 1], atariPalRgb[c2 * 3 + 2]);
                    int y = (z1[0] * z1[0] + z2[0] * z2[0]) / 2;
                    int u = (z1[1] * z1[1] + z2[1] * z2[1]) / 2;
                    int v = (z1[2] * z1[2] + z2[2] * z2[2]) / 2;
                    byte[] ave = YUV2RGB((byte)Clamp256((int)Math.Sqrt(y)), (byte)Clamp256((int)Math.Sqrt(u)), (byte)Clamp256((int)Math.Sqrt(v)));
                    return Color.FromArgb(ave[0], ave[1], ave[2]);
            }
            return Color.Yellow;
        }

        private void CreatePal(int method, bool noDraw = false)
        {
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
                        customColors[a * 4 + b, z] = AverageColor(l0[a], l1[b], method);
                    cline0[a, z] = AverageColor(l0[a], l0[a], method);
                    cline1[a, z] = AverageColor(l1[a], l1[a], method);
                }
            }

            //drawing part
            if (!noDraw)
            {
                Bitmap p = new Bitmap(128, 200);
                Graphics g = Graphics.FromImage(p);
                for (int z = 0; z < 2; z++)
                    for (int a = 0; a < 4; a++)
                    {
                        for (int b = 0; b < 4; b++)
                            g.FillRectangle(new SolidBrush(customColors[a * 4 + b, z]), new Rectangle(b * 16 + (65 * z), a * 16, 16, 16));
                        g.FillRectangle(new SolidBrush(cline0[a, z]), new Rectangle(a * 16 + 65 * z, 65, 16, 16));
                        g.FillRectangle(new SolidBrush(cline1[a, z]), new Rectangle(a * 16 + 65 * z, 65 + 16, 16, 16));
                    }
                pictureBoxPalette.Image = p;
            }
            Array.Clear(paletteMatch);
        }
        private void LoadPalette()
        {
            atariPalRgb = File.ReadAllBytes("altirraPAL.pal");
            AtariPalette.Load(atariPalRgb);
        }

        private void ButtonApproximate_Click(object sender, EventArgs e)
        {
            DoConvert(comboBoxAverMethod.SelectedIndex, comboBoxDistance.SelectedIndex, checkBoxUseDither.Checked, comboBoxDither.SelectedIndex);
        }

        public static int Clamp(int value, int min, int max)
        {
            return (value < min) ? min : (value > max) ? max : value;
        }

        public static int Clamp256(int value)
        {
            return (value < 0) ? 0 : (value > 255) ? 255 : value;
        }

        private void PictureToChannel(int method)
        {
            Bitmap b = (Bitmap)pictureBoxSource.Image;
            Bitmap t = new Bitmap(pictureBoxSource.Image.Width * 2, pictureBoxSource.Image.Height);
            srcChannelMatrix = new byte[b.Width, b.Height / 2, 3];
            for (int y = 0; y < pictureBoxSource.Image.Height / 2; y++)
            {
                for (int x = 0; x < pictureBoxSource.Image.Width; x++)
                {
                    Color src = AverageColor(b.GetPixel(x, y * 2), b.GetPixel(x, y * 2 + 1), method);
                    srcChannelMatrix[x, y, 0] = src.R;
                    srcChannelMatrix[x, y, 1] = src.G;
                    srcChannelMatrix[x, y, 2] = src.B;

                    t.SetPixel(x * 2, y * 2, src);
                    t.SetPixel(x * 2 + 1, y * 2, src);
                    t.SetPixel(x * 2, y * 2 + 1, src);
                    t.SetPixel(x * 2 + 1, y * 2 + 1, src);
                }
            }
            pictureBoxSrcData.Image = new Bitmap(t);
            pictureBoxSrcData.Size = t.Size;
            totalPixels = srcChannelMatrix.Length / 3;
        }

        private double CalcDiff(int averMethod, int distanceMethod, bool dither, int ditherMethod = 0)
        {
            int width = srcChannelMatrix.GetLength(0);
            int height = srcChannelMatrix.GetLength(1);
            int[,,] delta_matrix = new int[width + 1, height + 1, 3];
            diff_matrix = new int[width + 1, height + 1, 2];
            //bitmap = new int[width, height, 2];
            Array.Clear(customP8Match);
            Array.Clear(customP8MatchDist);
            for (int z = 0; z < 2; z++)
            {
                Array.Clear(delta_matrix);
                for (int y = 0; y < srcChannelMatrix.GetLength(1); y++)
                {
                    for (int x = 0; x < srcChannelMatrix.GetLength(0); x++)
                    {
                        // Color src = AverageColor(b.GetPixel(x, y * 2), b.GetPixel(x, y * 2 + 1), simple);
                        int[] channel = new int[3]{srcChannelMatrix[x,y,0] + delta_matrix[x,y,0],
                                               srcChannelMatrix[x,y,1] + delta_matrix[x,y,1],
                                               srcChannelMatrix[x,y,2] + delta_matrix[x,y,2]};
                        int[] rtn = new int[2];
                        if (!checkBoxColorReduction.Checked)
                        {
                            Color srcPlusDelta = Color.FromArgb(Clamp256(channel[0]), Clamp256(channel[1]), Clamp256(channel[2]));
                            rtn = FindCustomClosest2(dither ? srcPlusDelta : Color.FromArgb(srcChannelMatrix[x, y, 0], srcChannelMatrix[x, y, 1], srcChannelMatrix[x, y, 2]), distanceMethod, z == 0 ? false : true);   //use "src" to disable error diffusion
                        }
                        else
                        {
                            rtn = FindCustomClosest2Reduced(bit8map[x, y], distanceMethod, z == 0 ? false : true);   //use "src" to disable error diffusion
                        }
                        int index = rtn[0];
                        int diff = rtn[1];
                        diff_matrix[x, y, z] = diff; //(int)Distance(srcPlusDelta, customColors[index, z], distanceMethod);
                        // 1/2*|# 1|
                        //     |1 0|

                        // 1/4*|- # 2|
                        //     |1 1 0|

                        // 1/16*|- # 7|
                        //      |3 5 1|
                        if (dither)
                        {
                            int[] delta = new int[3] { srcChannelMatrix[x,y,0] - customColors[index,z].R + (channel[0] - Clamp256(channel[0])),
                                                   srcChannelMatrix[x,y,1] - customColors[index,z].G + (channel[1] - Clamp256(channel[1])),
                                                   srcChannelMatrix[x,y,2] - customColors[index,z].B + (channel[2] - Clamp256(channel[2]))};
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
                                        if (x - 1 >= 0)
                                            delta_matrix[x - 1, y + 1, i] = delta[i] / 4;
                                        delta_matrix[x, y + 1, i] = delta[i] / 4;
                                        break;
                                    case 2: //f-s
                                        delta_matrix[x + 1, y, i] = (int)float.Floor((delta[i] / 16.0f) * 7);
                                        if (x - 1 >= 0)
                                            delta_matrix[x - 1, y + 1, i] = (int)float.Floor((delta[i] / 16.0f) * 3);
                                        delta_matrix[x, y + 1, i] = (int)float.Floor((delta[i] / 16.0f) * 5);
                                        delta_matrix[x + 1, y + 1, i] = (int)float.Floor((delta[i] / 16.0f) * 1);
                                        break;
                                    default:
                                        throw new NotImplementedException();
                                }

                            }
                        }
                    }
                }
            }
            int charHeight = height / 4;
            mask = new int[32, charHeight];
            double[,,] chardiff = new double[32, charHeight, 2];
            double totalDiff = 0;
            for (int y = 0; y < charHeight; y++)
                for (int x = 0; x < 32; x++)
                {
                    for (int z = 0; z < 2; z++)
                        for (int cy = 0; cy < 4; cy++)
                            for (int cx = 0; cx < 4; cx++)
                                chardiff[x, y, z] += Math.Pow(diff_matrix[x * 4 + cx, y * 4 + cy, z], 2);

                    mask[x, y] = (chardiff[x, y, 0] > chardiff[x, y, 1]) ? 1 : 0; //1=black
                    totalDiff += chardiff[x, y, mask[x, y]];
                }
            return totalDiff;
        }

        private void DoConvert(int method, int distanceMethod, bool dither, int ditherMethod = 0)
        {
            Bitmap b = (Bitmap)pictureBoxSource.Image;
            Bitmap t = new Bitmap(b.Width * 2, b.Height);
            Bitmap a = new Bitmap(b.Width * 2, b.Height);
            int[,,] delta_matrix = new int[b.Width + 1, b.Height + 1, 3];
            diff_matrix = new int[b.Width + 1, b.Height + 1, 2];
            bitmap = new int[b.Width, b.Height, 2];
            Array.Clear(customP8Match);
            Array.Clear(customP8MatchDist);
            for (int z = 0; z < 2; z++)
            {
                Array.Clear(delta_matrix);
                for (int y = 0; y < srcChannelMatrix.GetLength(1); y++)
                {
                    for (int x = 0; x < srcChannelMatrix.GetLength(0); x++)
                    {
                        // Color src = AverageColor(b.GetPixel(x, y * 2), b.GetPixel(x, y * 2 + 1), simple);
                        int[] channel = new int[3]{srcChannelMatrix[x,y,0] + delta_matrix[x,y,0],
                                               srcChannelMatrix[x,y,1] + delta_matrix[x,y,1],
                                               srcChannelMatrix[x,y,2] + delta_matrix[x,y,2]};
                        int[] rtn = new int[2];
                        if (!checkBoxColorReduction.Checked)
                        {
                            Color srcPlusDelta = Color.FromArgb(Clamp256(channel[0]), Clamp256(channel[1]), Clamp256(channel[2]));
                            rtn = FindCustomClosest2(dither ? srcPlusDelta : Color.FromArgb(srcChannelMatrix[x, y, 0], srcChannelMatrix[x, y, 1], srcChannelMatrix[x, y, 2]), distanceMethod, z == 0 ? false : true);   //use "src" to disable error diffusion
                        }
                        else
                        {
                            rtn = FindCustomClosest2Reduced(bit8map[x, y], distanceMethod, z == 0 ? false : true);   //use "src" to disable error diffusion
                        }
                        int index = rtn[0];
                        int diff = rtn[1];
                        diff_matrix[x, y, z] = diff; //(int)Distance(srcPlusDelta, customColors[index, z], distanceMethod);
                        // 1/2*|# 1|
                        //     |1 0|

                        // 1/4*|- # 2|
                        //     |1 1 0|

                        // 1/16*|- # 7|
                        //      |3 5 1|
                        if (dither)
                        {
                            int[] delta = new int[3] { srcChannelMatrix[x,y,0] - customColors[index,z].R + (channel[0] - Clamp256(channel[0])),
                                                   srcChannelMatrix[x,y,1] - customColors[index,z].G + (channel[1] - Clamp256(channel[1])),
                                                   srcChannelMatrix[x,y,2] - customColors[index,z].B + (channel[2] - Clamp256(channel[2]))};
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
                                        if (x - 1 >= 0)
                                            delta_matrix[x - 1, y + 1, i] = delta[i] / 4;
                                        delta_matrix[x, y + 1, i] = delta[i] / 4;
                                        break;
                                    case 2: //f-s
                                        delta_matrix[x + 1, y, i] = (int)float.Floor((delta[i] / 16.0f) * 7);
                                        if (x - 1 >= 0)
                                            delta_matrix[x - 1, y + 1, i] = (int)float.Floor((delta[i] / 16.0f) * 3);
                                        delta_matrix[x, y + 1, i] = (int)float.Floor((delta[i] / 16.0f) * 5);
                                        delta_matrix[x + 1, y + 1, i] = (int)float.Floor((delta[i] / 16.0f) * 1);
                                        break;
                                    default:
                                        throw new NotImplementedException();
                                }

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
                case 3:
                    dist = YUVEuclidianDistance(col1, col2);
                    break;
                case 4:
                    dist = WeightedRGBDistance(col1, col2);
                    break;
                default:
                    throw new NotImplementedException();
            }
            return dist;
        }

        private int[] FindCustomClosest2Reduced(int p8index, int distanceMethod, bool inverse)
        {
            int inv = inverse ? 1 : 0;
            double[] mindist = new double[2] { double.MaxValue, double.MaxValue };
            int[] index = new int[2];
            int indexs = customP8Match[p8index, inv] - 1;
            if (indexs == -1)
            {
                for (byte i = 0; i < 16; i++)
                {
                    if (i<2 || i==4 || i==5 || i==15)   //common colors for normal and inverse
                    {
                        double dist = Distance(palette8[p8index], customColors[i, 0], distanceMethod);
                        if (dist < mindist[0])
                        {
                            mindist[0] = dist;
                            index[0] = i;
                        }
                        if (dist < mindist[1])
                        {
                            mindist[1] = dist;
                            index[1] = i;
                        }
                    }
                    else
                    {
                        for (int j = 0; j < 2; j++) //inverse
                        {
                            double dist = Distance(palette8[p8index], customColors[i, j], distanceMethod);
                            if (dist < mindist[j])
                            {
                                mindist[j] = dist;
                                index[j] = i;
                            }
                        }
                    }
                }
                customP8Match[p8index, 0] = (byte)(index[0] + 1);
                customP8MatchDist[p8index, 0] = mindist[0];
                customP8Match[p8index, 1] = (byte)(index[1] + 1);
                customP8MatchDist[p8index, 1] = mindist[1];
                indexs = index[inv];
            }
            else
            {
                mindist[0] = customP8MatchDist[p8index, inv];
            }
            return new int[2] { indexs, (int)mindist[0] };
        }

        private int[] FindCustomClosest2(Color color, int distanceMethod, bool inverse)
        {
            //return new int[2] { 0, 0 };
            double mindist = double.MaxValue;
            int index = paletteMatch[color.R, color.G, color.B, inverse ? 1 : 0] - 1;
            if (index < 0)
            {
                for (byte i = 0; i < 16; i++)
                {
                    double dist = Distance(color, customColors[i, inverse ? 1 : 0], distanceMethod);
                    if (dist < mindist)
                    {
                        mindist = dist;
                        index = i;
                    }
                }
                paletteMatch[color.R, color.G, color.B, inverse ? 1 : 0] = (byte)(index + 1);
            }
            else
            {
                mindist = Distance(color, customColors[index, inverse ? 1 : 0], distanceMethod);
            }
            return new int[2] { index, (int)mindist };
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
            //uint DISTANCE_MAX = 0xffffffff;

            int dr = col2.R - col1.R;
            int dg = col2.G - col1.G;
            int db = col2.B - col1.B;

            float dy = 0.299f * dr + 0.587f * dg + 0.114f * db;
            float du = (db - dy) * 0.565f;
            float dv = (dr - dy) * 0.713f;

            float d = dy * dy + du * du + dv * dv;

            //if (d > (float)DISTANCE_MAX)

            //    d = (float)DISTANCE_MAX;
            return d;
        }

        private float RGBEuclidianDistance(Color col1, Color col2)
        {
            //uint DISTANCE_MAX = 0xffffffff;

            int dr = col2.R - col1.R;
            int dg = col2.G - col1.G;
            int db = col2.B - col1.B;

            float d = dr * dr + dg * dg + db * db;

            //if (d > (float)DISTANCE_MAX)

            //    d = (float)DISTANCE_MAX;
            return d;
        }

        private float WeightedRGBDistance(Color e1, Color e2)
        {
            long rmean = ((long)e1.R + (long)e2.R) / 2;
            long r = (long)e1.R - (long)e2.R;
            long g = (long)e1.G - (long)e2.G;
            long b = (long)e1.B - (long)e2.B;
            return (float)Math.Sqrt((((512 + rmean) * r * r) >> 8) + 4 * g * g + (((767 - rmean) * b * b) >> 8));
        }

        private float YUVEuclidianDistance(Color c1, Color c2)
        {
            //uint DISTANCE_MAX = 0xffffffff;
            byte[] z1 = RGB2YUV(c1.R, c1.G, c1.B);
            byte[] z2 = RGB2YUV(c2.R, c2.G, c2.B);
            int dy = z1[0] - z2[0];
            int du = z1[1] - z2[1];
            int dv = z1[2] - z2[2];

            float d = dy * dy + du * du + dv * dv;

            //if (d > (float)DISTANCE_MAX)

            //    d = (float)DISTANCE_MAX;
            return d;
        }

        private void ButtonMixIt_Click(object sender, EventArgs e)
        {
            double totalDiffMix = MixIt();
            //double totalDiff = CalcDiff(comboBoxAverMethod.SelectedIndex, comboBoxDistance.SelectedIndex, checkBoxUseDither.Checked, comboBoxDither.SelectedIndex);
            //buttonMixIt.Text = $"Mix:{((int)(totalDiff/totalPixels)).ToString()} Calc:{((int)(totalDiffMix/totalPixels)).ToString()}";
            labelDiff.Text = $"Diff: {(int)(totalDiffMix/totalPixels)}";
        }

        private double MixIt()
        {
            Bitmap b = new Bitmap(pictureBoxAtariAprox.Image); //non inverse
            Bitmap m = new Bitmap(pictureBoxAtariAprox.Image);
            Bitmap a = new Bitmap(pictureBoxAprox.Image); //non inverse (non atari)
            Graphics gb = Graphics.FromImage(b);
            Graphics gm = Graphics.FromImage(m);
            Graphics ga = Graphics.FromImage(a);
            double totalDiff = 0;
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
                        gb.DrawImage(pictureBoxAtariAproxInverse.Image, x * 8, y * 8, new Rectangle(x * 8, y * 8, 8, 8), GraphicsUnit.Pixel);
                        ga.DrawImage(pictureBoxAproxInverse.Image, x * 8, y * 8, new Rectangle(x * 8, y * 8, 8, 8), GraphicsUnit.Pixel);
                        gm.FillRectangle(new SolidBrush(Color.Black), new Rectangle(x * 8, y * 8, 8, 8));
                        mask[x, y] = 1;
                        totalDiff += chardiff[x, y, 1];
                    }
                    else if (chardiff[x, y, 0] == chardiff[x, y, 1])
                    {
                        gm.FillRectangle(new SolidBrush(Color.Gray), new Rectangle(x * 8, y * 8, 8, 8));    //same one
                        mask[x, y] = 0;
                        totalDiff += chardiff[x, y, 1]; //does not matter if 1 or 0
                    }
                    else
                    {
                        gm.FillRectangle(new SolidBrush(Color.White), new Rectangle(x * 8, y * 8, 8, 8));
                        mask[x, y] = 0;
                        totalDiff += chardiff[x, y, 0];
                    }
                }
            pictureBoxAtariMix.Image = b;
            pictureBoxAtariMix.Size = b.Size;
            pictureBoxCharMask.Image = m;
            pictureBoxCharMask.Size = m.Size;
            pictureBoxAproxMix.Image = a;
            pictureBoxAproxMix.Size = a.Size;

            return totalDiff;
        }

        private void checkBoxUseDither_CheckedChanged(object sender, EventArgs e)
        {
            comboBoxDither.Enabled = checkBoxUseDither.Checked;
        }

        private void ButtonOpen_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                Bitmap bmp = new Bitmap(Bitmap.FromFile(openFileDialog1.FileName));
                if (!checkBoxAutoscale.Checked && (bmp.Width > 256 || bmp.Height > 192))
                {
                    if (MessageBox.Show($"Picture {openFileDialog1.FileName} has resolution {bmp.Width}x{bmp.Height} pixels. Would you like to enable autoscale?", "Input picture seems too big", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                    {
                        checkBoxAutoscale.Checked = true;
                    }
                }

                if (!checkBoxAutoscale.Checked)
                {
                    pictureBoxSource.Image = bmp;
                }
                else
                {
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
                PictureToChannel(comboBoxAverMethod.SelectedIndex);
                CreatePal(comboBoxAverMethod.SelectedIndex);
                if (checkBoxColorReduction.Checked)
                    ReduceColors();
            }
        }

        private void CentauriInit(int population)
        {
            alpaItems.Clear();
            Random rand = new Random();

            for (int i = 0; i < population; i++)
            {
                if (i > 0) //keep 0.iteration equal to colors currently set
                {
                    for (int j = 0; j < 4; j++)
                    {
                        line0[j] = (byte)(((byte)rand.Next(255)) & 0xFE);
                        line1[j] = (byte)(((byte)rand.Next(255)) & 0xFE);
                    }
                }
                line1[1] = line0[1]; //common color
                CreatePal(comboBoxAverMethod.SelectedIndex, true);
                double totalDiff = CalcDiff(comboBoxAverMethod.SelectedIndex, comboBoxDistance.SelectedIndex, checkBoxUseDither.Checked, comboBoxDither.SelectedIndex);
                AlpaItem ai = new AlpaItem();
                ai.Diff = totalDiff;
                ai.Ppdiff = totalDiff / totalPixels;
                ai.Line0 = (byte[])line0.Clone();
                ai.Line1 = (byte[])line1.Clone();
                alpaItems.Add(ai);
            }
            List<AlpaItem> sorted = alpaItems.OrderBy(d => d.Diff).ToList();
            if (alpaItems.Count > population / 4)
                sorted.RemoveRange(population / 4 - 1, alpaItems.Count - (population / 4) - 1);
            alpaItems = sorted;
            ListPopulation(); // fill up the listview
        }

        private void Centauri(int generations, int population)
        {
            progressBarAI.Value = 0;
            progressBarAI.Maximum = generations;
            //fix population not dividable by 4
            if (population % 4 != 0)
            {
                population += population % 4;
                numericUpDownPopulation.Value = population;
            }

            //fix change of population - generate randoms if population extended
            while (alpaItems.Count < population / 4)
            {
                for (int j = 0; j < 4; j++)
                {
                    line0[j] = (byte)(((byte)rand.Next(255)) & 0xFE);
                    line1[j] = (byte)(((byte)rand.Next(255)) & 0xFE);
                }
                line1[1] = line0[1]; //common color
                AlpaItem ai = new AlpaItem();
                ai.Diff = Double.MaxValue;
                ai.Ppdiff = Double.MaxValue;
                ai.Line0 = (byte[])line0.Clone();
                ai.Line1 = (byte[])line1.Clone();
                alpaItems.Add(ai);
            }

            double bestDiff = double.MaxValue;
            for (int i = 0; i < generations; i++)
            {
                CentauriGeneration(population);
                Array.Copy(alpaItems[0].Line0, line0, line0.Length);
                Array.Copy(alpaItems[0].Line1, line1, line1.Length);
                //line0 = alpaItems[0].line0;
                //line1 = alpaItems[0].line1;
                labelDiff.Text = $"Diff: {(int)alpaItems[0].Ppdiff}";
                if (checkBoxAutoUpdate.Checked && (bestDiff > alpaItems[0].Ppdiff)) //show progress
                {
                    bestDiff = alpaItems[0].Ppdiff;
                    CreatePal(comboBoxAverMethod.SelectedIndex, false);
                    DoConvert(comboBoxAverMethod.SelectedIndex, comboBoxDistance.SelectedIndex, checkBoxUseDither.Checked, comboBoxDither.SelectedIndex);
                    MixIt();
                }
                else
                {
                    CreatePal(comboBoxAverMethod.SelectedIndex, true);
                }
                progressBarAI.Value = i + 1;
                labelGenerationDone.Text = $"{i + 1}";
                this.Refresh();
            }
            ListPopulation(); // fill up the listview
            
            CreatePal(comboBoxAverMethod.SelectedIndex, false);
            DoConvert(comboBoxAverMethod.SelectedIndex, comboBoxDistance.SelectedIndex, checkBoxUseDither.Checked, comboBoxDither.SelectedIndex);
            MixIt();
        }

        private void ListPopulation()
        {
            listViewPopulation.Items.Clear();
            for (int i = 0; i < alpaItems.Count; i++)
            {
                listViewPopulation.Items.Add($"{i}: {((int)alpaItems[i].Ppdiff).ToString()}");
            }
        }

        private void CentauriGeneration(int population)
        {
            for (int j = 0; j < 3; j++)
                for (int i = 0; i < population / 4; i++)
                {
                    AlpaItem ai = new AlpaItem();
                    Mutate(alpaItems[i].Line0, alpaItems[i].Line1, ai);
                    for (int k = 0; k < j; k++)
                        Mutate(ai.Line0, ai.Line1, ai);
                    Array.Copy(ai.Line0, line0, line0.Length);
                    Array.Copy(ai.Line1, line1, line1.Length);
                    CreatePal(comboBoxAverMethod.SelectedIndex, true);
                    double totalDiff = CalcDiff(comboBoxAverMethod.SelectedIndex, comboBoxDistance.SelectedIndex, checkBoxUseDither.Checked, comboBoxDither.SelectedIndex);
                    ai.Diff = totalDiff;
                    ai.Ppdiff = totalDiff / totalPixels;
                    alpaItems.Add(ai);
                }
            List<AlpaItem> sorted = alpaItems.OrderBy(d => d.Diff).ToList();
            int amount = alpaItems.Count;
            if (amount > population / 4)
                sorted.RemoveRange(population / 4 - 1, amount - (population / 4) - 1);
            sorted = sorted.DistinctBy(d => d.Diff).ToList();
            alpaItems = sorted;

        }

        private void Mutate(byte[] srcline0, byte[] srcline1, AlpaItem ai)
        {
            byte[] shifts = { 256 - 2, 2, 256 - 16, 16 };
            byte[] line;
            byte[] lineSec;
            if (rand.Next(1) == 0)
            {
                line = (byte[])srcline0.Clone();
                lineSec = (byte[])srcline1.Clone();
            }
            else
            {
                line = (byte[])srcline1.Clone();
                lineSec = (byte[])srcline0.Clone();
            }
            int mutationType = rand.Next(6);
            int index4 = rand.Next(4);
            switch (mutationType)
            {
                case 0:
                case 1:
                case 2:
                case 3:
                    line[index4] += shifts[mutationType];
                    break;
                case 4: //random
                    line[index4] = (byte)(((byte)rand.Next(256)) & 0xfe);
                    break;
                case 5: //swap
                    int whichOne = rand.Next(8);
                    while (whichOne == 5)
                        whichOne = rand.Next(8);
                    if (whichOne < 4)
                        (line[index4], line[whichOne]) = (line[whichOne], line[index4]);
                    else
                        (line[index4], lineSec[whichOne - 4]) = (lineSec[whichOne - 4], line[index4]);
                    break;
            }
            lineSec[1] = line[1];   //common color
            ai.Line0 = line;
            ai.Line1 = lineSec;
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
                CreatePal(comboBoxAverMethod.SelectedIndex);
                if (checkBoxAutoUpdate.Checked)
                {
                    DoConvert(comboBoxAverMethod.SelectedIndex, comboBoxDistance.SelectedIndex, checkBoxUseDither.Checked, comboBoxDither.SelectedIndex);
                    MixIt();
                }
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

            byte[] colors = new byte[9] { line0[1], line0[4], line0[0], line1[0], line0[2], line1[2], line0[3], line1[3], 0 };
            colors[8] = (byte)(checkBoxInterlace.Checked ? 1 : 0);

            File.WriteAllBytes("vram.dat", vram);
            File.WriteAllBytes("font.fnt", charsets);
            File.WriteAllBytes("colors.dat", colors);

            //012c $4000 font   01d3
            //$1930 $6000 vram  19d7
            //$1c30 colors      1cd7
            byte[] xex = File.ReadAllBytes("alterline2.xex");
            Array.Copy(charsets, 0, xex, 0x01d3, charsets.Length);
            Array.Copy(vram, 0, xex, 0x19d7, vram.Length);
            Array.Copy(colors, 0, xex, 0x1cd7, colors.Length);
            File.WriteAllBytes("output.xex", xex);
        }

        private void buttonXex_Click(object sender, EventArgs e)
        {
            AtariExport();
        }

        private void buttonAlpaCentauriAI_Click(object sender, EventArgs e)
        {
            Centauri((int)numericUpDownGeneration.Value, (int)numericUpDownPopulation.Value);
        }

        private void buttonAlpaCentauriInit_Click(object sender, EventArgs e)
        {
            CentauriInit((int)numericUpDownGeneration.Value);
            buttonAlpaCentauriAI.Enabled = true;
        }

        private void listViewPopulation_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listViewPopulation.SelectedIndices.Count > 0)
            {
                Array.Copy(alpaItems[listViewPopulation.SelectedIndices[0]].Line0, line0, line0.Length);
                Array.Copy(alpaItems[listViewPopulation.SelectedIndices[0]].Line1, line1, line0.Length);
                CreatePal(comboBoxAverMethod.SelectedIndex, false);
                DoConvert(comboBoxAverMethod.SelectedIndex, comboBoxDistance.SelectedIndex, checkBoxUseDither.Checked, comboBoxDither.SelectedIndex);
                MixIt();
            }
        }

        public class RGBHisto
        {
            public int R { get; set; }

            public int G { get; set; }

            public int B { get; set; }
            public int Amount { get; set; }
        }

        public class BinarySpatialPartitioningNode
        {
            public BinarySpatialPartitioningNode(List<RGBHisto> colors)
            {
                Colors = colors;
            }
            public List<RGBHisto> Colors { get; set; }
        }

        private void ReduceColors()
        {

            Dictionary<int, int> histogram = new Dictionary<int, int>();
            int width = srcChannelMatrix.GetLength(0);
            int height = srcChannelMatrix.GetLength(1);
            //create histogram
            for (int y = 0; y < height; y++)
                for (int x = 0; x < width; x++)
                {
                    int rgb = ((srcChannelMatrix[x, y, 0] << 16)) | ((srcChannelMatrix[x, y, 1] << 8)) | (srcChannelMatrix[x, y, 2]);

                    if (histogram.ContainsKey(rgb))
                    {
                        histogram[rgb] += 1;
                    }
                    else
                    {
                        histogram.Add(rgb, 1);
                    }
                }
            List<RGBHisto> histoList = new List<RGBHisto>();
            foreach (int rgb in histogram.Keys)
            {
                RGBHisto item = new RGBHisto();
                item.R = (rgb >> 16) & 0xFF;
                item.G = (rgb >> 8) & 0xFF;
                item.B = rgb & 0xFF;
                histogram.TryGetValue(rgb, out int amount);
                item.Amount = amount;
                histoList.Add(item);
            }
            //binary spatial partitioning of the palette
            List<BinarySpatialPartitioningNode> nodes = new List<BinarySpatialPartitioningNode>();
            BinarySpatialPartitioningNode initNode = new BinarySpatialPartitioningNode(histoList);
            nodes.Add(initNode);

            for (int i = 0; i < 8; i++)
            {
                int colorPlane = i % 3;
                List<BinarySpatialPartitioningNode> newNodes = new List<BinarySpatialPartitioningNode>();
                foreach (BinarySpatialPartitioningNode node in nodes)
                {
                    List<RGBHisto> sorted = new();
                    switch (colorPlane)
                    {
                        case 0:
                            sorted = node.Colors.OrderBy(d => d.R).ToList();
                            break;
                        case 1:
                            sorted = node.Colors.OrderBy(d => d.G).ToList();
                            break;
                        case 2:
                            sorted = node.Colors.OrderBy(d => d.B).ToList();
                            break;
                    }

                    newNodes.Add(new BinarySpatialPartitioningNode(sorted.GetRange(0, node.Colors.Count / 2)));
                    newNodes.Add(new BinarySpatialPartitioningNode(sorted.GetRange(node.Colors.Count / 2, node.Colors.Count - node.Colors.Count / 2)));
                }
                nodes = newNodes;
            }
            List<Color> palette256 = new List<Color>();
            //weighted palette generation
            for (int i = 0; i < 256; i++)
            {
                int r = 0;
                int g = 0;
                int b = 0;
                int amountTotal = 0;
                foreach (RGBHisto item in nodes[i].Colors)
                {
                    r += item.R * item.R * item.Amount;
                    g += item.G * item.G * item.Amount;
                    b += item.B * item.B * item.Amount;
                    amountTotal += item.Amount;
                }
                if (amountTotal == 0)
                {
                    palette256.Add(Color.Black);
                    palette8[i] = Color.Black;
                }
                else
                {
                    r /= amountTotal;
                    g /= amountTotal;
                    b /= amountTotal;
                    Color col = Color.FromArgb((int)Math.Sqrt(r), (int)Math.Sqrt(g), (int)Math.Sqrt(b));
                    palette256.Add(col);
                    palette8[i] = col;
                }
            }
            //palette optimized picture drawing
            bit8map = new int[width, height];
            Bitmap srcBmp = (Bitmap)pictureBoxSource.Image;
            Bitmap tgtBmp = new Bitmap(width * 2, height * 2);
            //int width = srcChannelMatrix.GetLength(0);
            //int height = srcChannelMatrix.GetLength(1);
            for (int y = 0; y < height; y++)
                for (int x = 0; x < width; x++)
                {
                    int targetColorIndex = -1;
                    for (int i = 0; i < 256; i++)
                    {
                        int index = nodes[i].Colors.FindIndex(d => d.R == srcChannelMatrix[x, y, 0] && d.G == srcChannelMatrix[x, y, 1] && d.B == srcChannelMatrix[x, y, 2]);
                        if (index > -1)
                        {
                            targetColorIndex = i;
                            break;
                        }
                    }
                    if (targetColorIndex == -1) targetColorIndex = 0;
                    tgtBmp.SetPixel(x * 2, y * 2, palette256[targetColorIndex]);
                    tgtBmp.SetPixel(x * 2 + 1, y * 2, palette256[targetColorIndex]);
                    tgtBmp.SetPixel(x * 2, y * 2 + 1, palette256[targetColorIndex]);
                    tgtBmp.SetPixel(x * 2 + 1, y * 2 + 1, palette256[targetColorIndex]);
                    //srcChannelMatrix[x, y, 0] = palette256[targetColorIndex].R;
                    //srcChannelMatrix[x, y, 1] = palette256[targetColorIndex].G;
                    //srcChannelMatrix[x, y, 2] = palette256[targetColorIndex].B;
                    bit8map[x, y] = targetColorIndex;
                }
            pictureBoxSrcReduced.Image = tgtBmp;
            pictureBoxSrcReduced.Size = tgtBmp.Size;
        }

        private void checkBoxColorReduction_CheckedChanged(object sender, EventArgs e)
        {
            bool check = checkBoxColorReduction.Checked;
            checkBoxUseDither.Enabled = !check;
            comboBoxDither.Enabled = !check;
            if (check)
            {
                ReduceColors();
            }
        }

        private void ComboBoxAverMethod_SelectedIndexChanged(object sender, EventArgs e)
        {
            PictureToChannel(comboBoxAverMethod.SelectedIndex);
            CreatePal(comboBoxAverMethod.SelectedIndex);
        }

        private void ComboBoxDistance_SelectedIndexChanged(object sender, EventArgs e)
        {
            buttonAlpaCentauriAI.Enabled = false;
        }
    }
}