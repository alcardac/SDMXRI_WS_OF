// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IComplexDataQuerySelection.cs" company="Eurostat">
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

    using System;
    using System.Collections.Generic;

    #endregion

    /// <summary>
    /// It represents a single component (dimension or attribute), <br>
    /// with one or more code selections for it along with an operator. 
    /// </summary>
    public interface IComplexDataQuerySelection : IDisposable
    {
        #region Public Properties

        /// <summary>
        /// Returns the component Id that the code selection(s) are for.  The component Id is either a referenece to a dimension
        /// or an attribute
        /// </summary>
        string ComponentId
        {
            get;
        }

        /// <summary>
        /// Returns the component value of the code that has been selected for the component.
        /// <p/>
        /// If more then one value exists then calling this method will result in a exception.
        /// Check that hasMultipleValues() returns false before calling this method
        /// </summary>
        IComplexComponentValue Value
        {
            get;
        }

        /// <summary>
        /// Returns all the code values that have been selected for this component
        /// </summary>
        ISet<IComplexComponentValue> Values
        {
            get;
        }

        #endregion


        #region Public Methods and Operators

        /// <summary>
        /// Returns true if more then one value exists for this component
        /// </summary>
        /// <returns>
        /// The boolean
        /// </returns>
        bool HasMultipleValues();

        #endregion
    }
}
