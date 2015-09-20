// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ICodeObjectBase.cs" company="Eurostat">
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

    using Org.Sdmxsource.Sdmx.Api.Model.BaseObjects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Codelist;

    #endregion

    /// <summary>
    ///     A code is a id/value pair object, the id is typically what data references where the values is typically
    ///     the semantic meaning of the code
    ///     human readable form.
    ///     <p />
    ///     The BaseObjects representation of a code forms simple hierarchies where they exist, so a code within a codelist can have
    ///     a single parent, and many children.
    /// </summary>
    public interface ICodeObjectBase : IItemObjectBase<ICodelistObjectBase>, IHierarchical<ICodeObjectBase>
    {
        /// <summary>
        ///     Gets the code built from object.
        /// </summary>
        new ICode BuiltFrom { get; }
    }
}