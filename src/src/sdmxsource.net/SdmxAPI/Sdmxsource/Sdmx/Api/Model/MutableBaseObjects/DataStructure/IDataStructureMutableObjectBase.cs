// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IDataStructureMutableObjectBase.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Model.MutableBaseObjects.DataStructure
{
    #region Using directives

    using System.Collections.Generic;

    using Org.Sdmxsource.Sdmx.Api.Model.MutableBaseObjects.Base;

    #endregion

    /// <summary>
    ///     The DataStructureMutableObjectBase interface.
    /// </summary>
    public interface IDataStructureMutableObjectBase : IMaintainableMutableObjectBase
    {
        #region Public Properties

        /// <summary>
        ///     Gets the attributes.
        /// </summary>
        IList<IAttributeMutableObjectBase> Attributes { get; }

        /// <summary>
        ///     Gets the dimensions.
        /// </summary>
        IList<IDimensionMutableObjectBase> Dimensions { get; }

        /// <summary>
        ///     Gets the groups.
        /// </summary>
        IList<IGroupMutableObjectBase> Groups { get; }

        /// <summary>
        ///     Gets or sets the primary measure.
        /// </summary>
        IPrimaryMeasureMutableObjectBase PrimaryMeasure { get; set; }

        #endregion
    }
}