// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IDimensionObjectBase.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Model.BaseObjects.DataStructure
{
    #region Using directives

    using Org.Sdmxsource.Sdmx.Api.Model.BaseObjects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.DataStructure;

    #endregion

    /// <summary>
    ///     A dimension is one component of a key that together with other dimensions of the key
    ///     uniquely identify the observation.
    /// </summary>
    public interface IDimensionObjectBase : IComponentObjectBase
    {
        #region Public Properties

        /// <summary>
        ///     Gets the built from.
        /// </summary>
        new IDimension BuiltFrom { get; }

        /// <summary>
        ///     Gets a value indicating whether the dimension is the frequency dimension, false otherwise.
        /// </summary>
        /// <value> </value>
        bool FrequencyDimension { get; }

        /// <summary>
        ///     Gets a value indicating whether the dimension is the measure dimension, false otherwise.
        /// </summary>
        /// <value> </value>
        bool MeasureDimension { get; }

        /// <summary>
        ///     Gets a value indicating whether time dimension.
        /// </summary>
        bool TimeDimension { get; }

        #endregion
    }
}