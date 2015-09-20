// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IDatasetStructureReference.cs" company="Eurostat">
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

    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;

    #endregion

    /// <summary>
    ///     Structure is used in a Dataset header, where there is a one to one mapping between a Structure (which defines the DSD/Flow/or Provision and the Dimensions at the observation level)
    /// </summary>
    public interface IDatasetStructureReference
    {
        #region Public Properties

        /// <summary>
        ///     Gets the dimensionAtObservation is used to reference the dimension at the observation level for data messages.
        ///     <p />
        ///     Structure.ALL_DIMENSIONS denotes that the cross sectional data is in the flat format.
        ///     <p />
        ///     IPrimaryMeasure.FIXED_ID denotes that the data is in time series format
        /// </summary>
        string DimensionAtObservation { get; }

        /// <summary>
        ///     Gets the id of this structure, this
        /// </summary>
        /// <value> </value>
        string Id { get; }

        /// <summary>
        ///     Gets the serviceURL attribute indicates the Uri of an SDMX SOAP web service from which the
        ///     details of the object can be retrieved.
        ///     <p />
        ///     Note that this can be a registry or and SDMX structural metadata repository, as they both implement that same web service interface.
        /// </summary>
        /// <value> </value>
        Uri ServiceUrl { get; }

        /// <summary>
        ///     Gets the structure reference, either a provision agreement, dataflow, a data structure definition (DSD), or metadata structure definition (MSD)
        /// </summary>
        /// <value> </value>
        IStructureReference StructureReference { get; }

        /// <summary>
        ///     Gets the structureURL attribute indicates the Uri of a SDMX-ML structure message
        ///     (in the same version as the source document) in which the externally referenced object is contained.
        ///     <p />
        ///     Note that this may be a Uri of an SDMX <c>RESTful</c> web service which will return the referenced object.
        /// </summary>
        /// <value> </value>
        Uri StructureUrl { get; }

        /// <summary>
        ///     Gets a value indicating whether the getDimensionAtObservation() returns IPrimaryMeasure.FIXED_ID
        /// </summary>
        /// <value> </value>
        bool Timeseries { get; }

        #endregion
    }
}