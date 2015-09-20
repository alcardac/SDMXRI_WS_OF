// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IMetadataSetBase.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Model.BaseObjects.Metadata
{
    #region Using directives

    using System.Collections.Generic;

    using Org.Sdmxsource.Sdmx.Api.Model.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.BaseObjects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Metadata;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.MetadataStructure;

    #endregion

    /// <summary>
    ///     The MetadataSetBase interface.
    /// </summary>
    public interface IMetadataSetBase : IObjectBase
    {
        #region Public Properties

        /// <summary>
        ///     Gets the IMetadataSet that was used to build this Base Object.
        ///     Override from parent
        /// </summary>
        new IMetadataSet BuiltFrom { get; }

        /// <summary>
        ///     Gets DataProvider
        /// </summary>
        IDataProvider DataProvider { get; }

        /// <summary>
        ///     Gets  the metadata structure that defines the structure of this metadata set.
        ///     Mandatory
        /// </summary>
        IMetadataStructureDefinitionObject MetadataStructure { get; }

        /// <summary>
        ///     Gets the publication period
        /// </summary>
        object PublicationPeriod { get; }

        /// <summary>
        ///     Gets the four digit ISo 8601 year of publication
        /// </summary>
        ISdmxDate PublicationYear { get; }

        /// <summary>
        ///     Gets  the inclusive start time of the data reported in the metadata set
        /// </summary>
        ISdmxDate ReportingBeginDate { get; }

        /// <summary>
        ///     Gets the inclusive end time of the data reported in the metadata set
        /// </summary>
        ISdmxDate ReportingEndDate { get; }

        /// <summary>
        ///     Gets the reports.
        /// </summary>
        IList<IMetadataReportBase> Reports { get; }

        /// <summary>
        ///     Gets the inclusive start time of the validity of the info in the metadata set
        /// </summary>
        ISdmxDate ValidFromDate { get; }

        /// <summary>
        ///     Gets the inclusive end time of the validity of the info in the metadata set
        /// </summary>
        ISdmxDate ValidToDate { get; }

        #endregion
    }
}