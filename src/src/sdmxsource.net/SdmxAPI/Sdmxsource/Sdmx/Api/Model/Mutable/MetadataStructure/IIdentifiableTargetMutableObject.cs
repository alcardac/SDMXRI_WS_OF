// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IIdentifiableTargetMutableObject.cs" company="Eurostat">
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

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base;

    #endregion

    /// <summary>
    ///     The IdentifiableTargetMutableObject interface.
    /// </summary>
    public interface IIdentifiableTargetMutableObject : IComponentMutableObject
    {
        #region Public Properties

        /// <summary>
        ///     Gets or sets the referenced structure type.
        /// </summary>
        SdmxStructureType ReferencedStructureType { get; set; }

        #endregion
    }
}