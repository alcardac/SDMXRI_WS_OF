// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RetrievalEngineContainer.cs" company="Eurostat">
//   Date Created : 2013-03-04
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   The retrieval engine container.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Estat.Sri.MappingStoreRetrieval.Helper
{
    using Estat.Sri.MappingStoreRetrieval.Config;
    using Estat.Sri.MappingStoreRetrieval.Constants;
    using Estat.Sri.MappingStoreRetrieval.Engine;
    using Estat.Sri.MappingStoreRetrieval.Manager;

    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.CategoryScheme;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Codelist;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.ConceptScheme;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.DataStructure;

    /// <summary>
    /// The retrieval engine container.
    /// </summary>
    internal class RetrievalEngineContainer
    {
        #region Fields

        /// <summary>
        ///     The categorisation from dataflow retrieval engine.
        /// </summary>
        private readonly IRetrievalEngine<ICategorisationMutableObject> _categorisationRetrievalEngine;

        /// <summary>
        ///     The category scheme retrieval engine.
        /// </summary>
        private readonly IRetrievalEngine<ICategorySchemeMutableObject> _categorySchemeRetrievalEngine;

        /// <summary>
        ///     The code list retrieval engine.
        /// </summary>
        private readonly IRetrievalEngine<ICodelistMutableObject> _codeListRetrievalEngine;

        /// <summary>
        ///     The concept scheme retrieval engine.
        /// </summary>
        private readonly IRetrievalEngine<IConceptSchemeMutableObject> _conceptSchemeRetrievalEngine;

        /// <summary>
        ///     The dataflow retrieval engine.
        /// </summary>
        private readonly IRetrievalEngine<IDataflowMutableObject> _dataflowRetrievalEngine;

        /// <summary>
        ///     The DSD retrieval engine.
        /// </summary>
        private readonly IRetrievalEngine<IDataStructureMutableObject> _dsdRetrievalEngine;

        /// <summary>
        ///     The HCL retrieval engine.
        /// </summary>
        private readonly IRetrievalEngine<IHierarchicalCodelistMutableObject> _hclRetrievalEngine;

        /// <summary>
        /// The _partial code list retrieval engine.
        /// </summary>
        private readonly PartialCodeListRetrievalEngine _partialCodeListRetrievalEngine;
        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RetrievalEngineContainer"/> class. 
        /// </summary>
        /// <param name="mappingStoreDB">
        /// The mapping Store DB.
        /// </param>
        public RetrievalEngineContainer(Database mappingStoreDB)
        {
            DataflowFilter filter = ConfigManager.Config.DataflowConfiguration.IgnoreProductionForStructure ? DataflowFilter.Any : DataflowFilter.Production;
            this._categorisationRetrievalEngine = new CategorisationRetrievalEngine(mappingStoreDB, filter);
            this._categorySchemeRetrievalEngine = new CategorySchemeRetrievalEngine(mappingStoreDB);
            this._codeListRetrievalEngine = new CodeListRetrievalEngine(mappingStoreDB);
            this._conceptSchemeRetrievalEngine = new ConceptSchemeRetrievalEngine(mappingStoreDB);
            this._dataflowRetrievalEngine = new DataflowRetrievalEngine(mappingStoreDB, filter);
            this._dsdRetrievalEngine = new DsdRetrievalEngine(mappingStoreDB);
            this._hclRetrievalEngine = new HierarchicalCodeListRetrievealEngine(mappingStoreDB);
            this._partialCodeListRetrievalEngine = new PartialCodeListRetrievalEngine(mappingStoreDB);
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets the categorisation from dataflow retrieval engine.
        /// </summary>
        public IRetrievalEngine<ICategorisationMutableObject> CategorisationRetrievalEngine
        {
            get
            {
                return this._categorisationRetrievalEngine;
            }
        }

        /// <summary>
        ///     Gets the category scheme retrieval engine.
        /// </summary>
        public IRetrievalEngine<ICategorySchemeMutableObject> CategorySchemeRetrievalEngine
        {
            get
            {
                return this._categorySchemeRetrievalEngine;
            }
        }

        /// <summary>
        ///     Gets the code list retrieval engine.
        /// </summary>
        public IRetrievalEngine<ICodelistMutableObject> CodeListRetrievalEngine
        {
            get
            {
                return this._codeListRetrievalEngine;
            }
        }

        /// <summary>
        ///     Gets the concept scheme retrieval engine.
        /// </summary>
        public IRetrievalEngine<IConceptSchemeMutableObject> ConceptSchemeRetrievalEngine
        {
            get
            {
                return this._conceptSchemeRetrievalEngine;
            }
        }

        /// <summary>
        ///     Gets the DSD retrieval engine.
        /// </summary>
        public IRetrievalEngine<IDataStructureMutableObject> DSDRetrievalEngine
        {
            get
            {
                return this._dsdRetrievalEngine;
            }
        }

        /// <summary>
        ///     Gets the dataflow retrieval engine.
        /// </summary>
        public IRetrievalEngine<IDataflowMutableObject> DataflowRetrievalEngine
        {
            get
            {
                return this._dataflowRetrievalEngine;
            }
        }

        /// <summary>
        ///     Gets the HCL retrieval engine.
        /// </summary>
        public IRetrievalEngine<IHierarchicalCodelistMutableObject> HclRetrievalEngine
        {
            get
            {
                return this._hclRetrievalEngine;
            }
        }

        /// <summary>
        /// Gets the partial code list retrieval engine.
        /// </summary>
        public PartialCodeListRetrievalEngine PartialCodeListRetrievalEngine
        {
            get
            {
                return this._partialCodeListRetrievalEngine;
            }
        }

        #endregion
    }
}