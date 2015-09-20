// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ICategorySchemeMapMutableObject.cs" company="Eurostat">
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

    using System.Collections.Generic;

    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base;

    #endregion

    /// <summary>
    ///     The CategorySchemeMapMutableObject interface.
    /// </summary>
    public interface ICategorySchemeMapMutableObject : ISchemeMapMutableObject
    {
        #region Public Properties

        /// <summary>
        ///     Gets the category maps.
        /// </summary>
        IList<ICategoryMapMutableObject> CategoryMaps { get; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The add category map.
        /// </summary>
        /// <param name="categoryMap">
        /// The category map.
        /// </param>
        void AddCategoryMap(ICategoryMapMutableObject categoryMap);

        #endregion
    }
}