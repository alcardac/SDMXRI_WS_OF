// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IDatasetHeader.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Model.Header
{
    #region Using directives

    using System;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;

    #endregion

    /// <summary>
    ///     Contains information specifically related to a single dataset
    /// </summary>
    public interface IDatasetHeader
    {
        #region Public Properties

        /// <summary>
        ///     Gets the action for this dataset, defaults to INFORMATION
        /// </summary>
        /// <value> </value>
        DatasetAction Action { get; }

        /// <summary>
        ///     Gets a reference to the data provider for this data.
        /// </summary>
        /// <value> returns null if there is no reference </value>
        IMaintainableRefObject DataProviderReference { get; }

        /// <summary>
        ///     Gets a IDatasetStructureReference containing information about what structure is used to describe the structure of this dataset
        /// </summary>
        /// <value> returns null if there is no reference </value>
        IDatasetStructureReference DataStructureReference { get; }

        /// <summary>
        ///     Gets an id for the dataset
        /// </summary>
        /// <value> returns null if no id was given </value>
        string DatasetId { get; }

        /// <summary>
        ///     Gets the reporting end date for this dataset
        /// </summary>
        /// <value> returns null if there is no publication period defined </value>
        string PublicationPeriod { get; }

        /// <summary>
        ///     Gets the reporting end date for this dataset
        /// </summary>
        /// <value> returns -1 if there is no publication year given defined </value>
        int PublicationYear { get; }

        /// <summary>
        ///     Gets the reporting begin date for this dataset
        /// </summary>
        /// <value> returns null if there is no reporting begin date defined </value>
        DateTime ReportingBeginDate { get; }

        /// <summary>
        ///     Gets the reporting end date for this dataset
        /// </summary>
        /// <value> returns null if there is no reporting end date defined </value>
        DateTime ReportingEndDate { get; }

        /// <summary>
        ///     Gets a value indicating whether TimeSeries is true.
        ///     True if the DataReaderEngine is reading time series data (observations iterate over time).  If false time will be at the series level
        /// </summary>
        /// <value> </value>
        bool Timeseries { get; }

        /// <summary>
        ///     Gets the reporting end date for this dataset
        /// </summary>
        /// <value> returns null if there is no valid from date defined </value>
        DateTime ValidFrom { get; }

        /// <summary>
        ///     Gets the reporting end date for this dataset
        /// </summary>
        /// <value> returns null if there is no valid to defined </value>
        DateTime ValidTo { get; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Creates a new IDatasetHeader, copying over all the attributes of this header, but replacing the IDatasetStructureReference with the one passed in
        /// </summary>
        /// <param name="datasetStructureReference">Dataset structure
        /// </param>
        /// <returns>
        /// The <see cref="IDatasetHeader"/> .
        /// </returns>
        IDatasetHeader ModifyDataStructureReference(IDatasetStructureReference datasetStructureReference);

        #endregion
    }
}