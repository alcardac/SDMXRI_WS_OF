// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IMutableObject.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base
{
    #region Using directives

    using Org.Sdmxsource.Sdmx.Api.Constants;

    #endregion

    /// <summary>
    ///     The MutableObject interface.
    /// </summary>
    public interface IMutableObject
    {
        #region Public Properties

        /// <summary>
        ///     Gets the structure type of this component.
        /// </summary>
        /// <value> </value>
        SdmxStructureType StructureType { get; }

        #endregion
    }
}