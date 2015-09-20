// --------------------------------------------------------------------------------------------------------------------
// <copyright file="INameableMutableObject.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base
{
    #region Using directives

    using System.Collections.Generic;

    #endregion

    /// <summary>
    ///     The NameableMutableObject interface.
    /// </summary>
    public interface INameableMutableObject : IIdentifiableMutableObject
    {
        #region Public Properties

        /// <summary>
        ///     Gets the description.
        /// </summary>
        IList<ITextTypeWrapperMutableObject> Descriptions { get; }

        /// <summary>
        ///     Gets the name.
        /// </summary>
        IList<ITextTypeWrapperMutableObject> Names { get; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Adds a description, edits the current value if the locale already exists
        /// </summary>
        /// <param name="locale">The locale
        /// </param>
        /// <param name="name">The description name
        /// </param>
        void AddDescription(string locale, string name);

        /// <summary>
        /// Adds a name, edits the current value if the locale already exists
        /// </summary>
        /// <param name="locale">The locale
        /// </param>
        /// <param name="name">The name
        /// </param>
        void AddName(string locale, string name);

        /// <summary>
        /// The get name.
        /// </summary>
        /// <param name="defaultIfNull">
        /// The default if null.
        /// </param>
        /// <returns>
        /// The <see cref="string"/> .
        /// </returns>
        string GetName(bool defaultIfNull);

        #endregion
    }
}