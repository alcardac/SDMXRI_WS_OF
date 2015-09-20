// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SdmxStructureRetrievalManagerImpl.cs" company="Eurostat">
//   Date Created : 2012-10-12
//   //   Copyright (c) 2012 by the European   Commission, represented by Eurostat.   All rights reserved.
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   Retrieves structures and writes the results to the writer engine.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.Structureretrieval.Manager
{
    using System;
    using System.Collections.Generic;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Engine;
    using Org.Sdmxsource.Sdmx.Api.Exception;
    using Org.Sdmxsource.Sdmx.Api.Manager.Retrieval;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Categoryscheme;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Codelist;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Conceptscheme;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Datastructure;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Mapping;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Metadatastructure;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Process;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;
    using Org.Sdmxsource.Sdmx.Util.Objects.Container;

    /// <summary>
    ///  Retrieves structures and writes the results to the writer engine.
    /// </summary>
    public class SdmxStructureRetrievalManagerImpl : AbstractRetrevalManager, ISdmxStructureRetrievalManager
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SdmxStructureRetrievalManagerImpl"/> class.
        /// </summary>
        /// <param name="sdmxObjectRetrievalManager">
        /// The <c>SDMX</c> structure retrieval manager.
        /// </param>
        public SdmxStructureRetrievalManagerImpl(ISdmxRetrievalManager sdmxObjectRetrievalManager)
            : base(sdmxObjectRetrievalManager)
        {
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The get agencies.
        /// </summary>
        /// <param name="structureWriter">
        /// The structure writer.
        /// </param>
        /// <exception cref="UnsupportedException">
        /// Not implemented.
        /// </exception>
        public virtual void GetAgencies(IStructureWritingEngine structureWriter)
        {
            throw new UnsupportedException(ExceptionCode.Unsupported);
        }

        /// <summary>
        /// The get agency.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <param name="structureWriter">
        /// The structure writer.
        /// </param>
        /// <exception cref="UnsupportedException">
        /// </exception>
        public virtual void GetAgency(string id, IStructureWritingEngine structureWriter)
        {
            throw new UnsupportedException(ExceptionCode.Unsupported);
        }

        /// <summary>
        /// The get categorisation.
        /// </summary>
        /// <param name="xref">
        /// The xref.
        /// </param>
        /// <param name="structureWriter">
        /// The structure writer.
        /// </param>
        /// <exception cref="ArgumentException">
        /// </exception>
        public virtual void GetCategorisation(IMaintainableRefObject xref, IStructureWritingEngine structureWriter)
        {
            if (structureWriter == null)
            {
                throw new ArgumentException("IStructureWritingEngine can not be null");
            }

            IMaintainableObject bean = this.SdmxObjectRetrievalManager.GetCategorisation(xref);
            structureWriter.WriteStructure(bean);
        }

        /// <summary>
        /// The get categorisation beans.
        /// </summary>
        /// <param name="xref">
        /// The xref.
        /// </param>
        /// <param name="structureWriter">
        /// The structure writer.
        /// </param>
        /// <exception cref="ArgumentException">
        /// </exception>
        public virtual void GetCategorisationBeans(IMaintainableRefObject xref, IStructureWritingEngine structureWriter)
        {
            if (structureWriter == null)
            {
                throw new ArgumentException("IStructureWritingEngine can not be null");
            }

            ISet<ICategorisationObject> mBeans = this.SdmxObjectRetrievalManager.GetCategorisationBeans(xref);
            ISdmxObjects beans = new SdmxObjectsImpl(null, mBeans);
            structureWriter.WriteStructures(beans);
        }

        /// <summary>
        /// The get category scheme.
        /// </summary>
        /// <param name="xref">
        /// The xref.
        /// </param>
        /// <param name="structureWriter">
        /// The structure writer.
        /// </param>
        /// <exception cref="ArgumentException">
        /// </exception>
        public virtual void GetCategoryScheme(IMaintainableRefObject xref, IStructureWritingEngine structureWriter)
        {
            if (structureWriter == null)
            {
                throw new ArgumentException("IStructureWritingEngine can not be null");
            }

            IMaintainableObject bean = this.SdmxObjectRetrievalManager.GetCategoryScheme(xref);
            structureWriter.WriteStructure(bean);
        }

        /// <summary>
        /// The get category scheme beans.
        /// </summary>
        /// <param name="xref">
        /// The xref.
        /// </param>
        /// <param name="structureWriter">
        /// The structure writer.
        /// </param>
        /// <exception cref="ArgumentException">
        /// </exception>
        public virtual void GetCategorySchemeBeans(IMaintainableRefObject xref, IStructureWritingEngine structureWriter)
        {
            if (structureWriter == null)
            {
                throw new ArgumentException("IStructureWritingEngine can not be null");
            }

            ISet<ICategorySchemeObject> mBeans = this.SdmxObjectRetrievalManager.GetCategorySchemeBeans(xref);
            ISdmxObjects beans = new SdmxObjectsImpl(null, mBeans);
            structureWriter.WriteStructures(beans);
        }

        /// <summary>
        /// The get codelist.
        /// </summary>
        /// <param name="xref">
        /// The xref.
        /// </param>
        /// <param name="structureWriter">
        /// The structure writer.
        /// </param>
        /// <exception cref="ArgumentException">
        /// </exception>
        public virtual void GetCodelist(IMaintainableRefObject xref, IStructureWritingEngine structureWriter)
        {
            if (structureWriter == null)
            {
                throw new ArgumentException("IStructureWritingEngine can not be null");
            }

            IMaintainableObject bean = this.SdmxObjectRetrievalManager.GetCodelist(xref);
            structureWriter.WriteStructure(bean);
        }

        /// <summary>
        /// The get codelist beans.
        /// </summary>
        /// <param name="xref">
        /// The xref.
        /// </param>
        /// <param name="structureWriter">
        /// The structure writer.
        /// </param>
        /// <exception cref="ArgumentException">
        /// </exception>
        public virtual void GetCodelistBeans(IMaintainableRefObject xref, IStructureWritingEngine structureWriter)
        {
            if (structureWriter == null)
            {
                throw new ArgumentException("IStructureWritingEngine can not be null");
            }

            ISet<ICodelistObject> mBeans = this.SdmxObjectRetrievalManager.GetCodelistBeans(xref);
            ISdmxObjects beans = new SdmxObjectsImpl(null, mBeans);
            structureWriter.WriteStructures(beans);
        }

        /// <summary>
        /// The get concept scheme.
        /// </summary>
        /// <param name="xref">
        /// The xref.
        /// </param>
        /// <param name="structureWriter">
        /// The structure writer.
        /// </param>
        /// <exception cref="ArgumentException">
        /// </exception>
        public virtual void GetConceptScheme(IMaintainableRefObject xref, IStructureWritingEngine structureWriter)
        {
            if (structureWriter == null)
            {
                throw new ArgumentException("IStructureWritingEngine can not be null");
            }

            IMaintainableObject bean = this.SdmxObjectRetrievalManager.GetConceptScheme(xref);
            structureWriter.WriteStructure(bean);
        }

        /// <summary>
        /// The get concept scheme beans.
        /// </summary>
        /// <param name="xref">
        /// The xref.
        /// </param>
        /// <param name="structureWriter">
        /// The structure writer.
        /// </param>
        /// <exception cref="ArgumentException">
        /// </exception>
        public virtual void GetConceptSchemeBeans(IMaintainableRefObject xref, IStructureWritingEngine structureWriter)
        {
            if (structureWriter == null)
            {
                throw new ArgumentException("IStructureWritingEngine can not be null");
            }

            ISet<IConceptSchemeObject> mBeans = this.SdmxObjectRetrievalManager.GetConceptSchemeBeans(xref);
            ISdmxObjects beans = new SdmxObjectsImpl(null, mBeans);
            structureWriter.WriteStructures(beans);
        }

        /// <summary>
        /// The get data structure.
        /// </summary>
        /// <param name="xref">
        /// The xref.
        /// </param>
        /// <param name="structureWriter">
        /// The structure writer.
        /// </param>
        /// <exception cref="ArgumentException">
        /// </exception>
        public virtual void GetDataStructure(IMaintainableRefObject xref, IStructureWritingEngine structureWriter)
        {
            if (structureWriter == null)
            {
                throw new ArgumentException("IStructureWritingEngine can not be null");
            }

            IMaintainableObject bean = this.SdmxObjectRetrievalManager.GetDataStructure(xref);
            structureWriter.WriteStructure(bean);
        }

        /// <summary>
        /// The get data structure beans.
        /// </summary>
        /// <param name="xref">
        /// The xref.
        /// </param>
        /// <param name="structureWriter">
        /// The structure writer.
        /// </param>
        /// <exception cref="ArgumentException">
        /// </exception>
        public virtual void GetDataStructureBeans(IMaintainableRefObject xref, IStructureWritingEngine structureWriter)
        {
            if (structureWriter == null)
            {
                throw new ArgumentException("IStructureWritingEngine can not be null");
            }

            ISet<IDataStructureObject> mBeans = this.SdmxObjectRetrievalManager.GetDataStructureBeans(xref);
            ISdmxObjects beans = new SdmxObjectsImpl(null, mBeans);
            structureWriter.WriteStructures(beans);
        }

        /// <summary>
        /// The get dataflow.
        /// </summary>
        /// <param name="xref">
        /// The xref.
        /// </param>
        /// <param name="structureWriter">
        /// The structure writer.
        /// </param>
        /// <exception cref="ArgumentException">
        /// </exception>
        public virtual void GetDataflow(IMaintainableRefObject xref, IStructureWritingEngine structureWriter)
        {
            if (structureWriter == null)
            {
                throw new ArgumentException("IStructureWritingEngine can not be null");
            }

            IMaintainableObject bean = this.SdmxObjectRetrievalManager.GetDataflow(xref);
            structureWriter.WriteStructure(bean);
        }

        /// <summary>
        /// The get dataflow beans.
        /// </summary>
        /// <param name="xref">
        /// The xref.
        /// </param>
        /// <param name="structureWriter">
        /// The structure writer.
        /// </param>
        /// <exception cref="ArgumentException">
        /// </exception>
        public virtual void GetDataflowBeans(IMaintainableRefObject xref, IStructureWritingEngine structureWriter)
        {
            if (structureWriter == null)
            {
                throw new ArgumentException("IStructureWritingEngine can not be null");
            }

            ISet<IDataflowObject> mBeans = this.SdmxObjectRetrievalManager.GetDataflowBeans(xref);
            ISdmxObjects beans = new SdmxObjectsImpl(null, mBeans);
            structureWriter.WriteStructures(beans);
        }

        /// <summary>
        /// The get hierarchic code list.
        /// </summary>
        /// <param name="xref">
        /// The xref.
        /// </param>
        /// <param name="structureWriter">
        /// The structure writer.
        /// </param>
        /// <exception cref="ArgumentException">
        /// </exception>
        public virtual void GetHierarchicCodeList(IMaintainableRefObject xref, IStructureWritingEngine structureWriter)
        {
            if (structureWriter == null)
            {
                throw new ArgumentException("IStructureWritingEngine can not be null");
            }

            IMaintainableObject bean = this.SdmxObjectRetrievalManager.GetHierarchicCodeList(xref);
            structureWriter.WriteStructure(bean);
        }

        /// <summary>
        /// The get hierarchic code list beans.
        /// </summary>
        /// <param name="xref">
        /// The xref.
        /// </param>
        /// <param name="structureWriter">
        /// The structure writer.
        /// </param>
        /// <exception cref="ArgumentException">
        /// </exception>
        public virtual void GetHierarchicCodeListBeans(
            IMaintainableRefObject xref, IStructureWritingEngine structureWriter)
        {
            if (structureWriter == null)
            {
                throw new ArgumentException("IStructureWritingEngine can not be null");
            }

            ISet<IHierarchicalCodelistObject> mBeans = this.SdmxObjectRetrievalManager.GetHierarchicCodeListBeans(xref);
            ISdmxObjects beans = new SdmxObjectsImpl(null, mBeans);
            structureWriter.WriteStructures(beans);
        }

        /// <summary>
        /// The get maintainable.
        /// </summary>
        /// <param name="query">
        /// The query.
        /// </param>
        /// <param name="structureWriter">
        /// The structure writer.
        /// </param>
        /// <exception cref="ArgumentException">
        /// </exception>
        public virtual void GetMaintainable(IStructureReference query, IStructureWritingEngine structureWriter)
        {
            if (structureWriter == null)
            {
                throw new ArgumentException("IStructureWritingEngine can not be null");
            }

            IMaintainableObject bean = this.SdmxObjectRetrievalManager.GetMaintainable(query);
            structureWriter.WriteStructure(bean);
        }

        /// <summary>
        /// The get maintainable with references.
        /// </summary>
        /// <param name="query">
        /// The query.
        /// </param>
        /// <param name="structureWriter">
        /// The structure writer.
        /// </param>
        /// <exception cref="ArgumentException">
        /// </exception>
        public virtual void GetMaintainableWithReferences(
            IStructureReference query, IStructureWritingEngine structureWriter)
        {
            if (structureWriter == null)
            {
                throw new ArgumentException("IStructureWritingEngine can not be null");
            }

            ISet<IMaintainableObject> mBeans = this.SdmxObjectRetrievalManager.GetMaintainableWithReferences(query);
            ISdmxObjects beans = new SdmxObjectsImpl(null, mBeans);
            structureWriter.WriteStructures(beans);
        }

        /// <summary>
        /// The get metadata structure.
        /// </summary>
        /// <param name="xref">
        /// The xref.
        /// </param>
        /// <param name="structureWriter">
        /// The structure writer.
        /// </param>
        /// <exception cref="ArgumentException">
        /// </exception>
        public virtual void GetMetadataStructure(IMaintainableRefObject xref, IStructureWritingEngine structureWriter)
        {
            if (structureWriter == null)
            {
                throw new ArgumentException("IStructureWritingEngine can not be null");
            }

            IMaintainableObject bean = this.SdmxObjectRetrievalManager.GetMetadataStructure(xref);
            structureWriter.WriteStructure(bean);
        }

        /// <summary>
        /// The get metadata structure beans.
        /// </summary>
        /// <param name="xref">
        /// The xref.
        /// </param>
        /// <param name="structureWriter">
        /// The structure writer.
        /// </param>
        /// <exception cref="ArgumentException">
        /// </exception>
        public virtual void GetMetadataStructureBeans(
            IMaintainableRefObject xref, IStructureWritingEngine structureWriter)
        {
            if (structureWriter == null)
            {
                throw new ArgumentException("IStructureWritingEngine can not be null");
            }

            ISet<IMetadataStructureDefinitionObject> mBeans =
                this.SdmxObjectRetrievalManager.GetMetadataStructureBeans(xref);
            ISdmxObjects beans = new SdmxObjectsImpl(null, mBeans);
            structureWriter.WriteStructures(beans);
        }

        /// <summary>
        /// The get metadataflow.
        /// </summary>
        /// <param name="xref">
        /// The xref.
        /// </param>
        /// <param name="structureWriter">
        /// The structure writer.
        /// </param>
        /// <exception cref="ArgumentException">
        /// </exception>
        public virtual void GetMetadataflow(IMaintainableRefObject xref, IStructureWritingEngine structureWriter)
        {
            if (structureWriter == null)
            {
                throw new ArgumentException("IStructureWritingEngine can not be null");
            }

            IMaintainableObject bean = this.SdmxObjectRetrievalManager.GetMetadataflow(xref);
            structureWriter.WriteStructure(bean);
        }

        /// <summary>
        /// The get metadataflow beans.
        /// </summary>
        /// <param name="xref">
        /// The xref.
        /// </param>
        /// <param name="structureWriter">
        /// The structure writer.
        /// </param>
        /// <exception cref="ArgumentException">
        /// </exception>
        public virtual void GetMetadataflowBeans(IMaintainableRefObject xref, IStructureWritingEngine structureWriter)
        {
            if (structureWriter == null)
            {
                throw new ArgumentException("IStructureWritingEngine can not be null");
            }

            ISet<IMetadataFlow> mBeans = this.SdmxObjectRetrievalManager.GetMetadataflowBeans(xref);
            ISdmxObjects beans = new SdmxObjectsImpl(null, mBeans);
            structureWriter.WriteStructures(beans);
        }

        /// <summary>
        /// The get organisation scheme.
        /// </summary>
        /// <param name="xref">
        /// The xref.
        /// </param>
        /// <param name="structureWriter">
        /// The structure writer.
        /// </param>
        /// <exception cref="ArgumentException">
        /// </exception>
        public virtual void GetOrganisationScheme(IMaintainableRefObject xref, IStructureWritingEngine structureWriter)
        {
            if (structureWriter == null)
            {
                throw new ArgumentException("IStructureWritingEngine can not be null");
            }

            IMaintainableObject bean = this.SdmxObjectRetrievalManager.GetOrganisationUnitScheme(xref);
            structureWriter.WriteStructure(bean);
        }

        /// <summary>
        /// The get organisation scheme beans.
        /// </summary>
        /// <param name="xref">
        /// The xref.
        /// </param>
        /// <param name="structureWriter">
        /// The structure writer.
        /// </param>
        /// <exception cref="ArgumentException">
        /// </exception>
        public virtual void GetOrganisationSchemeBeans(
            IMaintainableRefObject xref, IStructureWritingEngine structureWriter)
        {
            if (structureWriter == null)
            {
                throw new ArgumentException("IStructureWritingEngine can not be null");
            }

            ISet<IOrganisationUnitSchemeObject> mBeans =
                this.SdmxObjectRetrievalManager.GetOrganisationUnitSchemeBeans(xref);
            ISdmxObjects beans = new SdmxObjectsImpl(null, mBeans);
            structureWriter.WriteStructures(beans);
        }

        /// <summary>
        /// The get process bean.
        /// </summary>
        /// <param name="xref">
        /// The xref.
        /// </param>
        /// <param name="structureWriter">
        /// The structure writer.
        /// </param>
        /// <exception cref="ArgumentException">
        /// </exception>
        public virtual void GetProcessBean(IMaintainableRefObject xref, IStructureWritingEngine structureWriter)
        {
            if (structureWriter == null)
            {
                throw new ArgumentException("IStructureWritingEngine can not be null");
            }

            IMaintainableObject bean = this.SdmxObjectRetrievalManager.GetProcessBean(xref);
            structureWriter.WriteStructure(bean);
        }

        /// <summary>
        /// The get process beans.
        /// </summary>
        /// <param name="xref">
        /// The xref.
        /// </param>
        /// <param name="structureWriter">
        /// The structure writer.
        /// </param>
        /// <exception cref="ArgumentException">
        /// </exception>
        public virtual void GetProcessBeans(IMaintainableRefObject xref, IStructureWritingEngine structureWriter)
        {
            if (structureWriter == null)
            {
                throw new ArgumentException("IStructureWritingEngine can not be null");
            }

            ISet<IProcessObject> mBeans = this.SdmxObjectRetrievalManager.GetProcessBeans(xref);
            ISdmxObjects beans = new SdmxObjectsImpl(null, mBeans);
            structureWriter.WriteStructures(beans);
        }

        /// <summary>
        /// The get reporting taxonomy.
        /// </summary>
        /// <param name="xref">
        /// The xref.
        /// </param>
        /// <param name="structureWriter">
        /// The structure writer.
        /// </param>
        /// <exception cref="ArgumentException">
        /// </exception>
        public virtual void GetReportingTaxonomy(IMaintainableRefObject xref, IStructureWritingEngine structureWriter)
        {
            if (structureWriter == null)
            {
                throw new ArgumentException("IStructureWritingEngine can not be null");
            }

            IMaintainableObject bean = this.SdmxObjectRetrievalManager.GetReportingTaxonomy(xref);
            structureWriter.WriteStructure(bean);
        }

        /// <summary>
        /// The get reporting taxonomy beans.
        /// </summary>
        /// <param name="xref">
        /// The xref.
        /// </param>
        /// <param name="structureWriter">
        /// The structure writer.
        /// </param>
        /// <exception cref="ArgumentException">
        /// </exception>
        public virtual void GetReportingTaxonomyBeans(
            IMaintainableRefObject xref, IStructureWritingEngine structureWriter)
        {
            if (structureWriter == null)
            {
                throw new ArgumentException("IStructureWritingEngine can not be null");
            }

            ISet<IReportingTaxonomyObject> mBeans = this.SdmxObjectRetrievalManager.GetReportingTaxonomyBeans(xref);
            ISdmxObjects beans = new SdmxObjectsImpl(null, mBeans);
            structureWriter.WriteStructures(beans);
        }

        /// <summary>
        /// The get structure set.
        /// </summary>
        /// <param name="xref">
        /// The xref.
        /// </param>
        /// <param name="structureWriter">
        /// The structure writer.
        /// </param>
        /// <exception cref="ArgumentException">
        /// </exception>
        public virtual void GetStructureSet(IMaintainableRefObject xref, IStructureWritingEngine structureWriter)
        {
            if (structureWriter == null)
            {
                throw new ArgumentException("IStructureWritingEngine can not be null");
            }

            IMaintainableObject bean = this.SdmxObjectRetrievalManager.GetStructureSet(xref);
            structureWriter.WriteStructure(bean);
        }

        /// <summary>
        /// The get structure set beans.
        /// </summary>
        /// <param name="xref">
        /// The xref.
        /// </param>
        /// <param name="structureWriter">
        /// The structure writer.
        /// </param>
        /// <exception cref="ArgumentException">
        /// </exception>
        public virtual void GetStructureSetBeans(IMaintainableRefObject xref, IStructureWritingEngine structureWriter)
        {
            if (structureWriter == null)
            {
                throw new ArgumentException("IStructureWritingEngine can not be null");
            }

            ISet<IStructureSetObject> mBeans = this.SdmxObjectRetrievalManager.GetStructureSetBeans(xref);
            ISdmxObjects beans = new SdmxObjectsImpl(null, mBeans);
            structureWriter.WriteStructures(beans);
        }

        #endregion
    }
}