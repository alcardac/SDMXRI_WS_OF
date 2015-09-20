// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TimeDimensionCodeListRetrievalEngine.cs" company="Eurostat">
//   Date Created : 2012-03-28
//   Copyright (c) 2012 by the European   Commission, represented by Eurostat. All rights reserved.
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   Time Dimension start and optionally end code list retrieval engine
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Estat.Nsi.StructureRetriever.Engines
{
    using System.Data;
    using System.Data.Common;

    using Estat.Nsi.StructureRetriever.Builders;
    using Estat.Nsi.StructureRetriever.Model;
    using Estat.Sri.MappingStoreRetrieval.Extensions;

    using Org.Sdmxsource.Sdmx.Api.Model.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Codelist;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Base;

    /// <summary>
    /// Time Dimension start and optionally end code list retrieval engine
    /// </summary>
    internal class TimeDimensionCodeListRetrievalEngine : ICodeListRetrievalEngine
    {
        #region Public Methods and Operators

        /// <summary>
        /// Retrieve Codelist
        /// </summary>
        /// <param name="info">
        /// The current StructureRetrieval state 
        /// </param>
        /// <returns>
        /// A <see cref="ICodelistMutableObject"/> 
        /// </returns>
        public ICodelistMutableObject GetCodeList(StructureRetrievalInfo info)
        {
            ISdmxDate minPeriod = new SdmxDateCore(CustomCodelistConstants.TimePeriodMax);
            ISdmxDate maxPeriod = new SdmxDateCore(CustomCodelistConstants.TimePeriodMin);

            var frequencyMapping = info.FrequencyInfo != null ? info.FrequencyInfo.ComponentMapping : null;
            var timeTranscoder = info.TimeTranscoder;
            using (DbConnection ddbConnection = DDbConnectionBuilder.Instance.Build(info))
            using (DbCommand cmd = ddbConnection.CreateCommand())
            {
                cmd.CommandTimeout = 0;
                cmd.CommandText = info.SqlQuery;
                using (IDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string frequency = frequencyMapping != null ? frequencyMapping.MapComponent(reader) : null; 
                        string period = timeTranscoder.MapComponent(reader, frequency);
                        if (!string.IsNullOrEmpty(period))
                        {
                            ISdmxDate currentPeriod = new SdmxDateCore(period);
                            if (currentPeriod.StartsBefore(minPeriod))
                            {
                                minPeriod = currentPeriod;
                            }

                            if (currentPeriod.EndsAfter(maxPeriod))
                            {
                                maxPeriod = currentPeriod;
                            }
                        }
                    }
                }
            }

            return TimeDimensionCodeListHelper.BuildTimeCodelist(minPeriod, maxPeriod);
        }

        #endregion
    }
}