// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IReferenceValueBase.cs" company="Eurostat">
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

    using Org.Sdmxsource.Sdmx.Api.Model.BaseObjects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Metadata;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Registry;

    #endregion

    /// <summary>
    ///     ReferenceValue contains a value for a target reference object reference.
    ///     <p />
    ///     When this is taken with its sibling elements, they identify the object or objects to which the reported metadata apply.
    ///     The content of this will either be a reference to an identifiable object, a data key, a reference to a data set, or a report period.
    /// </summary>
    public interface IReferenceValueBase : IObjectBase
    {
        #region Public Properties

        /// <summary>
        ///     Gets the IReferenceValue that was used to build this Base Object - Override from parent
        /// </summary>
        new IReferenceValue BuiltFrom { get; }

        /// <summary>
        ///     Gets the content constraint that this structure references, returns null if there is no reference
        /// </summary>
        /// <value> </value>
        IContentConstraintObject ContentConstraintReference { get; }

        /// <summary>
        ///     Gets a value indicating whether the reference is a content constraint reference, if true getContentConstraintReference() will return a not null value
        /// </summary>
        /// <value> </value>
        bool ContentConstriantReference { get; }

        /// <summary>
        ///     Gets a list of data keys, will return an empty list if isDatasetReference() is false
        /// </summary>
        /// <value> </value>
        IList<IDataKey> DataKeys { get; }

        /// <summary>
        ///     Gets a value indicating whether the the reference value is a datakey reference, if true getDataKeys() will return 1 or more items
        /// </summary>
        /// <value> </value>
        bool DatakeyReference { get; }

        /// <summary>
        ///     Gets the dataset id.
        /// </summary>
        string DatasetId { get; }

        /// <summary>
        ///     Gets a value indicating whether the is a dataset reference, if true GetIdentifiableReference() AND getDatasetId() will NOT be null
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
        IIdentifiableObject IdentifiableReference { get; }

        /// <summary>
        ///    Gets a value indicating whether this is an identifiable structure reference, if true GetIdentifiableReference() will NOT be null
        /// </summary>
        /// <value> </value>
        bool IsIdentifiableReference { get; }

        #endregion
    }
}