using UnityEngine;
using System.IO;
using System;

namespace CityBuilder.Core.Logging
{
    public enum LogLevel
    {
        Verbose,
        Info,
        Warning,
        Error,
        Exception
    }

    /// <summary>
    /// Professional logging system wrapping Unity's Debug.
    /// Supports log levels, enabling/disabling, and future file logging.
    /// </summary>
    public static class GameLogger
    {
        public static bool IsLoggingEnabled { get; set; } = true;
        public static LogLevel MinimumLogLevel { get; set; } = LogLevel.Info;
        
        // Future readiness for file logging
        private static string _logFilePath;

        static GameLogger()
        {
            // Setup file path for future implementation
            _logFilePath = Path.Combine(Application.persistentDataPath, "session_log.txt");
        }

        public static void Verbose(string message) => Log(LogLevel.Verbose, message);
        public static void Info(string message) => Log(LogLevel.Info, message);
        public static void Warning(string message) => Log(LogLevel.Warning, message);
        public static void Error(string message) => Log(LogLevel.Error, message);
        public static void Exception(Exception e)
        {
            if (!IsLoggingEnabled) return;
            Debug.LogException(e);
            WriteToFile($"[EXCEPTION] {e.Message}\n{e.StackTrace}");
        }

        private static void Log(LogLevel level, string message)
        {
            if (!IsLoggingEnabled || level < MinimumLogLevel) return;

            string formattedMessage = $"[{level.ToString().ToUpper()}] {message}";

            switch (level)
            {
                case LogLevel.Warning:
                    Debug.LogWarning(formattedMessage);
                    break;
                case LogLevel.Error:
                    Debug.LogError(formattedMessage);
                    break;
                default:
                    Debug.Log(formattedMessage);
                    break;
            }

            WriteToFile(formattedMessage);
        }

        private static void WriteToFile(string text)
        {
            // Placeholder for actual async file writing
            // File.AppendAllText(_logFilePath, $"{DateTime.Now}: {text}\n");
        }
    }
}
