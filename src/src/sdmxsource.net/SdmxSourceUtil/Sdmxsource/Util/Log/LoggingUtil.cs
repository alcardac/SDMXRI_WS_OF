// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LoggingUtil.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Util.Log
{
    using log4net;

    /// <summary>
    ///     The logging util.
    /// </summary>
    /// TODO remove this as direct use of log4net is better.
    public class LoggingUtil
    {
        #region Public Methods and Operators

        /// <summary>
        /// The debug.
        /// </summary>
        /// <param name="log">
        /// The log.
        /// </param>
        /// <param name="message">
        /// The message.
        /// </param>
        public static void Debug(ILog log, string message)
        {
            if (log != null && log.IsDebugEnabled)
            {
                log.Debug(message);
            }
        }

        /// <summary>
        /// The error.
        /// </summary>
        /// <param name="log">
        /// The log.
        /// </param>
        /// <param name="message">
        /// The message.
        /// </param>
        public static void Error(ILog log, string message)
        {
            if (log != null)
            {
                log.Error(message);
            }
        }

        /// <summary>
        /// The info.
        /// </summary>
        /// <param name="log">
        /// The log.
        /// </param>
        /// <param name="message">
        /// The message.
        /// </param>
        public static void Info(ILog log, string message)
        {
            if (log != null && log.IsInfoEnabled)
            {
                log.Info(message);
            }
        }

        /// <summary>
        /// The warn.
        /// </summary>
        /// <param name="log">
        /// The log.
        /// </param>
        /// <param name="message">
        /// The message.
        /// </param>
        public static void Warn(ILog log, string message)
        {
            if (log != null)
            {
                log.Warn(message);
            }
        }

        #endregion
    }
}