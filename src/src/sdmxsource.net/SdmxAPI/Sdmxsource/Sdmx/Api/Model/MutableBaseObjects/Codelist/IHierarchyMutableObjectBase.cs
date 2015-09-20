// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IHierarchyMutableObjectBase.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Model.MutableBaseObjects.Codelist
{
    #region Using directives

    using System.Collections.Generic;

    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Codelist;
    using Org.Sdmxsource.Sdmx.Api.Model.MutableBaseObjects.Base;

    #endregion

    /// <summary>
    ///     The HierarchyMutableObjectBase interface.
    /// </summary>
    public interface IHierarchyMutableObjectBase : INameableMutableObjectBase
    {
        #region Public Properties

        /// <summary>
        ///     Gets or sets the child level.
        /// </summary>
        ILevelMutableObject ChildLevel { get; set; }

        /// <summary>
        ///     Gets the code refs.
        /// </summary>
        IList<ICodeRefMutableObjectBase> CodeRefs { get; }

        /// <summary>
        ///    Gets or sets a value indicating whether the formal levels are set.
        /// </summary>
        bool FormalLevels { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     The has formal levels.
        /// </summary>
        /// <returns>
        ///     The <see cref="bool" /> .
        /// </returns>
        bool HasFormalLevels();

        #endregion
    }
}