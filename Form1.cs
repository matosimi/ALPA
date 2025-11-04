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
    using static AlterLinePictureAproximator.Form1;

    /*todo: dithering only in results

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
        public static byte[] atariPalRgb = new byte[256 * 3];
        //my colors:    1e,16,26,76,00
        //              14,16,48,b6,00
        public float[] atariPalLab = new float[256 * 3];
        //public Color[,] customColors;
        public static readonly byte[] line0default = new byte[COLORS + 1] { 0x1e, 0x16, 0x26, 0x76, 0x00, 0x0e };
        public static readonly byte[] line1default = new byte[COLORS + 1] { 0x14, 0x1a, 0x48, 0xb6, 0x00, 0x0e };
        /// <summary>colors that are joined between scanlines (background and PMG layer)</summary>
        public static readonly int[] JOINED_COLORS = { 0, 0, 0, 0, 1, 1 };
        /// <summary>colors that are locked for the change by AI (background)</summary>
        public static readonly int[] LOCKED_COLORS = { 0, 0, 0, 0, 0, 0 };
        /// <summary>RGB colors that are alternating, two sets for normal/inverse chars</summary>
        //public Color[,] cline0 = new Color[COLORS, 2];
        //public Color[,] cline1 = new Color[COLORS, 2];
        public int[,,,] diff_matrix;
        public AtariColorPicker atariColorPicker = new AtariColorPicker();
        public int[,] mask;
        public int[,] pmgData;
        public int[,,] bitmap;
        public byte[] bitmapPixelsDithered; //dithered bitmap pixels data in B-R-G order
        public byte[,,] srcChannelMatrix;   //matrix containing RGB channels of source image
        public int[,] bit8map;
//        public int[,,] colorIgnoreMatrix = new int[COLORS * COLORS, 2, 3];
        public Color[] palette8 = new Color[256];
        public int[,,] customP8Match = new int[256, 2, 2];   //reset every custom palette change
        public long[,,] customP8MatchDist = new long[256, 2, 2];    ////reset every custom palette change
        public List<AlpaItem> alpaItems = new List<AlpaItem>();
        private AlpaCollection myAlpaCollection;
        public Random rand = new Random();
        public int CharsPerLine => comboBoxCharsPerLine.SelectedIndex == 0 ? 32 : 40;
        public int TargetWidth => CharsPerLine * 4;
        private static readonly ThreadLocal<Random> ThreadRng = new ThreadLocal<Random>(() => new Random(unchecked(Environment.TickCount * 31 + Thread.CurrentThread.ManagedThreadId)));
        private static Random GetThreadRandom()
        {
            return ThreadRng.Value!;
        }
        public int totalPixels;
        private static Bitmap openedBitmap;
        //public byte[,,,,] customP24Match = new byte[256, 256, 256, 2, 2];
        //public long[,,,,] customP24MatchDist = new long[256, 256, 256, 2, 2];

        // HEPA optimization fields
        public static int[,] hepaIgnoreMatrix;  // [color1 << 1, color2 << 1]
        private int currentHepaLumaFilter = -1;
        private int currentHepaChromaFilter = -1;
        private int currentBrightnessMethod = -1;

        public class AlpaItem
        {
            public long Diff { get; set; }
            public int Ppdiff { get; set; }
            /// <summary>Atari color indexes color0,color1,color2,color3,colbak,colpm</summary>
            public byte[] Line0 { get; set; }
            public byte[] Line1 { get; set; }
            /// <summary>RGB Colors of color0,color1,color2/3,colbak,colpm -> note that 2nd parameter defines if char is normal/inverse hence if color2 or color3 is used</summary>
            public Color[,] Cline0 { get; set; }
            public Color[,] Cline1 { get; set; }
            public int Zero { get; set; }
            /// <summary>Contains color matrix of blended colors between Cline0 and Cline1, both normal and inverse</summary>
            public Color[,] BlendedColors { get; set; }
            /// <summary>Contains 0 when blended color is ignored due to HEPA incl. PMG or BG or none</summary>
            public int[,,] ColorIgnoreMatrix { get; set; }
            // Shallow copy
            public AlpaItem ShallowCopy()
            {
                return (AlpaItem)this.MemberwiseClone();
            }

            /// <summary>
            /// Recalculates CustomColors based on Line0 and Line1 using the specified averaging method
            /// </summary>
            /// <param name="atariPalRgb">Atari palette RGB data</param>
            /// <param name="averagingMethod">Averaging method index (0=rgb simple, 1=rgb euclid, 2=yuv euclid)</param>
            /// <param name="colorsPerLine">Number of colors per line (COLORS)</param>
            public void RecalculateCustomColors(byte[] atariPalRgb, int averagingMethod, int colorsPerLine, bool useHepa)
            {
                if (Line0 == null || Line1 == null) return;
                
                BlendedColors = new Color[colorsPerLine * colorsPerLine, 2];
                ColorIgnoreMatrix = new int[colorsPerLine * colorsPerLine, 2, 3];
                for (int z = 0; z < 2; z++)
                {
                    // Build local color lines without the removed index
                    int removeIndex = 3 - z;
                    byte[] l0 = new byte[colorsPerLine];
                    byte[] l1 = new byte[colorsPerLine];
                    int w = 0;
                    for (int ii = 0; ii < Line0.Length; ii++)
                    {
                        if (ii == removeIndex) continue;
                        l0[w] = Line0[ii];
                        l1[w] = Line1[ii];
                        //calculate 24bit colors too
                        Cline0[w, z] = AtariColorToColor(l0[w]);
                        Cline1[w, z] = AtariColorToColor(l1[w]);
                        w++;
                    }
                    
                    int index = 0;
                    for (int a = 0; a < colorsPerLine; a++)
                    {
                        for (int b = 0; b < colorsPerLine; b++)
                        {
                            BlendedColors[index, z] = CalculateAverageColor(l0[a], l1[b], averagingMethod, atariPalRgb);
                            int hepaItem;
                            if (hepaIgnoreMatrix != null && useHepa)
                            {
                                // Use pre-calculated HEPA value from the matrix
                                hepaItem = hepaIgnoreMatrix[l0[a], l1[b]];
                            }
                            else
                            {
                                // If HEPA is disabled, all colors are considered different
                                hepaItem = 1;
                            }
                            // Apply the HEPA filter along with other matrices
                            ColorIgnoreMatrix[index, z, 0] = hepaItem & IGNORE_PMG_MATRIX5[index];
                            ColorIgnoreMatrix[index, z, 1] = hepaItem & IGNORE_BG_MATRIX5[index];
                            ColorIgnoreMatrix[index, z, 2] = hepaItem;
                            index++;
                        }
                    }
                }
            }

            private Color CalculateAverageColor(int c1, int c2, int method, byte[] atariPalRgb)
            {
                // Use parent class's AverageColor method which accepts atari color indices
                // This duplicates the logic but is needed since we pass atariPalRgb as parameter
                // and the parent's method uses instance field atariPalRgb
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
                        return Color.FromArgb(Form1.Clamp256((int)Math.Sqrt(r2)), Form1.Clamp256((int)Math.Sqrt(g2)), Form1.Clamp256((int)Math.Sqrt(b2)));
                    case 2: //yuv euclid
                        byte[] z1 = RGB2YUV(atariPalRgb[c1 * 3], atariPalRgb[c1 * 3 + 1], atariPalRgb[c1 * 3 + 2]);
                        byte[] z2 = RGB2YUV(atariPalRgb[c2 * 3], atariPalRgb[c2 * 3 + 1], atariPalRgb[c2 * 3 + 2]);
                        int y = (z1[0] * z1[0] + z2[0] * z2[0]) / 2;
                        int u = (z1[1] * z1[1] + z2[1] * z2[1]) / 2;
                        int v = (z1[2] * z1[2] + z2[2] * z2[2]) / 2;
                        byte[] ave = YUV2RGB((byte)Form1.Clamp256((int)Math.Sqrt(y)), (byte)Form1.Clamp256((int)Math.Sqrt(u)), (byte)Form1.Clamp256((int)Math.Sqrt(v)));
                        return Color.FromArgb(ave[0], ave[1], ave[2]);
                }
                return Color.Yellow;
            }

            // Helper methods for color conversion (kept here since parent's methods are private instance methods)
            private static byte[] RGB2YUV(byte R, byte G, byte B)
            {
                byte y = (byte)Form1.Clamp256(((66 * (R) + 129 * (G) + 25 * (B) + 128) >> 8) + 16);
                byte u = (byte)Form1.Clamp256(((-38 * (R) - 74 * (G) + 112 * (B) + 128) >> 8) + 128);
                byte v = (byte)Form1.Clamp256(((112 * (R) - 94 * (G) - 18 * (B) + 128) >> 8) + 128);
                return new byte[3] { y, u, v };
            }

            private static byte[] YUV2RGB(byte y, byte u, byte v)
            {
                int C = y - 16;
                int D = u - 128;
                int E = v - 128;
                byte r = (byte)Form1.Clamp256((298 * C + 409 * E + 128) >> 8);
                byte g = (byte)Form1.Clamp256((298 * C - 100 * D - 208 * E + 128) >> 8);
                byte b = (byte)Form1.Clamp256((298 * C + 516 * D + 128) >> 8);
                return new byte[3] { r, g, b };
            }
        }

        public class AlpaCollection
        {
            private readonly List<AlpaItem> lineItems;
            public int Height { get; }
            public int ColorsPerLine { get; }
            public int Cursor { get; set; }

            public AlpaCollection(int height, int colorsPerLine)
            {
                if (height <= 0) throw new ArgumentOutOfRangeException(nameof(height), "Height must be positive");
                if (colorsPerLine <= 0) throw new ArgumentOutOfRangeException(nameof(colorsPerLine), "Colors per line must be positive");

                Height = height;
                ColorsPerLine = colorsPerLine;
                lineItems = new List<AlpaItem>(height);

                // Initialize with default AlpaItems
                for (int i = 0; i < height; i++)
                {
                    var item = new AlpaItem
                    {
                        Line0 = new byte[colorsPerLine],
                        Line1 = new byte[colorsPerLine],
                        Diff = long.MaxValue,
                        Ppdiff = int.MaxValue
                    };
                    lineItems.Add(item);
                }
                Cursor = 0; //by default point to first item
            }

            public AlpaItem this[int line]
            {
                get
                {
                    if (line < 0 || line >= Height)
                        throw new ArgumentOutOfRangeException(nameof(line), $"Line index must be between 0 and {Height - 1}");
                    return lineItems[line];
                }
                set
                {
                    if (line < 0 || line >= Height)
                        throw new ArgumentOutOfRangeException(nameof(line), $"Line index must be between 0 and {Height - 1}");
                    if (value == null) throw new ArgumentNullException(nameof(value));
                    if (value.Line0.Length != ColorsPerLine || value.Line1.Length != ColorsPerLine)
                        throw new ArgumentException("AlpaItem color arrays must match the collection's ColorsPerLine");

                    lineItems[line] = value;
                }
            }

            public IReadOnlyList<AlpaItem> GetAllItems() => lineItems.AsReadOnly();

            public AlpaCollection ShallowCopy()
            {
                var copy = new AlpaCollection(Height, ColorsPerLine);
                for (int i = 0; i < Height; i++)
                {
                    copy.lineItems[i] = lineItems[i].ShallowCopy();
                }
                return copy;
            }

            // Helper method to get a specific color from a specific line
            public byte GetColor(int line, int colorIndex, bool isLine1)
            {
                var item = this[line];
                return isLine1 ? item.Line1[colorIndex] : item.Line0[colorIndex];
            }

            // Helper method to set a specific color in a specific line
            public void SetColor(int line, int colorIndex, bool isLine1, byte value)
            {
                var item = this[line];
                if (isLine1)
                    item.Line1[colorIndex] = value;
                else
                    item.Line0[colorIndex] = value;
            }
        }

        private void PopulateDefaultPalette(AlpaCollection alpaCollection)
        {
            if (alpaCollection == null) return;
            
            // Populate AlpaCollection with default palette
            for (int y = 0; y < alpaCollection.Height; y++)
            {
                var item = alpaCollection[y];
                for (int c = 0; c < item.Line0.Length; c++)
                {
                    // Use modulo to cycle through the default palette colors
                    item.Line0[c] = (byte)line0default[c];
                    item.Line1[c] = (byte)line1default[c];
                }
                // Recalculate customColors for the item after setting values
                item.RecalculateCustomColors(atariPalRgb, comboBoxAverMethod.SelectedIndex, COLORS, false);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            openedBitmap = new Bitmap(pictureBoxSource.Image);
            this.Text = "AlterLinePictureApproximator (ALPA) v1.0 by MatoSimi 10.10.2025";

            trackBarGamma.Minimum = 1;
            trackBarGamma.Maximum = 30;
            trackBarGamma.Value = 10;

            trackBarLightness.Minimum = -50;
            trackBarLightness.Maximum = 50;
            trackBarLightness.Value = 0;

            trackBarSaturation.Minimum = 0;
            trackBarSaturation.Maximum = 200;
            trackBarSaturation.Value = 100;

            LoadPalette();
            comboBoxLightness.SelectedIndex = 0;
            comboBoxLightness.Tag = 0;  //previous value
            CreatePal(comboBoxAverMethod.SelectedIndex);
            comboBoxDistance.SelectedIndex = 0;
            comboBoxDither.SelectedIndex = 0;
            comboBoxAverMethod.SelectedIndex = 0;
            comboBoxCharsPerLine.SelectedIndex = 0; //preselect 32 chars narrow width
            ApplyImageAdjustments();
            ReduceColors();
            pictureBoxResultLines.ZoomFactor = PictureBoxWithInterpolationMode.PictureBoxZoomFactor.x1;
            checkBoxAutoGenerate.Checked = true;
            ButtonGenerate_Click(this, null);
        }

        private void AdjustmentControls_Changed(object sender, EventArgs e)
        {
            labelL.Text = $"L\n{trackBarLightness.Value}";
            labelS.Text = $"S\n{trackBarSaturation.Value}";
            labelG.Text = $"G\n{trackBarGamma.Value}";
            ApplyImageAdjustments();
            if (checkBoxAutoGenerate.Checked)
                ButtonGenerate_Click(this, null);
        }

        private void ApplyImageAdjustments()
        {
            Bitmap original = (Bitmap)pictureBoxSource.Image;
            Bitmap adjusted = new Bitmap(TargetWidth, srcHeight, original.PixelFormat);

            using (Graphics g = Graphics.FromImage(adjusted))
            {
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.DrawImage(original, 0, 0, TargetWidth, srcHeight);
            }

            float gamma = trackBarGamma.Value / 10f;
            float lightness = trackBarLightness.Value / 100f; // from -0.5 to 0.5
            float saturation = trackBarSaturation.Value / 100f;

            Bitmap gammaCorrected = new Bitmap(adjusted.Width, adjusted.Height);
            using (Graphics g = Graphics.FromImage(gammaCorrected))
            {
                ImageAttributes gammaAttributes = new ImageAttributes();
                gammaAttributes.SetGamma(gamma);
                g.DrawImage(adjusted, new Rectangle(0, 0, adjusted.Width, adjusted.Height),
                    0, 0, adjusted.Width, adjusted.Height, GraphicsUnit.Pixel, gammaAttributes);
            }

            Bitmap final = new Bitmap(adjusted.Width, adjusted.Height);
            using (Graphics g = Graphics.FromImage(final))
            {
                if (saturation != 1.0f || lightness != 0.0f)
                {
                    ImageAttributes satLightAttributes = new ImageAttributes();
                    float s = saturation;
                    float t_sat = (1.0f - s) / 2.0f;

                    ColorMatrix colorMatrix = new ColorMatrix(
                        new float[][]
                        {
                            new float[] {t_sat+s, t_sat, t_sat, 0, 0},
                            new float[] {t_sat, t_sat+s, t_sat, 0, 0},
                            new float[] {t_sat, t_sat, t_sat+s, 0, 0},
                            new float[] {0, 0, 0, 1, 0},
                            new float[] {lightness, lightness, lightness, 0, 1}
                        });
                    satLightAttributes.SetColorMatrix(colorMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
                    g.DrawImage(gammaCorrected, new Rectangle(0, 0, final.Width, final.Height),
                        0, 0, gammaCorrected.Width, gammaCorrected.Height, GraphicsUnit.Pixel, satLightAttributes);
                }
                else
                {
                    g.DrawImage(gammaCorrected, 0, 0);
                }
            }

            Bitmap t = new Bitmap(final.Width, final.Height / 2);
            srcChannelMatrix = new byte[t.Width, t.Height, 3];
            int method = comboBoxAverMethod.SelectedIndex;
            for (int y = 0; y < t.Height; y++)
            {
                for (int x = 0; x < t.Width; x++)
                {
                    Color src = AverageColor(final.GetPixel(x, y * 2), final.GetPixel(x, y * 2 + 1), method);
                    srcChannelMatrix[x, y, 0] = src.R;
                    srcChannelMatrix[x, y, 1] = src.G;
                    srcChannelMatrix[x, y, 2] = src.B;

                    t.SetPixel(x, y, src);
                }
            }
            pictureBoxSrcData.Image = new Bitmap(t);
            totalPixels = srcChannelMatrix.Length / 3;
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

        //<summary> Converts atari indexed color to 24bit Color object
        public static Color AtariColorToColor(int colorIndex)
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
            double vR = atariPalRgb[atariColor * 3] / 255f;
            double vG = atariPalRgb[atariColor * 3 + 1] / 255f;
            double vB = atariPalRgb[atariColor * 3 + 2] / 255f;
            double y = (0.2126 * RgbToLin(vR) + 0.7152 * RgbToLin(vG) + 0.0722 * RgbToLin(vB));

            if (y <= (216 / 24389f))
            {       // The CIE standard states 0.008856 but 216/24389 is the intent for 0.008856451679036
                return y * (24389 / 27f);  // The CIE standard states 903.3, but 24389/27 is the intent, making 903.296296296296296
            }
            else
            {
                return Math.Pow(y, (1 / 3f)) * 116 - 16;
            }
        }

        private int UpdateHepaIgnoreMatrix()
        {
            // Only update if HEPA is enabled and settings have changed
            if (!checkBoxHepa.Checked)
            {
                // If HEPA is disabled, set all colors to be considered (1)
                hepaIgnoreMatrix = null;
                return -1;
            }

            int hepaLumaFilter = (int)numericUpDownHepaLuma.Value;
            int hepaChromaFilter = (int)numericUpDownHepaChroma.Value;
            int brightnessMethod = comboBoxLightness.SelectedIndex;

            // Check if we need to update the matrix
            if (hepaIgnoreMatrix != null &&
                currentHepaLumaFilter == hepaLumaFilter &&
                currentHepaChromaFilter == hepaChromaFilter &&
                currentBrightnessMethod == brightnessMethod)
            {
                return -1; // No changes needed
            }

            // Update current settings
            currentHepaLumaFilter = hepaLumaFilter;
            currentHepaChromaFilter = hepaChromaFilter;
            currentBrightnessMethod = brightnessMethod;

            // Initialize the matrix (256 colors, 256 colors)
            hepaIgnoreMatrix = new int[256,256];

            // Pre-calculate HEPA matrix for all color combinations
            int possibleColors = 0;
            for (int c1 = 0; c1 < 256; c1++)
            {
                for (int c2 = 0; c2 < 256; c2++)
                {
                    //if colors match, it is automatically allowed
                    if (c1 == c2)
                    {
                        hepaIgnoreMatrix[c1, c2] = 1;
                        possibleColors++;
                        continue;
                    }

                    int loopingColorDifference = Math.Min(Math.Abs((c1 / 16) - (c2 / 16)), 16 - (Math.Abs((c1 / 16) - (c2 / 16))));
                    //if chroma difference is too big, automatically forbid
                    if (loopingColorDifference > hepaChromaFilter)
                    {
                        hepaIgnoreMatrix[c1, c2] = 0;
                        continue;
                    }

                    int cellValue = 0;
                    //https://stackoverflow.com/questions/596216/formula-to-determine-perceived-brightness-of-rgb-color
                    switch (brightnessMethod)
                    {
                        // atari color palette difference in low nibble
                        case 0:
                            cellValue = (Math.Abs((c1 % 16) - (c2 % 16)) <= hepaLumaFilter) ? 1 : 0;
                            break;
                        case 1: //(0.2126*R + 0.7152*G + 0.0722*B)
                            double lum0 = atariPalRgb[c1 * 3] * 0.2126 +
                                          atariPalRgb[c1 * 3 + 1] * 0.7152 +
                                          atariPalRgb[c1 * 3 + 2] * 0.0722;
                            double lum1 = atariPalRgb[c2 * 3] * 0.2126 +
                                          atariPalRgb[c2 * 3 + 1] * 0.7152 +
                                          atariPalRgb[c2 * 3 + 2] * 0.0722;
                            cellValue = (Math.Abs((int)(lum0 - lum1)) <= hepaLumaFilter) ? 1 : 0;
                            break;
                        case 2:
                            cellValue = (Math.Abs((int)(GetLStar(c1) - GetLStar(c2))) <= hepaLumaFilter) ? 1 : 0;
                            break;
                        default:
                            cellValue = 0;
                            break;
                    }
                    hepaIgnoreMatrix[c1, c2] = cellValue;
                    if (cellValue == 1) possibleColors++;
                }
            }
            return possibleColors / 8; //a x b == b x a .. so only half
        }

        private void CreatePal(int method, bool noDraw = false)
        {
            int possibleColors = UpdateHepaIgnoreMatrix();
            // Update the HEPA ignore matrix if needed
            if (possibleColors > 0)
            {
                groupBox1.Text = $"HEPA filter - possible colors {possibleColors}";
            }
            int brightnessMethod = comboBoxLightness.SelectedIndex;
            
            //drawing part and color count only when drawing is requested
            if (!noDraw)
            {
                // Get the appropriate AlpaCollection item to display
                Color[,] displayBlendedColors = myAlpaCollection[myAlpaCollection.Cursor].BlendedColors;
                Color[,] displayCline0 = myAlpaCollection[myAlpaCollection.Cursor].Cline0;
                Color[,] displayCline1 = myAlpaCollection[myAlpaCollection.Cursor].Cline1;
                int[,,] displayIgnoreMatrix = myAlpaCollection[myAlpaCollection.Cursor].ColorIgnoreMatrix;

                List<Color> colors = new();
                for (int z = 0; z < 2; z++)
                    for (int t = 0; t < 2; t++)
                        for (int i = 0; i < displayBlendedColors.GetLength(0); i++)
                        {
                            if (displayIgnoreMatrix[i, z, t] != 0 && !colors.Contains(displayBlendedColors[i, z]))
                                colors.Add(displayBlendedColors[i, z]);
                        }
                labelPossibleColors.Text = $"Colors: {colors.Count} ";
                colors.Clear();
                Bitmap p = new Bitmap(COLORS * 16 * 2, (COLORS + 2 + 2) * 16);
                Graphics g = Graphics.FromImage(p);
                const int OFFSET = COLORS * 16 + 1;
                // iconColorMatch shows the actual layout: { 0, 1, 2, 4, 5, -1, -1, 3, -1, -1 }
                // This means positions 0-4 display: indices 0, 1, 2, 4, 5 (skipping index 3)
                // Position 4 should show index 5 (the last color, 0x0e)
                int[] displayOrder = { 0, 1, 2, 4, 5 }; // The order for display positions 0-4
                for (int z = 0; z < 2; z++)
                {
                    for (int a = 0; a < COLORS; a++)
                    {
                        for (int b = 0; b < COLORS; b++)
                        {
                            g.FillRectangle(new SolidBrush(displayBlendedColors[a * COLORS + b, z]), new Rectangle(b * 16 + OFFSET * z, a * 16, 16, 16));
                        }
                        g.FillRectangle(new SolidBrush(displayCline0[a, z]), new Rectangle(a * 16 + OFFSET * z, OFFSET, 16, 16));
                        g.FillRectangle(new SolidBrush(displayCline1[a, z]), new Rectangle(a * 16 + OFFSET * z, OFFSET + 16, 16, 16));
                        /*// Display cline0 and cline1 colors
                        // For a=0,1,2: display indices 0,1,2 directly
                        // For a=3: display index 4
                        // For a=4: display index 5 (the last color, 0x0e)
                        int originalIndex = displayOrder[a];
                        Color color0, color1;
                        
                        // Get colors from the appropriate source
                        if (myAlpaCollection != null && displayCustomColors != localCustomColors)
                        {
                            // Using AlpaCollection item - get colors from original Line0/Line1
                            int itemIndex = checkBoxDividerEnabled.Checked ? (int)numericUpDownDividerStart.Value : 0;
                            if (itemIndex >= 0 && itemIndex < myAlpaCollection.Height)
                            {
                                var item = myAlpaCollection[itemIndex];
                                if (originalIndex < item.Line0.Length && originalIndex < item.Line1.Length)
                                {
                                    color0 = AtariColorToColor(item.Line0[originalIndex]);
                                    color1 = AtariColorToColor(item.Line1[originalIndex]);
                                }
                                else
                                {
                                    // Fallback to displayCline0/displayCline1 if available
                                    int l0Index = a < COLORS ? a : COLORS - 1;
                                    color0 = displayCline0[l0Index, z];
                                    color1 = displayCline1[l0Index, z];
                                }
                            }
                            else
                            {
                                int l0Index = a < COLORS ? a : COLORS - 1;
                                color0 = displayCline0[l0Index, z];
                                color1 = displayCline1[l0Index, z];
                            }
                        }
                        else
                        {
                            // Using global palette - get colors from original line0/line1
                            if (originalIndex < line0.Length && originalIndex < line1.Length)
                            {
                                color0 = AtariColorToColor(line0[originalIndex]);
                                color1 = AtariColorToColor(line1[originalIndex]);
                            }
                            else
                            {
                                int l0Index = a < COLORS ? a : COLORS - 1;
                                color0 = displayCline0[l0Index, z];
                                color1 = displayCline1[l0Index, z];
                            }
                        }

                        g.FillRectangle(new SolidBrush(color0), new Rectangle(a * 16 + OFFSET * z, OFFSET, 16, 16));
                        g.FillRectangle(new SolidBrush(color1), new Rectangle(a * 16 + OFFSET * z, OFFSET + 16, 16, 16));
                    */
                    }
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
            // Only clear 24-bit cache when actually used
            // if (!UseReducedSource)
            //   Array.Clear(customP24Match);
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
            ApplyImageAdjustments();
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
            var distanceFn = SelectDistance(distanceMethod);
            int width = srcChannelMatrix.GetLength(0);
            int height = srcChannelMatrix.GetLength(1);

            int charHeight = height / 4;
            int charsPerLine = CharsPerLine;
            int[,] charMask = new int[charsPerLine, charHeight];
            int[,] pmgMask = new int[charsPerLine, height];
            int[,] bitmapDataIndexed = new int[width, height];
            long charDiff0 = 0;
            long charDiff1 = 0;
            long totalDiff = 0;

            InitializeArrayToMinusOne(customP8Match);
            Array.Clear(customP8MatchDist);
            int[,,,] bitmapCharDataIndexed = new int[4, 4, 2, 2];
            int[,] pmgCharData = new int[4, 2];

            for (int y = 0; y < charHeight; y++)
                for (int x = 0; x < charsPerLine; x++)
                {
                    for (int z = 0; z < 2; z++) //inverse layer
                    {
                        if (z == 0) { charDiff0 = 0; } else { charDiff1 = 0; }
                        for (int cy = 0; cy < 4; cy++)
                        {
                            // Get customColors from AlpaCollection based on line
                            int lineIndex = y * 2 + cy / 2;
                            Color[,] lineCustomColors = null;
                            if (myAlpaCollection != null && lineIndex < myAlpaCollection.Height)
                            {
                                var item = myAlpaCollection[lineIndex];
                                if (item.BlendedColors == null)
                                {
                                    item.RecalculateCustomColors(atariPalRgb, comboBoxAverMethod.SelectedIndex, COLORS, checkBoxHepa.Checked);
                                }
                                lineCustomColors = item.BlendedColors;
                            }
                            if (lineCustomColors == null) continue; // Skip if no customColors available

                            // t=0 - pmg layer
                            long sum0 = 0;
                            for (int cx = 0; cx < 4; cx++)
                            {
                                long distance;
                                int index;
                                if (doDither)
                                {
                                    int pixelIndex = (x * 4 + cx + (y * 4 + cy) * TargetWidth) * 3;
                                    Color desiredColor = Color.FromArgb(bitmapPixelsDithered[pixelIndex + 2], bitmapPixelsDithered[pixelIndex + 1], bitmapPixelsDithered[pixelIndex]);
                                    (distance, index) = FindCustomClosest2(desiredColor, distanceFn, z, 0, lineCustomColors);
                                }
                                else if (UseReducedSource)
                                {
                                    (distance, index) = FindCustomClosest2Reduced(bit8map[x * 4 + cx, y * 4 + cy], distanceFn, z, 0, lineCustomColors);
                                }
                                else
                                {
                                    (distance, index) = FindCustomClosest2(palette8[bit8map[x * 4 + cx, y * 4 + cy]], distanceFn, z, 0, lineCustomColors);
                                }
                                sum0 += distance * distance;
                                bitmapCharDataIndexed[cx, cy, z, 0] = index;
                            }
                            // t=1
                            long sum1 = 0;
                            for (int cx = 0; cx < 4; cx++)
                            {
                                long distance;
                                int index;
                                if (doDither)
                                {
                                    int pixelIndex = (x * 4 + cx + (y * 4 + cy) * TargetWidth) * 3;
                                    Color desiredColor = Color.FromArgb(bitmapPixelsDithered[pixelIndex + 2], bitmapPixelsDithered[pixelIndex + 1], bitmapPixelsDithered[pixelIndex]);
                                    (distance, index) = FindCustomClosest2(desiredColor, distanceFn, z, 1, lineCustomColors);
                                }
                                else if (UseReducedSource)
                                {
                                    (distance, index) = FindCustomClosest2Reduced(bit8map[x * 4 + cx, y * 4 + cy], distanceFn, z, 1, lineCustomColors);
                                }
                                else
                                {
                                    (distance, index) = FindCustomClosest2(palette8[bit8map[x * 4 + cx, y * 4 + cy]], distanceFn, z, 1, lineCustomColors);
                                }
                                sum1 += distance * distance;
                                bitmapCharDataIndexed[cx, cy, z, 1] = index;
                            }
                            int bestT = (sum0 > sum1) ? 1 : 0;
                            pmgCharData[cy, z] = bestT;
                            if (z == 0) charDiff0 += (bestT == 0 ? sum0 : sum1); else charDiff1 += (bestT == 0 ? sum0 : sum1);
                        }
                    }
                    int selectedZ = (charDiff0 > charDiff1) ? 1 : 0; //1=black
                    charMask[x, y] = selectedZ;
                    totalDiff += (selectedZ == 0 ? charDiff0 : charDiff1);
                    for (int cy = 0; cy < 4; cy++)  //fill pmg data based on selected char (normal or inverse)
                    {
                        int pmgPixel = pmgCharData[cy, selectedZ];
                        pmgMask[x, y * 4 + cy] = pmgPixel;
                        for (int cx = 0; cx < 4; cx++)
                            bitmapDataIndexed[x * 4 + cx, y * 4 + cy] = bitmapCharDataIndexed[cx, cy, selectedZ, pmgPixel];
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
            int charsPerLine = CharsPerLine;
            int[,] charMask = new int[charsPerLine, charHeight];
            int[,] pmgMask = new int[charsPerLine, height];
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
            var distanceFn = SelectDistance(distanceMethod);
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
                // Get customColors from AlpaCollection based on line (y is the line index in srcChannelMatrix)
                Color[,] lineCustomColors = null;
                if (myAlpaCollection != null && y < myAlpaCollection.Height)
                {
                    var item = myAlpaCollection[y];
                    if (item.BlendedColors == null)
                    {
                        item.RecalculateCustomColors(atariPalRgb, comboBoxAverMethod.SelectedIndex, COLORS, checkBoxHepa.Checked);
                    }
                    lineCustomColors = item.BlendedColors;
                }
                if (lineCustomColors == null) continue; // Skip if no customColors available

                for (int x = 0; x < width; x++)
                {
                    int[] error = new int[3];
                    for (int i = 0; i < 3; i++)
                    {
                        error[i] = errorStorage[x, y % 2, i];
                        errorStorage[x, y % 2, i] = 0;
                    }

                    var (distance, color, errorNew) = FindCustomClosest3(palette8[bit8map[x, y]], error, distanceFn, lineCustomColors);
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
            int width = TargetWidth;
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
                // Get customColors from AlpaCollection based on line (y is pixel row, line = y / 2)
                int lineIndex = y / 2;
                Color[,] lineCustomColors = null;
                Color[,] cline0 = null;
                Color[,] cline1 = null;
                if (myAlpaCollection != null && lineIndex < myAlpaCollection.Height)
                {
                    var item = myAlpaCollection[lineIndex];
                    if (item.BlendedColors == null)
                    {
                        item.RecalculateCustomColors(atariPalRgb, comboBoxAverMethod.SelectedIndex, COLORS, checkBoxHepa.Checked);
                    }
                    lineCustomColors = item.BlendedColors;
                    cline0 = item.Cline0;
                    cline1 = item.Cline1;
                }
                if (lineCustomColors == null) continue; // Skip if no customColors available

                for (int x = 0; x < width; x++)
                {
                    var color = lineCustomColors[bitmapDataIndexed[x, y], charMask[x / 4, y / 4]];
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



        // Preselect distance function to avoid switching in hot loops
        private Func<Color, Color, long> SelectDistance(int distanceMethod)
        {
            return distanceMethod switch
            {
                0 => Difference,
                1 => RGBEuclidianDistance,
                2 => RGByuvDistance,
                3 => YUVEuclidianDistance,
                4 => WeightedRGBDistance,
                _ => throw new NotImplementedException()
            };
        }


        /// <summary>
        /// Finds closest color towards picture reduced to 256 colors
        /// </summary>
        /// <param name="p8index"></param>
        /// <param name="distanceFn"></param>
        /// <param name="inverse"></param>
        /// <param name="pmg"></param>
        /// <param name="customColors"></param>
        /// <returns></returns>
        private (long distance, int index) FindCustomClosest2Reduced(int p8index, Func<Color, Color, long> distanceFn, int inverse, int pmg, Color[,] customColors)
        {
            long mindist = long.MaxValue;
            int index = 0;

            int indexStored = customP8Match[p8index, inverse, pmg];
            if (indexStored == -1)
            {
                var item = myAlpaCollection[myAlpaCollection.Cursor];
                for (byte i = 0; i < COLORS * COLORS; i++)
                {
                    if (item.ColorIgnoreMatrix[i, inverse, pmg] != 0)
                    {
                        long dist = distanceFn(palette8[p8index], customColors[i, inverse]);
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
        /// <param name="distanceFn"></param>
        /// <param name="customColors"></param>
        /// <returns></returns>
        private (long distance, Color color, int[] error) FindCustomClosest3(Color desiredColor, int[] error, Func<Color, Color, long> distanceFn, Color[,] customColors)
        {
            long mindist = long.MaxValue;
            Color color = Color.Magenta;

            int[] rgbWithError = { desiredColor.R + error[0], desiredColor.G + error[1], desiredColor.B + error[2] };
            Color colorWithError = Color.FromArgb(Clamp256(rgbWithError[0]), Clamp256(rgbWithError[1]), Clamp256(rgbWithError[2]));
            var item = myAlpaCollection[myAlpaCollection.Cursor];
            for (byte i = 0; i < COLORS * COLORS; i++)
            {
                for (int inverse = 0; inverse < 2; inverse++)
                    if (item.ColorIgnoreMatrix[i, inverse, 2] != 0) //[x,x,2] means pmg colors allowed for each pixel
                    {
                        long dist = distanceFn(colorWithError, customColors[i, inverse]);
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
        /// <param name="color"></param>
        /// <param name="distanceFn"></param>
        /// <param name="inverse"></param>
        /// <param name="pmg"></param>
        /// <param name="customColors"></param>
        /// <returns></returns>
        private (long distance, int index) FindCustomClosest2(Color color, Func<Color, Color, long> distanceFn, int inverse, int pmg, Color[,] customColors)
        {
            long mindist = long.MaxValue;
            int index = 0;
            /*int index = customP24Match[color.R, color.G, color.B, inverse, pmg] - 1;
            if (index < 0)
            {*/
            var item = myAlpaCollection[myAlpaCollection.Cursor];
            for (byte i = 0; i < COLORS * COLORS; i++)
            {
                if (item.ColorIgnoreMatrix[i, inverse, pmg] != 0)
                {
                    long dist = distanceFn(color, customColors[i, inverse]);
                    if (dist < mindist)
                    {
                        mindist = dist;
                        index = i;
                    }
                }
            }
            /*   customP24Match[color.R, color.G, color.B, inverse, pmg] = (byte)(index + 1);
               customP24MatchDist[color.R, color.G, color.B, inverse, pmg] = mindist;
           }
           else
           {
               mindist = customP24MatchDist[color.R, color.G, color.B, inverse, pmg];
           }*/
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
            if (checkBoxAutoGenerate.Checked)
                ButtonGenerate_Click(this, null);
        }

        private void checkBoxHepa_CheckedChanged(object sender, EventArgs e)
        {
            numericUpDownHepaChroma.Enabled = checkBoxHepa.Checked;
            numericUpDownHepaLuma.Enabled = checkBoxHepa.Checked;
            comboBoxLightness.Enabled = checkBoxHepa.Checked;
            
            // Force recalculation of HEPA matrix
            currentHepaLumaFilter = -1;
            currentHepaChromaFilter = -1;
            currentBrightnessMethod = -1;
            
            if (checkBoxAutoGenerate.Checked)
                ButtonGenerate_Click(this, null);
        }

        private void comboBoxLightness_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Only process if the value actually changed
            if (comboBoxLightness.SelectedIndex != (int?)comboBoxLightness.Tag)
            {
                comboBoxLightness.Tag = comboBoxLightness.SelectedIndex;
                
                // Force recalculation of HEPA matrix
                currentBrightnessMethod = -1;
                
                if (checkBoxAutoGenerate.Checked)
                    ButtonGenerate_Click(this, null);
            }
        }

        private void numericUpDownHepaLuma_ValueChanged(object sender, EventArgs e)
        {
            // Force recalculation of HEPA matrix
            currentHepaLumaFilter = -1;
            
            if (checkBoxAutoGenerate.Checked)
                ButtonGenerate_Click(this, null);
        }

        private void numericUpDownHepaChroma_ValueChanged(object sender, EventArgs e)
        {
            // Force recalculation of HEPA matrix
            currentHepaChromaFilter = -1;
            
            if (checkBoxAutoGenerate.Checked)
                ButtonGenerate_Click(this, null);
        }

        private Bitmap ResizeSource()
        {
            Bitmap bmp = openedBitmap;

            //if halfwidth fallback then double the width first
            if (checkBoxHalfWidthFallback.Checked)
            {
                Bitmap srcBmp = new Bitmap(bmp.Width * 2, bmp.Height);
                Graphics g2 = Graphics.FromImage(srcBmp);
                g2.InterpolationMode = InterpolationMode.NearestNeighbor;
                g2.DrawImage(bmp, new Rectangle(0, 0, srcBmp.Width, srcBmp.Height));
                bmp = srcBmp;
            }

            //resize to selected 32 or 40 char width
            float ratio = bmp.Width / ((float)TargetWidth * 2);
            srcHeight = (int)(bmp.Height / ratio);
            if (srcHeight > 224)
                srcHeight = 224;
            else
                srcHeight += srcHeight % 8;
            srcHeightChar = srcHeight / 8;
            Bitmap scaled = new Bitmap(TargetWidth * 2, srcHeight);
            Graphics g = Graphics.FromImage(scaled);
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.DrawImage(bmp, new Rectangle(0, 0, TargetWidth * 2, srcHeight));

            return scaled;
        }

        private void ButtonOpen_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                Bitmap bmp;
                try
                {
                    bmp = new Bitmap(Bitmap.FromFile(openFileDialog1.FileName));
                    openedBitmap = bmp;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Picture {openFileDialog1.FileName} cannot be open: {ex.Message}");
                    return;
                }
                pictureBoxSource.Image = ResizeSource();
            }
            pictureBoxSource.Size = pictureBoxSource.Image.Size;
            labelOutputSize.Text = $"Output: {TargetWidth * 2} * {srcHeight} ({srcHeightChar})";
            // Reset controls to default values
            trackBarGamma.Value = 10;
            trackBarLightness.Value = 0;
            trackBarSaturation.Value = 100;

            PictureToChannel(comboBoxAverMethod.SelectedIndex);
            CreatePal(comboBoxAverMethod.SelectedIndex);
            ReduceColors();
        }



        private void CentauriInit(int population)
        {
            alpaItems.Clear();
            AlpaItem zeroItem = new();
            Random rand = new Random();

            var item = myAlpaCollection[myAlpaCollection.Cursor];
            for (int i = 0; i < population; i++)
            {
                if (i > 0) //keep 0.iteration equal to colors currently set
                {
                    for (int j = 0; j <= COLORS; j++)
                    {
                        if (LOCKED_COLORS[j] == 1) { continue; }
                        item.Line0[j] = (byte)(((byte)rand.Next(255)) & 0xFE);
                        if (JOINED_COLORS[j] == 1)
                            item.Line1[j] = item.Line0[j]; //common color
                        else
                            item.Line1[j] = (byte)(((byte)rand.Next(255)) & 0xFE);
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
                ai.Line0 = (byte[])item.Line0.Clone();
                ai.Line1 = (byte[])item.Line1.Clone();
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
            var item = myAlpaCollection[myAlpaCollection.Cursor];
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
                    item.Line0[j] = (byte)(((byte)rand.Next(255)) & 0xFE);
                    if (JOINED_COLORS[j] == 1)
                        item.Line1[j] = item.Line0[j]; //common color
                    else
                        item.Line1[j] = (byte)(((byte)rand.Next(255)) & 0xFE);
                }

                AlpaItem ai = new AlpaItem();
                ai.Diff = long.MaxValue;
                ai.Ppdiff = int.MaxValue;
                ai.Line0 = (byte[])item.Line0.Clone();
                ai.Line1 = (byte[])item.Line1.Clone();
                alpaItems.Add(ai);
            }

            long bestDiff = long.MaxValue;
            //solutions.Clear(); //clear solutions
            //solutionsKeyUsed = 0;
            for (int i = 0; i < generations; i++)
            {
                CentauriGeneration(population);
                //labelSolutions.Text = $"Solutions:{solutions.Count}, tries:{((i+1)*population)}, keysUsed:{solutionsKeyUsed}";
                Array.Copy(alpaItems[0].Line0, item.Line0, item.Line0.Length);
                Array.Copy(alpaItems[0].Line1, item.Line1, item.Line1.Length);
                //line0 = alpaItems[0].line0;
                //line1 = alpaItems[0].line1;
                labelDiff.Text = $"Diff: {(int)alpaItems[0].Ppdiff}";
                
                // Update AlpaCollection when divider is unchecked to sync with global line0/line1
                if (myAlpaCollection != null && !checkBoxDividerEnabled.Checked)
                {
                    var (startY, endY) = GetVerticalRange();
                    UpdateAlpaCollectionForRange(myAlpaCollection, startY, endY);
                }
                
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

            var item = myAlpaCollection[myAlpaCollection.Cursor];

            for (int j = 0; j < 4; j++)
                for (int i = 0; i < population / 4; i++)
                {
                    AlpaItem ai = new AlpaItem();
                    Mutate(alpaItems[i].Line0, alpaItems[i].Line1, ai);
                    for (int k = 0; k < j; k++)
                        Mutate(ai.Line0, ai.Line1, ai);
                    Array.Copy(ai.Line0, item.Line0, item.Line0.Length);
                    Array.Copy(ai.Line1, item.Line1, item.Line1.Length);
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
            Random rng = GetThreadRandom();
            if (rng.Next(2) == 0)
            {
                line = (byte[])srcline0.Clone();
                lineSec = (byte[])srcline1.Clone();
            }
            else
            {
                line = (byte[])srcline1.Clone();
                lineSec = (byte[])srcline0.Clone();
            }
            int mutationType = rng.Next(6);
            int colorIndex = rng.Next(COLORS + 1); //choose colorindex to mutate
            while (LOCKED_COLORS[colorIndex] == 1)
                colorIndex = rng.Next(COLORS + 1);

            switch (mutationType)
            {
                case 0:
                case 1:
                case 2:
                case 3:
                    line[colorIndex] += shifts[mutationType];
                    break;
                case 4: //random
                    line[colorIndex] = (byte)(((byte)rng.Next(256)) & 0xfe);
                    break;
                case 5: //swap
                    int whichOne = rng.Next((COLORS + 1) * 2); //0-11
                    while (whichOne == colorIndex || //cannot swap with itself
                           LOCKED_COLORS[whichOne % (COLORS + 1)] == 1 || //cannot swap locked color
                           ((whichOne - (COLORS + 1) == colorIndex) && (JOINED_COLORS[colorIndex] == 1))  //cannot switch same joined colors 
                        )
                        whichOne = rng.Next((COLORS + 1) * 2); ;
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

            var item = myAlpaCollection[myAlpaCollection.Cursor];
            
            if (e.Y > COLORS * 16)
            {
                int line = (e.Y - COLORS * 16 - 1) / 16;
                int index = (e.X / 16) % remap.Length;


                switch (line)
                {
                    case 0:
                        atariColorPicker.Pick(line == 0 ? item.Line0[remap[index]] : item.Line1[remap[index]]);
                        if (atariColorPicker.PickedNewColor)
                        {
                            item.Line0[remap[index]] = atariColorPicker.PickedColorIndex;
                            if (joint[index] == 1)
                                item.Line1[remap[index]] = atariColorPicker.PickedColorIndex;
                        }
                        break;
                    case 1:
                        atariColorPicker.Pick(line == 0 ? item.Line0[remap[index]] : item.Line1[remap[index]]);
                        if (atariColorPicker.PickedNewColor)
                        {
                            item.Line1[remap[index]] = atariColorPicker.PickedColorIndex;
                            if (joint[index] == 1)
                                item.Line0[remap[index]] = atariColorPicker.PickedColorIndex;
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
                
                // Update AlpaCollection when divider is unchecked to sync with global line0/line1
                if (myAlpaCollection != null && !checkBoxDividerEnabled.Checked)
                {
                    var (startY, endY) = GetVerticalRange();
                    UpdateAlpaCollectionForRange(myAlpaCollection, startY, endY);
                }
                
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
            int pmgs = CharsPerLine == 32 ? 4 : 5;
            (long totalDiff, int[,] bitmapDataIndexed, int[,] charMask, int[,] pmgMask) = CalcDiff2(comboBoxDistance.SelectedIndex, Dither);
            byte[] vram = new byte[CharsPerLine * srcHeightChar];
            int height = charMask.GetLength(1);

            if (CharsPerLine == 32)
            {
                for (int y = 0; y < height; y++)
                    for (int x = 0; x < CharsPerLine; x++)
                        vram[x + y * CharsPerLine] = (byte)(x + (y % 4) * CharsPerLine + 128 * charMask[x, y]);

                for (int y = height; y < srcHeightChar; y++)
                    for (int x = 0; x < CharsPerLine; x++)
                        vram[x + y * CharsPerLine] = (byte)(x + (y % 4) * CharsPerLine);
            }
            else
            {
                //40 char mode
                for (int y = 0; y < height; y++)
                    for (int x = 0; x < CharsPerLine; x++)
                        vram[x + y * CharsPerLine] = (byte)(x + (y % 3) * CharsPerLine + 128 * charMask[x, y]);
            }
            int pointer = 0;
            byte[] remap = { 1, 2, 3, 0, 0 };
            int charsetLength = (int)Math.Ceiling(srcHeightChar / (CharsPerLine == 32 ? 4f : 3f)) * 1024;
            byte[] charsets = new byte[charsetLength];
            int pixelValue = 0;
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < CharsPerLine; x++)
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
                if (CharsPerLine == 40 && y % 3 == 2)
                    pointer += 64;  //skip unused 8 chars in 40char mode
            }

            //byte[] colors = new byte[9] { line0[1], line0[4], line0[0], line1[0], line0[2], line1[2], line0[3], line1[3], 0 };
            var item = myAlpaCollection[myAlpaCollection.Cursor];
            byte[] line0 = item.Line0;
            byte[] line1 = item.Line1;
            byte[] colors = new byte[11] { line0[0], line0[1], line0[2], line0[3], line1[0], line1[1], line1[2], line1[3], line0[4], line0[5], 0 };
            colors[10] = (byte)(checkBoxInterlace.Checked ? 1 : 0);

            byte[] pmdata = new byte[128 * pmgs];
            int topOffset = 16;
            for (int y = 0; y < height * 4; y++)
                for (int pm = 0; pm < pmgs; pm++)
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
            if (CharsPerLine == 32)
            {
                byte[] xex = File.ReadAllBytes(@"atari\alterline4.xex");
                Array.Copy(pmdata, 0, xex, 0x02b0, pmdata.Length);
                Array.Copy(charsets, 0, xex, 0x04b4, charsets.Length);
                Array.Fill(xex, (byte)0, 0x04b4 + charsets.Length, 32 * 24 * 8 - charsets.Length);
                Array.Copy(vram, 0, xex, 0x1cb8, vram.Length);
                Array.Copy(colors, 0, xex, 0x1fb8, colors.Length);
                File.WriteAllBytes("output.xex", xex);
            }
            else //40chars
            {
                /*
                byte[] xex = File.ReadAllBytes(@"atari\alterline4_40.xex");
                Array.Copy(pmdata, 0, xex, 0x02b0, pmdata.Length);
                Array.Copy(charsets, 0, xex, 0x04b4, charsets.Length);
                Array.Fill(xex, (byte)0, 0x04b4 + charsets.Length, 32 * 24 * 8 - charsets.Length);
                Array.Copy(vram, 0, xex, 0x1cb8, vram.Length);
                Array.Copy(colors, 0, xex, 0x1fb8, colors.Length);
                File.WriteAllBytes("output.xex", xex);
                */
            }
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
                var item = myAlpaCollection[myAlpaCollection.Cursor];
                Array.Copy(alpaItems[listViewPopulation.SelectedIndices[0]].Line0, item.Line0, item.Line0.Length);
                Array.Copy(alpaItems[listViewPopulation.SelectedIndices[0]].Line1, item.Line1, item.Line0.Length);
                
                // Update AlpaCollection when divider is unchecked to sync with global line0/line1
                if (myAlpaCollection != null && !checkBoxDividerEnabled.Checked)
                {
                    var (startY, endY) = GetVerticalRange();
                    UpdateAlpaCollectionForRange(myAlpaCollection, startY, endY);
                }
                
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
            if (checkBoxAutoGenerate.Checked)
                ButtonGenerate_Click(this, null);
        }

        private void ComboBoxDistance_SelectedIndexChanged(object sender, EventArgs e)
        {
            buttonAlpaCentauriAI.Enabled = false;
            if (checkBoxAutoGenerate.Checked)
                ButtonGenerate_Click(this, null);
        }

        private (int startY, int endY) GetVerticalRange()
        {
            if (!checkBoxDividerEnabled.Checked)
            {
                // If divider is not enabled, process the whole image
                return (0, srcHeight / 2 - 1);
            }

            int startY = (int)numericUpDownDividerStart.Value;
            int length = (int)numericUpDownDividerLength.Value;
            int endY = Math.Min(startY + length - 1, srcHeight / 2 - 1);
            
            // Ensure startY is within bounds
            startY = Math.Max(0, Math.Min(startY, srcHeight / 2 - 1));
            
            return (startY, endY);
        }

        private void UpdateAlpaCollectionForRange(AlpaCollection collection, int startY, int endY)
        {
            if (collection == null) return;
            if (startY < 0 || startY >= collection.Height) return;
            var srcItem = collection[collection.Cursor];
            // When divider is unchecked, update ALL lines with cursor line0/line1 values
            if (!checkBoxDividerEnabled.Checked)
            {
                // Update all lines with current global palette values
                for (int y = 0; y < collection.Height; y++)
                {
                    var item = collection[y];
                    Array.Copy(srcItem.Line0, item.Line0, Math.Min(srcItem.Line0.Length, item.Line0.Length));
                    Array.Copy(srcItem.Line1, item.Line1, Math.Min(srcItem.Line1.Length, item.Line1.Length));
                    // Recalculate customColors for the item after setting values
                    item.RecalculateCustomColors(atariPalRgb, comboBoxAverMethod.SelectedIndex, COLORS, checkBoxHepa.Checked);
                }
            }
            else
            {
                // Then copy from startY to all subsequent lines until endY
                for (int y = startY + 1; y <= endY && y < collection.Height; y++)
                {
                    var item = collection[y];
                    Array.Copy(srcItem.Line0, item.Line0, Math.Min(srcItem.Line0.Length, item.Line0.Length));
                    Array.Copy(srcItem.Line1, item.Line1, Math.Min(srcItem.Line1.Length, item.Line1.Length));
                    // Recalculate customColors for the item after copying
                    item.RecalculateCustomColors(atariPalRgb, comboBoxAverMethod.SelectedIndex, COLORS, checkBoxHepa.Checked);
                }
            }
        }

        private void ButtonGenerate_Click(object sender, EventArgs e)
        {
            // Get the vertical range to process
            var (startY, endY) = GetVerticalRange();

            // Initialize or update AlpaCollection with default palette if needed
            if (myAlpaCollection == null)
            {
                myAlpaCollection = new AlpaCollection(srcHeight / 2, COLORS + 1);
                PopulateDefaultPalette(myAlpaCollection);
            }

            // Update only the specified range in the AlpaCollection
            UpdateAlpaCollectionForRange(myAlpaCollection, startY, endY);
            RedrawAlpaCollection();

            CreatePal(comboBoxAverMethod.SelectedIndex, true);
            CreateIdealDitheredSolution(comboBoxDistance.SelectedIndex, comboBoxDither.SelectedIndex, (float)numericUpDownDitherStrength.Value / 10f);
            ButtonAlpaCentauriInit_Click(this, null);
        }

        private void RedrawAlpaCollection()
        {
            if (myAlpaCollection == null) return;
            int lineLength = myAlpaCollection[0].Line0.Length;
            int width = lineLength*2;
            int height = srcChannelMatrix.GetLength(1);
            Bitmap p = new Bitmap(width, height, PixelFormat.Format24bppRgb);
            
            BitmapData bd = p.LockBits(new Rectangle(0, 0, p.Width, p.Height), ImageLockMode.WriteOnly, p.PixelFormat);
            IntPtr ptr = bd.Scan0;
            int stride = Math.Abs(bd.Stride);
            int size = stride * bd.Height;
            byte[] pixels = new byte[size];
            
            for (int y = 0; y < height && y < myAlpaCollection.Height; y++)
            {
                var item = myAlpaCollection[y];
                int pixelOffset = y * stride;
                
                // Draw Line0 colors (left side)
                for (int x = 0; x < item.Line0.Length; x++)
                {
                    Color color = AtariColorToColor(item.Line0[x]);
                    int pixelIndex = pixelOffset + x * 3;
                    if (pixelIndex + 2 < pixels.Length)
                    {
                        pixels[pixelIndex] = color.B;
                        pixels[pixelIndex + 1] = color.G;
                        pixels[pixelIndex + 2] = color.R;
                    }
                }
                
                // Draw Line1 colors (right side)
                for (int x = 0; x < item.Line1.Length; x++)
                {
                    Color color = AtariColorToColor(item.Line1[x]);
                    int pixelIndex = pixelOffset + (lineLength + x) * 3;
                    if (pixelIndex + 2 < pixels.Length)
                    {
                        pixels[pixelIndex] = color.B;
                        pixels[pixelIndex + 1] = color.G;
                        pixels[pixelIndex + 2] = color.R;
                    }
                }
            }
            
            Marshal.Copy(pixels, 0, ptr, size);
            p.UnlockBits(bd);
            
            pictureBoxPaletteCollection.Image = new Bitmap(p);
            // pictureBoxPaletteCollection.Size = p.Size;
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
                        brVal /= 100 / 16;
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

        private void ComboBoxDither_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (checkBoxAutoGenerate.Checked)
                ButtonGenerate_Click(this, null);
        }

        private void NumericUpDownDitherStrength_ValueChanged(object sender, EventArgs e)
        {
            if (checkBoxAutoGenerate.Checked)
                ButtonGenerate_Click(this, null);
        }

        private void label11_Click(object sender, EventArgs e)
        {

        }

        private void ButtonImportColors_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                var item = myAlpaCollection[myAlpaCollection.Cursor];
                byte[] data = File.ReadAllBytes(openFileDialog1.FileName);
                if (data.Length != 11)
                {
                    MessageBox.Show("wrong file");
                    return;
                }
                Array.Copy(data, 0, item.Line0, 0, 4);
                Array.Copy(data, 4, item.Line1, 0, 4);
                Array.Copy(data, 8, item.Line0, 4, 2);
                Array.Copy(data, 8, item.Line1, 4, 2);
                CreatePal(comboBoxAverMethod.SelectedIndex, false);
                ButtonAlpaCentauriInit_Click(this, null);
                //ButtonGenerate_Click(this, e);
            }
        }

        private void ComboBoxCharsPerLine_SelectedIndexChanged(object sender, EventArgs e)
        {
            pictureBoxSource.Image = ResizeSource();
            pictureBoxSource.Size = pictureBoxSource.Image.Size;
            labelOutputSize.Text = $"Output: {TargetWidth * 2} * {srcHeight} ({srcHeightChar})";
            ApplyImageAdjustments();
        }
    }

}