// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IRepresentationMapRef.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Model.Objects.Mapping
{
    #region Using directives

    using System.Collections.Generic;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;
    using Org.Sdmxsource.Sdmx.Api.Util;

    #endregion

    /// <summary>
    ///     Represents an SDMX Representation Map
    /// </summary>
    public interface IRepresentationMapRef : ISdmxStructure
    {
        #region Public Properties

        /// <summary>
        ///     Gets the codelist map.
        /// </summary>
        ICrossReference CodelistMap { get; }

        /// <summary>
        ///     Gets the to text format.
        /// </summary>
        ITextFormat ToTextFormat { get; }

        /// <summary>
        ///     Gets the to value type.
        /// </summary>
        ToValue ToValueType { get; }

        /// <summary>
        ///     Gets the value mappings.
        /// </summary>
        IDictionaryOfSets<string, string> ValueMappings { get; }

        #endregion
    }
}