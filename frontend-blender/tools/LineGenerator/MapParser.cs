using LineGenerator.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace LineGenerator
{
    public class MapParser
    {
        public static Dictionary<string, Bitmap> GetTerritoryTextures(Bitmap img)
        {
            var colorsToTerritories = TerritoryColors.ColorsHex.ToDictionary(pair => ColorTranslator.FromHtml(pair.Value), pair => pair.Key);
            var territoryTextures = TerritoryColors.ColorsHex.ToDictionary(pair => pair.Key, pair => new Bitmap(img.Width, img.Height));

            for (int x = 0; x < img.Width; x++)
            {
                for (int y = 0; y < img.Height; y++)
                {
                    var color = img.GetPixel(x, y);
                    string territory;

                    if(colorsToTerritories.TryGetValue(color, out territory))
                    {
                        territoryTextures[territory].SetPixel(x, y, Color.White);
                    }
                }
            }

            return territoryTextures;
        }
    }

    public struct Pixel
    {
        public int x;
        public int y;
        public bool Neighbours;
    }
}
