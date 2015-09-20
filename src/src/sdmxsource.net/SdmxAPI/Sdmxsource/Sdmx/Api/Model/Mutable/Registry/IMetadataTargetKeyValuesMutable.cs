// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IMetadataTargetKeyValuesMutable.cs" company="Eurostat">
//   Date Created : 2013-08-19
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Model.Mutable.Registry
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IMetadataTargetKeyValuesMutable : IKeyValuesMutable
    {
        /// <summary>
        /// Gets the object references.
        /// </summary>
        /// <value>
        /// The object references.
        /// </value>
        IList<IStructureReference> ObjectReferences { get; }

        /// <summary>
        /// Adds the object reference.
        /// </summary>
        /// <param name="sRef">The arguments preference.</param>
        void AddObjectReference(IStructureReference sRef);

        /// <summary>
        /// Gets the dataset references.
        /// </summary>
        /// <value>
        /// The dataset references.
        /// </value>
        IList<IDataSetReferenceMutableObject> DatasetReferences { get; }

        /// <summary>
        /// Adds the dataset reference.
        /// </summary>
        /// <param name="reference">The reference.</param>
        void AddDatasetReference(IDataSetReferenceMutableObject reference);
    }
}
