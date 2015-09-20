// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IHierarchicalCodelistMutableObject.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Model.Mutable.Codelist
{
    #region Using directives

    using System.Collections.Generic;

    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Reference;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Codelist;

    #endregion

    /// <summary>
    ///     The HierarchicalCodelistMutableObject interface.
    /// </summary>
    public interface IHierarchicalCodelistMutableObject : IMaintainableMutableObject
    {
        #region Public Properties

        /// <summary>
        ///     Gets the codelist ref.
        /// </summary>
        IList<ICodelistRefMutableObject> CodelistRef { get; }

        /// <summary>
        ///     Gets the hierarchies.
        /// </summary>
        IList<IHierarchyMutableObject> Hierarchies { get; }

        /// <summary>
        ///     Gets a representation of itself in a @object which can not be modified, modifications to the mutable @object
        ///     are not reflected in the returned instance of the IMaintainableObject.
        /// </summary>
        /// <value> </value>
        new IHierarchicalCodelistObject ImmutableInstance { get; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The add codelist ref.
        /// </summary>
        /// <param name="codelistRef">
        /// The codelist ref.
        /// </param>
        void AddCodelistRef(ICodelistRefMutableObject codelistRef);

        /// <summary>
        /// The add hierarchies.
        /// </summary>
        /// <param name="hierarchy">
        /// The hierarchy.
        /// </param>
        void AddHierarchies(IHierarchyMutableObject hierarchy);

        #endregion
    }
}