// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IAttributable.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Model.Data
{
    #region Using directives

    using System.Collections.Generic;

    #endregion

    /// <summary>
    ///     An Attributable artifact has the ability to contain attribute values
    /// </summary>
    public interface IAttributable
    {
        #region Public Properties

        /// <summary>
        ///     Gets a list of all the attribute values
        /// </summary>
        /// <value> </value>
        IList<IKeyValue> Attributes { get; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Gets an attribtue for the given concept/attribute id
        /// </summary>
        /// <param name="concept">
        /// The concept.
        /// </param>
        /// <returns>
        /// The <see cref="IKeyValue"/> .
        /// </returns>
        IKeyValue GetAttribute(string concept);

        #endregion
    }
}