namespace AlterLinePictureAproximator
{
    using ColorSpace;
    using System.Drawing;
    using System.Numerics;

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
        public Color[] customColors;
        public byte[] line0 = new byte[5] { 0x1e, 0x16, 0x26, 0x76, 0x00 };
        public byte[] line1 = new byte[5] { 0x14, 0x16, 0x48, 0xb6, 0x00 };
        public Color[] cline0 = new Color[4];
        public Color[] cline1 = new Color[4];

        private void Form1_Load(object sender, EventArgs e)
        {
            /*Vector4 rgb = new Vector4(30/255f, 60/255f, 90/255f, 0);
            Vector4 lab = Conversion.RGBToLab(rgb);
            Vector4 rgb2 = Conversion.LabToRGB(lab);*/
            LoadPalette();
            CreatePal(false);
        }

        private Color AverageColor(int c1, int c2)
        {
            double r2 = (Math.Pow((int)atariPalRgb[c1 * 3], 2) + Math.Pow((int)atariPalRgb[c2 * 3], 2)) / 2;
            double g2 = (Math.Pow((int)atariPalRgb[c1 * 3 + 1], 2) + Math.Pow((int)atariPalRgb[c2 * 3 + 1], 2)) / 2;
            double b2 = (Math.Pow((int)atariPalRgb[c1 * 3 + 2], 2) + Math.Pow((int)atariPalRgb[c2 * 3 + 2], 2)) / 2;
            return Color.FromArgb((int)double.Floor(Math.Sqrt(r2)), (int)double.Floor(Math.Sqrt(g2)), (int)double.Floor(Math.Sqrt(b2)));


            /*Vector4 cv1 = Rgb2Vector(atariPalRgb[c1 * 3], atariPalRgb[c1 * 3+1], atariPalRgb[c1 * 3+2]);
            Vector4 cv2 = Rgb2Vector(atariPalRgb[c2 * 3], atariPalRgb[c2 * 3 + 1], atariPalRgb[c2 * 3 + 2]);
            Vector4 cl1 = ColorSpace.Conversion.RGBToLab(cv1);
            Vector4 cl2 = ColorSpace.Conversion.RGBToLab(cv2);
            Vector4 nc = new Vector4((cl1.X+cl2.X)/2,(cl1.Y+cl2.Y)/2,(cl1.Z+cl2.Z)/2,0);
            Vector4 ncrgb = ColorSpace.Conversion.LabToRGB(cl1); // nc);

            return Color.FromArgb((int)(ncrgb.X * 255), (int)(ncrgb.Y * 255), (int)(ncrgb.Z * 255));
            */
            /*
            int r = ((int)atariPalRgb[c1 * 3] + (int)atariPalRgb[c2 * 3]) / 2;
            int g = ((int)atariPalRgb[c1 * 3 + 1] + (int)atariPalRgb[c2 * 3 + 1]) / 2;
            int b = ((int)atariPalRgb[c1 * 3 + 2] + (int)atariPalRgb[c2 * 3 + 2]) / 2;

            //return Color.FromArgb(Math.Abs(atariPalRgb[c1] - atariPalRgb[c2]), Math.Abs(atariPalRgb[c1 + 256] - atariPalRgb[c2 + 256]), Math.Abs(atariPalRgb[c1 + 2 * 256] - atariPalRgb[c2 + 2 * 256]));
            return Color.FromArgb(r, g, b);*/
        }

        private void CreatePal(bool inverse)
        {
            Bitmap p = new Bitmap(128, 200);
            Graphics g = Graphics.FromImage(p);
            customColors = new Color[16];
            List<byte> l0 = line0.ToList();
            List<byte> l1 = line1.ToList();

            l0.RemoveAt(3 - (inverse ? 1 : 0));
            l1.RemoveAt(3 - (inverse ? 1 : 0));

            for (int a = 0; a < 4; a++)
            {
                for (int b = 0; b < 4; b++)
                {
                    customColors[a * 4 + b] = AverageColor(l0[a], l1[b]);
                    g.FillRectangle(new SolidBrush(customColors[a * 4 + b]), new Rectangle(b * 32, a * 32, 32, 32));
                }
                cline0[a] = AverageColor(l0[a], l0[a]);
                cline1[a] = AverageColor(l1[a], l1[a]);
                g.FillRectangle(new SolidBrush(cline0[a]), new Rectangle(a * 32, 128 + 8, 32, 32));
                g.FillRectangle(new SolidBrush(cline1[a]), new Rectangle(a * 32, 128 + 32 + 8, 32, 32));
            }
            pictureBox4.Image = p;
        }
        private void LoadPalette()
        {
            atariPalRgb = File.ReadAllBytes("altirraPAL.pal");

        }

