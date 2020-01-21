using System;
using System.Text;
using System.IO;

namespace HashBugTracker.Models
{
    public enum LogType
    {
        info, debug, error, console
    }

    public static class Logger
    {
        private static string infolog = "infolog.txt";
        private static string debuglog = "debuglog.txt";
        private static string errorlog = "errorlog.txt";
        private static string consoleLog = "consolelog.txt";

        private static void WriteLog(string msg, string filename)
        {
            try
            {
                using (StreamWriter w = File.AppendText(filename))
                {
                    w.WriteLine("[{0}] {1}", DateTime.Now, msg);
                }
            }
            catch (IOException e)
            {
                Console.WriteLine(e.Message);
            }

        }

        private static void Print(string msg, LogType logType)
        {
            string filename = "log.txt";
            switch (logType)
            {
                case LogType.info:
                    filename = infolog;
                    break;
                case LogType.debug:
                    filename = debuglog;
                    break;
                case LogType.error:
                    filename = errorlog;
                    break;
                case LogType.console:
                    filename = consoleLog;
                    break;
            }
            filename = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), @"HashBugTracker\" + filename);
            WriteLog(msg, filename);
        }

        public static void Debug(string message, params object[] paramList)
        {
            string er = "Debug";
            if (paramList != null && paramList.Length > 0) message = string.Format(message, paramList);
            Print(er + ": " + message, LogType.debug);
        }

        public static void Error(string message, params object[] paramList)
        {
            string er = "Error";
            Print(er + ": " + message, LogType.error);
        }

        public static void ErrorException(string message, Exception exception, params object[] paramList)
        {
            string er = "Error exception";
            Print(er + ": " + message, LogType.error);
        }

        public static void Fatal(string message, params object[] paramList)
        {
            string er = "Fatal";
            Print(er + ": " + message, LogType.error);
        }

        public static void FatalException(string message, Exception exception, params object[] paramList)
        {
            string er = "Fatal exception";
            Print(er + ": " + message, LogType.error);
        }

        public static void Info(string message, params object[] paramList)
        {
            string er = "Info";
            if (paramList != null) message = string.Format(message, paramList);
            Print(er + ": " + message, LogType.info);
        }

        public static void Log(string message, params object[] paramList)
        {
            string er = "Log";
            if (paramList != null) message = string.Format(message, paramList);
            Print(er + ": " + message, LogType.info);
        }

        public static void LogMultiline(string message, StringBuilder additionalContent)
        {
            string er = "Log multiline";
            Print(er + ": " + message, LogType.info);
        }

        public static void Warn(string message, params object[] paramList)
        {
            string er = "Warn";
            Print(er + ": " + message, LogType.debug);
        }

        public static void Cons(string message)
        {
            Print(message, LogType.console);
        }
    }
}
