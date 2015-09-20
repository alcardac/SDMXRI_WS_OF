// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IHierarchyMutableObject.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base
{
    #region Using directives

    using System.Collections.Generic;

    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Codelist;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Reference;

    #endregion

    /// <summary>
    ///     The HierarchyMutableObject interface.
    /// </summary>
    public interface IHierarchyMutableObject : INameableMutableObject
    {
        #region Public Properties

        /// <summary>
        ///     Gets or sets the child level.
        /// </summary>
        ILevelMutableObject ChildLevel { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether the hierarchy has formal levels
        /// </summary>
        /// <returns>
        ///     true this indicates that the hierarchy has formal levels. In this case, every code should have a level associated with it.
        ///     false this does not have formal levels.  This hierarchy may still have levels, getLevel() may still return a value, the levels are not formal and the call to a code for its level may return null.
        /// </returns>
        bool FormalLevels { get; set; }

        /// <summary>
        ///     Gets the hierarchical code Objects.
        /// </summary>
        IList<ICodeRefMutableObject> HierarchicalCodeObjects { get; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The add hierarchical code.
        /// </summary>
        /// <param name="codeRef">
        /// The code ref.
        /// </param>
        void AddHierarchicalCode(ICodeRefMutableObject codeRef);

        #endregion
    }
}