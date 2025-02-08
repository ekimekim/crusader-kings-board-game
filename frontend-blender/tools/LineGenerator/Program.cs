using System.Drawing.Imaging;

namespace LineGenerator
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Console.WriteLine("Loading");
            var territoriesImg = new Bitmap("../../../../../textures/territories_1024.png");
            Console.WriteLine("Generating");
            var textures = MapParser.GetTerritoryTextures(territoriesImg);
            Console.WriteLine("Saving");

            foreach(var (territory, img) in textures ) 
            {
                img.Save($"../../../../../textures/territories/{territory}_1024.png", ImageFormat.Png);
                Console.WriteLine($"Saved {territory}_1024.png");
            }
            Console.WriteLine("Finished");
            Console.WriteLine("Press any key to close");
            Console.ReadLine();
        }
    }
}