// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IConceptObject.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Model.Objects.ConceptScheme
{
    #region Using directives

    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;

    #endregion

    /// <summary>
    ///     Represents an SDMX Concept
    /// </summary>
    public interface IConceptObject : IItemObject, IConceptBaseObject
    {
        #region Public Properties

        /// <summary>
        ///     Gets the core representation for this concept
        /// </summary>
        /// <value> null if no core representation is defined </value>
        IRepresentation CoreRepresentation { get; }

        /// <summary>
        ///     Gets the ISO concept reference for this concept
        /// </summary>
        /// <value> null if no ISO concept reference is defined </value>
        ICrossReference IsoConceptReference { get; }

        /// <summary>
        ///     Gets the parent concept agency
        /// </summary>
        /// <value> null if no concept agency is defined </value>
        string ParentAgency { get; }

        /// <summary>
        ///     Gets the parent concept Id
        /// </summary>
        /// <value> null if no concept agency is defined </value>
        string ParentConcept { get; }

        #endregion
    }
}