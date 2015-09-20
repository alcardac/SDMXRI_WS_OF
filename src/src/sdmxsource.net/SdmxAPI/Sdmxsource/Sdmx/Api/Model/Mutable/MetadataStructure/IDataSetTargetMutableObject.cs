// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IDataSetTargetMutableObject.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Model.Mutable.MetadataStructure
{
    #region Using directives

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base;

    #endregion

    /// <summary>
    ///     The DataSetTargetMutableObject interface.
    /// </summary>
    public interface IDataSetTargetMutableObject : IIdentifiableMutableObject
    {
        #region Public Properties

        /// <summary>
        ///     Gets or sets the text type.
        /// </summary>
        TextType TextType { get; set; }

        #endregion
    }
}