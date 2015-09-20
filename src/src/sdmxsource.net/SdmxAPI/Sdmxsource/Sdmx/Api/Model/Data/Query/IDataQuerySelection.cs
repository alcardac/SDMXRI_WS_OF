// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IDataQuerySelection.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Model.Data.Query
{
    #region Using directives

    using System.Collections.Generic;

    #endregion

    /// <summary>
    ///     A DataQuerySelection represents a single dimension, with one or more code selections for it.
    /// </summary>
    public interface IDataQuerySelection
    {
        #region Public Properties

        /// <summary>
        ///   Gets the component Id that the code selection(s) are for.  
        /// The component Id is either a referenece to a dimension or an attribute
        /// </summary>
        /// <value> </value>
        string ComponentId { get; }

        /// <summary>
        ///     Gets the value of the code that has been selected for the dimension.
        ///     <p />
        ///     If more then one value exists then calling this method will result in a excpetion.
        ///     Check that hasMultipleValues() returns false before calling this method
        /// </summary>
        /// <value> </value>
        string Value { get; }

        /// <summary>
        ///     Gets all the code values that have been selected for this dimension
        /// </summary>
        /// <value> </value>
        ISet<string> Values { get; }

        #endregion

     
        /// <summary>
        ///     Gets a value indicating whether the more then one value exists for this dimension
        /// </summary>
        /// <returns>
        ///     The <see cref="bool" /> .
        /// </returns>
        bool HasMultipleValues { get; }

    }
}