// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IConceptMutableObject.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Model.Mutable.ConceptScheme
{
    #region Using directives

    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;

    #endregion

    /// <summary>
    ///     The ConceptMutableObject interface.
    /// </summary>
    public interface IConceptMutableObject : IItemMutableObject, IConceptBaseMutableObject
    {
        #region Public Properties

        /// <summary>
        ///     Gets or sets the core representation.
        /// </summary>
        IRepresentationMutableObject CoreRepresentation { get; set; }

        /// <summary>
        ///     Gets or sets the ISO concept reference.
        /// </summary>
        IStructureReference IsoConceptReference { get; set; }

        /// <summary>
        ///     Gets or sets the parent agency.
        /// </summary>
        string ParentAgency { get; set; }

        /// <summary>
        ///     Gets or sets the parent concept.
        /// </summary>
        string ParentConcept { get; set; }

        #endregion
    }
}