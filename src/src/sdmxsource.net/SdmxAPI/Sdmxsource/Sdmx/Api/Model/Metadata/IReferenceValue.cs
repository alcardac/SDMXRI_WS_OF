// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IReferenceValue.cs" company="Eurostat">
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
    ///     ReferenceValue contains a value for a target reference object reference.
    ///     <p />
    ///     When this is taken with its sibling elements, they identify the object or objects to which the reported metadata apply.
    ///     The content of this will either be a reference to an identifiable object, a data key, a reference to a data set, or a report period.
    /// </summary>
    public interface IReferenceValue : ISdmxObject
    {
        #region Public Properties

        /// <summary>
        ///     Gets the reference to the content constraint, if there is one
        /// </summary>
        /// <value> </value>
        ICrossReference ContentConstraintReference { get; }

        /// <summary>
        ///     Gets a value indicating whether the is a content constraint reference, if true getContentConstraintReference() will return a not null value
        /// </summary>
        /// <value> </value>
        bool IsContentConstriantReference { get; }

        /// <summary>
        ///     Gets a list of data keys, will return an empty list if isDatasetReference() is false
        /// </summary>
        /// <value> </value>
        IList<IDataKey> DataKeys { get; }

        /// <summary>
        ///    Gets a value indicating whether this is a datakey reference, if true getDataKeys() will return 1 or more items
        /// </summary>
        /// <value> </value>
        bool DatakeyReference { get; }

        /// <summary>
        ///     Gets the dataset id.
        /// </summary>
        string DatasetId { get; }

        /// <summary>
        /// Gets the date for which this report is relevant
        /// </summary>
        ISdmxDate ReportPeriod { get; }

        /// <summary>
        /// Gets an enum defining what this reference value is referencing
        /// </summary>
        TargetType TargetType { get; }

        /// <summary>
        ///      Gets a value indicating whether this is a dataset reference, if true GetIdentifiableReference() AND getDatasetId() will NOT be null
        /// </summary>
        /// <value> </value>
        bool DatasetReference { get; }

        /// <summary>
        ///     Gets the id of this reference value
        /// </summary>
        /// <value> </value>
        string Id { get; }

        /// <summary>
        ///     Gets identifiable reference.
        /// </summary>
        /// <value> The &lt; see cref= &quot; ICrossReference &quot; / &gt; . </value>
        ICrossReference IdentifiableReference { get; }

        /// <summary>
        ///     Gets a value indicating whether the is an identifiable structure reference, if true GetIdentifiableReference() will NOT be null
        /// </summary>
        /// <value> </value>
        bool IsIdentifiableReference { get; }

        #endregion
    }
}