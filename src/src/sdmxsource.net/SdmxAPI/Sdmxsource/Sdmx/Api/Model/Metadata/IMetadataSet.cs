// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IMetadataSet.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Model.Metadata
{
    #region Using directives

    using System.Collections.Generic;

    using Org.Sdmxsource.Sdmx.Api.Model.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;

    #endregion

    /// <summary>
    ///     The MetadataSet interface.
    /// </summary>
    public interface IMetadataSet : ISdmxObject
    {
        #region Public Properties


        /// <summary>
        /// Gets a list of metadata sets, built from this metadata set and metadata reports.  Each metadata set has one metadata report in it.
        /// </summary>
        IList<IMetadataSet> SplitReports { get; }

        /// <summary>
        /// Gets an identification for the metadata set
        /// </summary>
        string SetId { get; }

        /// <summary>
        /// Gets a list of names for this component - will return an empty list if no Names exist.
        /// <remarks>
        /// The list is a copy so modifying the returned list will not be reflected in the IdentifiableBean instance
        /// </remarks>
        /// </summary>
        /// <returns></returns>
        IList<ITextTypeWrapper> Names { get; }


        /// <summary>
        ///     Gets reference to the DataProvider
        /// </summary>
        /// <value> </value>
        ICrossReference DataProviderReference { get; }

        /// <summary>
        ///     Gets a reference to the metadata structure that defines the structure of this metadata set. Mandatory
        /// </summary>
        ICrossReference MsdReference { get; }

        /// <summary>
        ///     Gets  the publication period
        /// </summary>
        /// <value> </value>
        object PublicationPeriod { get; }

        /// <summary>
        ///     Gets the four digit ISo 8601 year of publication
        /// </summary>
        ISdmxDate PublicationYear { get; }

        /// <summary>
        ///     Gets the inclusive start time of the data reported in the metadata set
        /// </summary>
        /// <value> </value>
        ISdmxDate ReportingBeginDate { get; }

        /// <summary>
        ///     Gets  the inclusive end time of the data reported in the metadata set
        /// </summary>
        /// <value> </value>
        ISdmxDate ReportingEndDate { get; }

        /// <summary>
        ///     Gets the reports.
        /// </summary>
        IList<IMetadataReport> Reports { get; }

        /// <summary>
        ///     Gets the inclusive start time of the validity of the info in the metadata set
        /// </summary>
        /// <value> </value>
        ISdmxDate ValidFromDate { get; }

        /// <summary>
        ///     Gets the inclusive end time of the validity of the info in the metadata set.
        /// </summary>
        /// <value>  </value>
        ISdmxDate ValidToDate { get; }

        #endregion
    }
}