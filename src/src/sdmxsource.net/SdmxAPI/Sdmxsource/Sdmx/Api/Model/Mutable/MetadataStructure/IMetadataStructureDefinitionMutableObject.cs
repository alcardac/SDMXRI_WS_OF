// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IMetadataStructureDefinitionMutableObject.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Model.Mutable.MetadataStructure
{
    #region Using directives

    using System.Collections.Generic;

    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.MetadataStructure;

    #endregion

    /// <summary>
    ///     The MetadataStructureDefinitionMutableObject interface.
    /// </summary>
    public interface IMetadataStructureDefinitionMutableObject : IMaintainableMutableObject
    {
        #region Public Properties

        /// <summary>
        ///     Gets a representation of itself in a @object which can not be modified, modifications to the mutable @object
        ///     are not reflected in the returned instance of the IMaintainableObject.
        /// </summary>
        /// <value> </value>
        new IMetadataStructureDefinitionObject ImmutableInstance { get; }

        /// <summary>
        ///     Gets the metadata targets.
        /// </summary>
        IList<IMetadataTargetMutableObject> MetadataTargets { get; }

        /// <summary>
        ///     Gets the report structures.
        /// </summary>
        IList<IReportStructureMutableObject> ReportStructures { get; }

        #endregion
    }
}