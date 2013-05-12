using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ASCII_lookup_table_generator;
using System.Drawing;

namespace LookupTableGenerator
{
    class Program
    {
        private const int PIXELS_WIDTH = 2;
        private const int PIXELS_HEIGHT = 3*PIXELS_WIDTH/2;

        static void Main(string[] args)
        {
            var character_set_brightness_data = CharacterSetProcessor.ProcessCharacters(PIXELS_WIDTH, PIXELS_HEIGHT, new System.Drawing.Font("Lucilda Console", CharacterBrightness.DEFAULT_CONSOLE_FONT_WIDTH));

            foreach (var char_data in character_set_brightness_data)
            {
                Console.WriteLine("'" + char_data.Key + "': ");

                for (uint i = 0; i < PIXELS_WIDTH; ++i)
                    for (uint j = 0; j < PIXELS_HEIGHT; ++j)
                        Console.WriteLine("  pixel[" + i + "][" + j + "] brightness: " + char_data.Value.pixel_brightness[i, j].ToString("0.0000") + " (" + (char_data.Value.pixel_brightness[i, j] * 100).ToString() + "%)");
            }

            var patterns = ConsoleToScreenPatternsGenerator.generate(PIXELS_WIDTH, PIXELS_HEIGHT);

            Console.ReadKey();
        }
    }
}
