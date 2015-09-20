// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IHierarchicalCodelistObjectBase.cs" company="Eurostat">
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

    #endregion

    /// <summary>
    ///     A hierarchical codelist is made up of one or more hierarchies,
    ///     each containing codes, which can be hierarchical, where the definition
    ///     of each code is in a SDMX codelist.
    ///     <p />
    ///     Note that the hierarchical codelist allows the same code to be used in more than one hierarchy.
    /// </summary>
    public interface IHierarchicalCodelistObjectBase : IMaintainableObjectBase
    {
        #region Public Properties

        /// <summary>
        ///     Gets the set of hierarchies that this hierarchical codelist
        ///     has reference to
        /// </summary>
        IHierarchyObjectBaseSet<IHierarchicalCodelistObjectBase> Hierarchies { get; }

        #endregion
    }
}