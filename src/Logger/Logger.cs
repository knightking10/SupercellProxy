using System;
using System.IO;
using System.Text;

namespace SupercellProxy
{
    enum LogType
    {
        INFO, // A normal text (i.e. "Proxy started")
        WARNING, // A warning (i.e. 2 running proxys)
        CONFIG, // A configuration value (i.e. "Host")
        PACKET, // A client/server packet (i.e. KeepAlive)
        API, // An API message (i.e. "WebAPI started")
        EXCEPTION // An exception (i.e. NullReferenceException)
    }

    class Logger
    {
        public static void CenterStr(string str)
        {
            // (window width - strlen) / 2 = center
            Console.SetCursorPosition((Console.WindowWidth - str.Length) / 2, Console.CursorTop);
            Console.WriteLine(str);
            // reset
            Console.SetCursorPosition(Console.CursorLeft, Console.CursorTop);
        }

        /// <summary>
        /// Logs passed text
        /// </summary>
        public static void Log(string text, LogType type = LogType.INFO)
        {
            // Print line out
            switch (type)
            {
                case LogType.INFO:
                    Console.ForegroundColor = ConsoleColor.Green;
                    break;
                case LogType.WARNING:
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    break;
                case LogType.EXCEPTION:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case LogType.API:
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    break;
                case LogType.PACKET:
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    break;
                case LogType.CONFIG:
                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                    break;
            }

            Console.Write("[" + type + "] ");
            Console.ResetColor();
            Console.WriteLine(text);

            // Log line to file
            string path = Environment.CurrentDirectory + @"\\Logs\\" + DateTime.UtcNow.ToLocalTime().ToString("dd-MM-yyyy") + ".log";

            // Make sure the Logs Directory exists to prevent crash.
            if (!Directory.Exists(Environment.CurrentDirectory + @"\\Logs\\"))
                Directory.CreateDirectory(Environment.CurrentDirectory + @"\\Logs\\");

            using (FileStream fs = new FileStream(path, FileMode.Append, FileAccess.Write, FileShare.ReadWrite))
            {
                using (StreamWriter StreamWriter = new StreamWriter(fs))
                {
                    StreamWriter.WriteLine("[" + DateTime.UtcNow.ToLocalTime().ToString("hh-mm-ss") + "-" + type + "] " + text);
                    StreamWriter.Close();
                }
            }
        }
    }
}
