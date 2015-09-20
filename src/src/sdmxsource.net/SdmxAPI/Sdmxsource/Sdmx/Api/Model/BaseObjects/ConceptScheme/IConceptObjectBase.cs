// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IConceptObjectBase.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Model.BaseObjects.ConceptScheme
{
    #region Using directives

    using Org.Sdmxsource.Sdmx.Api.Model.BaseObjects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.BaseObjects.Codelist;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.ConceptScheme;

    #endregion

    /// <summary>
    ///     A concept is a characteristic.  It is the basic building block of data and metadata structures e.g all dimensions, attributes and
    ///     dimensions in a data structure must reference a concept.
    /// </summary>
    public interface IConceptObjectBase : INameableObjectBase
    {
        #region Public Properties

        /// <summary>
        ///     Gets the built from.
        /// </summary>
        new IConceptObject BuiltFrom { get; }

        /// <summary>
        ///     Gets the representation of this concept - this will return null if <see cref="HasRepresentation" /> is false
        /// </summary>
        /// <value> </value>
        ICodelistObjectBase CoreRepresentation { get; }

        /// <summary>
        ///     Gets a value indicating whether this concept has core representation
        /// </summary>
        bool HasRepresentation { get; }

        /// <summary>
        ///     Gets a value indicating whether the concept is a stand alone concept
        /// </summary>
        /// <value> </value>
        bool StandAloneConcept { get; }

        /// <summary>
        ///     Gets the text format.
        /// </summary>
        ITextFormat TextFormat { get; }

        #endregion
    }
}