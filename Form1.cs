using System.ComponentModel;
namespace AlterLinePictureAproximator
{
    using AtariMapMaker;
    using System;
    using System.CodeDom;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Diagnostics.Eventing.Reader;
    using System.Diagnostics.Metrics;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Drawing.Imaging;
    using System.Globalization;
    using System.Linq;
    using System.Net.Security;
    using System.Numerics;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Security;
    using System.Security.Cryptography.X509Certificates;
    using System.Text.Json.Serialization.Metadata;

    /*todo: dithering only in results
     *todo: hepa luma as difference between RGB vals
     *todo: proper initialization (do not delete current color setup)
     */
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        public const int COLORS = 5;
        /// <summary> defines which colors are same in the normal and inverse palette in 5-color mode (1=same, 0=different) </summary>
        public static readonly int[] COMMON_MATRIX5 = { 1 , 1, 0 ,1 ,1,
                                                        1 , 1, 0 ,1 ,1,
                                                        0 , 0 ,0 ,0 ,0,
                                                        1 , 1, 0 ,1 ,1,
                                                        1 , 1, 0 ,1 ,1};
        /// <summary> defines which colors are affected by PMG color</summary>
        public static readonly int[] IGNORE_PMG_MATRIX5 = { 1, 1, 1, 1, 0,
                                                            1, 1, 1, 1, 0,
                                                            1, 1, 1, 1, 0,
                                                            1, 1, 1, 1, 0,
                                                            0, 0, 0, 0, 0};
        /// <summary> defines which colors are affected by background color</summary>
        public static readonly int[] IGNORE_BG_MATRIX5 = { 1, 1, 1, 0, 1,
                                                           1, 1, 1, 0, 1,
                                                           1, 1, 1, 0, 1,
                                                           0, 0, 0, 0, 0,
                                                           1, 1, 1, 0, 1};
        private static int srcHeightChar = 24;
        public static int srcHeight = 8 * 24;
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool Dither { get; set; } = false;
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool UseReducedSource { get; set; } = true;
        public byte[] atariPalRgb = new byte[256 * 3];
        //my colors:    1e,16,26,76,00
        //              14,16,48,b6,00
        public float[] atariPalLab = new float[256 * 3];
        public Color[,] customColors;
        public byte[] line0 = new byte[COLORS + 1] { 0x1e, 0x16, 0x26, 0x76, 0x00, 0x0e };
        public byte[] line1 = new byte[COLORS + 1] { 0x14, 0x1a, 0x48, 0xb6, 0x00, 0x0e };
        /// <summary>colors that are joined between scanlines (background and PMG layer)</summary>
        public static readonly int[] JOINED_COLORS = { 0, 0, 0, 0, 1, 1 };
        /// <summary>colors that are locked for the change by AI (background)</summary>
        public static readonly int[] LOCKED_COLORS = { 0, 0, 0, 0, 0, 0 };

        public Color[,] cline0 = new Color[COLORS, 2];
        public Color[,] cline1 = new Color[COLORS, 2];
        public int[,,,] diff_matrix;
        public AtariColorPicker atariColorPicker = new AtariColorPicker();
        public int[,] mask;
        public int[,] pmgData;
        public int[,,] bitmap;
        public byte[] bitmapPixelsDithered; //dithered bitmap pixels data in B-R-G order
        public byte[,,] srcChannelMatrix;   //matrix containing RGB channels of source image
        public int[,] bit8map;
        public int[,,] colorIgnoreMatrix = new int[COLORS * COLORS, 2, 3];
        public Color[] palette8 = new Color[256];
        public int[,,] customP8Match = new int[256, 2, 2];   //reset every custom palette change
        public long[,,] customP8MatchDist = new long[256, 2, 2];    ////reset every custom palette change
        public List<AlpaItem> alpaItems = new List<AlpaItem>();
        public Random rand = new Random();
        public int totalPixels;
        public byte[,,,,] customP24Match = new byte[256, 256, 256, 2, 2];
        public long[,,,,] customP24MatchDist = new long[256, 256, 256, 2, 2];


        public class AlpaItem
        {
            public long Diff { get; set; }
            public int Ppdiff { get; set; }
            public byte[] Line0 { get; set; }
            public byte[] Line1 { get; set; }
            public int Zero { get; set; }

            // Shallow copy
            public AlpaItem ShallowCopy()
            {
                return (AlpaItem)this.MemberwiseClone();
            }
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
            comboBoxLightness.SelectedIndex = 0;
            comboBoxLightness.Tag = 0;  //previous value
            PictureToChannel(comboBoxAverMethod.SelectedIndex);
            ReduceColors();
            pictureBoxResultLines.ZoomFactor = PictureBoxWithInterpolationMode.PictureBoxZoomFactor.x1;
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

