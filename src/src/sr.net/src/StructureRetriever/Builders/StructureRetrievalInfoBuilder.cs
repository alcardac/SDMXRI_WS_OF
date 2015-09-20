// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StructureRetrievalInfoBuilder.cs" company="Eurostat">
//   Date Created : 2012-03-28
//   Copyright (c) 2012 by the European   Commission, represented by Eurostat. All rights reserved.
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   Builds a <see cref="StructureRetrievalInfo" /> object
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Estat.Nsi.StructureRetriever.Builders
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data.Common;
    using System.Globalization;
    using System.Linq;

    using Estat.Nsi.StructureRetriever.Model;
    using Estat.Sri.CustomRequests.Constants;
    using Estat.Sri.CustomRequests.Model;
    using Estat.Sri.MappingStoreRetrieval.Config;
    using Estat.Sri.MappingStoreRetrieval.Constants;
    using Estat.Sri.MappingStoreRetrieval.Engine;
    using Estat.Sri.MappingStoreRetrieval.Engine.Mapping;
    using Estat.Sri.MappingStoreRetrieval.Helper;
    using Estat.Sri.MappingStoreRetrieval.Manager;
    using Estat.Sri.MappingStoreRetrieval.Model.MappingStoreModel;

    using log4net;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Exception;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Registry;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Registry;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.Registry;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Registry;

    /// <summary>
    /// Builds a <see cref="StructureRetrievalInfo"/> object
    /// </summary>
    internal class StructureRetrievalInfoBuilder : IStructureRetrievalInfoBuilder
    {
        /// <summary>
        /// The logger
        /// </summary>
        private static readonly ILog _logger = LogManager.GetLogger(typeof(StructureRetrievalInfoBuilder));

        #region Public Methods and Operators

        /// <summary>
        /// Build a <see cref="StructureRetrievalInfo"/> from the specified parameters
        /// </summary>
        /// <param name="dataflow">
        /// The dataflow to get the available data for 
        /// </param>
        /// <param name="connectionStringSettings">
        /// The Mapping Store connection string settings 
        /// </param>
        /// <param name="allowedDataflows">
        /// The collection of allowed dataflows 
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// connectionStringSettings is null
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// dataflow is null
        /// </exception>
        /// <exception cref="StructureRetrieverException">
        /// Parsing error or mapping store exception error
        /// </exception>
        /// <returns>
        /// a <see cref="StructureRetrievalInfo"/> from the specified parameters 
        /// </returns>
        public StructureRetrievalInfo Build(
            IConstrainableStructureReference dataflow,
            ConnectionStringSettings connectionStringSettings,
            IList<IMaintainableRefObject> allowedDataflows)
        {
            if (connectionStringSettings == null)
            {
                throw new ArgumentNullException("connectionStringSettings");
            }

            if (dataflow == null)
            {
                throw new ArgumentNullException("dataflow");
            }

            var info = new StructureRetrievalInfo(_logger, allowedDataflows, connectionStringSettings);
            try
            {
                info.MappingSet = MappingSetRetriever.GetMappingSet(
                info.ConnectionStringSettings,
                dataflow.MaintainableReference.MaintainableId,
                dataflow.MaintainableReference.Version,
                dataflow.MaintainableReference.AgencyId,
                info.AllowedDataflows);
                if (info.MappingSet != null)
                {
                    ParserDataflowRef(dataflow, info);
                    Initialize(info);
                }
            }
            catch (SdmxException)
            {
                throw;
            }
            catch (StructureRetrieverException e)
            {
                _logger.Error(e.Message, e);
                switch (e.ErrorType)
                {
                    case StructureRetrieverErrorTypes.ParsingError:
                        throw new SdmxSyntaxException(e, ExceptionCode.XmlParseException);
                    case StructureRetrieverErrorTypes.MissingStructure:
                    case StructureRetrieverErrorTypes.MissingStructureRef:
                        throw new SdmxNoResultsException(e.Message);
                    default:
                        throw new SdmxException(e, SdmxErrorCode.GetFromEnum(SdmxErrorCodeEnumType.InternalServerError), e.Message);
                }
            }
            catch (DbException e)
            {
                string mesage = "Mapping Store connection error." + e.Message;
                _logger.Error(mesage);
                _logger.Error(e.ToString());
                throw new SdmxException(e, SdmxErrorCode.GetFromEnum(SdmxErrorCodeEnumType.InternalServerError), mesage);
            }
            catch (Exception e)
            {
                string mesage = string.Format(
                    CultureInfo.CurrentCulture,
                    ErrorMessages.ErrorRetrievingMappingSetFormat4,
                    dataflow.MaintainableReference.AgencyId,
                    dataflow.MaintainableReference.MaintainableId,
                    dataflow.MaintainableReference.Version,
                    e.Message);
                _logger.Error(mesage);
                _logger.Error(e.ToString());
                throw new SdmxException(e, SdmxErrorCode.GetFromEnum(SdmxErrorCodeEnumType.InternalServerError), mesage);
            }

            return info;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Initialize component mappings for coded components and time dimension used in mappings in the dataflow
        /// </summary>
        /// <param name="info">
        /// The current structure retrieval state 
        /// </param>
        /// <remarks>
        /// This method should be called only once
        /// </remarks>
        private static void Initialize(StructureRetrievalInfo info)
        {
            NormallizeDatabaseProvider(info.ConnectionStringSettings);
            info.MastoreAccess = new SpecialMutableObjectRetrievalManager(info.ConnectionStringSettings);
            info.XSMeasureDimensionConstraints.Clear();

            info.ComponentMapping.Clear();
            info.InnerSqlQuery = info.MappingSet.DataSet.Query;
            info.MeasureComponent = null;
            info.TimeTranscoder = null;
            info.TimeMapping = null;
            info.TimeDimension = null;
            bool measureDimensionMapped = false;

            foreach (MappingEntity mapping in info.MappingSet.Mappings)
            {
                foreach (ComponentEntity component in mapping.Components)
                {
                    if (component.ComponentType == SdmxComponentType.TimeDimension)
                    {
                        info.TimeMapping = mapping;
                        info.TimeDimension = component.Id;
                    }
                    else if (component.CodeList != null)
                    {
                        if (component.MeasureDimension)
                        {
                            measureDimensionMapped = true;
                        }

                        var compInfo = new ComponentInfo
                        {
                            Mapping = mapping,
                            ComponentMapping = ComponentMapping.CreateComponentMapping(component, mapping)
                        };
                        compInfo.CodelistRef.MaintainableId = component.CodeList.Id;
                        compInfo.CodelistRef.Version = component.CodeList.Version;
                        compInfo.CodelistRef.AgencyId = component.CodeList.Agency;
                        var id = component.Id;
                        info.ComponentMapping.Add(id, compInfo);
                        if (id.Equals(info.RequestedComponent))
                        {
                            info.RequestedComponentInfo = compInfo;
                        }

                        if (component.FrequencyDimension)
                        {
                            info.FrequencyInfo = compInfo;
                        }
                    }
                }
            }

            if (info.TimeMapping != null)
            {
                        info.TimeTranscoder = TimeDimensionMapping.Create(
                            info.TimeMapping, info.FrequencyInfo != null ? info.FrequencyInfo.ComponentMapping : null, info.MappingSet.DataSet.Connection.DBType);
            }

            if (!measureDimensionMapped)
            {
                foreach (ComponentEntity component in info.MappingSet.Dataflow.Dsd.Dimensions)
                {
                    if (component.MeasureDimension)
                    {
                        info.MeasureComponent = component.Id;
                    }
                }

                foreach (ComponentEntity xsMeasure in info.MappingSet.Dataflow.Dsd.CrossSectionalMeasures)
                {
                    info.XSMeasureDimensionConstraints.Add(xsMeasure.Id, xsMeasure);
                }
            }
        }

        /// <summary>
        /// Gets the requested component unique identifier.
        /// </summary>
        /// <param name="dsdEntity">The DSD entity.</param>
        /// <param name="conceptId">The requested component.</param>
        /// <returns>
        /// The Component.Id
        /// </returns>
        private static string GetRequestedComponentId(DsdEntity dsdEntity, string conceptId)
        {
            if (dsdEntity.TimeDimension != null && dsdEntity.TimeDimension.Concept.Id.Equals(conceptId))
            {
                conceptId = dsdEntity.TimeDimension.Id;
            }
            else
            {
                var dimension = dsdEntity.Dimensions.FirstOrDefault(entity => entity.Concept.Id.Equals(conceptId));
                conceptId = dimension == null ? conceptId : dimension.Id;
            }

            return conceptId;
        }

        /// <summary>
        /// Normalizes the DDB provider names so both DDB and MASTORE use the same.
        /// </summary>
        /// <param name="connectionStringSettings">
        /// The connection to the mapping store 
        /// </param>
        private static void NormallizeDatabaseProvider(ConnectionStringSettings connectionStringSettings)
        {
            if (connectionStringSettings.ProviderName.Contains("Oracle"))
            {
                DatabaseType.Mappings[MappingStoreDefaultConstants.OracleName].Provider =
                    connectionStringSettings.ProviderName;
            }
        }

        /// <summary>
        /// Parse the specified DataflowRefBean object and populate the <see cref="StructureRetrievalInfo.RequestedComponent"/> and <see cref="StructureRetrievalInfo.Criteria"/> fields
        /// </summary>
        /// <param name="d">
        /// The DataflowRefBean to parse 
        /// </param>
        /// <param name="info">
        /// The current structure retrieval state 
        /// </param>
        private static void ParserDataflowRef(IConstrainableStructureReference d, StructureRetrievalInfo info)
        {
            if (d.ConstraintObject != null && d.ConstraintObject.IncludedCubeRegion != null)
            {
                foreach (IKeyValues member in d.ConstraintObject.IncludedCubeRegion.KeyValues)
                {
                    if (member.Values.Count == 0 || (member.Values.Count == 1 && SpecialValues.DummyMemberValue.Equals(member.Values[0])))
                    {
                        info.RequestedComponent = GetRequestedComponentId(info.MappingSet.Dataflow.Dsd, member.Id);
                    }
                    else
                    {
                        IKeyValuesMutable normalizedMember = new KeyValuesMutableImpl(member) { Id = GetRequestedComponentId(info.MappingSet.Dataflow.Dsd, member.Id) };
                        var keyValuesCore = new KeyValuesCore(normalizedMember, member.Parent);
                        info.Criteria.Add(keyValuesCore);
                    }
                }

                info.ReferencePeriod = d.ConstraintObject.MutableInstance.ReferencePeriod;
            }
        }

        #endregion
    }
}