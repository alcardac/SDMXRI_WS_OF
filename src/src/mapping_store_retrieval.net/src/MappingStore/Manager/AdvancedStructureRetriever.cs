// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AdvancedStructureRetriever.cs" company="Eurostat">
//   Date Created : 2013-06-13
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   The advanced structure retriever.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Estat.Sri.MappingStoreRetrieval.Manager
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;

    using Estat.Sdmxsource.Extension.Extension;
    using Estat.Sri.MappingStoreRetrieval.Extensions;
    using Estat.Sri.MappingStoreRetrieval.Helper;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Exception;
    using Org.Sdmxsource.Sdmx.Api.Manager.Retrieval.Mutable;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.CategoryScheme;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Codelist;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.ConceptScheme;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.DataStructure;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Mapping;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.MetadataStructure;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Process;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Registry;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference.Complex;

    /// <summary>
    /// The advanced structure retriever.
    /// </summary>
    public class AdvancedStructureRetriever : IAdvancedSdmxMutableObjectRetrievalManager
    {
        #region Fields

        /// <summary>
        ///     The mapping store database
        /// </summary>
        private readonly Database _mappingStoreDB;

        /// <summary>
        ///     The retrieval engine container.
        /// </summary>
        private readonly RetrievalEngineContainer _retrievalEngineContainer;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AdvancedStructureRetriever"/> class.
        /// </summary>
        /// <param name="mappingStoreConnectionStringSettings">
        /// The mapping store connection string settings.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="mappingStoreConnectionStringSettings"/> is null
        /// </exception>
        public AdvancedStructureRetriever(ConnectionStringSettings mappingStoreConnectionStringSettings)
        {
            if (mappingStoreConnectionStringSettings == null)
            {
                throw new ArgumentNullException("mappingStoreConnectionStringSettings");
            }

            this._mappingStoreDB = new Database(mappingStoreConnectionStringSettings);
            this._retrievalEngineContainer = new RetrievalEngineContainer(this._mappingStoreDB);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AdvancedStructureRetriever"/> class.
        /// </summary>
        /// <param name="mappingStoreDB">
        /// The mapping store database.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="mappingStoreDB"/> is null
        /// </exception>
        public AdvancedStructureRetriever(Database mappingStoreDB)
        {
            if (mappingStoreDB == null)
            {
                throw new ArgumentNullException("mappingStoreDB");
            }

            this._mappingStoreDB = mappingStoreDB;
            this._retrievalEngineContainer = new RetrievalEngineContainer(this._mappingStoreDB);
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Gets a single Agency Scheme, this expects the ref object either to contain
        ///     a URN or all the attributes required to uniquely identify the object.  If version information
        ///     is missing then the latest version is assumed.
        /// </summary>
        /// <param name="complexRef">
        /// The reference object defining the search parameters.
        /// </param>
        /// <param name="returnDetail">
        /// The return Detail.
        /// </param>
        /// <returns>
        /// The <see cref="T:Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base.IAgencySchemeMutableObject"/> .
        /// </returns>
        public IAgencySchemeMutableObject GetMutableAgencyScheme(IComplexStructureReferenceObject complexRef, ComplexStructureQueryDetail returnDetail)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets AgencySchemeMutableObject that match the parameters in the ref @object.  If the ref @object is null or
        ///     has no attributes set, then this will be interpreted as a search for all CodelistObjects
        /// </summary>
        /// <param name="complexRef">
        /// The reference object defining the search parameters.
        /// </param>
        /// <param name="returnDetail">
        /// The return Detail.
        /// </param>
        /// <returns>
        /// list of sdmxObjects that match the search criteria
        /// </returns>
        public ISet<IAgencySchemeMutableObject> GetMutableAgencySchemeObjects(IComplexStructureReferenceObject complexRef, ComplexStructureQueryDetail returnDetail)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets a single Categorisation, this expects the ref object either to contain
        ///     a URN or all the attributes required to uniquely identify the object.  If version information
        ///     is missing then the latest version is assumed.
        /// </summary>
        /// <param name="complexRef">
        /// The reference object defining the search parameters.
        /// </param>
        /// <param name="returnDetail">
        /// The return Detail.
        /// </param>
        /// <returns>
        /// The <see cref="T:Org.Sdmxsource.Sdmx.Api.Model.Mutable.CategoryScheme.ICategorisationMutableObject"/> .
        /// </returns>
        public ICategorisationMutableObject GetMutableCategorisation(IComplexStructureReferenceObject complexRef, ComplexStructureQueryDetail returnDetail)
        {
            var xref = complexRef.GetMaintainableRefObject();
            if (xref.HasVersion())
            {
                return this._retrievalEngineContainer.CategorisationRetrievalEngine.Retrieve(xref, returnDetail.EnumType, complexRef.GetVersionConstraints()).FirstOrDefault();
            }

            //// TODO Change this when mapping store is modified to host proper categorisation artefacts
            return this._retrievalEngineContainer.CategorisationRetrievalEngine.RetrieveLatest(xref, returnDetail.EnumType);
        }

        /// <summary>
        /// Gets CategorisationObjects that match the parameters in the ref @object.  If the ref @object is null or
        ///     has no attributes set, then this will be interpreted as a search for all CodelistObjects
        /// </summary>
        /// <param name="complexRef">
        /// The reference object defining the search parameters.
        /// </param>
        /// <param name="returnDetail">
        /// The return Detail.
        /// </param>
        /// <returns>
        /// list of sdmxObjects that match the search criteria
        /// </returns>
        public ISet<ICategorisationMutableObject> GetMutableCategorisationObjects(IComplexStructureReferenceObject complexRef, ComplexStructureQueryDetail returnDetail)
        {
            var xref = complexRef.GetMaintainableRefObject();
            return this._retrievalEngineContainer.CategorisationRetrievalEngine.Retrieve(xref, returnDetail.EnumType, complexRef.GetVersionConstraints());
        }

        /// <summary>
        /// Gets a single CategoryScheme , this expects the ref object either to contain
        ///     a URN or all the attributes required to uniquely identify the object.  If version information
        ///     is missing then the latest version is assumed.
        /// </summary>
        /// <param name="complexRef">
        /// The reference object defining the search parameters.
        /// </param>
        /// <param name="returnDetail">
        /// The return Detail.
        /// </param>
        /// <returns>
        /// The <see cref="T:Org.Sdmxsource.Sdmx.Api.Model.Mutable.CategoryScheme.ICategorySchemeMutableObject"/> .
        /// </returns>
        public ICategorySchemeMutableObject GetMutableCategoryScheme(IComplexStructureReferenceObject complexRef, ComplexStructureQueryDetail returnDetail)
        {
            var xref = complexRef.GetMaintainableRefObject();
            return xref.HasVersion()
                       ? this._retrievalEngineContainer.CategorySchemeRetrievalEngine.Retrieve(xref, returnDetail.EnumType, complexRef.GetVersionConstraints()).FirstOrDefault()
                       : this._retrievalEngineContainer.CategorySchemeRetrievalEngine.RetrieveLatest(xref, returnDetail.EnumType);
        }

        /// <summary>
        /// Gets CategorySchemeObjects that match the parameters in the ref @object.  If the ref @object is null or
        ///     has no attributes set, then this will be interpreted as a search for all CategorySchemeObjects
        /// </summary>
        /// <param name="complexRef">
        /// The reference object defining the search parameters.
        /// </param>
        /// <param name="returnDetail">
        /// The return Detail.
        /// </param>
        /// <returns>
        /// list of sdmxObjects that match the search criteria
        /// </returns>
        public ISet<ICategorySchemeMutableObject> GetMutableCategorySchemeObjects(IComplexStructureReferenceObject complexRef, ComplexStructureQueryDetail returnDetail)
        {
            var xref = complexRef.GetMaintainableRefObject();
            return this._retrievalEngineContainer.CategorySchemeRetrievalEngine.Retrieve(xref, returnDetail.EnumType, complexRef.GetVersionConstraints());
        }

        /// <summary>
        /// Gets a single CodeList , this expects the ref object either to contain
        ///     a URN or all the attributes required to uniquely identify the object.  If version information
        ///     is missing then the latest version is assumed.
        /// </summary>
        /// <param name="complexRef">
        /// The reference object defining the search parameters.
        /// </param>
        /// <param name="returnDetail">
        /// The return Detail.
        /// </param>
        /// <returns>
        /// The <see cref="T:Org.Sdmxsource.Sdmx.Api.Model.Mutable.Codelist.ICodelistMutableObject"/> .
        /// </returns>
        public ICodelistMutableObject GetMutableCodelist(IComplexStructureReferenceObject complexRef, ComplexStructureQueryDetail returnDetail)
        {
            var xref = complexRef.GetMaintainableRefObject();
            return xref.HasVersion()
                       ? this._retrievalEngineContainer.CodeListRetrievalEngine.Retrieve(xref, returnDetail.EnumType, complexRef.GetVersionConstraints()).FirstOrDefault()
                       : this._retrievalEngineContainer.CodeListRetrievalEngine.RetrieveLatest(xref, returnDetail.EnumType);
        }

        /// <summary>
        /// Gets CodelistObjects that match the parameters in the ref @object.  If the ref @object is null or
        ///     has no attributes set, then this will be interpreted as a search for all CodelistObjects
        /// </summary>
        /// <param name="complexRef">
        /// The reference object defining the search parameters.
        /// </param>
        /// <param name="returnDetail">
        /// The return Detail.
        /// </param>
        /// <returns>
        /// list of sdmxObjects that match the search criteria
        /// </returns>
        public ISet<ICodelistMutableObject> GetMutableCodelistObjects(IComplexStructureReferenceObject complexRef, ComplexStructureQueryDetail returnDetail)
        {
            var xref = complexRef.GetMaintainableRefObject();
            return this._retrievalEngineContainer.CodeListRetrievalEngine.Retrieve(xref, returnDetail.EnumType, complexRef.GetVersionConstraints());
        }

        /// <summary>
        /// Gets a single ConceptScheme , this expects the ref object either to contain
        ///     a URN or all the attributes required to uniquely identify the object.  If version information
        ///     is missing then the latest version is assumed.
        /// </summary>
        /// <param name="complexRef">
        /// The reference object defining the search parameters.
        /// </param>
        /// <param name="returnDetail">
        /// The return Detail.
        /// </param>
        /// <returns>
        /// The <see cref="T:Org.Sdmxsource.Sdmx.Api.Model.Mutable.ConceptScheme.IConceptSchemeMutableObject"/> .
        /// </returns>
        public IConceptSchemeMutableObject GetMutableConceptScheme(IComplexStructureReferenceObject complexRef, ComplexStructureQueryDetail returnDetail)
        {
            var xref = complexRef.GetMaintainableRefObject();
            return xref.HasVersion()
                       ? this._retrievalEngineContainer.ConceptSchemeRetrievalEngine.Retrieve(xref, returnDetail.EnumType, complexRef.GetVersionConstraints()).FirstOrDefault()
                       : this._retrievalEngineContainer.ConceptSchemeRetrievalEngine.RetrieveLatest(xref, returnDetail.EnumType);
        }

        /// <summary>
        /// Gets ConceptSchemeObjects that match the parameters in the ref @object.  If the ref @object is null or
        ///     has no attributes set, then this will be interpreted as a search for all ConceptSchemeObjects
        /// </summary>
        /// <param name="complexRef">
        /// The reference object defining the search parameters.
        /// </param>
        /// <param name="returnDetail">
        /// The return Detail.
        /// </param>
        /// <returns>
        /// list of sdmxObjects that match the search criteria
        /// </returns>
        public ISet<IConceptSchemeMutableObject> GetMutableConceptSchemeObjects(IComplexStructureReferenceObject complexRef, ComplexStructureQueryDetail returnDetail)
        {
            var xref = complexRef.GetMaintainableRefObject();
            return this._retrievalEngineContainer.ConceptSchemeRetrievalEngine.Retrieve(xref, returnDetail.EnumType, complexRef.GetVersionConstraints());
        }

        /// <summary>
        /// Returns a single Content Constraint, this expects the ref object either to contain
        ///     a URN or all the attributes required to uniquely identify the object.  If version information
        ///     is missing then the latest version is assumed.
        /// </summary>
        /// <param name="complexRef">
        /// The reference object defining the search parameters.
        /// </param>
        /// <param name="returnDetail">
        /// The return Detail.
        /// </param>
        /// <returns>
        /// The Content constraint.
        /// </returns>
        public IContentConstraintMutableObject GetMutableContentConstraint(IComplexStructureReferenceObject complexRef, ComplexStructureQueryDetail returnDetail)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns ContentConstraintBeans that match the parameters in the ref bean.  If the ref bean is null or
        ///     has no attributes set, then this will be interpreted as a search for all ContentConstraintObjects
        /// </summary>
        /// <param name="complexRef">
        /// the reference object defining the search parameters, can be empty or null
        /// </param>
        /// <param name="returnDetail">
        /// The return Detail.
        /// </param>
        /// <returns>
        /// list of objects that match the search criteria
        /// </returns>
        public ISet<IContentConstraintMutableObject> GetMutableContentConstraintObjects(IComplexStructureReferenceObject complexRef, ComplexStructureQueryDetail returnDetail)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets a single data consumer scheme, this expects the ref object either to contain
        ///     a URN or all the attributes required to uniquely identify the object.  If version information
        ///     is missing then the latest version is assumed.
        /// </summary>
        /// <param name="complexRef">
        /// The reference object defining the search parameters.
        /// </param>
        /// <param name="returnDetail">
        /// The return Detail.
        /// </param>
        /// <returns>
        /// The <see cref="T:Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base.IDataConsumerSchemeMutableObject"/> .
        /// </returns>
        public IDataConsumerSchemeMutableObject GetMutableDataConsumerScheme(IComplexStructureReferenceObject complexRef, ComplexStructureQueryDetail returnDetail)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets DataConsumerSchemeMutableObjects that match the parameters in the ref @object.  If the ref @object is null or
        ///     has no attributes set, then this will be interpreted as a search for all CodelistObjects
        /// </summary>
        /// <param name="complexRef">
        /// The reference object defining the search parameters.
        /// </param>
        /// <param name="returnDetail">
        /// The return Detail.
        /// </param>
        /// <returns>
        /// list of sdmxObjects that match the search criteria
        /// </returns>
        public ISet<IDataConsumerSchemeMutableObject> GetMutableDataConsumerSchemeObjects(IComplexStructureReferenceObject complexRef, ComplexStructureQueryDetail returnDetail)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets a single Data Provider Scheme, this expects the ref object either to contain
        ///     a URN or all the attributes required to uniquely identify the object.  If version information
        ///     is missing then the latest version is assumed.
        /// </summary>
        /// <param name="complexRef">
        /// The reference object defining the search parameters.
        /// </param>
        /// <param name="returnDetail">
        /// The return Detail.
        /// </param>
        /// <returns>
        /// The <see cref="T:Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base.IDataProviderSchemeMutableObject"/> .
        /// </returns>
        public IDataProviderSchemeMutableObject GetMutableDataProviderScheme(IComplexStructureReferenceObject complexRef, ComplexStructureQueryDetail returnDetail)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets DataProviderSchemeMutableObjects that match the parameters in the ref @object.  If the ref @object is null or
        ///     has no attributes set, then this will be interpreted as a search for all CodelistObjects
        /// </summary>
        /// <param name="complexRef">
        /// The reference object defining the search parameters.
        /// </param>
        /// <param name="returnDetail">
        /// The return Detail.
        /// </param>
        /// <returns>
        /// list of sdmxObjects that match the search criteria
        /// </returns>
        public ISet<IDataProviderSchemeMutableObject> GetMutableDataProviderSchemeObjects(IComplexStructureReferenceObject complexRef, ComplexStructureQueryDetail returnDetail)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets a single DataStructure.
        ///     This expects the ref object either to contain a URN or all the attributes required to uniquely identify the object.
        ///     If version information is missing then the latest version is assumed.
        /// </summary>
        /// <param name="complexRef">
        /// The reference object defining the search parameters.
        /// </param>
        /// <param name="returnDetail">
        /// The return Detail.
        /// </param>
        /// <returns>
        /// The <see cref="T:Org.Sdmxsource.Sdmx.Api.Model.Mutable.DataStructure.IDataStructureMutableObject"/> .
        /// </returns>
        public IDataStructureMutableObject GetMutableDataStructure(IComplexStructureReferenceObject complexRef, ComplexStructureQueryDetail returnDetail)
        {
            var xref = complexRef.GetMaintainableRefObject();
            return xref.HasVersion()
                       ? this._retrievalEngineContainer.DSDRetrievalEngine.Retrieve(xref, returnDetail.EnumType, complexRef.GetVersionConstraints()).FirstOrDefault()
                       : this._retrievalEngineContainer.DSDRetrievalEngine.RetrieveLatest(xref, returnDetail.EnumType);
        }

        /// <summary>
        /// Gets DataStructureObjects that match the parameters in the ref @object.  If the ref @object is null or
        ///     has no attributes set, then this will be interpreted as a search for all dataStructureObjects
        /// </summary>
        /// <param name="complexRef">
        /// The reference object defining the search parameters.
        /// </param>
        /// <param name="returnDetail">
        /// The return Detail.
        /// </param>
        /// <returns>
        /// list of sdmxObjects that match the search criteria
        /// </returns>
        public ISet<IDataStructureMutableObject> GetMutableDataStructureObjects(IComplexStructureReferenceObject complexRef, ComplexStructureQueryDetail returnDetail)
        {
            var xref = complexRef.GetMaintainableRefObject();
            return this._retrievalEngineContainer.DSDRetrievalEngine.Retrieve(xref, returnDetail.EnumType, complexRef.GetVersionConstraints());
        }

        /// <summary>
        /// Gets a single Dataflow , this expects the ref object either to contain
        ///     a URN or all the attributes required to uniquely identify the object.  If version information
        ///     is missing then the latest version is assumed.
        /// </summary>
        /// <param name="complexRef">
        /// The reference object defining the search parameters.
        /// </param>
        /// <param name="returnDetail">
        /// The return Detail.
        /// </param>
        /// <returns>
        /// The <see cref="T:Org.Sdmxsource.Sdmx.Api.Model.Mutable.DataStructure.IDataflowMutableObject"/> .
        /// </returns>
        public IDataflowMutableObject GetMutableDataflow(IComplexStructureReferenceObject complexRef, ComplexStructureQueryDetail returnDetail)
        {
            var xref = complexRef.GetMaintainableRefObject();
            return xref.HasVersion()
                       ? this._retrievalEngineContainer.DataflowRetrievalEngine.Retrieve(xref, returnDetail.EnumType, complexRef.GetVersionConstraints()).FirstOrDefault()
                       : this._retrievalEngineContainer.DataflowRetrievalEngine.RetrieveLatest(xref, returnDetail.EnumType);
        }

        /// <summary>
        /// Gets DataflowObjects that match the parameters in the ref @object.  If the ref @object is null or
        ///     has no attributes set, then this will be interpreted as a search for all DataflowObjects
        /// </summary>
        /// <param name="complexRef">
        /// The reference object defining the search parameters.
        /// </param>
        /// <param name="returnDetail">
        /// The return Detail.
        /// </param>
        /// <returns>
        /// list of sdmxObjects that match the search criteria
        /// </returns>
        public ISet<IDataflowMutableObject> GetMutableDataflowObjects(IComplexStructureReferenceObject complexRef, ComplexStructureQueryDetail returnDetail)
        {
            var xref = complexRef.GetMaintainableRefObject();
            return this._retrievalEngineContainer.DataflowRetrievalEngine.Retrieve(xref, returnDetail.EnumType, complexRef.GetVersionConstraints());
        }

        /// <summary>
        /// Gets a single HierarchicCodeList , this expects the ref object either to contain
        ///     a URN or all the attributes required to uniquely identify the object.  If version information
        ///     is missing then the latest version is assumed.
        /// </summary>
        /// <param name="complexRef">
        /// The reference object defining the search parameters.
        /// </param>
        /// <param name="returnDetail">
        /// The return Detail.
        /// </param>
        /// <returns>
        /// The <see cref="T:Org.Sdmxsource.Sdmx.Api.Model.Mutable.Codelist.IHierarchicalCodelistMutableObject"/> .
        /// </returns>
        public IHierarchicalCodelistMutableObject GetMutableHierarchicCodeList(IComplexStructureReferenceObject complexRef, ComplexStructureQueryDetail returnDetail)
        {
            var xref = complexRef.GetMaintainableRefObject();
            return xref.HasVersion()
                       ? this._retrievalEngineContainer.HclRetrievalEngine.Retrieve(xref, returnDetail.EnumType, complexRef.GetVersionConstraints()).FirstOrDefault()
                       : this._retrievalEngineContainer.HclRetrievalEngine.RetrieveLatest(xref, returnDetail.EnumType);
        }

        /// <summary>
        /// Gets HierarchicalCodelistObjects that match the parameters in the ref @object.  If the ref @object is null or
        ///     has no attributes set, then this will be interpreted as a search for all HierarchicalCodelistObjects
        /// </summary>
        /// <param name="complexRef">
        /// The reference object defining the search parameters.
        /// </param>
        /// <param name="returnDetail">
        /// The return Detail.
        /// </param>
        /// <returns>
        /// list of sdmxObjects that match the search criteria
        /// </returns>
        public ISet<IHierarchicalCodelistMutableObject> GetMutableHierarchicCodeListObjects(IComplexStructureReferenceObject complexRef, ComplexStructureQueryDetail returnDetail)
        {
            var xref = complexRef.GetMaintainableRefObject();
            return this._retrievalEngineContainer.HclRetrievalEngine.Retrieve(xref, returnDetail.EnumType, complexRef.GetVersionConstraints());
        }

        /// <summary>
        /// Gets a set of maintainable objects which includes the maintainable being queried for, defined by the StructureQueryObject parameter.
        ///     <p/>
        ///     Expects only ONE maintainable to be returned from this query
        /// </summary>
        /// <param name="complexRef">
        /// The complex Ref.
        /// </param>
        /// <param name="returnDetail">
        /// The return Detail.
        /// </param>
        /// <returns>
        /// The <see cref="T:Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base.IMaintainableMutableObject"/> .
        /// </returns>
        public IMaintainableMutableObject GetMutableMaintainable(IComplexStructureReferenceObject complexRef, ComplexStructureQueryDetail returnDetail)
        {
            switch (complexRef.ReferencedStructureType.EnumType)
            {
                case SdmxStructureEnumType.AgencyScheme:
                    return this.GetMutableAgencyScheme(complexRef, returnDetail);
                case SdmxStructureEnumType.DataConsumerScheme:
                    return this.GetMutableDataConsumerScheme(complexRef, returnDetail);
                case SdmxStructureEnumType.DataProviderScheme:
                    return this.GetMutableDataProviderScheme(complexRef, returnDetail);
                case SdmxStructureEnumType.Categorisation:
                    return this.GetMutableCategorisation(complexRef, returnDetail);
                case SdmxStructureEnumType.CategoryScheme:
                    return this.GetMutableCategoryScheme(complexRef, returnDetail);
                case SdmxStructureEnumType.CodeList:
                    return this.GetMutableCodelist(complexRef, returnDetail);
                case SdmxStructureEnumType.ConceptScheme:
                    return this.GetMutableConceptScheme(complexRef, returnDetail);
                case SdmxStructureEnumType.Dataflow:
                    return this.GetMutableDataflow(complexRef, returnDetail);
                case SdmxStructureEnumType.HierarchicalCodelist:
                    return this.GetMutableHierarchicCodeList(complexRef, returnDetail);
                case SdmxStructureEnumType.Dsd:
                    return this.GetMutableDataStructure(complexRef, returnDetail);
                case SdmxStructureEnumType.MetadataFlow:
                    return this.GetMutableMetadataflow(complexRef, returnDetail);
                case SdmxStructureEnumType.Msd:
                    return this.GetMutableMetadataStructure(complexRef, returnDetail);
                case SdmxStructureEnumType.OrganisationUnitScheme:
                    return this.GetMutableOrganisationUnitScheme(complexRef, returnDetail);
                case SdmxStructureEnumType.Process:
                    return this.GetMutableProcessObject(complexRef, returnDetail);
                case SdmxStructureEnumType.ReportingTaxonomy:
                    return this.GetMutableReportingTaxonomy(complexRef, returnDetail);
                case SdmxStructureEnumType.StructureSet:
                    return this.GetMutableStructureSet(complexRef, returnDetail);
                case SdmxStructureEnumType.ProvisionAgreement:
                    return this.GetMutableProvisionAgreement(complexRef, returnDetail);
                case SdmxStructureEnumType.ContentConstraint:
                    return this.GetMutableContentConstraint(complexRef, returnDetail);
                default:
                    throw new SdmxNotImplementedException(ExceptionCode.Unsupported, complexRef.ReferencedStructureType);
            }
        }

        /// <summary>
        /// Gets a set of maintainable objects which includes the maintainable being queried for, defined by the StructureQueryObject parameter.
        /// </summary>
        /// <param name="complexRef">
        /// The complex Ref.
        /// </param>
        /// <param name="returnDetail">
        /// The return Detail.
        /// </param>
        /// <returns>
        /// The <see cref="T:System.Collections.Generic.ISet`1"/> .
        /// </returns>
        public ISet<IMaintainableMutableObject> GetMutableMaintainables(IComplexStructureReferenceObject complexRef, ComplexStructureQueryDetail returnDetail)
        {
            switch (complexRef.ReferencedStructureType.EnumType)
            {
                case SdmxStructureEnumType.AgencyScheme:
                    return new HashSet<IMaintainableMutableObject>(this.GetMutableAgencySchemeObjects(complexRef, returnDetail));
                case SdmxStructureEnumType.DataConsumerScheme:
                    return new HashSet<IMaintainableMutableObject>(this.GetMutableDataConsumerSchemeObjects(complexRef, returnDetail));
                case SdmxStructureEnumType.DataProviderScheme:
                    return new HashSet<IMaintainableMutableObject>(this.GetMutableDataProviderSchemeObjects(complexRef, returnDetail));
                case SdmxStructureEnumType.Categorisation:
                    return new HashSet<IMaintainableMutableObject>(this.GetMutableCategorisationObjects(complexRef, returnDetail));
                case SdmxStructureEnumType.CategoryScheme:
                    return new HashSet<IMaintainableMutableObject>(this.GetMutableCategorySchemeObjects(complexRef, returnDetail));
                case SdmxStructureEnumType.CodeList:
                    return new HashSet<IMaintainableMutableObject>(this.GetMutableCodelistObjects(complexRef, returnDetail));
                case SdmxStructureEnumType.ConceptScheme:
                    return new HashSet<IMaintainableMutableObject>(this.GetMutableConceptSchemeObjects(complexRef, returnDetail));
                case SdmxStructureEnumType.Dataflow:
                    return new HashSet<IMaintainableMutableObject>(this.GetMutableDataflowObjects(complexRef, returnDetail));
                case SdmxStructureEnumType.HierarchicalCodelist:
                    return new HashSet<IMaintainableMutableObject>(this.GetMutableHierarchicCodeListObjects(complexRef, returnDetail));
                case SdmxStructureEnumType.Dsd:
                    return new HashSet<IMaintainableMutableObject>(this.GetMutableDataStructureObjects(complexRef, returnDetail));
                case SdmxStructureEnumType.MetadataFlow:
                    return new HashSet<IMaintainableMutableObject>(this.GetMutableMetadataflowObjects(complexRef, returnDetail));
                case SdmxStructureEnumType.Msd:
                    return new HashSet<IMaintainableMutableObject>(this.GetMutableMetadataStructureObjects(complexRef, returnDetail));
                case SdmxStructureEnumType.OrganisationUnitScheme:
                    return new HashSet<IMaintainableMutableObject>(this.GetMutableOrganisationUnitSchemeObjects(complexRef, returnDetail));
                case SdmxStructureEnumType.Process:
                    return new HashSet<IMaintainableMutableObject>(this.GetMutableProcessObjects(complexRef, returnDetail));
                case SdmxStructureEnumType.ReportingTaxonomy:
                    return new HashSet<IMaintainableMutableObject>(this.GetMutableReportingTaxonomyObjects(complexRef, returnDetail));
                case SdmxStructureEnumType.StructureSet:
                    return new HashSet<IMaintainableMutableObject>(this.GetMutableStructureSetObjects(complexRef, returnDetail));
                case SdmxStructureEnumType.ProvisionAgreement:
                    return new HashSet<IMaintainableMutableObject>(this.GetMutableProvisionAgreementBeans(complexRef, returnDetail));
                case SdmxStructureEnumType.ContentConstraint:
                    return new HashSet<IMaintainableMutableObject>(this.GetMutableContentConstraintObjects(complexRef, returnDetail));
                default:
                    throw new SdmxNotImplementedException(ExceptionCode.Unsupported, complexRef.ReferencedStructureType);
            }
        }

        /// <summary>
        /// Gets a single MetadataStructure , this expects the ref object either to contain
        ///     a URN or all the attributes required to uniquely identify the object.  If version information
        ///     is missing then the latest version is assumed.
        /// </summary>
        /// <param name="complexRef">
        /// The reference object defining the search parameters.
        /// </param>
        /// <param name="returnDetail">
        /// The return Detail.
        /// </param>
        /// <returns>
        /// The <see cref="T:Org.Sdmxsource.Sdmx.Api.Model.Mutable.MetadataStructure.IMetadataStructureDefinitionMutableObject"/> .
        /// </returns>
        public IMetadataStructureDefinitionMutableObject GetMutableMetadataStructure(IComplexStructureReferenceObject complexRef, ComplexStructureQueryDetail returnDetail)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets MetadataStructureObjects that match the parameters in the ref @object.  If the ref @object is null or
        ///     has no attributes set, then this will be interpreted as a search for all MetadataStructureObjects
        /// </summary>
        /// <param name="complexRef">
        /// The reference object defining the search parameters.
        /// </param>
        /// <param name="returnDetail">
        /// The return Detail.
        /// </param>
        /// <returns>
        /// list of sdmxObjects that match the search criteria
        /// </returns>
        public ISet<IMetadataStructureDefinitionMutableObject> GetMutableMetadataStructureObjects(IComplexStructureReferenceObject complexRef, ComplexStructureQueryDetail returnDetail)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets a single Metadataflow , this expects the ref object either to contain
        ///     a URN or all the attributes required to uniquely identify the object.  If version information
        ///     is missing then the latest version is assumed.
        /// </summary>
        /// <param name="complexRef">
        /// The reference object defining the search parameters.
        /// </param>
        /// <param name="returnDetail">
        /// The return Detail.
        /// </param>
        /// <returns>
        /// The <see cref="T:Org.Sdmxsource.Sdmx.Api.Model.Mutable.MetadataStructure.IMetadataFlowMutableObject"/> .
        /// </returns>
        public IMetadataFlowMutableObject GetMutableMetadataflow(IComplexStructureReferenceObject complexRef, ComplexStructureQueryDetail returnDetail)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets MetadataFlowObjects that match the parameters in the ref @object.  If the ref @object is null or
        ///     has no attributes set, then this will be interpreted as a search for all MetadataFlowObjects
        /// </summary>
        /// <param name="complexRef">
        /// The reference object defining the search parameters.
        /// </param>
        /// <param name="returnDetail">
        /// The return Detail.
        /// </param>
        /// <returns>
        /// list of sdmxObjects that match the search criteria
        /// </returns>
        public ISet<IMetadataFlowMutableObject> GetMutableMetadataflowObjects(IComplexStructureReferenceObject complexRef, ComplexStructureQueryDetail returnDetail)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets a single organization scheme, this expects the ref object either to contain
        ///     a URN or all the attributes required to uniquely identify the object.  If version information
        ///     is missing then the latest version is assumed.
        /// </summary>
        /// <param name="complexRef">
        /// The reference object defining the search parameters.
        /// </param>
        /// <param name="returnDetail">
        /// The return Detail.
        /// </param>
        /// <returns>
        /// The <see cref="T:Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base.IOrganisationUnitSchemeMutableObject"/> .
        /// </returns>
        public IOrganisationUnitSchemeMutableObject GetMutableOrganisationUnitScheme(IComplexStructureReferenceObject complexRef, ComplexStructureQueryDetail returnDetail)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets OrganisationUnitSchemeMutableObject that match the parameters in the ref @object.  If the ref @object is null or
        ///     has no attributes set, then this will be interpreted as a search for all OrganisationUnitSchemeMutableObject
        /// </summary>
        /// <param name="complexRef">
        /// The reference object defining the search parameters.
        /// </param>
        /// <param name="returnDetail">
        /// The return Detail.
        /// </param>
        /// <returns>
        /// list of sdmxObjects that match the search criteria
        /// </returns>
        public ISet<IOrganisationUnitSchemeMutableObject> GetMutableOrganisationUnitSchemeObjects(IComplexStructureReferenceObject complexRef, ComplexStructureQueryDetail returnDetail)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets a process @object, this expects the ref object either to contain
        ///     a URN or all the attributes required to uniquely identify the object.  If version information
        ///     is missing then the latest version is assumed.
        /// </summary>
        /// <param name="complexRef">
        /// The reference object defining the search parameters.
        /// </param>
        /// <param name="returnDetail">
        /// The return Detail.
        /// </param>
        /// <returns>
        /// The <see cref="T:Org.Sdmxsource.Sdmx.Api.Model.Mutable.Process.IProcessMutableObject"/> .
        /// </returns>
        public IProcessMutableObject GetMutableProcessObject(IComplexStructureReferenceObject complexRef, ComplexStructureQueryDetail returnDetail)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets ProcessObjects that match the parameters in the ref @object.  If the ref @object is null or
        ///     has no attributes set, then this will be interpreted as a search for all IProcessObject
        /// </summary>
        /// <param name="complexRef">
        /// The reference object defining the search parameters.
        /// </param>
        /// <param name="returnDetail">
        /// The return Detail.
        /// </param>
        /// <returns>
        /// list of sdmxObjects that match the search criteria
        /// </returns>
        public ISet<IProcessMutableObject> GetMutableProcessObjects(IComplexStructureReferenceObject complexRef, ComplexStructureQueryDetail returnDetail)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns a provision agreement bean, this expects the ref object to contain
        ///     all the attributes required to uniquely identify the object.  If version information
        ///     is missing then the latest version is assumed.
        /// </summary>
        /// <param name="complexRef">
        /// The reference object defining the search parameters.
        /// </param>
        /// <param name="returnDetail">
        /// The return Detail.
        /// </param>
        /// <returns>
        /// list of sdmxObjects that match the search criteria
        /// </returns>
        public IProvisionAgreementMutableObject GetMutableProvisionAgreement(IComplexStructureReferenceObject complexRef, ComplexStructureQueryDetail returnDetail)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns ProvisionAgreement beans that match the parameters in the ref bean. If the ref bean is null or
        ///     has no attributes set, then this will be interpreted as a search for all ProvisionAgreement beans.
        /// </summary>
        /// <param name="complexRef">
        /// the reference object defining the search parameters, can be empty or null
        /// </param>
        /// <param name="returnDetail">
        /// The return Detail.
        /// </param>
        /// <returns>
        /// list of objects that match the search criteria
        /// </returns>
        public ISet<IProvisionAgreementMutableObject> GetMutableProvisionAgreementBeans(IComplexStructureReferenceObject complexRef, ComplexStructureQueryDetail returnDetail)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets a reporting taxonomy @object, this expects the ref object either to contain
        ///     a URN or all the attributes required to uniquely identify the object.  If version information
        ///     is missing then the latest version is assumed.
        /// </summary>
        /// <param name="complexRef">
        /// The reference object defining the search parameters.
        /// </param>
        /// <param name="returnDetail">
        /// The return Detail.
        /// </param>
        /// <returns>
        /// The <see cref="T:Org.Sdmxsource.Sdmx.Api.Model.Mutable.CategoryScheme.IReportingTaxonomyMutableObject"/> .
        /// </returns>
        public IReportingTaxonomyMutableObject GetMutableReportingTaxonomy(IComplexStructureReferenceObject complexRef, ComplexStructureQueryDetail returnDetail)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets ReportingTaxonomyObjects that match the parameters in the ref @object.  If the ref @object is null or
        ///     has no attributes set, then this will be interpreted as a search for all ReportingTaxonomyObjects
        /// </summary>
        /// <param name="complexRef">
        /// The reference object defining the search parameters.
        /// </param>
        /// <param name="returnDetail">
        /// The return Detail.
        /// </param>
        /// <returns>
        /// list of sdmxObjects that match the search criteria
        /// </returns>
        public ISet<IReportingTaxonomyMutableObject> GetMutableReportingTaxonomyObjects(IComplexStructureReferenceObject complexRef, ComplexStructureQueryDetail returnDetail)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets a structure set @object, this expects the ref object either to contain
        ///     a URN or all the attributes required to uniquely identify the object.  If version information
        ///     is missing then the latest version is assumed.
        /// </summary>
        /// <param name="complexRef">
        /// The reference object defining the search parameters.
        /// </param>
        /// <param name="returnDetail">
        /// The return Detail.
        /// </param>
        /// <returns>
        /// The <see cref="T:Org.Sdmxsource.Sdmx.Api.Model.Mutable.Mapping.IStructureSetMutableObject"/> .
        /// </returns>
        public IStructureSetMutableObject GetMutableStructureSet(IComplexStructureReferenceObject complexRef, ComplexStructureQueryDetail returnDetail)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets StructureSetObjects that match the parameters in the ref @object.  If the ref @object is null or
        ///     has no attributes set, then this will be interpreted as a search for all StructureSetObjects
        /// </summary>
        /// <param name="complexRef">
        /// The reference object defining the search parameters.
        /// </param>
        /// <param name="returnDetail">
        /// The return Detail.
        /// </param>
        /// <returns>
        /// list of sdmxObjects that match the search criteria
        /// </returns>
        public ISet<IStructureSetMutableObject> GetMutableStructureSetObjects(IComplexStructureReferenceObject complexRef, ComplexStructureQueryDetail returnDetail)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}