// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IAttributeList.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Model.Objects.DataStructure
{
    #region Using directives

    using System.Collections.Generic;

    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;

    #endregion

    /// <summary>
    ///     Represents an SDMX Attribute List
    /// </summary>
    public interface IAttributeList : IIdentifiableObject
    {
        #region Public Properties

        /// <summary>
        ///     Gets the list of attributes that this attribute list is holding.
        ///     <p />
        ///     The returned list will be a copy of the underlying list, and is expected to have at least one AttributeObject
        /// </summary>
        /// <value> </value>
        IList<IAttributeObject> Attributes { get; }

        #endregion
    }
}