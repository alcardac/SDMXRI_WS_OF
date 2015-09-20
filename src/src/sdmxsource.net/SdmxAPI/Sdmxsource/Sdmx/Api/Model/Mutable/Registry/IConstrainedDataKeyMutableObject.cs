// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IConstrainedDataKeyMutableObject.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Model.Mutable.Registry
{
    #region Using directives

    using System.Collections.Generic;

    using Org.Sdmxsource.Sdmx.Api.Model.Data;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base;

    #endregion

    /// <summary>
    ///     The ConstrainedDataKeyMutableObject interface.
    /// </summary>
    public interface IConstrainedDataKeyMutableObject : IMutableObject
    {
        #region Public Properties

        /// <summary>
        ///     Gets the key values.
        /// </summary>
        IList<IKeyValue> KeyValues { get; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Adds the specified key value.
        /// </summary>
        /// <param name="keyvalue">
        /// The key value.
        /// </param>
        void AddKeyValue(IKeyValue keyvalue);

        #endregion
    }
}