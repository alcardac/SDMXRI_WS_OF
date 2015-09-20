// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IComponentMapObject.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Model.Objects.Mapping
{
    #region Using directives

    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;

    #endregion

    /// <summary>
    ///     Represents an SDMX Component Map
    /// </summary>
    public interface IComponentMapObject : IAnnotableObject
    {
        #region Public Properties

        /// <summary>
        ///     Gets the map concept ref.
        /// </summary>
        string MapConceptRef { get; }

        /// <summary>
        ///     Gets the map target concept ref.
        /// </summary>
        string MapTargetConceptRef { get; }

        /// <summary>
        ///     Gets the rep map ref.
        /// </summary>
        IRepresentationMapRef RepMapRef { get; }

        #endregion
    }
}