// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IDataKey.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Model.Metadata
{
    #region Using directives

    using Org.Sdmxsource.Sdmx.Api.Model.Data;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;

    #endregion

    /// <summary>
    ///     The DataKey interface.
    /// </summary>
    public interface IDataKey : ISdmxObject
    {
        #region Public Properties

        /// <summary>
        ///     Gets a value indicating whether included.
        /// </summary>
        bool Included { get; }

        /// <summary>
        ///     Gets the key value.
        /// </summary>
        IKeyValue KeyValue { get; }

        #endregion
    }
}