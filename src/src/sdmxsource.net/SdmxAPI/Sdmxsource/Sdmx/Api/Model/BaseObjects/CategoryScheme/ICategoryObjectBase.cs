// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ICategoryObjectBase.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Model.BaseObjects.CategoryScheme
{
    #region Using directives

    using Org.Sdmxsource.Sdmx.Api.Model.BaseObjects.Base;

    #endregion

    /// <summary>
    ///     A Category represents a statistical subject matter domain, it can have reference to a dataflow, and they can be hierarchical
    /// </summary>
    public interface ICategoryObjectBase : IItemObjectBase<ICategorySchemeObjectBase>, 
                                           IHierarchical<ICategoryObjectBase>
    {
    }
}