using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NLog;

namespace Ffsti.Library
{
    public static class Logging
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public static void Trace(string message, params object[] args)
        {
            logger.Trace(string.Format(message, args));
            logger.Trace("-------------------------------------------------------------------------------------------------------------");
        }

        public static void Trace(string message, bool traco = true)
        {
            logger.Trace(message);

            if (traco)
                logger.Trace("-------------------------------------------------------------------------------------------------------------");
        }

        public static void Error(string message)
        {
            logger.Error(message);
        }

        public static void Debug(string message)
        {
            logger.Debug(message);
        }

        public static void Debug(Exception exception, bool logInnerException = false)
        {
            logger.Debug(exception.Message);
            logger.Debug(exception.StackTrace.ToString());

            if (exception.InnerException != null && logInnerException)
            {
                logger.Debug("Inner Exception");
                logger.Debug(exception.InnerException.Message);
                logger.Debug(exception.InnerException.StackTrace.ToString());
            }
        }

        public static void Error(Exception exception, bool logInnerException = false)
        {
            logger.Error(exception.Message);
            logger.Error(exception.StackTrace.ToString());

            if (exception.InnerException != null && logInnerException)
            {
                logger.Error("Inner Exception");
                logger.Error(exception.InnerException.Message);
                logger.Error(exception.InnerException.StackTrace.ToString());
            }
        }

        public static void Error(string message, params object[] args)
        {
            logger.Trace(string.Format(message, args));
        }

        public static void Warn(string message)
        {
            logger.Warn(message);
        }
    }
}
