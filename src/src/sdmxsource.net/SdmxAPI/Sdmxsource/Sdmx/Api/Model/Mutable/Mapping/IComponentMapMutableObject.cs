// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IComponentMapMutableObject.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Model.Mutable.Mapping
{
    #region Using directives

    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base;

    #endregion

    /// <summary>
    ///     The ComponentMapMutableObject interface.
    /// </summary>
    public interface IComponentMapMutableObject : IAnnotableMutableObject
    {
        #region Public Properties

        /// <summary>
        ///     Gets or sets the map concept ref.
        /// </summary>
        string MapConceptRef { get; set; }

        /// <summary>
        ///     Gets or sets the map target concept ref.
        /// </summary>
        string MapTargetConceptRef { get; set; }

        /// <summary>
        ///     Gets or sets the rep map ref.
        /// </summary>
        IRepresentationMapRefMutableObject RepMapRef { get; set; }

        #endregion
    }
}