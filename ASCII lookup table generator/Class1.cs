using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

using System.Threading.Tasks;
using System.Collections.Specialized;
using System.IO;

namespace ASCII_lookup_table_generator
{
    public class CharacterBrightness
    {
        public char character { get; private set; }
        public float[,] pixel_brightness { get; private set; }
        public uint pixels_count_x { get; private set; }
        public uint pixels_count_y { get; private set; }
        public Font font { get; private set; }

        public uint pixel_width  { get { return DEFAULT_CONSOLE_FONT_WIDTH / pixels_count_x; } }
        public uint pixel_height { get { return DEFAULT_CONSOLE_FONT_HEIGHT / pixels_count_y; } }

        public const int DEFAULT_CONSOLE_FONT_WIDTH  = 8;
        public const int DEFAULT_CONSOLE_FONT_HEIGHT = 12;


        public CharacterBrightness(char character, uint char_pixels_width, uint char_pixels_height, Font font)
        {
            this.character = character;
            this.font = font;
            pixels_count_x = char_pixels_width;
            pixels_count_y = char_pixels_height;

            pixel_brightness = new float[pixels_count_x, pixels_count_y];

            _process_char();
        }

        private Bitmap _make_char_bitmap()
        {
            var bitmap = new Bitmap(DEFAULT_CONSOLE_FONT_WIDTH, DEFAULT_CONSOLE_FONT_HEIGHT,System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            using (var gfx = Graphics.FromImage(bitmap))
            {
                gfx.Clear(Color.White);
                gfx.DrawString(character.ToString(), font, new SolidBrush(Color.Black), new PointF(0, 0));
                gfx.Flush();
            }

            return bitmap;
        }

        private void _process_char()
        {
            var bitmap = _make_char_bitmap();
            Color color;

            for (int x = 0; x < DEFAULT_CONSOLE_FONT_WIDTH; ++x)
                for (int y = 0; y < DEFAULT_CONSOLE_FONT_HEIGHT; ++y)
                {
                    color = bitmap.GetPixel(x, y);

                    if ( color.R != 255 || color.G != 255 || color.B != 255 )
                        pixel_brightness[x / pixel_width, y / pixel_height]++;
                }

            for (int x = 0; x < pixels_count_x; ++x)
                for (int y = 0; y < pixels_count_y; ++y)
                    pixel_brightness[x, y] /= pixel_width * pixel_height;

            bitmap.Dispose();
        }


    }

    public struct LookupTableItem
    {
        public char character { get; private set; }
        public int index      { get; private set; }
        public bool[,] pixels { get; private set; }

        public LookupTableItem(char character , int index , bool[,] pixels)
        {
            this.character = character;
            this.index = index;
            this.pixels = pixels;
        }
    }

    public class LookupTableGenerator
    {
        public static const string OUTPUTPATH_NOOUTPUT = "";

        public static bool IsASCII(char c) { return (uint)c < 128; }

        public static Dictionary<char,CharacterBrightness> ProcessCharacters(uint character_pixels_width, uint character_pixels_height, Font font)
        {
            var character_list = new Dictionary<char,CharacterBrightness>();

            for (Char c = Char.MinValue; (int)c < 128; ++c)
                if (!Char.IsControl(c) && !character_list.ContainsKey((char)c))
                    character_list.Add((char)c, new CharacterBrightness((char)c, character_pixels_width, character_pixels_height,font));

            return character_list;
        }

        public static bool[][,] GenerateConsoleToScreenPatterns(uint pixels_count_x, uint pixels_count_y)
        {
            List<bool[,]> patterns = new List<bool[,]>();

            for (Int32 i = 0; i < Math.Pow(2, pixels_count_x * pixels_count_y); ++i)
            {
                var pattern = new bool[pixels_count_x,pixels_count_y];
                var bitset = new BitArray(new int[] { i });

                for (uint x = 0; x < pixels_count_x; ++x)
                    for (uint y = 0; y < pixels_count_y; ++y)
                        pattern[x, pixels_count_y - 1 - y] = bitset.Get((int)(x + (y * pixels_count_y)));

                patterns.Add(pattern);
            }

            return patterns.ToArray();
        }

        public static LookupTableItem[] GenerateItems(Dictionary<char, CharacterBrightness> brightness_char_set_data, bool[][,] patterns)
        {
            return null; /* TODO */
        }

        public static string GenerateCode(uint pixels_count_x, uint pixels_count_y, Font font, string output_path = OUTPUTPATH_NOOUTPUT)
        {
            int TOTAL_PIXEL_PATTERNS = (int)Math.Pow(2,pixels_count_x*pixels_count_y);

            string code = "/*********************************************************" + "\n" +
                          "* ASCII raster character loockup table                   *" + "\n" +
                          "*                                                        *" + "\n" +
                          "* Character dimensions: " + pixels_count_x + "x" + pixels_count_y + " pixels *" + "\n" +
                          "*********************************************************/" + "\n" + "\n" +
                          "const char _pixels_to_char_lookup_table[" + TOTAL_PIXEL_PATTERNS + "] = { ";

            foreach (var item in GenerateItems(ProcessCharacters(pixels_count_x, pixels_count_y, font), GenerateConsoleToScreenPatterns(pixels_count_x, pixels_count_y)))
            {
                code += item.character + " , ";
            }

            code += "};";

            if (output_path != OUTPUTPATH_NOOUTPUT)
            {
                var writer = File.CreateText(output_path);

                writer.WriteLine(code);

                writer.Flush();
                writer.Close();
            }

            return code;
        }
    }
}
