using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTANonCrown.Data.Services
{
    using System;
    using System.Diagnostics;
    using System.IO;

    public static class AppLogger
    {
        private static readonly string LogFilePath = $"C://temp//log_{DateTime.Now:yyyyMMdd}.txt";

        public static void Log(string message, string? context = null)
        {
            try
            {
                var logMessage = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff} " +
                                 (context != null ? $"[{context}] " : "") +
                                 message;

                File.AppendAllText(LogFilePath, logMessage + Environment.NewLine);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Logging failed: {ex.Message}");
            }
        }

      
    }

}
