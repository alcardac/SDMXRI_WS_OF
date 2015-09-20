// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IDimension.cs" company="Eurostat">
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

    using System;
    using System.Collections.Generic;

    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;

    #endregion

    /// <summary>
    ///     Represents an SDMX Dimensions in a Data Structure.
    ///     <p />
    ///     Each break down described above is an example of a dimension, which links semantic with codeded/uncodeed values.
    ///     <p />
    ///     A dimension defines the semantic using a (<c>IConceptObject</c>, and the representation using either (<c>ICodelistObject</c>
    ///     a <c>ITextFormat</c> or just free text)
    /// </summary>
    public interface IDimension : IComponent, IComparable<IDimension>
    {
        // $$$ string TIME_DIMENSION_FIXED_ID = "TIME_PERIOD";
        #region Public Properties

        /// <summary>
        ///     Gets cross references to the concept(s) that are defining the role of this dimension. The list is a copy of the
        ///     underlying list.
        ///     <p />
        ///     Gets an empty list if there are no concept roles defined,
        /// </summary>
        /// <value> </value>
        IList<ICrossReference> ConceptRole { get; }

        /// <summary>
        ///     Gets a value indicating whether the dimension is the frequency dimension.
        ///     <p />
        ///     <b>NOTE :</b> This is mutually exclusive with isMeasureDimension() and isTimeDimension().
        /// </summary>
        /// <value> </value>
        bool FrequencyDimension { get; }

        /// <summary>
        ///     Gets a value indicating whether the dimension is the measure dimension.
        ///     <p />
        ///     <b>NOTE :</b> This is mutually exclusive with isFrequencyDimension() and isTimeDimension().
        /// </summary>
        /// <value> </value>
        bool MeasureDimension { get; }

        /// <summary>
        ///     Gets the index of this dimension in the dimension list
        /// </summary>
        /// <value> </value>
        int Position { get; }

        /// <summary>
        ///     Gets a value indicating whether the dimension is the time dimension.
        ///     <p />
        ///     <b>NOTE :</b> This is mutually exclusive with isFrequencyDimension() and isMeasureDimension().
        /// </summary>
        /// <value> </value>
        bool TimeDimension { get; }

        #endregion
    }
}