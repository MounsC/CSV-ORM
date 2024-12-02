using System;
using System.IO;

namespace CsvOrm.Utils
{
    public static class Logger
    {
        private static readonly string logFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "log.txt");

        public static void Log(string message)
        {
            var logEntry = $"[{DateTime.Now}] INFO: {message}";
            Console.WriteLine(logEntry);
            File.AppendAllText(logFilePath, logEntry + Environment.NewLine);
        }

        public static void LogError(string message)
        {
            var logEntry = $"[{DateTime.Now}] ERROR: {message}";
            Console.Error.WriteLine(logEntry);
            File.AppendAllText(logFilePath, logEntry + Environment.NewLine);
        }
    }
}