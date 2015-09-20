// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IDimensionMutableObject.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Model.Mutable.DataStructure
{
    #region Using directives

    using System.Collections.Generic;

    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;

    #endregion

    /// <summary>
    ///     The DimensionMutableObject interface.
    /// </summary>
    public interface IDimensionMutableObject : IComponentMutableObject
    {
        #region Public Properties

        /// <summary>
        ///     Gets the concept role.
        /// </summary>
        IList<IStructureReference> ConceptRole { get; }

        /// <summary>
        ///     Gets or sets a value indicating whether frequency dimension.
        /// </summary>
        bool FrequencyDimension { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether measure dimension.
        /// </summary>
        bool MeasureDimension { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether time dimension.
        /// </summary>
        bool TimeDimension { get; set; }

        #endregion
    }
}