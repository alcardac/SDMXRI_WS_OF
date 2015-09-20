// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IComplexComponentValue.cs" company="Eurostat">
//   Date Created : 2013-08-19
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Model.Data.Query.Complex
{
    #region Using directives

    using Org.Sdmxsource.Sdmx.Api.Constants;

    #endregion

    /// <summary>
    /// It models the dimension or attribute or primary measure or time value for which data is requested.<br>
    /// The value of the component is accompanied by the operator to apply to the string value. 
    /// </summary>
    public interface IComplexComponentValue
    {
        #region Public Properties

        /// <summary>
        /// Returns the value of the dimension or attribute
        /// </summary>
        string Value
        {
            get;
        }

        /// <summary>
        /// Returns the operator to apply. Does not concern a dimension or time value
        /// </summary>
        TextSearch TextSearchOperator
        {
            get;
        }

        /// <summary>
        /// Returns the operator to apply.
        /// </summary>
        OrderedOperator OrderedOperator
        {
            get;
        }

        #endregion
    }
}
