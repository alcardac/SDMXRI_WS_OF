// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IMetadataSetMutableObject.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Model.Metadata.Mutable
{
    #region Using directives

    using System;
    using System.Collections.Generic;
    using System.Globalization;

    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;

    #endregion

    /// <summary>
    ///     The MetadataSetMutableObject interface.
    /// </summary>
    public interface IMetadataSetMutableObject : IMutableObject
    {
        #region Public Properties

        /// <summary>
        ///     Gets or sets reference to the DataProvider
        /// </summary>
        /// <value> </value>
        IStructureReference DataProviderReference { get; set; }

        /// <summary>
        ///     Gets or sets the publication period
        /// </summary>
        /// <value></value>
        string PublicationPeriod { get; set; }

        /// <summary>
        ///     Gets or sets the four digit ISo 8601 year of publication
        /// </summary>
        /// <value> </value>
        DateTime PublicationYear { get; set; }

        /// <summary>
        ///     Gets or sets the reportingBeginDate the inclusive start time of the data reported in the metadata set
        /// </summary>
        DateTime ReportingBeginDate { get; set; }

        /// <summary>
        ///     Gets or sets the inclusive end time of the data reported in the metadata set
        /// </summary>
        /// <value>  </value>
        DateTime ReportingEndDate { get; set; }

        /// <summary>
        ///     Gets the reports.
        /// </summary>
        IList<IReportMutableObject> Reports { get;  }

        /// <summary>
        ///     Gets or sets the reference to the structure in the Header above. Mandatory
        /// </summary>
        /// <value>  </value>
        string StructureRef { get; set; }

        /// <summary>
        ///     Gets or sets the inclusive start time of the validity of the info in the metadata set
        /// </summary>
        /// <value>  </value>
        DateTime ValidFromDate { get; set; }

        /// <summary>
        ///     Gets or sets the inclusive end time of the validity of the info in the metadata set
        /// </summary>
        /// <value>  </value>
        DateTime ValidToDate { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The add report.
        /// </summary>
        /// <param name="report">
        /// The report.
        /// </param>
        void AddReport(IReportMutableObject report);

        #endregion

        // FUNC footer messages
    }
}