        //<summary> Converts atari indexed color to 24bit Color object
        private Color AtariColorToColor(int colorIndex)
        {
            return Color.FromArgb((int)atariPalRgb[colorIndex * 3], (int)atariPalRgb[colorIndex * 3 + 1], (int)atariPalRgb[colorIndex * 3 + 2]);
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

        private double RgbToLin(double colorChannel)
        {
            // Send this function a decimal sRGB gamma encoded color value
            // between 0.0 and 1.0, and it returns a linearized value.

            if (colorChannel <= 0.04045)
            {
                return colorChannel / 12.92;
            }
            else
            {
                return Math.Pow(((colorChannel + 0.055) / 1.055), 2.4);
            }
        }
        private double GetLStar(int atariColor)
        {
            double vR = atariPalRgb[atariColor * 3] / 255;
            double vG = atariPalRgb[atariColor * 3 + 1] / 255;
            double vB = atariPalRgb[atariColor * 3 + 2] / 255;
            double y = (0.2126 * RgbToLin(vR) + 0.7152 * RgbToLin(vG) + 0.0722 * RgbToLin(vB));

            if (y <= (216 / 24389))
            {       // The CIE standard states 0.008856 but 216/24389 is the intent for 0.008856451679036
                return y * (24389 / 27);  // The CIE standard states 903.3, but 24389/27 is the intent, making 903.296296296296296
            }
            else
            {
                return Math.Pow(y, (1 / 3)) * 116 - 16;
            }
        }

        private void CreatePal(int method, bool noDraw = false)
        {
            //int[,] hepaMatrix = new int[COLORS * COLORS, 2];   //color-ignore matrix based on the HEPA filtering
            int hepaLumaFilter = checkBoxHepa.Checked ? (int)numericUpDownHepaLuma.Value : 14;
            int hepaChromaFilter = checkBoxHepa.Checked ? (int)numericUpDownHepaChroma.Value : 15;
            int brightnessMethod = comboBoxLightness.SelectedIndex;
            customColors = new Color[COLORS * COLORS, 2];
            for (int z = 0; z < 2; z++)
            {
                List<byte> l0 = line0.ToList();
                List<byte> l1 = line1.ToList();
                l0.RemoveAt(3 - z);
                l1.RemoveAt(3 - z);
                int index = 0;
                for (int a = 0; a < COLORS; a++)
                {
                    for (int b = 0; b < COLORS; b++)
                    {
                        customColors[index, z] = AverageColor(l0[a], l1[b], method);
                        int loopingColorDifference = Math.Min(Math.Abs((l0[a] / 16) - (l1[b] / 16)), 16 - (Math.Abs((l0[a] / 16) - (l1[b] / 16))));
                        int hepaItem;

                        //https://stackoverflow.com/questions/596216/formula-to-determine-perceived-brightness-of-rgb-color
                        switch (brightnessMethod)
                        {
                            case 0:
                                hepaItem = (Math.Abs((l0[a] % 16) - (l1[b] % 16)) <= hepaLumaFilter) && (loopingColorDifference <= hepaChromaFilter) ? 1 : 0;
                                break;
                            case 1: //(0.2126*R + 0.7152*G + 0.0722*B)
                                double lum0 = atariPalRgb[l0[a] * 3] * 0.2126 +
                                              atariPalRgb[l0[a] * 3 + 1] * 0.7152 +
                                              atariPalRgb[l0[a] * 3 + 2] * 0.0722;
                                double lum1 = atariPalRgb[l1[b] * 3] * 0.2126 +
                                              atariPalRgb[l1[b] * 3 + 1] * 0.7152 +
                                              atariPalRgb[l1[b] * 3 + 2] * 0.0722;
                                hepaItem = (Math.Abs((int)(lum0 - lum1)) <= hepaLumaFilter) && (loopingColorDifference <= hepaChromaFilter) ? 1 : 0;
                                break;
                            case 2:
                                hepaItem = (Math.Abs((int)(GetLStar(l0[a]) - GetLStar(l1[b]))) <= hepaLumaFilter) && (loopingColorDifference <= hepaChromaFilter) ? 1 : 0;
                                break;
                            default:
                                hepaItem = 0;
                                break;
                        }
                        colorIgnoreMatrix[index, z, 0] = hepaItem & IGNORE_PMG_MATRIX5[index];
                        colorIgnoreMatrix[index, z, 1] = hepaItem & IGNORE_BG_MATRIX5[index];
                        colorIgnoreMatrix[index, z, 2] = hepaItem;
                        index++;
                    }
                    cline0[a, z] = AtariColorToColor(l0[a]);
                    cline1[a, z] = AtariColorToColor(l1[a]);
                }
            }
            //build solution key
            //solutionKey = "";
            //for (int i = 0; i < line0.Length; i++)
            //    solutionKey += $"{line0[i].ToString()}_{line1[i].ToString()}_";

            //count the colors
            List<Color> colors = new();
            for (int z = 0; z < 2; z++)
                for (int t = 0; t < 2; t++)
                    for (int i = 0; i < customColors.GetLength(0); i++)
                    {
                        if (colorIgnoreMatrix[i, z, t] != 0 && !colors.Contains(customColors[i, z]))
                            colors.Add(customColors[i, z]);
                    }
            labelPossibleColors.Text = $"Colors: {colors.Count} ";
            colors.Clear();
            //drawing part
            if (!noDraw)
            {
                Bitmap p = new Bitmap(COLORS * 16 * 2, (COLORS + 2 + 2) * 16);
                Graphics g = Graphics.FromImage(p);
                const int OFFSET = COLORS * 16 + 1;
                for (int z = 0; z < 2; z++)
                    for (int a = 0; a < COLORS; a++)
                    {
                        for (int b = 0; b < COLORS; b++)
                        {
                            g.FillRectangle(new SolidBrush(customColors[a * COLORS + b, z]), new Rectangle(b * 16 + OFFSET * z, a * 16, 16, 16));
                        }
                        g.FillRectangle(new SolidBrush(cline0[a, z]), new Rectangle(a * 16 + OFFSET * z, OFFSET, 16, 16));
                        g.FillRectangle(new SolidBrush(cline1[a, z]), new Rectangle(a * 16 + OFFSET * z, OFFSET + 16, 16, 16));
                    }
                pictureBoxPalette.Image = p;
                //draw the lock and join icons
                int[] iconColorMatch = new int[10] { 0, 1, 2, 4, 5, -1, -1, 3, -1, -1 };
                for (int i = 0; i < COLORS * 2; i++)  //how many squares we have there
                {
                    if (iconColorMatch[i] >= 0)
                    {
                        int lockIndex = LOCKED_COLORS[iconColorMatch[i]] == 0 ? 14 : 0;
                        g.DrawImage(pictureBoxIcons.Image, new Rectangle(i * 16, OFFSET + 32, 14, 14), new Rectangle(lockIndex, 0, 14, 14), GraphicsUnit.Pixel);
                        int jointIndex = JOINED_COLORS[iconColorMatch[i]] == 0 ? 14 : 0;
                        if (iconColorMatch[i] > 3) jointIndex = 28; //forced join for BG and PMG
                        g.DrawImage(pictureBoxIcons.Image, new Rectangle(i * 16, OFFSET + 32 + 16, 14, 14), new Rectangle(jointIndex, 14, 14, 14), GraphicsUnit.Pixel);
                    }
                }
            }
            Array.Clear(customP24Match);
        }
        private void LoadPalette()
        {
            atariPalRgb = File.ReadAllBytes("altirraPAL.pal");
            AtariPalette.Load(atariPalRgb);
        }

        private void ButtonApproximate_Click(object sender, EventArgs e)
        {
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
            Bitmap t = new Bitmap(pictureBoxSource.Image.Width, pictureBoxSource.Image.Height / 2);
            srcChannelMatrix = new byte[t.Width, t.Height, 3];
            for (int y = 0; y < t.Height; y++)
            {
                for (int x = 0; x < t.Width; x++)
                {
                    Color src = AverageColor(b.GetPixel(x, y * 2), b.GetPixel(x, y * 2 + 1), method);
                    srcChannelMatrix[x, y, 0] = src.R;
                    srcChannelMatrix[x, y, 1] = src.G;
                    srcChannelMatrix[x, y, 2] = src.B;

                    t.SetPixel(x, y, src);
                }
            }
            pictureBoxSrcData.Image = new Bitmap(t);
            totalPixels = srcChannelMatrix.Length / 3;
        }

        //initialize customP8Match with -1 value in each cell
        private void InitializeArrayToMinusOne(Array array)
        {

            var data = MemoryMarshal.CreateSpan(
                ref Unsafe.As<byte, int>(ref MemoryMarshal.GetArrayDataReference(array)),
                array.Length);
            data.Fill(-1);
        }

        //new calcdiff for 8bit palette-reduced picture
        private (long totalDiff, int[,] bitmapDataIndexed, int[,] charMask, int[,] pmgMask) CalcDiff2(int distanceMethod, bool doDither)
        {
            int width = srcChannelMatrix.GetLength(0);
            int height = srcChannelMatrix.GetLength(1);

            int charHeight = height / 4;
            int[,] charMask = new int[32, charHeight];
            int[,] pmgMask = new int[32, height];
            int[,] bitmapDataIndexed = new int[width, height];
            long[] charDiff = new long[2];
            long totalDiff = 0;
            long[,,] pmgDiff = new long[4, 2, 2];

            InitializeArrayToMinusOne(customP8Match);
            Array.Clear(customP8MatchDist);
            int[,,,] bitmapCharDataIndexed = new int[4, 4, 2, 2];
            int[,] pmgCharData = new int[4, 2];

            for (int y = 0; y < charHeight; y++)
                for (int x = 0; x < 32; x++)
                {
                    for (int z = 0; z < 2; z++) //inverse layer
                    {
                        charDiff[z] = 0;
                        for (int cy = 0; cy < 4; cy++)
                        {
                            for (int t = 0; t < 2; t++) //pmg layer
                            {
                                pmgDiff[cy, t, z] = 0;
                                for (int cx = 0; cx < 4; cx++)
                                {
                                    long distance;
                                    int index;
                                    if (doDither)
                                    {
                                        int pixelIndex = (x * 4 + cx + (y * 4 + cy) * 128) * 3;
                                        Color desiredColor = Color.FromArgb(bitmapPixelsDithered[pixelIndex + 2], bitmapPixelsDithered[pixelIndex + 1], bitmapPixelsDithered[pixelIndex]);
                                        (distance, index) = FindCustomClosest2(desiredColor, distanceMethod, z, t);
                                        //distance = Distance(palette8[bit8map[x * 4 + cx, y * 4 + cy]], customColors[index, z], distanceMethod); //proper distance
                                    }
                                    else
                                        if (UseReducedSource)
                                        (distance, index) = FindCustomClosest2Reduced(bit8map[x * 4 + cx, y * 4 + cy], distanceMethod, z, t);
                                    else
                                        (distance, index) = FindCustomClosest2(palette8[bit8map[x * 4 + cx, y * 4 + cy]], distanceMethod, z, t);

                                    pmgDiff[cy, t, z] += (long)Math.Pow(distance, 2);
                                    bitmapCharDataIndexed[cx, cy, z, t] = index;
                                }
                            }
                            pmgCharData[cy, z] = (pmgDiff[cy, 0, z] > pmgDiff[cy, 1, z]) ? 1 : 0;
                            charDiff[z] += pmgDiff[cy, pmgCharData[cy, z], z];
                        }
                    }
                    charMask[x, y] = (charDiff[0] > charDiff[1]) ? 1 : 0; //1=black
                    totalDiff += charDiff[charMask[x, y]];
                    for (int cy = 0; cy < 4; cy++)  //fill pmg data based on selected char (normal or inverse)
                    {
                        int pmgPixel = pmgCharData[cy, charMask[x, y]];
                        pmgMask[x, y * 4 + cy] = pmgPixel;
                        for (int cx = 0; cx < 4; cx++)
                            bitmapDataIndexed[x * 4 + cx, y * 4 + cy] = bitmapCharDataIndexed[cx, cy, charMask[x, y], pmgPixel];
                    }
                }
            return (totalDiff, bitmapDataIndexed, charMask: charMask, pmgMask: pmgMask);
        }

        private void CreateIdealDitheredSolution(int DistanceMethod, int ditherMethod, float ditherFactor)
        {
            float[,] ditherMatrices = new float[3, 4] {
                {.5f, 0, .5f, 0 }, //chess
                { .5f,.25f,.25f,0 }, //sierra lite
                {7f/16,3f/16,5f/16,1f/16 } //f-s
            };

            float[] ditherValues = new float[4];
            for (int i = 0; i < 4; i++)
                ditherValues[i] = ditherMatrices[ditherMethod, i] * ditherFactor;

            int width = srcChannelMatrix.GetLength(0);
            int height = srcChannelMatrix.GetLength(1);

            int charHeight = height / 4;
            int[,] charMask = new int[32, charHeight];
            int[,] pmgMask = new int[32, height];
            int[,] bitmapDataIndexed = new int[width, height];
            long[] charDiff = new long[2];
            long totalDiff = 0;
            long[,,] pmgDiff = new long[4, 2, 2];

            InitializeArrayToMinusOne(customP8Match);
            Array.Clear(customP8MatchDist);
            //int[,,,] bitmapCharDataIndexed = new int[4, 4, 2, 2];
            //int[,] pmgCharData = new int[4, 2];

            Bitmap d = new Bitmap(width, height, PixelFormat.Format24bppRgb);
            int distanceMethod = comboBoxDistance.SelectedIndex;
            BitmapData dd = d.LockBits(new Rectangle(0, 0, d.Width, d.Height), ImageLockMode.WriteOnly, d.PixelFormat);
            IntPtr pointer = dd.Scan0;
            int size = Math.Abs(dd.Stride) * dd.Height;
            bitmapPixelsDithered = new byte[size];
            Marshal.Copy(pointer, bitmapPixelsDithered, 0, size);

            int pixelIndex = 0;
            int[,,] errorStorage = new int[width, 2, 3];
            //int pixelIndex2 = dd.Stride;
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    int[] error = new int[3];
                    for (int i = 0; i < 3; i++)
                    {
                        error[i] = errorStorage[x, y % 2, i];
                        errorStorage[x, y % 2, i] = 0;
                    }

                    var (distance, color, errorNew) = FindCustomClosest3(palette8[bit8map[x, y]], error, distanceMethod);
                    bitmapPixelsDithered[pixelIndex] = color.B;
                    bitmapPixelsDithered[pixelIndex + 1] = color.G;
                    bitmapPixelsDithered[pixelIndex + 2] = color.R;

                    //errorNew has to be distributed to neighboring pixels
                    // -  X  0
                    // 1  2  3

                    if (ditherMethod == 0)  //chess
                    {
                        if ((x + 1 < width) && (y + 1 < height))
                            for (int i = 0; i < 3; i++)
                                if ((x + y) % 2 == 0)
                                {
                                    errorStorage[x + 1, y % 2, i] += (int)(errorNew[i] * ditherValues[0]);
                                    errorStorage[x, 1 - (y % 2), i] += (int)(errorNew[i] * ditherValues[2]);
                                }
                    }
                    else
                    {
                        if ((x > 0) && (x + 1 < width) && (y + 1 < height))
                        {
                            for (int i = 0; i < 3; i++)
                            {
                                errorStorage[x + 1, y % 2, i] += (int)(errorNew[i] * ditherValues[0]);
                                errorStorage[x - 1, 1 - (y % 2), i] += (int)(errorNew[i] * ditherValues[1]);
                                errorStorage[x, 1 - (y % 2), i] += (int)(errorNew[i] * ditherValues[2]);
                                errorStorage[x + 1, 1 - (y % 2), i] += (int)(errorNew[i] * ditherValues[3]);
                            }
                        }
                    }

                    pixelIndex += 3;
                    totalDiff += distance;
                }
            }
            if (checkBoxAutoUpdate.Checked)
            {
                Marshal.Copy(bitmapPixelsDithered, 0, pointer, size);
                d.UnlockBits(dd);
                pictureBoxIdealDither.Image = new Bitmap(d);
            }
            else
                d.UnlockBits(dd);
            return;
        }
        private void Redraw(int[,] bitmapDataIndexed, int[,] charMask, int[,] pmgMask)
        {
            int width = charMask.GetLength(0) * 4;
            int height = charMask.GetLength(1) * 4;
            Bitmap t = new Bitmap(width, height, PixelFormat.Format24bppRgb);
            Bitmap u = new Bitmap(width * 2, height * 2, PixelFormat.Format24bppRgb); //alterlines (double the size)
            Bitmap m = new Bitmap(width, height, PixelFormat.Format24bppRgb);
            byte[] remap = { 1, 2, 3, 0, 0 };

            BitmapData dt = t.LockBits(new Rectangle(0, 0, t.Width, t.Height), ImageLockMode.WriteOnly, t.PixelFormat);
            IntPtr dtPointer = dt.Scan0;
            int size = Math.Abs(dt.Stride) * dt.Height;
            byte[] dtPixels = new byte[size];
            Marshal.Copy(dtPointer, dtPixels, 0, size);

            BitmapData dm = m.LockBits(new Rectangle(0, 0, m.Width, m.Height), ImageLockMode.WriteOnly, m.PixelFormat);
            IntPtr dmPointer = dm.Scan0;
            int sizeM = Math.Abs(dm.Stride) * dm.Height;
            byte[] dmPixels = new byte[sizeM];
            Marshal.Copy(dmPointer, dmPixels, 0, sizeM);

            BitmapData du = u.LockBits(new Rectangle(0, 0, u.Width, u.Height), ImageLockMode.WriteOnly, u.PixelFormat);
            IntPtr duPointer = du.Scan0;
            int sizeU = Math.Abs(du.Stride) * du.Height;
            byte[] duPixels = new byte[sizeU];
            Marshal.Copy(duPointer, duPixels, 0, sizeU);

            int pixelIndex = 0;
            int pixelIndexU = 0;
            int pixelIndexU2 = du.Stride;
            int pixelIndexM = 0;
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    var color = customColors[bitmapDataIndexed[x, y], charMask[x / 4, y / 4]];
                    dtPixels[pixelIndex] = color.B;
                    dtPixels[pixelIndex + 1] = color.G;
                    dtPixels[pixelIndex + 2] = color.R;

                    color = cline0[bitmapDataIndexed[x, y] / COLORS, charMask[x / 4, y / 4]];

                    duPixels[pixelIndexU] = color.B;
                    duPixels[pixelIndexU + 1] = color.G;
                    duPixels[pixelIndexU + 2] = color.R;
                    duPixels[pixelIndexU + 3] = color.B;
                    duPixels[pixelIndexU + 4] = color.G;
                    duPixels[pixelIndexU + 5] = color.R;

                    color = cline1[bitmapDataIndexed[x, y] % COLORS, charMask[x / 4, y / 4]];

                    duPixels[pixelIndexU2] = color.B;
                    duPixels[pixelIndexU2 + 1] = color.G;
                    duPixels[pixelIndexU2 + 2] = color.R;
                    duPixels[pixelIndexU2 + 3] = color.B;
                    duPixels[pixelIndexU2 + 4] = color.G;
                    duPixels[pixelIndexU2 + 5] = color.R;

                    if (x % 4 == 0)
                    {
                        color = charMask[x / 4, y / 4] == 0 ?
                           ((pmgMask[x / 4, y] == 1) ? Color.OrangeRed : Color.White)
                         : ((pmgMask[x / 4, y] == 0) ? Color.Gray : Color.DarkRed);
                        for (int i = 0; i < 4; i++)
                        {
                            dmPixels[pixelIndexM++] = color.B;
                            dmPixels[pixelIndexM++] = color.G;
                            dmPixels[pixelIndexM++] = color.R;
                        }
                    }

                    pixelIndex += 3;
                    pixelIndexU += 6;
                    pixelIndexU2 += 6;
                }
                pixelIndexU += du.Stride;
                pixelIndexU2 += du.Stride;
            }
            Marshal.Copy(dtPixels, 0, dtPointer, size);
            t.UnlockBits(dt);
            Marshal.Copy(duPixels, 0, duPointer, sizeU);
            u.UnlockBits(du);
            Marshal.Copy(dmPixels, 0, dmPointer, sizeM);
            m.UnlockBits(dm);
            pictureBoxResult.Image = new Bitmap(t);
            pictureBoxResultLines.Image = new Bitmap(u);
            pictureBoxMasks.Image = m;
        }


        private long Distance(Color col1, Color col2, int distanceMethod)
        {
            long dist = long.MaxValue;
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

        /// <summary>
        /// Finds closest color towards picture reduced to 256 colors
        /// </summary>
        /// <param name="p8index"></param>
        /// <param name="distanceMethod"></param>
        /// <param name="inverse"></param>
        /// <param name="pmg"></param>
        /// <returns></returns>
        private (long distance, int index) FindCustomClosest2Reduced(int p8index, int distanceMethod, int inverse, int pmg)
        {
            long mindist = long.MaxValue;
            int index = 0;

            int indexStored = customP8Match[p8index, inverse, pmg];
            if (indexStored == -1)
            {
                for (byte i = 0; i < COLORS * COLORS; i++)
                {
                    if (colorIgnoreMatrix[i, inverse, pmg] != 0)
                    {
                        long dist = Distance(palette8[p8index], customColors[i, inverse], distanceMethod);
                        if (dist < mindist)
                        {
                            mindist = dist;
                            index = i;
                        }
                    }
                }
                customP8Match[p8index, inverse, pmg] = (byte)(index);
                customP8MatchDist[p8index, inverse, pmg] = mindist;
                return (distance: mindist, index: index);
            }
            else
                return (distance: customP8MatchDist[p8index, inverse, pmg], index: indexStored);
        }

        /// <summary>
        /// Finds closest color, incl.pmg+bg for ideal dithered image (from which we do atari dithered image with fat pmg pixels)
        /// </summary>
        /// <param name="desiredColor"></param>
        /// <param name="error"></param>
        /// <param name="distanceMethod"></param>
        /// <returns></returns>
        private (long distance, Color color, int[] error) FindCustomClosest3(Color desiredColor, int[] error, int distanceMethod)
        {
            long mindist = long.MaxValue;
            Color color = Color.Magenta;
            int atariColorIndex;

            int[] rgbWithError = { desiredColor.R + error[0], desiredColor.G + error[1], desiredColor.B + error[2] };
            Color colorWithError = Color.FromArgb(Clamp256(rgbWithError[0]), Clamp256(rgbWithError[1]), Clamp256(rgbWithError[2]));

            for (byte i = 0; i < COLORS * COLORS; i++)
            {
                for (int inverse = 0; inverse < 2; inverse++)
                    if (colorIgnoreMatrix[i, inverse, 2] != 0) //[x,x,2] means pmg colors allowed for each pixel
                    {
                        long dist = Distance(colorWithError, customColors[i, inverse], distanceMethod);
                        if (dist < mindist)
                        {
                            mindist = dist;
                            color = customColors[i, inverse];
                        }
                    }
            }

            error[0] = rgbWithError[0] - color.R;
            error[1] = rgbWithError[1] - color.G;
            error[2] = rgbWithError[2] - color.B;

            return (distance: mindist, color: color, error: error);
        }

        /// <summary>
        /// Finds closest color towards original source picture (24bit)
        /// </summary>
        /// <param name="p8index"></param>
        /// <param name="distanceMethod"></param>
        /// <param name="inverse"></param>
        /// <param name="pmg"></param>
        /// <returns></returns>
        private (long distance, int index) FindCustomClosest2(Color color, int distanceMethod, int inverse, int pmg)
        {
            long mindist = long.MaxValue;
            int index = customP24Match[color.R, color.G, color.B, inverse, pmg] - 1;
            if (index < 0)
            {
                for (byte i = 0; i < COLORS * COLORS; i++)
                {
                    long dist;
                    if (colorIgnoreMatrix[i, inverse, pmg] != 0)
                    {
                        dist = Distance(color, customColors[i, inverse], distanceMethod);
                        if (dist < mindist)
                        {
                            mindist = dist;
                            index = i;
                        }
                    }
                }
                customP24Match[color.R, color.G, color.B, inverse, pmg] = (byte)(index + 1);
                customP24MatchDist[color.R, color.G, color.B, inverse, pmg] = mindist;
            }
            else
            {
                mindist = customP24MatchDist[color.R, color.G, color.B, inverse, pmg];
            }
            return (distance: mindist, index: index);
        }

        private static long Difference(Color col1, Color col2)
        {
            int dr = col2.R - col1.R;
            int dg = col2.G - col1.G;
            int db = col2.B - col1.B;
            return Math.Abs(dr) + Math.Abs(dg) + Math.Abs(db);
        }

        private long RGByuvDistance(Color col1, Color col2)
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
            return (long)d;
        }

        private long RGBEuclidianDistance(Color col1, Color col2)
        {
            //uint DISTANCE_MAX = 0xffffffff;

            int dr = col2.R - col1.R;
            int dg = col2.G - col1.G;
            int db = col2.B - col1.B;

            long d = dr * dr + dg * dg + db * db;

            //if (d > (float)DISTANCE_MAX)

            //    d = (float)DISTANCE_MAX;
            return d;
        }

        private long WeightedRGBDistance(Color e1, Color e2)
        {
            long rmean = ((long)e1.R + (long)e2.R) / 2;
            long r = (long)e1.R - (long)e2.R;
            long g = (long)e1.G - (long)e2.G;
            long b = (long)e1.B - (long)e2.B;
            //return (float)Math.Sqrt((((512 + rmean) * r * r) >> 8) + 4 * g * g + (((767 - rmean) * b * b) >> 8));
            return (((512 + rmean) * r * r) >> 8) + 4 * g * g + (((767 - rmean) * b * b) >> 8);
        }

        private long YUVEuclidianDistance(Color c1, Color c2)
        {
            byte[] z1 = RGB2YUV(c1.R, c1.G, c1.B);
            byte[] z2 = RGB2YUV(c2.R, c2.G, c2.B);
            int dy = z1[0] - z2[0];
            int du = z1[1] - z2[1];
            int dv = z1[2] - z2[2];
            long d = dy * dy + du * du + dv * dv;
            return d;
        }

        private void ButtonMixIt_Click(object sender, EventArgs e)
        {
        }


        private void CheckBoxUseDither_CheckedChanged(object sender, EventArgs e)
        {
            Dither = checkBoxUseDither.Checked;
        }

        private void ButtonOpen_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                Bitmap bmp;
                try
                {
                    bmp = new Bitmap(Bitmap.FromFile(openFileDialog1.FileName));
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Picture {openFileDialog1.FileName} cannot be open: {ex.Message}");
                    return;
                }
                if (bmp.Width == 128 && bmp.Height <= 192)
                    pictureBoxSource.Image = bmp;
                else
                {
                    float ratio = bmp.Width / 256f;
                    srcHeight = (int)(bmp.Height / ratio);
                    Bitmap scaled = new Bitmap(256, srcHeight);
                    Graphics g = Graphics.FromImage(scaled);
                    g.DrawImage(bmp, new Rectangle(0, 0, 256, srcHeight));
                    if (srcHeight > 192)
                        srcHeight = 192;
                    else
                        srcHeight += srcHeight % 8;
                    srcHeightChar = srcHeight / 8;

                    Bitmap input = new Bitmap(128, srcHeight);
                    g = Graphics.FromImage(input);
                    g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    g.DrawImage(scaled, new Rectangle(0, 0, 128, srcHeight), new Rectangle(0, 0, 256, srcHeight), GraphicsUnit.Pixel);
                    pictureBoxSource.Image = input;
                    pictureBoxSource.Size = pictureBoxSource.Image.Size;
                    g.Dispose();
                    bmp.Dispose();
                    scaled.Dispose();
                    labelOutputSize.Text = $"Output: 256 * {srcHeight} ({srcHeightChar})";
                }
                PictureToChannel(comboBoxAverMethod.SelectedIndex);
                CreatePal(comboBoxAverMethod.SelectedIndex);
                ReduceColors();
            }
        }

        private void CentauriInit(int population)
        {
            alpaItems.Clear();
            AlpaItem zeroItem = new();
            Random rand = new Random();

            for (int i = 0; i < population; i++)
            {
                if (i > 0) //keep 0.iteration equal to colors currently set
                {
                    for (int j = 0; j <= COLORS; j++)
                    {
                        if (LOCKED_COLORS[j] == 1) { continue; }
                        line0[j] = (byte)(((byte)rand.Next(255)) & 0xFE);
                        if (JOINED_COLORS[j] == 1)
                            line1[j] = line0[j]; //common color
                        else
                            line1[j] = (byte)(((byte)rand.Next(255)) & 0xFE);
                    }
                }
                CreatePal(comboBoxAverMethod.SelectedIndex, true);

                long totalDiff;
                int[,] bitmapDataIndexed;
                int[,] charMask;
                int[,] pmgMask;
                //if (i == 0)
                // if (Dither)
                //   CreateIdealDitheredSolution(comboBoxDistance.SelectedIndex, comboBoxDither.SelectedIndex, (float)numericUpDownDitherStrength.Value / 10f);

                (totalDiff, bitmapDataIndexed, charMask, pmgMask) = CalcDiff2(comboBoxDistance.SelectedIndex, false);

                //if (i == 0)
                //{
                // if (Dither)
                //    (totalDiff, bitmapDataIndexed, charMask, pmgMask) = CalcDiff2(comboBoxDistance.SelectedIndex, Dither);
                //Redraw(bitmapDataIndexed, charMask, pmgMask);
                //}
                AlpaItem ai = new();
                ai.Diff = totalDiff;
                ai.Ppdiff = (int)(totalDiff / totalPixels);
                ai.Line0 = (byte[])line0.Clone();
                ai.Line1 = (byte[])line1.Clone();
                if (i == 0)
                {
                    ai.Zero = 1;    //index zero (manual/imported)
                    zeroItem = ai.ShallowCopy();
                }
                else
                {
                    alpaItems.Add(ai);
                }
            }
            List<AlpaItem> sorted = alpaItems.OrderBy(d => d.Diff).ToList();
            sorted.Insert(0, zeroItem);
            if (sorted.Count > population / 4)
                sorted.RemoveRange(population / 4 - 1, sorted.Count - ((population / 4) - 1));
            alpaItems = sorted;
            ListPopulation(); // fill up the listview
            listViewPopulation.Items[0].Selected = true;
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
                for (int j = 0; j <= COLORS; j++)
                {
                    if (LOCKED_COLORS[j] == 1) { continue; }
                    line0[j] = (byte)(((byte)rand.Next(255)) & 0xFE);
                    if (JOINED_COLORS[j] == 1)
                        line1[j] = line0[j]; //common color
                    else
                        line1[j] = (byte)(((byte)rand.Next(255)) & 0xFE);
                }

                AlpaItem ai = new AlpaItem();
                ai.Diff = long.MaxValue;
                ai.Ppdiff = int.MaxValue;
                ai.Line0 = (byte[])line0.Clone();
                ai.Line1 = (byte[])line1.Clone();
                alpaItems.Add(ai);
            }

            long bestDiff = long.MaxValue;
            //solutions.Clear(); //clear solutions
            //solutionsKeyUsed = 0;
            for (int i = 0; i < generations; i++)
            {
                CentauriGeneration(population);
                //labelSolutions.Text = $"Solutions:{solutions.Count}, tries:{((i+1)*population)}, keysUsed:{solutionsKeyUsed}";
                Array.Copy(alpaItems[0].Line0, line0, line0.Length);
                Array.Copy(alpaItems[0].Line1, line1, line1.Length);
                //line0 = alpaItems[0].line0;
                //line1 = alpaItems[0].line1;
                labelDiff.Text = $"Diff: {(int)alpaItems[0].Ppdiff}";
                if (checkBoxAutoUpdate.Checked && (bestDiff > alpaItems[0].Ppdiff)) //show progress
                {
                    bestDiff = alpaItems[0].Ppdiff;
                    CreatePal(comboBoxAverMethod.SelectedIndex, false);
                    if (Dither)
                        CreateIdealDitheredSolution(comboBoxDistance.SelectedIndex, comboBoxDither.SelectedIndex, (float)numericUpDownDitherStrength.Value / 10f);
                    (long totalDiff, int[,] bitmapDataIndexed, int[,] charMask, int[,] pmgMask) = CalcDiff2(comboBoxDistance.SelectedIndex, Dither);
                    Redraw(bitmapDataIndexed, charMask, pmgMask);
                }
                else
                {
                    CreatePal(comboBoxAverMethod.SelectedIndex, true);
                    //if (checkBoxUseDither.Checked)
                    //    CreateIdealDitheredSolution(comboBoxDistance.SelectedIndex, comboBoxDither.SelectedIndex, (float)numericUpDownDitherStrength.Value / 10f);
                }
                progressBarAI.Value = i + 1;
                labelGenerationDone.Text = $"{i + 1}";
                this.Refresh();
            }
            ListPopulation(); // fill up the listview

            CreatePal(comboBoxAverMethod.SelectedIndex, false);
            if (Dither)
                CreateIdealDitheredSolution(comboBoxDistance.SelectedIndex, comboBoxDither.SelectedIndex, (float)numericUpDownDitherStrength.Value / 10f);
            (long totalDiff2, int[,] bitmapDataIndexed2, int[,] charMask2, int[,] pmgMask2) = CalcDiff2(comboBoxDistance.SelectedIndex, Dither);
            Redraw(bitmapDataIndexed2, charMask2, pmgMask2);
        }

        private void ListPopulation()
        {
            listViewPopulation.Items.Clear();
            for (int i = 0; i < alpaItems.Count; i++)
            {
                var lvi = listViewPopulation.Items.Add($"{i}: {((int)alpaItems[i].Ppdiff).ToString()}");
                if (alpaItems[i].Zero == 1)
                    lvi.Font = new Font(lvi.Font, FontStyle.Bold);
            }
        }

        private void CentauriGeneration(int population)
        {
            int averageMethod = comboBoxAverMethod.SelectedIndex;
            int distanceMethod = comboBoxDistance.SelectedIndex;
            bool useDither = checkBoxUseDither.Checked;
            int ditherMethod = comboBoxDither.SelectedIndex;

            for (int j = 0; j < 4; j++)
                for (int i = 0; i < population / 4; i++)
                {
                    AlpaItem ai = new AlpaItem();
                    Mutate(alpaItems[i].Line0, alpaItems[i].Line1, ai);
                    for (int k = 0; k < j; k++)
                        Mutate(ai.Line0, ai.Line1, ai);
                    Array.Copy(ai.Line0, line0, line0.Length);
                    Array.Copy(ai.Line1, line1, line1.Length);
                    CreatePal(averageMethod, true);
                    long totalDiff = CalcDiff2(distanceMethod, false).totalDiff;

                    ai.Diff = totalDiff;
                    ai.Ppdiff = (int)(totalDiff / totalPixels);
                    alpaItems.Add(ai);
                }
            List<AlpaItem> sorted = alpaItems.OrderBy(d => d.Diff).ToList();
            int amount = alpaItems.Count;
            if (amount > population / 4)
                sorted.RemoveRange(population / 4 - 1, amount - ((population / 4) - 1));
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
            int colorIndex = rand.Next(COLORS + 1); //choose colorindex to mutate
            while (LOCKED_COLORS[colorIndex] == 1)
                colorIndex = rand.Next(COLORS + 1);

            switch (mutationType)
            {
                case 0:
                case 1:
                case 2:
                case 3:
                    line[colorIndex] += shifts[mutationType];
                    break;
                case 4: //random
                    line[colorIndex] = (byte)(((byte)rand.Next(256)) & 0xfe);
                    break;
                case 5: //swap
                    int whichOne = rand.Next((COLORS + 1) * 2); //0-11
                    while (whichOne == colorIndex || //cannot swap with itself
                           LOCKED_COLORS[whichOne % (COLORS + 1)] == 1 || //cannot swap locked color
                           ((whichOne - (COLORS + 1) == colorIndex) && (JOINED_COLORS[colorIndex] == 1))  //cannot switch same joined colors 
                        )
                        whichOne = rand.Next((COLORS + 1) * 2); ;
                    if (whichOne < COLORS + 1)
                        (line[colorIndex], line[whichOne]) = (line[whichOne], line[colorIndex]);
                    else
                        (line[colorIndex], lineSec[whichOne - (COLORS + 1)]) = (lineSec[whichOne - (COLORS + 1)], line[colorIndex]);
                    break;
            }
            //apply joined (common) colors
            for (int a = 0; a <= COLORS; a++)
                if (JOINED_COLORS[a] == 1)
                    lineSec[a] = line[a];   //common color

            ai.Line0 = line;
            ai.Line1 = lineSec;
        }

        private void PictureBoxPalette_MouseDown(object sender, MouseEventArgs e)
        {
            int[] remap = { 0, 1, 2, 4, 5, 0, 1, 3, 4, 5 };
            int[] joint = { 0, 0, 0, 1, 1, 0, 0, 0, 1, 1 };   //defines which colors are joined(constant) between dlilines
            if (e.Y > COLORS * 16)
            {
                int line = (e.Y - COLORS * 16 - 1) / 16;
                int index = (e.X / 16) % remap.Length;


                switch (line)
                {
                    case 0:
                        atariColorPicker.Pick(line == 0 ? line0[remap[index]] : line1[remap[index]]);
                        if (atariColorPicker.PickedNewColor)
                        {
                            line0[remap[index]] = atariColorPicker.PickedColorIndex;
                            if (joint[index] == 1)
                                line1[remap[index]] = atariColorPicker.PickedColorIndex;
                        }
                        break;
                    case 1:
                        atariColorPicker.Pick(line == 0 ? line0[remap[index]] : line1[remap[index]]);
                        if (atariColorPicker.PickedNewColor)
                        {
                            line1[remap[index]] = atariColorPicker.PickedColorIndex;
                            if (joint[index] == 1)
                                line0[remap[index]] = atariColorPicker.PickedColorIndex;
                        }
                        break;
                    case 2:
                        LOCKED_COLORS[remap[index]] = LOCKED_COLORS[remap[index]] == 0 ? 1 : 0;
                        break;
                    case 3:
                        if (remap[index] < 4) //only for colors that can be joined
                            JOINED_COLORS[remap[index]] = JOINED_COLORS[remap[index]] == 0 ? 1 : 0;
                        break;
                }

                CreatePal(comboBoxAverMethod.SelectedIndex);
                if (checkBoxAutoUpdate.Checked)
                {
                    (long totalDiff, int[,] bitmapDataIndexed, int[,] charMask, int[,] pmgMask) = CalcDiff2(comboBoxDistance.SelectedIndex, Dither);
                    Redraw(bitmapDataIndexed, charMask, pmgMask);
                    //DoConvert(comboBoxAverMethod.SelectedIndex, comboBoxDistance.SelectedIndex, checkBoxUseDither.Checked, comboBoxDither.SelectedIndex);
                    //MixIt();
                }
            }
        }

        private void AtariExport()
        {
            (long totalDiff, int[,] bitmapDataIndexed, int[,] charMask, int[,] pmgMask) = CalcDiff2(comboBoxDistance.SelectedIndex, Dither);
            byte[] vram = new byte[32 * srcHeightChar];
            int height = charMask.GetLength(1);
            for (int y = 0; y < height; y++)
                for (int x = 0; x < 32; x++)
                    vram[x + y * 32] = (byte)(x + (y % 4) * 32 + 128 * charMask[x, y]);

            for (int y = height; y < srcHeightChar; y++)
                for (int x = 0; x < 32; x++)
                    vram[x + y * 32] = (byte)(x + (y % 4) * 32);

            int pointer = 0;
            byte[] remap = { 1, 2, 3, 0, 0 };
            byte[] charsets = new byte[32 * srcHeightChar * 8];
            int pixelValue = 0;
            for (int y = 0; y < height; y++)
                for (int x = 0; x < 32; x++)
                {
                    //int picnum = mask[x, y];
                    for (int i = 0; i < 8; i++) //8 bytes of a char
                    {
                        byte multiplyer = 64;
                        for (int b = 0; b < 4; b++)
                        {
                            //charsets[pointer] += (byte)(multiplyer * remap[bitmap[x * 4 + b, y * 8 + i, picnum]]);
                            pixelValue = i % 2 == 0 ? remap[bitmapDataIndexed[x * 4 + b, y * 4 + i / 2] / COLORS] : remap[bitmapDataIndexed[x * 4 + b, y * 4 + i / 2] % COLORS];
                            charsets[pointer] += (byte)(multiplyer * pixelValue);
                            multiplyer >>= 2;
                        }
                        /*
                        if (x == 0) charsets[pointer] &= 0x3f;
                        else if (x == 31)
                            charsets[pointer] &= 0xfc;*/
                        pointer++;
                    }
                }

            //byte[] colors = new byte[9] { line0[1], line0[4], line0[0], line1[0], line0[2], line1[2], line0[3], line1[3], 0 };
            byte[] colors = new byte[11] { line0[0], line0[1], line0[2], line0[3], line1[0], line1[1], line1[2], line1[3], line0[4], line0[5], 0 };
            colors[10] = (byte)(checkBoxInterlace.Checked ? 1 : 0);

            byte[] pmdata = new byte[128 * 4];
            int topOffset = 16;
            for (int y = 0; y < height * 4; y++)
                for (int pm = 0; pm < 4; pm++)
                {
                    int bit = 0x80;
                    int value = 0;
                    for (int x = 0; x < 8; x++)
                    {
                        value += pmgMask[pm * 8 + x, y] * bit;
                        bit >>= 1;
                    }
                    pmdata[pm * 128 + y + topOffset] = (byte)value;
                }

            File.WriteAllBytes("vram.dat", vram);
            File.WriteAllBytes("font.fnt", charsets);
            File.WriteAllBytes("colors2.dat", colors);
            File.WriteAllBytes("pmdata.dat", pmdata);

            /*
            //$012c $4000 font   01d3
            //$1930 $6000 vram  19d7
            //$1c30 colors      1cd7
            byte[] xex = File.ReadAllBytes("alterline2.xex");
            Array.Copy(charsets, 0, xex, 0x01d3, charsets.Length);
            Array.Copy(vram, 0, xex, 0x19d7, vram.Length);
            Array.Copy(colors, 0, xex, 0x1cd7, colors.Length);
            File.WriteAllBytes("output.xex", xex);*/
            /*
            byte[] xex = File.ReadAllBytes("alterline3.xex");
            Array.Copy(pmdata, 0, xex, 0x020e, pmdata.Length);
            Array.Copy(charsets, 0, xex, 0x0412, charsets.Length);
            Array.Fill(xex, (byte)0, 0x0412 + charsets.Length, 32 * 24 * 8 - charsets.Length);
            Array.Copy(vram, 0, xex, 0x1c16, vram.Length);
            Array.Copy(colors, 0, xex, 0x1f16, colors.Length);
            File.WriteAllBytes("output.xex", xex);
            */
            byte[] xex = File.ReadAllBytes("alterline4.xex");
            Array.Copy(pmdata, 0, xex, 0x02b0, pmdata.Length);
            Array.Copy(charsets, 0, xex, 0x04b4, charsets.Length);
            Array.Fill(xex, (byte)0, 0x04b4 + charsets.Length, 32 * 24 * 8 - charsets.Length);
            Array.Copy(vram, 0, xex, 0x1cb8, vram.Length);
            Array.Copy(colors, 0, xex, 0x1fb8, colors.Length);
            File.WriteAllBytes("output.xex", xex);
        }

        private void ButtonXex_Click(object sender, EventArgs e)
        {
            AtariExport();
        }

        private void ButtonAlpaCentauriAI_Click(object sender, EventArgs e)
        {
            Centauri((int)numericUpDownGeneration.Value, (int)numericUpDownPopulation.Value);
        }

        private void ButtonAlpaCentauriInit_Click(object sender, EventArgs e)
        {
            CentauriInit((int)numericUpDownPopulation.Value);
            buttonAlpaCentauriAI.Enabled = true;
        }

        private void ListViewPopulation_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listViewPopulation.SelectedIndices.Count > 0)
            {
                Array.Copy(alpaItems[listViewPopulation.SelectedIndices[0]].Line0, line0, line0.Length);
                Array.Copy(alpaItems[listViewPopulation.SelectedIndices[0]].Line1, line1, line0.Length);
                CreatePal(comboBoxAverMethod.SelectedIndex, false);
                //DoConvert(comboBoxAverMethod.SelectedIndex, comboBoxDistance.SelectedIndex, checkBoxUseDither.Checked, comboBoxDither.SelectedIndex);
                //MixIt();
                (long totalDiff, int[,] bitmapDataIndexed, int[,] charMask, int[,] pmgMask) = CalcDiff2(comboBoxDistance.SelectedIndex, Dither);
                Redraw(bitmapDataIndexed, charMask, pmgMask);
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

                    //if (histogram.ContainsKey(rgb))
                    if (!histogram.TryAdd(rgb, 1))

                        histogram[rgb] += 1;
                    /*
                    else
                    {
                        histogram.Add(rgb, 1);
                    }*/
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
            Bitmap tgtBmp = new Bitmap(width, height);
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
                    tgtBmp.SetPixel(x, y, palette256[targetColorIndex]);
                    /*tgtBmp.SetPixel(x * 2, y * 2, palette256[targetColorIndex]);
                    tgtBmp.SetPixel(x * 2 + 1, y * 2, palette256[targetColorIndex]);
                    tgtBmp.SetPixel(x * 2, y * 2 + 1, palette256[targetColorIndex]);
                    tgtBmp.SetPixel(x * 2 + 1, y * 2 + 1, palette256[targetColorIndex]);
                    */
                    //srcChannelMatrix[x, y, 0] = palette256[targetColorIndex].R;
                    //srcChannelMatrix[x, y, 1] = palette256[targetColorIndex].G;
                    //srcChannelMatrix[x, y, 2] = palette256[targetColorIndex].B;
                    bit8map[x, y] = targetColorIndex;
                }
            pictureBoxSrcReduced.Image = tgtBmp;
            pictureBoxSrcReduced.Invalidate();
        }

        private void CheckBoxColorReduction_CheckedChanged(object sender, EventArgs e)
        {

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

        private void ButtonGenerate_Click(object sender, EventArgs e)
        {
            CreatePal(comboBoxAverMethod.SelectedIndex, true);
            CreateIdealDitheredSolution(comboBoxDistance.SelectedIndex, comboBoxDither.SelectedIndex, (float)numericUpDownDitherStrength.Value / 10f);
            (long totalDiff, int[,] bitmapDataIndexed, int[,] charMask, int[,] pmgMask) = CalcDiff2(comboBoxDistance.SelectedIndex, Dither);
            Redraw(bitmapDataIndexed, charMask, pmgMask);

        }

        private void ButtonImportColors_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                byte[] data = File.ReadAllBytes(openFileDialog1.FileName);
                if (data.Length != 11)
                {
                    MessageBox.Show("wrong file");
                    return;
                }
                Array.Copy(data, 0, line0, 0, 4);
                Array.Copy(data, 4, line1, 0, 4);
                Array.Copy(data, 8, line0, 4, 2);
                Array.Copy(data, 8, line1, 4, 2);
                CreatePal(comboBoxAverMethod.SelectedIndex, false);
                ButtonAlpaCentauriInit_Click(this, null);
                //ButtonGenerate_Click(this, e);
            }
        }

        private void ComboBoxLightness_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((comboBoxLightness.Tag == null) || (comboBoxLightness.SelectedIndex == (int)comboBoxLightness.Tag))
                return;

            int prevIndex = (int)comboBoxLightness.Tag;
            decimal brMax = numericUpDownHepaLuma.Maximum;
            decimal brVal = numericUpDownHepaLuma.Value;
            switch (comboBoxLightness.SelectedIndex)
            {
                case 0:
                    brMax = 15;
                    if (prevIndex == 1)
                        brVal /= 16;
                    else
                        brVal /= 100/16;
                    break;
                case 1:
                    brMax = 255;
                    if (prevIndex == 0)
                        brVal *= 16;
                    else
                        brVal = (decimal)((int)brVal * 2.55f);
                    break;
                case 2:
                    brMax = 100;
                    if (prevIndex == 0)
                        brVal *= 100 / 16;
                    else
                        brVal = (decimal)((int)brVal / 2.55f);
                    break;

            }
            numericUpDownHepaLuma.Maximum = brMax;
            numericUpDownHepaLuma.Value = Math.Min(brVal, brMax);
            comboBoxLightness.Tag = comboBoxLightness.SelectedIndex;
        }
    }

}