// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IDataQuerySelectionGroup.cs" company="Eurostat">
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

    using Org.Sdmxsource.Sdmx.Api.Model.Base;

    #endregion

    /// <summary>
    ///     A Data Query selection group contains a set of DataQuerySelection's implicit ANDED each DataQuerySelection
    ///     contains a concept along with one or more selection values.  For example DataQuerySelection may contain FREQ=A,B which would
    ///     equate to FREQ = A OR B.
    ///     Example :
    ///     DataQuerySelection FREQ=A,B
    ///     DataQuerySelection COUNTRY=UK
    ///     Equates to (FREQ = A OR B) AND (COUNTRY = UK)
    /// </summary>
    /// <example>
    ///     A sample implementation in C# of <see cref="IDataQuerySelectionGroup" />
    ///     <code source="..\ReUsingExamples\DataQuery\ReUsingDataQueryParsingManager.cs" lang="cs" />
    /// </example>
    public interface IDataQuerySelectionGroup
    {
        #region Public Properties

        /// <summary>
        ///     Gets the date from in this selection group
        /// </summary>
        /// <value> </value>
        ISdmxDate DateFrom { get; }

        /// <summary>
        ///     Gets the date to in this selection group
        /// </summary>
        /// <value> </value>
        ISdmxDate DateTo { get; }

        /// <summary>
        ///     Gets the set of selections for this group - these are implicitly ANDED
        /// </summary>
        /// <value> </value>
        ISet<IDataQuerySelection> Selections { get; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Gets the selection(s) for the given dimension id, returns null if no selection exist for the dimension id
        /// </summary>
        /// <param name="componentId">The component Id. </param>
        /// <returns>
        /// The <see cref="IDataQuerySelection"/> .
        /// </returns>
        IDataQuerySelection GetSelectionsForConcept(string componentId);

        /// <summary>
        /// Gets a value indicating whether the selections exist for this dimension Id
        /// </summary>
        /// <param name="dimensionId">
        /// The dimension Id.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/> .
        /// </returns>
        bool HasSelectionForConcept(string dimensionId);

        #endregion
    }
}