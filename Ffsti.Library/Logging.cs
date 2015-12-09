using System;
using NLog;

namespace Ffsti.Library
{
    /// <summary>
    /// Class with logging support, using the NLog
    /// </summary>
    public static class Logging
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        //TODO: Restruturar para testes
        //public static void ConfigureMail(string to, string from, bool html, string header, string footer,
        //	string smtpPassword, int smtpPort, string smtpServer, string smtpUserName, bool enableSsl,
        //	NLog.Targets.SmtpAuthenticationMode smtpAuthenticationMode, string body,
        //	string mailTargetName, string mailRuleName, LogLevel minLogLevel)
        //{
        //	var emailTarget = new NLog.Targets.MailTarget();
        //	var config = new NLog.Config.LoggingConfiguration();

        //	config.AddTarget(mailTargetName, emailTarget);

        //	emailTarget.Use(t =>
        //	{
        //		t.To = to;
        //		t.From = from;
        //		t.Html = html;
        //		t.Header = header;
        //		t.Footer = footer;
        //		t.SmtpPassword = smtpPassword;
        //		t.ReplaceNewlineWithBrTagInHtml = html;
        //		t.SmtpPort = smtpPort;
        //		t.SmtpServer = smtpServer;
        //		t.SmtpUserName = smtpUserName;
        //		t.EnableSsl = enableSsl;
        //		t.SmtpAuthentication = smtpAuthenticationMode;
        //		t.Body = body;
        //	});

        //	var emailRule = new NLog.Config.LoggingRule(mailTargetName, minLogLevel, emailTarget);

        //	config.AddTarget(mailTargetName, emailTarget);
        //	config.LoggingRules.Add(emailRule);

        //	LogManager.Configuration = config;
        //	Logger = LogManager.GetCurrentClassLogger();
        //}

        /// <summary>
        /// Log a trace occurrence
        /// </summary>
        /// <param name="message">The message to be logged, using the string.Format formatting</param>
        /// <param name="args">The arguments to be used with the message</param>
        public static void Trace(string message, params object[] args)
        {
            Logger.Trace(string.Format(message, args));
        }

        /// <summary>
        /// Log a trace occurrence
        /// </summary>
        /// <param name="message">The message to be logged</param>
        public static void Trace(string message)
        {
            Logger.Trace(message);
        }

        /// <summary>
        /// Log a debug occurrence
        /// </summary>
        /// <param name="message">The message to be logged</param>
        public static void Debug(string message)
        {
            Logger.Debug(message);
        }

        /// <summary>
        /// Log a debug occurrence
        /// </summary>
        /// <param name="exception">The exception to be logged</param>
        /// <param name="logInnerException">If true, log all the inner exceptions</param>
        public static void Debug(Exception exception, bool logInnerException = false)
        {
            Logger.Debug(exception.Message);
            Logger.Debug(exception.StackTrace);

            if (exception.InnerException == null || !logInnerException) return;

            Logger.Debug("Inner Exception");
            Debug(exception.InnerException, logInnerException);
        }

        /// <summary>
        /// Log a error occurrence
        /// </summary>
        /// <param name="exception">The exception to be logged</param>
        /// <param name="logInnerException">If true, log all the inner exceptions</param>
        public static void Error(Exception exception, bool logInnerException = false)
        {
            var innerException = "";
            if (logInnerException)
            {
                innerException = ReadInnerException(exception);
            }

            Logger.Error(exception.Message + "\n" + innerException);
            Logger.Error(exception.StackTrace);

            if (exception.InnerException == null || !logInnerException) return;

            Logger.Error("Inner Exception");
            Error(exception.InnerException, logInnerException);
        }

        private static string ReadInnerException(Exception exception)
        {
            return exception.InnerException != null
                ? exception.InnerException.Message + ReadInnerException(exception)
                : "";
        }

        /// <summary>
        /// Log a error occurrence
        /// </summary>
        /// <param name="message">The message to be logged, using the string.Format formatting</param>
        /// <param name="args">The arguments to be used with the message</param>
        public static void Error(string message, params object[] args)
        {
            Logger.Trace(string.Format(message, args));
        }

        /// <summary>
        /// Log a error occurrence
        /// </summary>
        /// <param name="message">The message to be logged</param>
        public static void Error(string message)
        {
            Logger.Error(message);
        }

        public static void Error(string message, Exception ex)
        {
            Logger.Error(message, ex);
        }

        /// <summary>
        /// Log a warning occurrence
        /// </summary>
        /// <param name="message">The message to be logged</param>
        public static void Warn(string message)
        {
            Logger.Warn(message);
        }

        public static void Info(string message)
        {
            Logger.Info(message);
        }
    }
}
