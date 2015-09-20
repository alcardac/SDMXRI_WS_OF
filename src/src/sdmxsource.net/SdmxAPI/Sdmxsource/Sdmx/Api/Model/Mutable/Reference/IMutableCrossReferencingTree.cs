// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IMutableCrossReferencingTree.cs" company="Eurostat">
//   Date Created : 2013-08-19
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Model.Mutable.Reference
{
    #region Using directives

    using System.Collections.Generic;

    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base;

    #endregion


    /// <summary>
    /// Defines a tree structure of Identifiable along with a set of all the structures that cross reference the Identifiable
    /// </summary>
    public interface IMutableCrossReferencingTree
    {
        #region Public Properties

        /// <summary>
        /// Gets the maintainable for which the structures are referencing.
        /// </summary>
        IMaintainableMutableObject Maintainable { get; }

        /// <summary>
        /// Gets a set of maintainables that reference the given identifiable.
        /// </summary>
        IList<IMutableCrossReferencingTree> ReferencingStructures { get; }

        #endregion
    }
}