// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IStructureSetObject.cs" company="Eurostat">
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

    using System;
    using System.Collections.Generic;

    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Mapping;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;

    #endregion

    /// <summary>
    ///     Represents an SDMX Structure Set
    /// </summary>
    public interface IStructureSetObject : IMaintainableObject
    {
        #region Public Properties

        /// <summary>
        ///     Gets the category scheme map list.
        /// </summary>
        IList<ICategorySchemeMapObject> CategorySchemeMapList { get; }

        /// <summary>
        ///     Gets the CodelistMap links a source and target codes from different
        ///     lists where there is a semantic equivalence between them.
        /// </summary>
        /// <value> </value>
        IList<ICodelistMapObject> CodelistMapList { get; }

        /// <summary>
        ///     Gets the ConceptSchemeMap links a source and target concepts from different schemes where there is a semantic equivalence between them.
        /// </summary>
        /// <value> </value>
        IList<IConceptSchemeMapObject> ConceptSchemeMapList { get; }

        /// <summary>
        ///     Gets a representation of itself in a @object which can be modified, modifications to the mutable @object
        ///     are not reflected in the instance of the IMaintainableObject.
        /// </summary>
        /// <value> </value>
        new IStructureSetMutableObject MutableInstance { get; }

        /// <summary>
        ///     Gets the organisation scheme map list.
        /// </summary>
        IList<IOrganisationSchemeMapObject> OrganisationSchemeMapList { get; }

        /// <summary>
        ///     Gets the relatedStructures contains references to structures (key families and metadata structure definitions) and
        ///     structure usages (data flows and metadata flows) to indicate that a semantic relationship exist between them.
        ///     The details of these relationships can be found in the structure maps.
        /// </summary>
        /// <value> </value>
        IRelatedStructures RelatedStructures { get; }

        /// <summary>
        ///     Gets the StructureMap maps components from one structure to components to another structure,
        ///     and can describe how the value of the components are related.
        /// </summary>
        /// <value> </value>
        IList<IStructureMapObject> StructureMapList { get; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Gets a stub reference of itself.
        ///     <p/>
        ///     A stub @object only contains Maintainable and Identifiable Attributes, not the composite Objects that are
        ///     contained within the Maintainable
        /// </summary>
        /// <returns>
        /// The <see cref="IStructureSetObject"/> .
        /// </returns>
        /// <param name="actualLocation">
        /// the Uri indicating where the full structure can be returned from
        /// </param>
        /// <param name="isServiceUrl">
        /// if true this Uri will be present on the serviceURL attribute, otherwise it will be treated as a structureURL attribute
        /// </param>
        new IStructureSetObject GetStub(Uri actualLocation, bool isServiceUrl);

        #endregion
    }
}