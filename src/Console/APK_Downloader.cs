using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace SupercellProxy
{
    class APK_Downloader
    {
        private static WebClient wc;
        private static Game DesiredGame;

        /// <summary>
        /// Starts the APK downloader
        /// </summary>
        public static void Start()
        { 
            Console.WriteLine("STEP 1: SELECT YOUR DESIRED GAME CLIENT");
            Console.WriteLine("  A) Clash Royale");
            Console.WriteLine("  B) Clash of Clans");
            Console.WriteLine("  C) Boom Beach");
            Console.Write("> ");

            switch(Console.ReadKey().Key)
            {
                case ConsoleKey.A:
                    DesiredGame = Game.CLASH_ROYALE;
                    break;
                case ConsoleKey.B:
                    DesiredGame = Game.CLASH_OF_CLANS;
                    break;
                case ConsoleKey.C:
                    DesiredGame = Game.BOOM_BEACH;
                    break;
                default:
                    Program.WaitAndClose();
                    break;
            }

            Console.Clear();
            Console.WriteLine("STEP 2: DOWNLOADING PROCESS");

            try
            {
                wc = new WebClient();

                Console.WriteLine("Downloading..");
                if (DesiredGame == Game.CLASH_ROYALE)
                    wc.DownloadFile(new Uri("https://infinitymodding.net/projects/supercellproxy/modded_clash_royale.apk"), @"Gameclients\\Clash Royale.apk");
                else if (DesiredGame == Game.CLASH_OF_CLANS)
                    wc.DownloadFile(new Uri("https://infinitymodding.net/projects/supercellproxy/modded_clash_of_clans.apk"), @"Gameclients\\Clash of Clans.apk");
                else
                    wc.DownloadFile(new Uri("https://infinitymodding.net/projects/supercellproxy/modded_boom_beach.apk"), @"Gameclients\\Boom Beach.apk");

                Console.WriteLine(DesiredGame + " APK downloaded.");
                Program.WaitAndClose();
            }
            catch(Exception ex)
            {
                Console.WriteLine("Failed to download file (" + ex.GetType() + ")!");
                Console.WriteLine("Please check your internet connection.");
                Program.WaitAndClose();
            }
        }
    }
}
