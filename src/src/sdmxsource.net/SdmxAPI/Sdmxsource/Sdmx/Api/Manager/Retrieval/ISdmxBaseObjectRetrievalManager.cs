// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ISdmxBaseObjectRetrievalManager.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Manager.Retrieval
{
    #region Using directives

    using System.Collections.Generic;

    using Org.Sdmxsource.Sdmx.Api.Model.BaseObjects.CategoryScheme;
    using Org.Sdmxsource.Sdmx.Api.Model.BaseObjects.Codelist;
    using Org.Sdmxsource.Sdmx.Api.Model.BaseObjects.ConceptScheme;
    using Org.Sdmxsource.Sdmx.Api.Model.BaseObjects.DataStructure;
    using Org.Sdmxsource.Sdmx.Api.Model.BaseObjects.Registry;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;

    #endregion

    /// <summary>
    ///     Manages the retrieval of structures and returns the responses as SDMX BaseObjects
    /// </summary>
    public interface ISdmxBaseObjectRetrievalManager
    {
        #region Public Methods and Operators

        /// <summary>
        /// Gets a single ICategorySchemeObjectBase , this expects the ref object either to contain
        ///     a URN or all the attributes required to uniquely identify the object.  If version information
        ///     is missing then the latest version is assumed.
        ///     If no category scheme is found with the given reference, then null is returned
        /// </summary>
        /// <param name="xref">
        /// The xref.
        /// </param>
        /// <returns>
        /// The <see cref="ICategorySchemeObjectBase"/> .
        /// </returns>
        ICategorySchemeObjectBase GetCategorySchemeBaseObject(IMaintainableRefObject xref);

        /// <summary>
        /// Gets ICategorySchemeObjectBase that match the parameters in the ref maintainableObject.  If the ref maintainableObject is null or
        ///     has no attributes set, then this will be interpreted as a search for all ICategorySchemeObjectBase
        /// </summary>
        /// <param name="xref">
        /// The xref.
        /// </param>
        /// <returns>
        /// list of Objects that match the search criteria
        /// </returns>
        ISet<ICategorySchemeObjectBase> GetCategorySchemeBaseObjects(IMaintainableRefObject xref);

        /// <summary>
        /// Gets a single ICodelistObjectBase , this expects the ref object either to contain
        ///     a URN or all the attributes required to uniquely identify the object.  If version information
        ///     is missing then the latest version is assumed.
        ///     If no codelist is found with the given refernce, then null is returned
        /// </summary>
        /// <param name="xref">
        /// The xref.
        /// </param>
        /// <returns>
        /// The <see cref="ICodelistObjectBase"/> .
        /// </returns>
        ICodelistObjectBase GetCodelistBaseObject(IMaintainableRefObject xref);

        /// <summary>
        /// Gets ICodelistObjectBase that match the parameters in the ref maintainableObject.  If the ref maintainableObject is null or
        ///     has no attributes set, then this will be interpreted as a search for all ICodelistObjectBase
        /// </summary>
        /// <param name="xref">
        /// The xref.
        /// </param>
        /// <returns>
        /// list of Objects that match the search criteria
        /// </returns>
        ISet<ICodelistObjectBase> GetCodelistBaseObjects(IMaintainableRefObject xref);

        /// <summary>
        /// Gets a single IConceptSchemeObjectBase , this expects the ref object either to contain
        ///     a URN or all the attributes required to uniquely identify the object.  If version information
        ///     is missing then the latest version is assumed.
        ///     If no concept scheme is found with the given reference, then null is returned
        /// </summary>
        /// <param name="xref">
        /// The xref.
        /// </param>
        /// <returns>
        /// The <see cref="IConceptSchemeObjectBase"/> .
        /// </returns>
        IConceptSchemeObjectBase GetConceptSchemeBaseObject(IMaintainableRefObject xref);

        /// <summary>
        /// Gets IConceptSchemeObjectBase that match the parameters in the ref maintainableObject.  If the ref maintainableObject is null or
        ///     has no attributes set, then this will be interpreted as a search for all ConceptSchemeBaseObjectss
        /// </summary>
        /// <param name="xref">
        /// The xref.
        /// </param>
        /// <returns>
        /// list of Objects that match the search criteria
        /// </returns>
        ISet<IConceptSchemeObjectBase> GetConceptSchemeBaseObjects(IMaintainableRefObject xref);

        /// <summary>
        /// Gets a single IDataStructureObjectBase , this expects the ref object either to contain
        ///     a URN or all the attributes required to uniquely identify the object.  If version information
        ///     is missing then the latest version is assumed.
        ///     If no DSD is found with the given reference, then null is returned
        /// </summary>
        /// <param name="xref">
        /// The xref.
        /// </param>
        /// <returns>
        /// The <see cref="IDataStructureObjectBase"/> .
        /// </returns>
        IDataStructureObjectBase GetDataStructureBaseObject(IMaintainableRefObject xref);

        /// <summary>
        /// Gets DataStructureBaseObjects that match the parameters in the ref maintainableObject.  If the ref maintainableObject is null or
        ///     has no attributes set, then this will be interpreted as a search for all DataStructureBaseObjects.
        /// </summary>
        /// <param name="xref">
        /// The xref.
        /// </param>
        /// <returns>
        /// list of Objects that match the search criteria
        /// </returns>
        ISet<IDataStructureObjectBase> GetDataStructureBaseObjects(IMaintainableRefObject xref);

        /// <summary>
        /// Gets a single Dataflow , this expects the ref object either to contain
        ///     a URN or all the attributes required to uniquely identify the object.  If version information
        ///     is missing then the latest version is assumed.
        ///     If no dataflow is found with the given reference, then null is returned
        /// </summary>
        /// <param name="xref">
        /// The xref.
        /// </param>
        /// <returns>
        /// The <see cref="IDataflowObjectBase"/> .
        /// </returns>
        IDataflowObjectBase GetDataflowBaseObject(IMaintainableRefObject xref);

        /// <summary>
        /// Gets IDataflowObjectBase that match the parameters in the ref maintainableObject.  If the ref maintainableObject is null or
        ///     has no attributes set, then this will be interpreted as a search for all IDataflowObjectBase
        /// </summary>
        /// <param name="xref">
        /// The xref.
        /// </param>
        /// <returns>
        /// list of Objects that match the search criteria
        /// </returns>
        ISet<IDataflowObjectBase> GetDataflowBaseObjects(IMaintainableRefObject xref);

        /// <summary>
        /// Gets a single HierarchicCodeList , this expects the ref object either to contain
        ///     a URN or all the attributes required to uniquely identify the object.  If version information
        ///     is missing then the latest version is assumed.
        ///     If no hierarchical codelist is found with the given reference, then null is returned
        /// </summary>
        /// <param name="xref">
        /// The xref.
        /// </param>
        /// <returns>
        /// The <see cref="IHierarchicalCodelistObjectBase"/> .
        /// </returns>
        IHierarchicalCodelistObjectBase GetHierarchicCodeListBaseObject(IMaintainableRefObject xref);

        /// <summary>
        /// Gets IHierarchicalCodelistObjectBase that match the parameters in the ref maintainableObject.  If the ref maintainableObject is null or
        ///     has no attributes set, then this will be interpreted as a search for all IHierarchicalCodelistObjectBase
        /// </summary>
        /// <param name="xref">
        /// The xref.
        /// </param>
        /// <returns>
        /// list of Objects that match the search criteria
        /// </returns>
        ISet<IHierarchicalCodelistObjectBase> GetHierarchicCodeListBaseObjects(IMaintainableRefObject xref);

        /// <summary>
        /// Gets a single IProvisionAgreementObjectBase , this expects the ref object either to contain
        ///     a URN or all the attributes required to uniquely identify the object.  If version information
        ///     is missing then the latest version is assumed.
        ///     If no Provision Agreement is found with the given reference, then null is returned
        /// </summary>
        /// <param name="xref">
        /// The xref.
        /// </param>
        /// <returns>
        /// The <see cref="IProvisionAgreementObjectBase"/> .
        /// </returns>
        IProvisionAgreementObjectBase GetProvisionAgreementBaseObject(IMaintainableRefObject xref);

        /// <summary>
        /// Gets ProvisionAgreementBaseObjects that match the parameters in the ref maintainableObject.  If the ref maintainableObject is null or
        ///     has no attributes set, then this will be interpreted as a search for all ProvisionAgreementBaseObjectss
        /// </summary>
        /// <param name="xref">
        /// The xref.
        /// </param>
        /// <returns>
        /// list of Objects that match the search criteria
        /// </returns>
        ISet<IProvisionAgreementObjectBase> GetProvisionAgreementBaseObjects(IMaintainableRefObject xref);

        #endregion
    }
}