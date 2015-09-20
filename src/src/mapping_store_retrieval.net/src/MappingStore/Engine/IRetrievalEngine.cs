// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IRetrievalEngine.cs" company="Eurostat">
//   Date Created : 2013-02-12
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   The RetrievalEngine interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Estat.Sri.MappingStoreRetrieval.Engine
{
    using System.Collections.Generic;

    using Estat.Sri.MappingStoreRetrieval.Constants;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;

    /// <summary>
    /// The RetrievalEngine interface.
    /// </summary>
    /// <typeparam name="TMaint">
    /// The <see cref="IMaintainableMutableObject"/> type
    /// </typeparam>
    public interface IRetrievalEngine<TMaint>
        where TMaint : IMaintainableMutableObject
    {
        #region Public Methods and Operators

        /// <summary>
        /// Retrieve the set of <see cref="IMaintainableMutableObject"/> from Mapping Store.
        /// </summary>
        /// <param name="maintainableRef">
        ///     The maintainable reference which may contain ID, AGENCY ID and/or VERSION.
        /// </param>
        /// <param name="detail">
        ///     The <see cref="StructureQueryDetail"/> which controls if the output will include details or not.
        /// </param>
        /// <param name="versionConstraints">The version related constraints.</param>
        /// <returns>
        /// The <see cref="ISet{IMaintainableMutableObject}"/>.
        /// </returns>
        ISet<TMaint> Retrieve(IMaintainableRefObject maintainableRef, ComplexStructureQueryDetailEnumType detail, VersionQueryType versionConstraints);

        /// <summary>
        /// Retrieve the <see cref="IMaintainableMutableObject"/> with the latest version group by ID and AGENCY from Mapping Store.
        /// </summary>
        /// <param name="maintainableRef">
        ///     The maintainable reference which may contain ID, AGENCY ID and/or VERSION.
        /// </param>
        /// <param name="detail">
        ///     The <see cref="StructureQueryDetail"/> which controls if the output will include details or not.
        /// </param>
        /// <returns>
        /// The <see cref="IMaintainableMutableObject"/>.
        /// </returns>
        TMaint RetrieveLatest(IMaintainableRefObject maintainableRef, ComplexStructureQueryDetailEnumType detail);

        /// <summary>
        /// Retrieve the set of <see cref="IMaintainableMutableObject"/> from Mapping Store that references <paramref name="referencedStructure"/>.
        /// </summary>
        /// <param name="referencedStructure">
        ///     The maintainable reference which may contain ID, AGENCY ID and/or VERSION. This is the referenced structure.
        /// </param>
        /// <param name="detail">
        ///     The <see cref="StructureQueryDetail"/> which controls if the output will include details or not.
        /// </param>
        /// <returns>
        /// The <see cref="ISet{IMaintainableMutableObject}"/>.
        /// </returns>
        ISet<TMaint> RetrieveFromReferenced(IStructureReference referencedStructure, ComplexStructureQueryDetailEnumType detail);

        /// <summary>
        /// Retrieve the set of <see cref="IMaintainableMutableObject"/> from Mapping Store.
        /// </summary>
        /// <param name="maintainableRef">
        ///     The maintainable reference which may contain ID, AGENCY ID and/or VERSION.
        /// </param>
        /// <param name="detail">
        ///     The <see cref="StructureQueryDetail"/> which controls if the output will include details or not.
        /// </param>
        /// <param name="versionConstraints">The version related constraints</param>
        /// <param name="allowedDataflows">
        ///     The allowed Dataflows.
        /// </param>
        /// <returns>
        /// The <see cref="ISet{IMaintainableMutableObject}"/>.
        /// </returns>
        ISet<TMaint> Retrieve(IMaintainableRefObject maintainableRef, ComplexStructureQueryDetailEnumType detail, VersionQueryType versionConstraints, IList<IMaintainableRefObject> allowedDataflows);

        /// <summary>
        /// Retrieve the <see cref="IMaintainableMutableObject"/> with the latest version group by ID and AGENCY from Mapping Store.
        /// </summary>
        /// <param name="maintainableRef">
        ///     The maintainable reference which may contain ID, AGENCY ID and/or VERSION.
        /// </param>
        /// <param name="detail">
        ///     The <see cref="StructureQueryDetail"/> which controls if the output will include details or not.
        /// </param>
        /// <param name="allowedDataflows">
        ///     The allowed Dataflows.
        /// </param>
        /// <returns>
        /// The <see cref="IMaintainableMutableObject"/>.
        /// </returns>
        TMaint RetrieveLatest(IMaintainableRefObject maintainableRef, ComplexStructureQueryDetailEnumType detail, IList<IMaintainableRefObject> allowedDataflows);

        /// <summary>
        /// Retrieve the set of <see cref="IMaintainableMutableObject"/> from Mapping Store that references <paramref name="referencedStructure"/>.
        /// </summary>
        /// <param name="referencedStructure">
        ///     The maintainable reference which may contain ID, AGENCY ID and/or VERSION. This is the referenced structure.
        /// </param>
        /// <param name="detail">
        ///     The <see cref="StructureQueryDetail"/> which controls if the output will include details or not.
        /// </param>
        /// <param name="allowedDataflows">
        ///     The allowed Dataflows.
        /// </param>
        /// <returns>
        /// The <see cref="ISet{IMaintainableMutableObject}"/>.
        /// </returns>
        ISet<TMaint> RetrieveFromReferenced(IStructureReference referencedStructure, ComplexStructureQueryDetailEnumType detail, IList<IMaintainableRefObject> allowedDataflows);

        #endregion
    }
}