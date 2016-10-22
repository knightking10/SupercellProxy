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

    static class Logger
    {
        // Path pointing to the .log file.
        private static readonly string s_logPath;

        static Logger()
        {
            if (!Directory.Exists("Logs"))
                Directory.CreateDirectory("Logs");

            string fileName = DateTime.UtcNow.ToLocalTime().ToString("dd-MM-yyyy") + ".log";
            s_logPath = Path.Combine("Logs", fileName);
        }

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

            // Append line to log file.
            var fs = new FileStream(s_logPath, FileMode.Append, FileAccess.Write, FileShare.ReadWrite);
            using (StreamWriter StreamWriter = new StreamWriter(fs))
                StreamWriter.WriteLine("[" + DateTime.UtcNow.ToLocalTime().ToString("hh-mm-ss") + "-" + type + "] " + text);
        }
    }
}

