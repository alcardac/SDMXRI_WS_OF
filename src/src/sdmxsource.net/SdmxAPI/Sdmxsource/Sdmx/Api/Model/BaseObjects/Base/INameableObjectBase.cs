// --------------------------------------------------------------------------------------------------------------------
// <copyright file="INameableObjectBase.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Model.BaseObjects.Base
{
    #region Using directives

    using System.Collections.Generic;
    using System.Globalization;

    #endregion

    /// <summary>
    ///     The NameableObjectBase interface.
    /// </summary>
    public interface INameableObjectBase : IIdentifiableObjectBase
    {
        #region Public Properties

        /// <summary>
        ///     Gets the Description of the Identifiable object for a the default Locale
        /// </summary>
        /// <value> </value>
        /// <value> </value>
        string Description { get; }

        /// <summary>
        ///     Gets the descriptions.
        /// </summary>
        IDictionary<CultureInfo, string> Descriptions { get; }

        /// <summary>
        ///     Gets the Name of the Identifiable object for a the default Locale
        /// </summary>
        /// <value> </value>
        /// <value> </value>
        string Name { get; }

        /// <summary>
        ///     Gets the names.
        /// </summary>
        IDictionary<CultureInfo, string> Names { get; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Gets the description of the Identifiable Object for the given Locale
        /// </summary>
        /// <param name="loc">The locale
        /// </param>
        /// <returns>
        /// The <see cref="string"/> .
        /// </returns>
        string GetDescription(CultureInfo loc);

        /// <summary>
        /// Gets the Name of the Identifiable object for a given Locale
        /// </summary>
        /// <param name="loc">The locale
        /// </param>
        /// <returns>
        /// The <see cref="string"/> .
        /// </returns>
        string GetName(CultureInfo loc);

        #endregion
    }
}