        private void button1_Click(object sender, EventArgs e)
        {
            DoConvert();
        }

        public static int Clamp(int value, int min, int max)
        {
            return (value < min) ? min : (value > max) ? max : value;
        }

        public static int Clamp256(int value)
        {
            return (value < 0) ? 0 : (value > 255) ? 255 : value;
        }
        private void DoConvert()
        {
            Bitmap b = (Bitmap)pictureBox1.Image;
            Bitmap t = new Bitmap(b.Width * 2, b.Height);
            Bitmap a = new Bitmap(b.Width * 2, b.Height);
            int[,,] delta_matrix = new int[b.Width + 1, b.Height + 1, 3];

            for (int y = 0; y < pictureBox1.Image.Height / 2; y++)
            {
                for (int x = 1; x < pictureBox1.Image.Width - 1; x++)
                {
                    Color src = b.GetPixel(x, y * 2);
                    int[] channel = new int[3]{src.R + delta_matrix[x,y,0],
                                               src.G + delta_matrix[x,y,1],
                                               src.B + delta_matrix[x,y,2]};
                    Color srcPlusDelta = Color.FromArgb(Clamp256(channel[0]), Clamp256(channel[1]), Clamp256(channel[2]));
                    int index = FindCustomClosest2(srcPlusDelta);   //use "src" to disable error diffusion
                    // 1/2*|# 1|
                    //     |1 0|

                    // 1/4*|- # 2|
                    //     |1 1 0|

                    // 1/16*|- # 7|
                    //      |3 5 1|
                    int[] delta = new int[3] { src.R - customColors[index].R + (channel[0] - Clamp256(channel[0])),
                                               src.G - customColors[index].G + (channel[1] - Clamp256(channel[1])),
                                               src.B - customColors[index].B + (channel[2] - Clamp256(channel[2]))};
                    for (int i = 0; i < 3; i++)
                    {
                        //chess
                        if ((x + y) % 2 != 0)
                        {
                            delta_matrix[x + 1, y, i] = delta[i] / 2;
                            delta_matrix[x, y + 1, i] = delta[i] / 2;
                        }
                        /*delta_matrix[x + 1,y  ,i] = delta[i] / 2;
                        delta_matrix[x + 1,y+1,i] = delta[i]/4 ;
                        delta_matrix[x    ,y+1,i] = delta[i] / 4;*/

                        //f-s
                        delta_matrix[x + 1, y, i] = (int)float.Floor((delta[i] / 16.0f) * 7);
                        delta_matrix[x - 1, y + 1, i] = (int)float.Floor((delta[i] / 16.0f) * 3);
                        delta_matrix[x, y + 1, i] = (int)float.Floor((delta[i] / 16.0f) * 5);
                        delta_matrix[x + 1, y + 1, i] = (int)float.Floor((delta[i] / 16.0f) * 1);
                    }

                    Color p1 = cline0[index / 4];
                    Color p2 = cline1[index % 4];

                    a.SetPixel(x * 2, y * 2, p1);
                    a.SetPixel(x * 2 + 1, y * 2, p1);
                    a.SetPixel(x * 2, y * 2 + 1, p2);
                    a.SetPixel(x * 2 + 1, y * 2 + 1, p2);

                    t.SetPixel(x * 2, y * 2, customColors[index]);
                    t.SetPixel(x * 2 + 1, y * 2, customColors[index]);
                    t.SetPixel(x * 2, y * 2 + 1, customColors[index]);
                    t.SetPixel(x * 2 + 1, y * 2 + 1, customColors[index]);
                }
                /*for (int x = 1; x < pictureBox1.Image.Width - 1; x++)
                {
                    for (int i = 0; i < 3; i++)
                        delta_line[x, i] = 0;
                }*/
            }
            pictureBox2.Image = t;
            pictureBox3.Image = a;
        }


        private int FindCustomClosest2(Color color)
        {
            int index = 0;
            double maxdist = double.MaxValue;
            for (int i = 0; i < 16; i++)
            {
                double dist = RGByuvDistance(color, customColors[i]);

                if (dist < maxdist)
                {
                    maxdist = dist;
                    index = i;
                }
            }
            return index;
        }

        private static double Difference(Vector4 vct1, Vector4 vct2)
        {
            return Math.Sqrt(Math.Pow(vct1.X - vct2.X, 2) + Math.Pow(vct1.Y - vct2.Y, 2) + Math.Pow(vct1.Z - vct2.Z, 2));
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
            CreatePal(checkBox1.Checked);
        }
    }
}