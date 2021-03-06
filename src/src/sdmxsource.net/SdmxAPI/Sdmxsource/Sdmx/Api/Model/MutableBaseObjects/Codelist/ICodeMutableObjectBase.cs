// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ICodeMutableObjectBase.cs" company="Eurostat">
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

    using Org.Sdmxsource.Sdmx.Api.Model.MutableBaseObjects.Base;

    #endregion

    /// <summary>
    ///     The CodeMutableObjectBase interface.
    /// </summary>
    public interface ICodeMutableObjectBase : IIdentifiableMutableObjectBase
    {
        #region Public Properties

        /// <summary>
        ///     Gets the children.
        /// </summary>
        IList<ICodeMutableObjectBase> Children { get; }

        /// <summary>
        ///     Gets or sets the parent.
        /// </summary>
        ICodeMutableObjectBase Parent { get; set; }

        /// <summary>
        ///     Gets or sets the value.
        /// </summary>
        string Value { get; set; }

        #endregion
    }
}