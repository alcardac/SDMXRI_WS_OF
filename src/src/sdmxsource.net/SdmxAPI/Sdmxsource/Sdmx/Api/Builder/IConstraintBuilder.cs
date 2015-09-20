// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IConstraintBuilder.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Builder
{
    #region Using directives

    using Org.Sdmxsource.Sdmx.Api.Engine;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Registry;

    #endregion

    /// <summary>
    ///     The ConstraintBuilder interface.
    /// </summary>
    public interface IConstraintBuilder
    {
        #region Public Methods and Operators

        /// <summary>
        /// Build constraint.
        /// </summary>
        /// <param name="dataReaderEngine">
        /// The data reader engine.
        /// </param>
        /// <param name="attachment">
        /// The attachment.
        /// </param>
        /// <param name="dataSourceAttachment">
        /// The data source attachment.
        /// </param>
        /// <param name="indexAttributes">
        /// The index attributes.
        /// </param>
        /// <param name="indexDataset">
        /// The index dataset.
        /// </param>
        /// <param name="indexReportingPeriod">
        /// The index reporting period.
        /// </param>
        /// <param name="indexTimeSeries">
        /// The index time series.
        /// </param>
        /// <param name="definingDataPresent">
        /// The defining data present.
        /// </param>
        /// <param name="refParameters">
        /// The ref parameters.
        /// </param>
        /// <returns>
        /// The <see cref="IContentConstraintObject"/> .
        /// </returns>
        IContentConstraintObject BuildConstraint(
            IDataReaderEngine dataReaderEngine, 
            IStructureReference attachment, 
            IDataSource dataSourceAttachment, 
            bool indexAttributes, 
            bool indexDataset, 
            bool indexReportingPeriod, 
            bool indexTimeSeries, 
            bool definingDataPresent, 
            IMaintainableRefObject refParameters);

        #endregion
    }
}