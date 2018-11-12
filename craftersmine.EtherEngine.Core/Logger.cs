using System;
using System.Collections.Generic;
using System.IO;

namespace craftersmine.EtherEngine.Core
{
    /// <summary>
    /// Represents logging methods. This class cannot be inherited
    /// </summary>
    public sealed class Logger
    {
        private string _loadTime;
        private readonly string _file;
        /// <summary>
        /// Log entries
        /// </summary>
        public List<LogEntry> LogEntries { get; } = new List<LogEntry>();

        /// <summary>
        /// Creates new <see cref="Logger"/> instance
        /// </summary>
        /// <param name="logsRoot">Sets where logs will stored</param>
        /// <param name="name">Prefix of log file</param>
        public Logger(string logsRoot, string name)
        {
            string directory = logsRoot;
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);
            if (DateTime.Now.Hour.ToString().Length < 2)
                _loadTime = DateTime.Now.ToShortDateString() + "_0" + DateTime.Now.ToShortTimeString();
            else _loadTime = DateTime.Now.ToShortDateString() + "_" + DateTime.Now.ToShortTimeString();
            _file = Path.Combine(directory, name + "_" + _loadTime.Replace(':', '-') + ".log");
            File.WriteAllText(_file, "");
        }

        /// <summary>
        /// Writes new entry in log-file
        /// </summary>
        /// <param name="prefix">Line prefix</param>
        /// <param name="contents">Line contents</param>
        /// <param name="isOnlyConsole">If <code>true</code> log line will be only in console, else writes at file also</param>
        public void Log(LogEntryType prefix, string contents, bool isOnlyConsole = false)
        {
            string _date;
            if (DateTime.Now.Hour.ToString().Length < 2)
                _date = DateTime.Now.ToShortDateString() + " 0" + DateTime.Now.ToLongTimeString();
            else _date = DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToLongTimeString();
            string logLineCtor = _date + " [" + prefix.ToString().ToUpper() + "]" + " " + contents + "\r\n";
#if DEBUG
            if (!isOnlyConsole)
                File.AppendAllText(_file, logLineCtor);
            switch (prefix)
            {
                default:
                case LogEntryType.Info:
                case LogEntryType.Done:
                case LogEntryType.Success:
                case LogEntryType.Connection:
                    Console.ForegroundColor = ConsoleColor.Gray;
                    break;
                case LogEntryType.Error:
                case LogEntryType.Critical:
                case LogEntryType.Crash:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case LogEntryType.Warning:
                case LogEntryType.Unknown:
                case LogEntryType.Stacktrace:
                case LogEntryType.Debug:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
            }
            Console.Write(logLineCtor);
            LogEntry entry = new LogEntry { Contents = contents, EntryDateTime = _date, Type = prefix };
            LogEntries.Add(entry);
#else
            if (prefix != LogEntryType.Debug)
            {
                if (!isOnlyConsole)
                    File.AppendAllText(_file, logLineCtor);
                Console.Write(logLineCtor);
                LogEntry entry = new LogEntry { Contents = contents, EntryDateTime = _date, Type = prefix };
                LogEntries.Add(entry);
            }
#endif
            Console.ResetColor();
        }

        /// <summary>
        /// Writes exception data in log-file
        /// </summary>
        /// <param name="prefix">Lines prefixes</param>
        /// <param name="exception"><see cref="Exception"/> to write</param>
        public void LogException(LogEntryType prefix, Exception exception)
        {
            if (exception != null)
            {
                string[] stacktrace = exception.StackTrace.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);
                Log(prefix, "An exception has occurred!");
                Log(prefix, "Exception message: " + exception.Message);
                Log(prefix, "Exception type: " + exception.GetType().ToString());
                Log(prefix, "==== START OF STACKTRACE ====");
                foreach (var stln in stacktrace)
                {
                    Log(prefix, stln);
                }
                Log(prefix, "====  END OF STACKTRACE  ====");
            }
            else
            {
                Log(prefix, "An exception has occurred!");
                Log(prefix, "Exception was null! No additional information!");
            }
        }
    }

    /// <summary>
    /// Represents log entry with data
    /// </summary>
    public struct LogEntry
    {
        /// <summary>
        /// Prefix of log entry
        /// </summary>
        public LogEntryType Type { get; set; }
        /// <summary>
        /// Contents of log entry
        /// </summary>
        public string Contents { get; set; }
        /// <summary>
        /// Date and time of entry creation
        /// </summary>
        public string EntryDateTime { get; set; }
    }

    /// <summary>
    /// Log entry prefixes
    /// </summary>
    public enum LogEntryType
    {
        /// <summary>
        /// Error message
        /// </summary>
        Error = 1,
        /// <summary>
        /// Info message
        /// </summary>
        Info = 0,
        /// <summary>
        /// Warning message
        /// </summary>
        Warning = 2,
        /// <summary>
        /// Critical message
        /// </summary>
        Critical = 5,
        /// <summary>
        /// Network connection info, error, warning message
        /// </summary>
        Connection = 7,
        /// <summary>
        /// Successful operation message
        /// </summary>
        Success = 6,
        /// <summary>
        /// Operation done message
        /// </summary>
        Done = 3,
        /// <summary>
        /// Debug message
        /// </summary>
        Debug = 10,
        /// <summary>
        /// Stacktrace message
        /// </summary>
        Stacktrace = 4,
        /// <summary>
        /// Unknown message
        /// </summary>
        Unknown = 9,
        /// <summary>
        /// Application crash message
        /// </summary>
        Crash = 8
    }
}

