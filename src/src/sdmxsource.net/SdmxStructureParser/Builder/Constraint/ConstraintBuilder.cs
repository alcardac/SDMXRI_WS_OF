// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConstraintBuilder.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The constraint builder implementation.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.Structureparser.Builder.Constraint
{
    using System;
    using System.Collections.Generic;

    using Org.Sdmxsource.Sdmx.Api.Builder;
    using Org.Sdmxsource.Sdmx.Api.Engine;
    using Org.Sdmxsource.Sdmx.Api.Exception;
    using Org.Sdmxsource.Sdmx.Api.Model.Data;
    using Org.Sdmxsource.Sdmx.Api.Model.Header;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Registry;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Registry;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.Registry;
    using Org.Sdmxsource.Util.Extensions;

    /// <summary>
    ///     The constraint builder implementation.
    /// </summary>
    public class ConstraintBuilder : IConstraintBuilder
    {
        #region Public Methods and Operators

        /// <summary>
        /// Build constraint.
        /// </summary>
        /// <param name="dataReaderEngine">
        /// The data reader engine.
        /// </param>
        /// <param name="attachment">
        /// The attachment.
        /// </param>
        /// <param name="dataSourceAttachment">
        /// The data source attachment.
        /// </param>
        /// <param name="indexAttributes">
        /// The index attributes.
        /// </param>
        /// <param name="indexDataset">
        /// The index dataset.
        /// </param>
        /// <param name="indexReportingPeriod">
        /// The index reporting period.
        /// </param>
        /// <param name="indexTimeSeries">
        /// The index time series.
        /// </param>
        /// <param name="definingDataPresent">
        /// The defining data present.
        /// </param>
        /// <param name="refParameters">
        /// The ref parameters.
        /// </param>
        /// <returns>
        /// The <see cref="IContentConstraintObject"/>.
        /// </returns>
        public virtual IContentConstraintObject BuildConstraint(
            IDataReaderEngine dataReaderEngine,
            IStructureReference attachment,
            IDataSource dataSourceAttachment,
            bool indexAttributes,
            bool indexDataset,
            bool indexReportingPeriod,
            bool indexTimeSeries,
            bool definingDataPresent,
            IMaintainableRefObject refParameters)
        {
            dataReaderEngine.Reset();

            // TODO Should it check if there is more then one dataset in the datasource
            if (!dataReaderEngine.MoveNextDataset())
            {
                throw new SdmxSemmanticException(
                    "Can not index time series for registered datasource, the data retrieved from the datasource does not contain a dataset");
            }

            if (!indexAttributes && !indexDataset && !indexReportingPeriod && !indexTimeSeries)
            {
                return null;
            }

            IDatasetHeader header = dataReaderEngine.CurrentDatasetHeader;
            if (indexTimeSeries && !header.Timeseries)
            {
                throw new SdmxSemmanticException(
                    "Can not index time series for registered datasource, the data retrieved from the datasource is not time series");
            }

            // Create mutable Maintainable
            IContentConstraintMutableObject mutableBean = new ContentConstraintMutableCore();
            mutableBean.AgencyId = refParameters.AgencyId;
            mutableBean.Id = refParameters.MaintainableId;
            mutableBean.Version = refParameters.Version;
            mutableBean.AddName("en", "Generated Constraint");
            mutableBean.AddDescription("en", "Constraint built from dataset");
            mutableBean.IsDefiningActualDataPresent = true;
            mutableBean.ConstraintAttachment = BuildAttachement(attachment);

            IConstraintDataKeySetMutableObject dataKeySet = null;

            IDictionary<string, ISet<string>> cubeRegionMap = new Dictionary<string, ISet<string>>();
            IDictionary<string, ISet<string>> attributeMap = new Dictionary<string, ISet<string>>();
            DateTime? reportFrom = null;
            DateTime? reportTo = null;
            ISet<string> processedDates = new HashSet<string>();

            while (dataReaderEngine.MoveNextKeyable())
            {
                IKeyable key = dataReaderEngine.CurrentKey;
                if (key.Series)
                {
                    // TODO Check if Cross Sectional and put out exception if it is
                    // 1. If indexing the time series store the time Series Key on the Constraint
                    if (indexTimeSeries)
                    {
                        if (dataKeySet == null)
                        {
                            dataKeySet = new ConstraintDataKeySetMutableCore();
                            mutableBean.IncludedSeriesKeys = dataKeySet;
                        }

                        IConstrainedDataKeyMutableObject dataKey = new ConstrainedDataKeyMutableCore();
                        dataKey.KeyValues.AddAll(key.Key);
                        dataKeySet.AddConstrainedDataKey(dataKey);
                    }

                    // 2. If indexing the dataset, store the individual code values
                    if (indexDataset)
                    {
                        StoreKeyValuesOnMap(key.Key, cubeRegionMap);
                    }

                    // 3. If indexing attributes, store the individual code values for each attribute
                    if (indexAttributes)
                    {
                        StoreKeyValuesOnMap(key.Attributes, attributeMap);
                    }
                }

                if (indexAttributes || indexReportingPeriod)
                {
                    while (dataReaderEngine.MoveNextObservation())
                    {
                        IObservation obs = dataReaderEngine.CurrentObservation;

                        // If indexing the dates, determine the data start and end dates from the obs dates
                        // To save time, do not process the same date twice
                        if (indexReportingPeriod)
                        {
                            if (!processedDates.Contains(obs.ObsTime))
                            {
                                DateTime obsDate = obs.ObsAsTimeDate.GetValueOrDefault();
                                if (reportFrom == null || (reportFrom.Value.Ticks / 10000) > (obsDate.Ticks / 10000))
                                {
                                    reportFrom = obsDate;
                                }

                                if (reportTo == null || (reportTo.Value.Ticks / 10000) < (obsDate.Ticks / 10000))
                                {
                                    reportTo = obsDate;
                                }

                                processedDates.Add(obs.ObsTime);
                            }
                        }

                        if (indexAttributes)
                        {
                            StoreKeyValuesOnMap(obs.Attributes, attributeMap);
                        }
                    }
                }
            }

            if (indexAttributes || indexDataset)
            {
                ICubeRegionMutableObject cubeRegionMutableBean = new CubeRegionMutableCore();
                mutableBean.IncludedCubeRegion = cubeRegionMutableBean;
                if (indexAttributes)
                {
                    CreateKeyValues(attributeMap, cubeRegionMutableBean.AttributeValues);
                }

                if (indexDataset)
                {
                    CreateKeyValues(cubeRegionMap, cubeRegionMutableBean.KeyValues);
                }
            }

            if (indexReportingPeriod && reportFrom != null && reportTo.HasValue)
            {
                IReferencePeriodMutableObject refPeriodMutable = new ReferencePeriodMutableCore();
                refPeriodMutable.EndTime = reportTo.Value;
                refPeriodMutable.StartTime = reportFrom.Value;
                mutableBean.ReferencePeriod = refPeriodMutable;
            }

            return mutableBean.ImmutableInstance;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Build attachment.
        /// </summary>
        /// <param name="attachment">
        /// The attachment.
        /// </param>
        /// <returns>
        /// The <see cref="IConstraintAttachmentMutableObject"/>.
        /// </returns>
        private static IConstraintAttachmentMutableObject BuildAttachement(IStructureReference attachment)
        {
            IConstraintAttachmentMutableObject mutable = new ContentConstraintAttachmentMutableCore();
            mutable.AddStructureReference(attachment);
            return mutable;
        }

        /// <summary>
        /// The create key values.
        /// </summary>
        /// <param name="cubeRegionMap">
        /// The cube region map.
        /// </param>
        /// <param name="populateMap">
        /// The populate map.
        /// </param>
        private static void CreateKeyValues(
            IDictionary<string, ISet<string>> cubeRegionMap, ICollection<IKeyValuesMutable> populateMap)
        {
            foreach (var currentConcept in cubeRegionMap)
            {
                IKeyValuesMutable kvs = new KeyValuesMutableImpl();
                kvs.Id = currentConcept.Key;
                kvs.KeyValues.AddAll(currentConcept.Value);
                populateMap.Add(kvs);
            }
        }

        /// <summary>
        /// Store key values on map.
        /// </summary>
        /// <param name="keyValues">
        /// The key value list.
        /// </param>
        /// <param name="cubeRegionMap">
        /// The cube region map.
        /// </param>
        private static void StoreKeyValuesOnMap(
            IEnumerable<IKeyValue> keyValues, IDictionary<string, ISet<string>> cubeRegionMap)
        {
            /* foreach */
            foreach (IKeyValue kv in keyValues)
            {
                ISet<string> valuesForConcept;
                if (!cubeRegionMap.TryGetValue(kv.Concept, out valuesForConcept))
                {
                    valuesForConcept = new HashSet<string>();
                    cubeRegionMap.Add(kv.Concept, valuesForConcept);
                }

                valuesForConcept.Add(kv.Code);
            }
        }

        #endregion
    }
}