// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LocaleUtil.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Util.Objects
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;

    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Util;

    /// <summary>
    ///     The locale util.
    /// </summary>
    //// TODO Not used anywhere. Do we need this ?
    public class LocaleUtil
    {
        // TODO should this be user defined?
        #region Static Fields

        /// <summary>
        ///     The default locales.
        /// </summary>
        private static IList<CultureInfo> _defaultLocales = new List<CultureInfo> { CultureInfo.GetCultureInfo("en") };

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets the default locales.
        /// </summary>
        public static IList<CultureInfo> DefaultLocales
        {
            get
            {
                return _defaultLocales;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The build local map.
        /// </summary>
        /// <param name="textTypes">
        /// The text types.
        /// </param>
        /// <returns>
        /// The local map
        /// </returns>
        public static IDictionary<CultureInfo, string> BuildLocalMap(IList<ITextTypeWrapper> textTypes)
        {
            if (textTypes == null)
            {
                return null;
            }

            IDictionary<CultureInfo, string> buildLocalMap = new Dictionary<CultureInfo, string>();

            /* foreach */
            foreach (ITextTypeWrapper currentTextType in textTypes)
            {
                string lang = currentTextType.Locale;
                string valueRen = currentTextType.Value;
                var loc = new CultureInfo(lang);
                buildLocalMap.Add(loc, valueRen);
            }

            return buildLocalMap;
        }

        /// <summary>
        /// The get string by default locale.
        /// </summary>
        /// <param name="localToStringMap">
        /// The local to string map.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// Throws ArgumentException
        /// </exception>
        public static string GetStringByDefaultLocale(IDictionary<CultureInfo, string> localToStringMap)
        {
            if (!ObjectUtil.ValidCollection(DefaultLocales))
            {
                throw new ArgumentException("No Default Locale found");
            }

            /* foreach */
            foreach (CultureInfo currentLocale in DefaultLocales)
            {
                string str;
                if (localToStringMap.TryGetValue(currentLocale, out str))
                {
                    if (string.IsNullOrWhiteSpace(str))
                    {
                        return str;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// The set default locale.
        /// </summary>
        /// <param name="defaultLocales0">
        /// The default locales_0.
        /// </param>
        public void SetDefaultLocale(IList<CultureInfo> defaultLocales0)
        {
            _defaultLocales = defaultLocales0;
        }

        #endregion
    }
}