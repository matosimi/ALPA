using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing.Imaging;
using System.Drawing;
using System.IO;

namespace AtariMapMaker
{
    [Serializable]
    public static class AtariPalette
    {
        [NonSerialized]
        private static readonly ColorPalette myPalette;

        static AtariPalette()
        {
            Bitmap bmp = new Bitmap(1, 1, PixelFormat.Format8bppIndexed);
            myPalette = bmp.Palette;
            bmp.Dispose();
        }

        public static ColorPalette GetPalette()
        {
            return myPalette;
        }

        /// <summary>
        /// This palette is used in AtariFontRenderer to show the font data, where there are taken 5 atari default colors and moved to palette indices 0..4.
        /// This way it helps to easily translate font data color (0..4) to given dli color just by indexing without matching.
        /// </summary>
        /// <returns></returns>
        /*public static ColorPalette GetIndexedColor5Palette()
        {
            Bitmap bmp = new Bitmap(1, 1, PixelFormat.Format8bppIndexed);
            ColorPalette pal = bmp.Palette;
            bmp.Dispose();
            for (int i = 0; i < 5; i++)
                pal.Entries[i] = myPalette.Entries[AtariFontRenderer.Color5[i]];
            return pal;
        }*/
        public static Color GetColor(int index)
        {
            return myPalette.Entries[index];
        }

        public static void Load(byte[] rawdata)
        {
            for (int a = 0; a < 256; a++)
                myPalette.Entries[a] = Color.FromArgb(255, rawdata[a * 3], rawdata[a * 3 + 1], rawdata[a * 3 + 2]);
        }
    }
}
