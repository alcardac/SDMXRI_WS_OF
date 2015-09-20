// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IDimensionListMutableObject.cs" company="Eurostat">
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

    #endregion

    /// <summary>
    ///     The DimensionListMutableObject interface.
    /// </summary>
    public interface IDimensionListMutableObject : IIdentifiableMutableObject
    {
        #region Public Properties

        /// <summary>
        ///     Gets the dimensions.
        /// </summary>
        IList<IDimensionMutableObject> Dimensions { get; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The add dimension.
        /// </summary>
        /// <param name="dimension">
        /// The dimension.
        /// </param>
        void AddDimension(IDimensionMutableObject dimension);

        #endregion
    }
}