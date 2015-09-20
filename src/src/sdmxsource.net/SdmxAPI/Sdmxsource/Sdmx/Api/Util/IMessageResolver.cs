// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IMessageResolver.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Util
{
    #region Using directives

    using System.Globalization;

    #endregion

    /// <summary>
    ///     Used to resolve message codes to text strings based in the given locale
    /// </summary>
    public interface IMessageResolver
    {
        #region Public Methods and Operators

        /// <summary>
        /// Resolves the message against a resource which contains code-&gt;string mappings, inserting any args
        ///     where there are placeholders.  If there is no resource found, or no mapping could be found
        ///     for the messageCode, then the original messageCode will be returned.
        /// </summary>
        /// <param name="messageCode">
        /// The message Code.
        /// </param>
        /// <param name="locale">
        /// - the locale to resolve the message in
        /// </param>
        /// <param name="args">
        /// The args.
        /// </param>
        /// <returns>
        /// the resolved message, or the original if could not be resolved
        /// </returns>
        string ResolveMessage(string messageCode, CultureInfo locale, params object[] args);

        #endregion
    }
}