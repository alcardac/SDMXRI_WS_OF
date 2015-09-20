// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IRepresentationMapRefMutableObject.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Model.Mutable.Mapping
{
    #region Using directives

    using System.Collections.Generic;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;
    using Org.Sdmxsource.Sdmx.Api.Util;

    #endregion

    /// <summary>
    ///     The RepresentationMapRefMutableObject interface.
    /// </summary>
    public interface IRepresentationMapRefMutableObject : IMutableObject
    {
        #region Public Properties

        /// <summary>
        ///     Gets or sets the codelist map.
        /// </summary>
        IStructureReference CodelistMap { get; set; }

        /// <summary>
        ///     Gets or sets the to text format.
        /// </summary>
        ITextFormatMutableObject ToTextFormat { get; set; }

        /// <summary>
        ///     Gets or sets the to value type.
        /// </summary>
        ToValue ToValueType { get; set; }

        /// <summary>
        ///     Gets the value mappings.
        /// </summary>
        IDictionaryOfSets<string, string> ValueMappings { get; }

        /// <summary>
        /// Maps the component id to the component with the given value
        /// </summary>
        /// <param name="componentId">The component unique identifier.</param>
        /// <param name="componentValue">The component value.</param>
        void AddMapping(string componentId, string componentValue);

        #endregion
    }
}