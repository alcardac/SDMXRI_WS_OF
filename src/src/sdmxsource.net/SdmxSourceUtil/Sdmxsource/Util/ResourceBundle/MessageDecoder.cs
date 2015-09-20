// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MessageDecoder.cs" company="Eurostat">
//   Date Created : 2013-08-19
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Util.ResourceBundle
{
    #region Using directives

    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Resources;

    using log4net;

    using Org.Sdmxsource.Sdmx.Api.Exception;
    using Org.Sdmxsource.Sdmx.Api.Util;
    using Org.Sdmxsource.Sdmx.Util.Objects.Container;

    #endregion

    /// <summary>
    /// TODO
    /// </summary>
    public class MessageDecoder : IMessageResolver
    {
        #region Static Fields

        /// <summary>
        /// The _log
        /// </summary>
        private static readonly ILog _log = LogManager.GetLogger(typeof(MessageDecoder));

        private static ResourceManager _messageSource;

        private static CultureInfo _loc = new CultureInfo("en");

        #endregion


        #region Public Methods and Operators

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageDecoder"/> class
        /// </summary>
        public MessageDecoder() 
        {
            SdmxException.SetMessageResolver(this);

            _messageSource = ExceptionMessages.ResourceManager;

        }

        #endregion


        #region Public Properties

        /// <summary>
        /// The _messageSource
        /// </summary>
        public static ResourceManager MessageSource
        {
            get
            {
                return _messageSource;
            }
        }

        #endregion


        #region Public Methods and Operators

        /// <summary>
        /// Add a new base name
        /// </summary>
        /// <param name="baseName">
        /// The base name
        /// </param>
        public static void AddBaseName(string baseName) 
        {
        }	

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="id">
        /// The id
        /// </param>
        /// <param name="args">
        /// The args
        /// </param>
        /// <returns>
        /// The message
        /// </returns>
        public static string DecodeMessage(string id, params object[] args) 
        {

            if(_messageSource == null) 
            {
                return id;
            }

            return string.Format(GetString(id, _loc), args);
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="id">
        /// The id
        /// </param>
        /// <param name="args">
        /// The args
        /// </param>
        /// <returns>
        /// The message
        /// </returns>
        public static string DecodeMessageDefaultLocale(string id, params object[] args) 
        {
            return string.Format(GetString(id, _loc), args);
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="id">
        /// The id
        /// </param>
        /// <param name="lang">
        /// The lang
        /// </param>
        /// <param name="args">
        /// The args
        /// </param>
        /// <returns>
        /// The message
        /// </returns>
        public static string DecodeMessageGivenLocale(string id, string lang, params object[] args) 
        {

            if(_messageSource == null) 
            {
                return id;
            }

            var format = GetString(id, _loc);
            return string.Format(format, args);
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="messageCode">
        /// The message code
        /// </param>
        /// <param name="locale">
        /// The locale
        /// </param>
        /// <param name="args">
        /// The args
        /// </param>
        /// <returns>
        /// The message
        /// </returns>
        public string ResolveMessage(string messageCode, CultureInfo locale, params object[] args)
        {
            string format = null;
            try
            {
                format = GetString(messageCode, locale);
                if (args != null && args.Length > 0)
                {
                    if (format != null)
                    {
                        return string.Format(format, args);
                    }

                    return messageCode + " - " + string.Join(" - ", args.Where(o => o != null).Select(o => o.ToString()));
                }
                
                if (format != null)
                {
                    return format;
                }

                return messageCode;
            }
            catch (FormatException th)
            {
                _log.ErrorFormat(CultureInfo.InvariantCulture, "{0} - {1} - {2} - {3}", messageCode, format, locale, args);
                _log.Error("While ResolveMessage", th);
                return string.Format("{0} - {1}", messageCode, format ?? string.Empty);
            }
            catch (Exception th) 
            {
                _log.ErrorFormat(CultureInfo.InvariantCulture, "{0} - {1} - {2} - {3}", messageCode, format, locale, args);
                _log.Error("While ResolveMessage", th);
                return messageCode;
            }
        }

        /// <summary>
        /// Add new basenames in the basename list
        /// </summary>
        /// <param name="basenames">
        /// The base names
        /// </param>
        public void SetBasenames(ISet<string> basenames) 
        {
        }

        #endregion

        /// <summary>
        /// Gets the string.
        /// </summary>
        /// <param name="messageCode">The message code.</param>
        /// <param name="locale">The locale.</param>
        /// <returns>the message for this locale;otherwise null.</returns>
        private static string GetString(string messageCode, CultureInfo locale)
        {
            var name = "ID" + messageCode;
            if (locale == null)
            {
                return ExceptionMessages.ResourceManager.GetString(name);
            }

            return ExceptionMessages.ResourceManager.GetString(name, locale);
        }
    }
}

