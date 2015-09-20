// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IStructureSetMutableObject.cs" company="Eurostat">
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

    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Mapping;

    #endregion

    /// <summary>
    ///     The StructureSetMutableObject interface.
    /// </summary>
    public interface IStructureSetMutableObject : IMaintainableMutableObject
    {
        #region Public Properties

        /// <summary>
        ///     Gets the category scheme map list.
        /// </summary>
        IList<ICategorySchemeMapMutableObject> CategorySchemeMapList { get; }

        /// <summary>
        ///     Gets the codelist map list.
        /// </summary>
        IList<ICodelistMapMutableObject> CodelistMapList { get; }

        /// <summary>
        ///     Gets the concept scheme map list.
        /// </summary>
        IList<IConceptSchemeMapMutableObject> ConceptSchemeMapList { get; }

        /// <summary>
        ///     Gets a representation of itself in a @object which can not be modified, modifications to the mutable @object
        ///     are not reflected in the returned instance of the IMaintainableObject.
        /// </summary>
        /// <value> </value>
        new IStructureSetObject ImmutableInstance { get; }

        /// <summary>
        ///     Gets the organisation scheme map list.
        /// </summary>
        IList<IOrganisationSchemeMapMutableObject> OrganisationSchemeMapList { get; }

        /// <summary>
        ///     Gets or sets the related structures.
        /// </summary>
        IRelatedStructuresMutableObject RelatedStructures { get; set; }

        /// <summary>
        ///     Gets the structure map list.
        /// </summary>
        IList<IStructureMapMutableObject> StructureMapList { get; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The add category scheme map.
        /// </summary>
        /// <param name="categorySchemeMap">
        /// The category scheme map.
        /// </param>
        void AddCategorySchemeMap(ICategorySchemeMapMutableObject categorySchemeMap);

        /// <summary>
        /// The add codelist map.
        /// </summary>
        /// <param name="codelistMap">
        /// The codelist map.
        /// </param>
        void AddCodelistMap(ICodelistMapMutableObject codelistMap);

        /// <summary>
        /// The add concept scheme map.
        /// </summary>
        /// <param name="conceptSchemeMap">
        /// The concept scheme map.
        /// </param>
        void AddConceptSchemeMap(IConceptSchemeMapMutableObject conceptSchemeMap);

        /// <summary>
        /// The add organisation scheme map.
        /// </summary>
        /// <param name="organisationSchemeMap">
        /// The organisation scheme map.
        /// </param>
        void AddOrganisationSchemeMap(IOrganisationSchemeMapMutableObject organisationSchemeMap);

        /// <summary>
        /// The add structure map.
        /// </summary>
        /// <param name="structureMap">
        /// The structure map.
        /// </param>
        void AddStructureMap(IStructureMapMutableObject structureMap);

        #endregion
    }
}