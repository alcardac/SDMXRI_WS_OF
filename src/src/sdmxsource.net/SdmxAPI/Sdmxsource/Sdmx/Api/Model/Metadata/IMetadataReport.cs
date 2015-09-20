// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IMetadataReport.cs" company="Eurostat">
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

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;

    #endregion

    /// <summary>
    ///     The MetadataReport interface.
    /// </summary>
    public interface IMetadataReport : ISdmxObject
    {
        #region Public Properties

        /// <summary>
        ///     Gets the id.
        /// </summary>
        string Id { get; }

        /// <summary>
        ///    Gets a copy of the list of the ReportedAttributes
        /// </summary>
        IList<IReportedAttributeObject> ReportedAttributes { get; }

        /// <summary>
        ///    Gets the target, defining what this metadata report attaches to.  The same information can be found
        ///  using the Target...properties on this interface.
        /// </summary>
        ITarget Target { get; }


        /// <summary>
        /// Gets the targets that exist in the target object
        /// </summary>
        ISet<TargetType> Targets { get; }

        /// <summary>
        /// Gets the id of the dataset this bean is referencing, returns null if this is not a dataset reference.
        /// This will search the Target.ReportedAttribute for any that contain a datasetid, and will return null if none do.
        /// <remarks>
        /// This is to be used in conjunction with the IdentifiableReference which will return the data provider reference
        /// </remarks>
        /// </summary>
        string TargetDatasetId { get; }

        /// <summary>
        /// Returns the date for which this report is relevant
        /// This will search the Target.IReportedAttribute for any that contain a reportPeriod, and will return null if none do.
        /// </summary>
        ISdmxDate TargetReportPeriod { get; }


        /// <summary>
        /// Gets null if there is no reference to an identifiable structure
        /// This will search the Target.IReportedAttribute for any that contain a reportPeriod, and will return null if none do.
        /// </summary>
        ICrossReference TargetIdentifiableReference { get; }

        /// <summary>
        /// Gets the reference to the content constraint, if there is one
        /// This will search the Target.IReportedAttribute for any that contain a constraint reference, and will return null if none do.
        /// </summary>
        ICrossReference TargetContentConstraintReference { get; }

        /// <summary>
        /// Returns a list of data keys, will return an empty list if IsDatasetReference is false
        /// This will search the Target.IReportedAttribute for any that contain data keys, and will return null if none do.
        /// </summary>
        IList<IDataKey> TargetDataKeys { get; }

        #endregion
    }
}