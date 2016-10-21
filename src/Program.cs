using System;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using System.Threading;

namespace SupercellProxy
{
    class Program
    {
        private static void SetupConsole(string[] args)
        {
            // Parse console args
            new ConsoleArgs(args).Parse();

            // Console configurations
            Console.Title = "SupercellProxy - Let's play " + Config.Game.ReadableName() + "!";
            Console.InputEncoding = Encoding.UTF8;

            // Disable resizing/maximizing
            NativeCalls.DisableMenus();
        }

        /// <summary>
        /// Entry point
        /// </summary>
        static void Main(string[] args)
        {
            SetupConsole(args);           
            Proxy.Start();
        }

        /// <summary>
        /// Closes the program
        /// </summary>
        public static void Close()
        {
            Environment.Exit(0);
        }

        /// <summary>
        /// Waits a certain amount of seconds and closes the program
        /// </summary>
        public static void WaitAndClose(int ms = 1350)
        {
            Thread.Sleep(ms);
            Close();
        }

        /// <summary>
        /// Restarts the program
        /// </summary>
        public static void Restart()
        {
            Process.Start(Assembly.GetExecutingAssembly().Location);
            Close();
        }
    }
}