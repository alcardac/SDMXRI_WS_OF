// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ICubeRegionMutableObject.cs" company="Eurostat">
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

    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base;

    #endregion

    /// <summary>
    ///     The CubeRegionMutableObject interface.
    /// </summary>
    public interface ICubeRegionMutableObject : IMutableObject
    {
        #region Public Properties

        /// <summary>
        ///     Gets the attribute values.
        /// </summary>
        IList<IKeyValuesMutable> AttributeValues { get; }

        /// <summary>
        ///     Gets the key values.
        /// </summary>
        IList<IKeyValuesMutable> KeyValues { get; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The add attribute value.
        /// </summary>
        /// <param name="attvalue">
        /// The attvalue.
        /// </param>
        void AddAttributeValue(IKeyValuesMutable attvalue);

        /// <summary>
        /// The add key value.
        /// </summary>
        /// <param name="keyvalue">
        /// The keyvalue.
        /// </param>
        void AddKeyValue(IKeyValuesMutable keyvalue);

        #endregion
    }
}