// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IRepresentation.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Model.Objects.Base
{
    #region Using directives

    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;

    #endregion

    /// <summary>
    ///     Represents an SDMX Representation, this Object references a TextFormat and / or a Codelist to define how the data should be
    ///     represented
    /// </summary>
    public interface IRepresentation : ISdmxStructure
    {
        #region Public Properties

        /// <summary>
        ///     Gets the codelist reference, returns null if there is no reference
        /// </summary>
        /// <value> </value>
        ICrossReference Representation { get; }

        /// <summary>
        ///     Gets the text format.
        /// </summary>
        ITextFormat TextFormat { get; }

        #endregion
    }
}