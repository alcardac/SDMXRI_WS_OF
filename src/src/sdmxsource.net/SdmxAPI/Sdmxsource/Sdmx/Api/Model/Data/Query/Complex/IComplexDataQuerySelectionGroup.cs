// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IComplexDataQuerySelectionGroup.cs" company="Eurostat">
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

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Base;

    #endregion

    /// <summary>
    /// A DataQuerySelectionGroup contains a set of DataQuerySelections which are implicitly ANDED together. 
    /// Each DataQuerySelection contains a concept along with one or more selection values.
    /// </p> 
    /// For example a DataQuerySelection could be FREQ (=A), (=B) which would equate to FREQ = A OR B.  <br>
    /// or could be FREQ (>A), (<M) which would equate to FREQ > A AND FREQ <M.  
    ///  </p>
    ///  When there are more than one DataQuerySelections they are ANDED together. For example the DataQuerySelections:
    ///  <pre>
    ///  DataQuerySelection FREQ=A,B
    ///  DataQuerySelection COUNTRY=UK
    ///  </pre>
    ///  Equate to:
    ///  <pre>(FREQ = A OR B) AND (COUNTRY = UK)
    ///  </pre>
    /// </summary>
    public interface IComplexDataQuerySelectionGroup : IDisposable
    {
        #region Public Properties

        /// <summary>
        /// Returns the set of selections for this group. These DataQuerySelections are implicitly ANDED together. 
        /// </summary>
        ISet<IComplexDataQuerySelection> Selections { get; }

        /// <summary>
        /// Returns the "date from" in this selection group.
        /// </summary>
        ISdmxDate DateFrom { get; }

        /// <summary>
        /// Returns the operator for the dateFrom
        /// The operator cannot take the 'NOT_EQUAL' value
        /// </summary>
        OrderedOperator DateFromOperator { get; }

        /// <summary>
        /// Returns the "date to" in this selection group.
        /// </summary>
        ISdmxDate DateTo { get; }

        /// <summary>
        /// Returns the operator for the dateTo
        /// The operator cannot take the 'NOT_EQUAL' value
        /// </summary>
        OrderedOperator DateToOperator { get; }

        /// <summary>
        /// Returns the component value (s) for a primary measure value.
        /// </summary>
        ISet<IComplexComponentValue> PrimaryMeasureValue { get; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Returns the selection(s) for the given component id (dimension or attribute) or returns null if no selection exists for the component id.
        /// </summary>
        /// <param name="componentId">
        /// The component id
        /// </param>
        /// <returns>
        /// The selection(s)
        /// </returns>
        IComplexDataQuerySelection GetSelectionsForConcept(string componentId);

        /// <summary>
        /// Returns true if selections exist for this dimension Id.
        /// </summary>
        /// <param name="componentId">
        /// The dimension id
        /// </param>
        /// <returns>
        /// The boolean
        /// </returns>
        bool HasSelectionForConcept(string componentId);

        #endregion

    }
}
