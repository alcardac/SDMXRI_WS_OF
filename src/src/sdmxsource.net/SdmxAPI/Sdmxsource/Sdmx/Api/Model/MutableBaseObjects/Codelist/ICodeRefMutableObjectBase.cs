// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ICodeRefMutableObjectBase.cs" company="Eurostat">
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

    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Codelist;
    using Org.Sdmxsource.Sdmx.Api.Model.MutableBaseObjects.Base;

    #endregion

    /// <summary>
    ///     The CodeRefMutableObjectBase interface.
    /// </summary>
    public interface ICodeRefMutableObjectBase : IIdentifiableMutableObjectBase
    {
        #region Public Properties

        /// <summary>
        ///     Gets or sets the code.
        /// </summary>
        ICodeMutableObject Code { get; set; }

        /// <summary>
        ///     Gets the code refs.
        /// </summary>
        IList<ICodeRefMutableObjectBase> CodeRefs { get; }

        #endregion
    }
}