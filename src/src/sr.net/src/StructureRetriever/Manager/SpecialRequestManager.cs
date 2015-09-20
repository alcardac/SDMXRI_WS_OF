// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SpecialRequestManager.cs" company="Eurostat">
//   Date Created : 2012-03-28
//   Copyright (c) 2012 by the European   Commission, represented by Eurostat. All rights reserved.
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   A special request manager
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Estat.Nsi.StructureRetriever.Manager
{
    using System;
    using System.Globalization;

    using Estat.Nsi.StructureRetriever.Builders;
    using Estat.Nsi.StructureRetriever.Engines;
    using Estat.Nsi.StructureRetriever.Model;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Codelist;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;

    /// <summary>
    /// A special request manager
    /// </summary>
    internal class SpecialRequestManager : ISpecialRequestManager
    {
        #region Constants and Fields

        /// <summary>
        ///   SQL Code builder for getting component codelist
        /// </summary>
        private static readonly ISqlBuilder _componentSqlBuilder = new ComponentSqlBuilder();

        /// <summary>
        ///   Constant code codelist retrieval engine
        /// </summary>
        private static readonly ICodeListRetrievalEngine _constantCodeListRetrieval =
            new ConstantCodeListRetrievalEngine();

        /// <summary>
        ///   Constant code time dimension value retrieval engine
        /// </summary>
        private static readonly ICodeListRetrievalEngine _constantTimeDimensionCodeListRetrieval =
            new ConstantTimeDimensionCodeListRetrievalEngine();

        /// <summary>
        ///   Count codelist retrieval engine
        /// </summary>
        private static readonly ICodeListRetrievalEngine _countCodeListRetrieval = new CountCodeListRetrievalEngine();

        /// <summary>
        ///   SQL Code builder for getting the COUNT
        /// </summary>
        private static readonly ISqlBuilder _countSqlBuilder = new CountSqlBuilder();

        /// <summary>
        ///   partial codelist retrieval engine
        /// </summary>
        private static readonly ICodeListRetrievalEngine _partialCodeListRetrieval =
            new PartialCodeListRetrievalEngine();

        /// <summary>
        ///   simple codelist retrieval engine (for unmapped measure dimension)
        /// </summary>
        private static readonly ICodeListRetrievalEngine _simpleCodeListRetrieval = new SimpleCodeListRetrievalEngine();

        /// <summary>
        ///   Ttime dimension value retrieval engine
        /// </summary>
        private static readonly ICodeListRetrievalEngine _timeDimensionCodeListRetrieval =
            new TimeDimensionCodeListRetrievalEngine();

        /// <summary>
        ///   transcoded codelist retrieval engine
        /// </summary>
        private static readonly ICodeListRetrievalEngine _transcodedCodeListRetrieval =
            new TranscodedCodeListRetrievalEngine(true);

        /// <summary>
        ///   local codelist retrieval engine 
        /// </summary>
        private static readonly ICodeListRetrievalEngine _localCodeListRetrieval =
            new TranscodedCodeListRetrievalEngine(false);

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Retrieve the codelist that is referenced by the given <paramref name="codelistRef"/> . The codelist
        /// </summary>
        /// <param name="codelistRef">
        /// The codelist reference containing the id, agency and version of the requested dimension. Can be empty for time dimension 
        /// </param>
        /// <param name="info">
        /// The current structure retrieval state. 
        /// </param>
        /// <returns>
        /// The partial codelist 
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="codelistRef"/>
        ///   is null
        /// </exception>
        public ICodelistMutableObject RetrieveAvailableData(IMaintainableRefObject codelistRef, StructureRetrievalInfo info)
        {
            info.Logger.Info("Starting special request manager for codelist" + codelistRef);

            if (codelistRef == null)
            {
                throw new ArgumentNullException("codelistRef");
            }

            if (info.MappingSet == null)
            {
                info.Logger.Warn("|-- Warning: Codes not retrieved. Couldn't find a mapping set.");
                return null;
            }

            // set the codelist ref
            info.CodelistRef = codelistRef;

            ISqlBuilder sqlBuilder = null;
            ICodeListRetrievalEngine codeListRetrievalEngine = null;

            ISqlBuilder fallBackSqlBuilder = null;
            ICodeListRetrievalEngine fallBackCodeListRetrievalEngine = null;

            if (!IsRequestedComponentCodelist(codelistRef, info))
            {
                if (CustomCodelistConstants.IsCountRequest(codelistRef))
                {
                    // COUNT data request
                    sqlBuilder = _countSqlBuilder;
                    codeListRetrievalEngine = _countCodeListRetrieval;
                }
            }
            else if (info.RequestedComponent.Equals(info.MeasureComponent))
            {
                // measure dimension codelist request if measure dimension is not mapped
                // get the entire codelist for measure dimension
                codeListRetrievalEngine = _simpleCodeListRetrieval;
            }
            else if (info.RequestedComponent.Equals(info.TimeDimension))
            {
                // time dimension codelist request
                // get the time dimension special codelist with start and possibly end codes.
                if (info.TimeMapping.Columns.Count > 0)
                {
                    sqlBuilder = _componentSqlBuilder;
                    codeListRetrievalEngine = _timeDimensionCodeListRetrieval;
                }
                else
                {
                    codeListRetrievalEngine = _constantTimeDimensionCodeListRetrieval;
                }
            }
            else
            {
                // partial code list request for mapped coded components
                if (info.RequestedComponentInfo != null)
                {
                    if (info.RequestedComponentInfo.Mapping.Columns.Count > 0)
                    {
                        if (info.Criteria.Count == 0)
                        {
                            if (info.RequestedComponentInfo.Mapping.Transcoding != null)
                            {
                                codeListRetrievalEngine = _transcodedCodeListRetrieval;
                            }
                            else
                            {
                                codeListRetrievalEngine = _localCodeListRetrieval;
                                fallBackSqlBuilder = _componentSqlBuilder;
                                fallBackCodeListRetrievalEngine = _partialCodeListRetrieval;
                            }
                        }
                        else
                        {
                            sqlBuilder = _componentSqlBuilder;
                            codeListRetrievalEngine = _partialCodeListRetrieval;
                        }
                    }
                    else
                    {
                        codeListRetrievalEngine = _constantCodeListRetrieval;
                    }
                }
            }

            if (codeListRetrievalEngine == null)
            {
                info.Logger.Warn("|-- Warning: Codes not retrieved. Could not determine which code list retrieval engine to use.");
                return null;
            }

            var codeListBean = Execute(info, codeListRetrievalEngine, sqlBuilder);
            if ((codeListBean == null || codeListBean.Items.Count == 0) && fallBackCodeListRetrievalEngine != null)
            {
                codeListBean = Execute(info, fallBackCodeListRetrievalEngine, fallBackSqlBuilder);
            }

            return codeListBean;
        }

        /// <summary>
        /// Execute the specified <paramref name="sqlBuilder"/>, if not null and <paramref name="codeListRetrievalEngine"/>
        /// </summary>
        /// <param name="info">
        /// The current codelist retrieval information
        /// </param>
        /// <param name="codeListRetrievalEngine">
        /// The code list retrieval engine.
        /// </param>
        /// <param name="sqlBuilder">
        /// The sql builder.
        /// </param>
        /// <returns>
        /// The partial codelist 
        /// </returns>
        private static ICodelistMutableObject Execute(
            StructureRetrievalInfo info, ICodeListRetrievalEngine codeListRetrievalEngine, ISqlBuilder sqlBuilder)
        {
            if (sqlBuilder != null)
            {
                info.Logger.InfoFormat(
                        CultureInfo.InvariantCulture,
                        "|-- Generating SQL for dissemination database... using sql builder :{0}",
                        sqlBuilder);
                info.SqlQuery = sqlBuilder.GenerateSql(info);
                info.Logger.Info("|-- SQL for dissemination database generated:\n" + info.SqlQuery);
            }

            info.Logger.Info("|-- Retrieving codes... using codelist retrieval engine " + codeListRetrievalEngine);

            ICodelistMutableObject codeListBean = codeListRetrievalEngine.GetCodeList(info);

            if (codeListBean != null)
            {
                info.Logger.Info("|-- Codes retrieved successfully, found : " + codeListBean.Items.Count + " codes");
            }
            else
            {
                info.Logger.Warn(
                    "|-- Warning: Codes not retrieved. The engine " + codeListRetrievalEngine + " returned no codelist.");
            }

            return codeListBean;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Check if the requested Component is set and if the requested codelist ref is for that component
        /// </summary>
        /// <param name="codelistRef">
        /// The requested codelistRef 
        /// </param>
        /// <param name="structureRetrievalInfo">
        /// The current structure retrieval state 
        /// </param>
        /// <returns>
        /// The is requested component codelist. 
        /// </returns>
        private static bool IsRequestedComponentCodelist(
            IMaintainableRefObject codelistRef, StructureRetrievalInfo structureRetrievalInfo)
        {
            if (structureRetrievalInfo.RequestedComponent == null)
            {
                return false;
            }

            if (structureRetrievalInfo.RequestedComponent.Equals(structureRetrievalInfo.TimeDimension)
                && (string.IsNullOrEmpty(codelistRef.MaintainableId) || CustomCodelistConstants.IsTimeDimensionRequest(codelistRef)))
            {
                return true;
            }

            if (structureRetrievalInfo.MeasureComponent != null
                && structureRetrievalInfo.MeasureComponent.Equals(structureRetrievalInfo.RequestedComponent))
            {
                return true;
            }

            if (structureRetrievalInfo.RequestedComponentInfo != null)
            {
                IMaintainableRefObject c = structureRetrievalInfo.CodelistRef;
                return string.Equals(c.MaintainableId, codelistRef.MaintainableId) && string.Equals(c.AgencyId, codelistRef.AgencyId);
            }

            return false;
        }

        #endregion
    }
}