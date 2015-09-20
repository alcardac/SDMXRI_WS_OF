// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IHierarchyObjectBase.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Model.BaseObjects.Codelist
{
    #region Using directives

    using System.Collections.Generic;

    using Org.Sdmxsource.Sdmx.Api.Model.BaseObjects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Codelist;

    #endregion

    /// <summary>
    /// The HierarchyObjectBase interface.
    /// </summary>
    /// <typeparam name="T">Generic type parameter. Type of IMaintainableObjectBase.
    /// </typeparam>
    public interface IHierarchyObjectBase<T> : INameableObjectBase
        where T : IMaintainableObjectBase
    {
        #region Public Properties

        /// <summary>
        ///     Gets the codes in this codelist
        /// </summary>
        /// <value> </value>
        IList<IHierarchicalCodeObjectBase> Codes { get; }

        /// <summary>
        ///     Gets the level.
        /// </summary>
        /// <value> the level for this hierarchy, returns null if there is no level </value>
        ILevelObject Level { get; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Gets the ILevelObject at the position indicated, by walking the ILevelObject hierarchy of this Hierarchy Object, returns null if there is no level
        /// </summary>
        /// <param name="levelPos">The level position
        /// </param>
        /// <returns>
        /// the level at the given hierarchical position (0 indexed) - returns null if there is no level at that position
        /// </returns>
        ILevelObject GetLevelAtPosition(int levelPos);

        /// <summary>
        ///     If true this indicates that the hierarchy has formal levels. In this case, every code should have a level associated with it.
        ///     If false this does not have formal levels.  This hierarchy may still have levels, getLevel() may still return a value, the levels are not formal and the call to a code for its level may return null.
        /// </summary>
        /// <returns>
        ///     The <see cref="bool" /> .
        /// </returns>
        bool HasFormalLevels();

        #endregion
    }
}