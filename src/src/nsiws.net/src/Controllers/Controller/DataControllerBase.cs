// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataControllerBase.cs" company="Eurostat">
//   Date Created : 2013-10-11
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   The data controller base.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Estat.Sri.Ws.Controllers.Controller
{
    using System.Globalization;
    using System.Linq;

    using Estat.Nsi.AuthModule;
    using Estat.Nsi.DataRetriever.Properties;
    using Estat.Sri.MappingStoreRetrieval.Manager;
    using Estat.Sri.Ws.Controllers.Manager;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Exception;
    using Org.Sdmxsource.Sdmx.Api.Manager.Retrieval;
    using Org.Sdmxsource.Sdmx.Api.Model.Data.Query;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.DataStructure;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;
    using Org.Sdmxsource.Sdmx.Api.Util;
    using Org.Sdmxsource.Sdmx.Structureparser.Manager.Parsing;

    /// <summary>
    ///     The data controller base.
    /// </summary>
    public abstract class DataControllerBase
    {
        #region Fields

        /// <summary>
        ///     The dataflow principal
        /// </summary>
        private readonly DataflowPrincipal _principal;

        /// <summary>
        ///     The _sdmx retrieval manager.
        /// </summary>
        private readonly ISdmxObjectRetrievalManager _sdmxRetrievalManager;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DataControllerBase"/> class.
        /// </summary>
        /// <param name="principal">
        /// The principal.
        /// </param>
        /// <exception cref="SdmxSemmanticException">
        /// Operation not accepted with query used
        /// </exception>
        protected DataControllerBase(DataflowPrincipal principal)
        {
            this._principal = principal;
            this._sdmxRetrievalManager = new MappingStoreSdmxObjectRetrievalManager(SettingsManager.MappingStoreConnectionSettings);
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets the SDMX retrieval manager.
        /// </summary>
        /// <value>
        ///     The SDMX retrieval manager.
        /// </value>
        public ISdmxObjectRetrievalManager SdmxRetrievalManager
        {
            get
            {
                return this._sdmxRetrievalManager;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Try to authorize using the dataflow from <see cref="IDataflowObject"/>.
        ///     It uses and checks the <see cref="_principal"/> if there is an authorized user.
        /// </summary>
        /// <param name="dataflow">
        /// The dataflow.
        /// </param>
        /// <exception cref="SdmxUnauthorisedException">
        /// Not authorized
        /// </exception>
        protected void Authorize(IDataflowObject dataflow)
        {
            if (this._principal != null)
            {
                IMaintainableRefObject dataflowRefBean = this._principal.GetAllowedDataflow(dataflow);

                if (dataflowRefBean == null)
                {
                    string errorMessage = string.Format(CultureInfo.CurrentCulture, Resources.NoMappingForDataflowFormat1, dataflow.Id);
                    throw new SdmxUnauthorisedException(errorMessage);
                }
            }
        }

        /// <summary>
        /// Gets the data query from stream.
        /// </summary>
        /// <param name="input">
        /// The input.
        /// </param>
        /// <exception cref="Org.Sdmxsource.Sdmx.Api.Exception.SdmxSemmanticException">
        /// Operation not accepted
        /// </exception>
        /// <returns>
        /// The <see cref="IDataQuery"/>.
        /// </returns>
        protected IDataQuery GetDataQueryFromStream(IReadableDataLocation input)
        {
            IDataQueryParseManager dataQueryParseManager = new DataQueryParseManager(SdmxSchemaEnumType.VersionTwo);
            var dataQuery = dataQueryParseManager.BuildDataQuery(input, this.SdmxRetrievalManager).FirstOrDefault();
            if (dataQuery == null)
            {
                throw new SdmxSemmanticException(Properties.Resources.ErrorOperationNotAccepted);
            }

            return dataQuery;
        }

        #endregion
    }
}