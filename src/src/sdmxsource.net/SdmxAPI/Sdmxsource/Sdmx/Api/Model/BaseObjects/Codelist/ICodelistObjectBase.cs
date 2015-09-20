// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ICodelistObjectBase.cs" company="Eurostat">
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
    ///     A ICodelistObjectBase is a container for codes.
    /// </summary>
    public interface ICodelistObjectBase : IMaintainableObjectBase
    {
        #region Public Properties

        /// <summary>
        ///     Gets the built from.
        /// </summary>
        /// <value> The ICodelistObject that this ICodelistObjectBase was built from. </value>
        new ICodelistObject BuiltFrom { get; }

        /// <summary>
        ///     Gets the codes in this codelist. As codes are hierarchical only the top level codes, with no parents, will be returned.
        /// </summary>
        /// <value> the codes in this codelist. </value>
        IList<ICodeObjectBase> Codes { get; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Iterates through the code hierarchy and returns the ICodeObjectBase that has the same id as that supplied.
        /// </summary>
        /// <param name="id">
        /// the id of a ICodeObjectBase to search for.
        /// </param>
        /// <returns>
        /// the matching ICodeObjectBase or null if there was no match.
        /// </returns>
        ICodeObjectBase GetCodeByValue(string id);

        #endregion
    }
}