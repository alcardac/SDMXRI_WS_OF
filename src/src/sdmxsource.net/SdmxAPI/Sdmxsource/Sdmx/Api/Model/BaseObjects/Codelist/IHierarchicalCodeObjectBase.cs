// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IHierarchicalCodeObjectBase.cs" company="Eurostat">
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
    ///     The HierarchicalCodeObjectBase interface.
    /// </summary>
    public interface IHierarchicalCodeObjectBase : IIdentifiableObjectBase
    {
        #region Public Properties

        /// <summary>
        ///     Gets the code.
        /// </summary>
        ICode Code { get; }

        /// <summary>
        ///     Gets the code refs.
        /// </summary>
        IList<IHierarchicalCodeObjectBase> CodeRefs { get; }

        /// <summary>
        ///     Gets the IHierarchicalCode built from object.
        /// </summary>
        new IHierarchicalCode BuiltFrom { get; }

        #endregion
    }
}