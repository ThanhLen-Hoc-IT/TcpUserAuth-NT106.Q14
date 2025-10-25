using System;
using System.IO;

namespace ServerApp
{
    internal class Logger
    {
        private static readonly object _lock = new object();
        private static readonly string _logFile = "server_log.txt";

        public static void Info(string message)
        {
            Write("INFO", message);
        }

        public static void Error(string message)
        {
            Write("ERROR", message);
        }

        public static void Warning(string message)
        {
            Write("WARN", message);
        }

        private static void Write(string level, string message)
        {
            string line = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] [{level}] {message}";
            Console.WriteLine(line); // hiển thị trên console nếu có

            lock (_lock)
            {
                File.AppendAllText(_logFile, line + Environment.NewLine);
            }
        }
    }
}
