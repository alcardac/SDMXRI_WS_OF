// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ITextFormatMutableObject.cs" company="Eurostat">
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
    ///     The TextFormatMutableObject interface.
    /// </summary>
    public interface ITextFormatMutableObject : IMutableObject
    {
        #region Public Properties

        /// <summary>
        ///     Gets or sets the decimals.
        /// </summary>
        long? Decimals { get; set; }

        /// <summary>
        ///     Gets or sets the end value.
        /// </summary>
        decimal? EndValue { get; set; }

        /// <summary>
        ///     Gets or sets the interval.
        /// </summary>
        decimal? Interval { get; set; }

        /// <summary>
        ///     Gets or sets the max length.
        /// </summary>
        long? MaxLength { get; set; }

        /// <summary>
        ///     Gets or sets the max value.
        /// </summary>
        decimal? MaxValue { get; set; }

        /// <summary>
        ///     Gets or sets the min length.
        /// </summary>
        long? MinLength { get; set; }

        /// <summary>
        ///     Gets or sets the min value.
        /// </summary>
        decimal? MinValue { get; set; }

        /// <summary>
        ///     Gets or sets the multi lingual.
        /// </summary>
        TertiaryBool Multilingual { get; set; }

        /// <summary>
        ///     Gets or sets the pattern.
        /// </summary>
        string Pattern { get; set; }

        /// <summary>
        ///     Gets or sets the sequence.
        /// </summary>
        TertiaryBool Sequence { get; set; }

        /// <summary>
        ///     Gets or sets the start value.
        /// </summary>
        decimal? StartValue { get; set; }

        /// <summary>
        ///     Gets or sets the text type.
        /// </summary>
        TextType TextType { get; set; }

        /// <summary>
        ///     Gets or sets the time interval.
        /// </summary>
        string TimeInterval { get; set; }

        #endregion
    }
}