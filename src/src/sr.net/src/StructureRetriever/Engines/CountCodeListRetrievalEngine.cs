// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CountCodeListRetrievalEngine.cs" company="Eurostat">
//   Date Created : 2012-03-28
//   Copyright (c) 2012 by the European   Commission, represented by Eurostat. All rights reserved.
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   The COUNT code list retrieval
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Estat.Nsi.StructureRetriever.Engines
{
    using System;
    using System.Data.Common;
    using System.Globalization;

    using Estat.Nsi.StructureRetriever.Builders;
    using Estat.Nsi.StructureRetriever.Model;

    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Codelist;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.Base;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.Codelist;

    /// <summary>
    /// The COUNT code list retrieval
    /// </summary>
    internal class CountCodeListRetrievalEngine : ICodeListRetrievalEngine
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
            var codelist = new CodelistMutableCore();
            var name = new TextTypeWrapperMutableCore { Locale = CustomCodelistConstants.Lang, Value = CustomCodelistConstants.CountCodeListName, };
            codelist.Names.Add(name);
            codelist.Id = CustomCodelistConstants.CountCodeList;
            codelist.AgencyId = CustomCodelistConstants.Agency;
            codelist.Version = CustomCodelistConstants.Version;
            int xsMeasureMult = 1;

            if (info.MeasureComponent != null)
            {
                info.Logger.Info("|-- Get XS Measure count");
                xsMeasureMult = GetXsMeasureCount(info);
            }

            object value = ExecuteSql(info);

            // setup count codelist
            var countCode = new CodeMutableCore();
            var text = new TextTypeWrapperMutableCore { Locale = CustomCodelistConstants.Lang, Value = CustomCodelistConstants.CountCodeDescription, };
            countCode.Names.Add(text);

            // normally count(*) should always return a number. Checking just in case I missed something.
            if (value != null && !Convert.IsDBNull(value))
            {
                // in .net, oracle will return 128bit decimal, sql server 32bit int, while mysql & sqlite 64bit long.
                long count = Convert.ToInt64(value, CultureInfo.InvariantCulture);

                // check if there are XS measure mappings. In this case there could be multiple measures/obs per row. 
                // even if they are not, then will be static mappings
                count *= xsMeasureMult;

                countCode.Id = count.ToString(CultureInfo.InvariantCulture);
                codelist.AddItem(countCode);
            }
            else
            {
                countCode.Id = CustomCodelistConstants.CountCodeDefault;
            }

            return codelist;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Execute the <see cref="StructureRetrievalInfo.SqlQuery"/> against the DDB and get a single value
        /// </summary>
        /// <param name="info">
        /// The current Structure retrieval state 
        /// </param>
        /// <returns>
        /// The scalar value 
        /// </returns>
        /// <exception cref="DbException">
        /// DDB communication error
        /// </exception>
        private static object ExecuteSql(StructureRetrievalInfo info)
        {
            object value;
            using (DbConnection ddbConnection = DDbConnectionBuilder.Instance.Build(info))
            {
                using (DbCommand cmd = ddbConnection.CreateCommand())
                {
                    cmd.CommandText = info.SqlQuery;
                    cmd.CommandTimeout = 0;
                    value = cmd.ExecuteScalar();
                }
            }

            return value;
        }

        /// <summary>
        /// Get CrossSectional Measure count.
        /// </summary>
        /// <param name="info">
        /// The current Structure retrieval state 
        /// </param>
        /// <returns>
        /// The CrossSectional Measure count. 
        /// </returns>
        private static int GetXsMeasureCount(StructureRetrievalInfo info)
        {
            int xsMeasureCount = info.XSMeasureDimensionConstraints.Count;
            foreach (var member in info.Criteria)
            {
                if (member.Id.Equals(info.MeasureComponent))
                {
                    // get the unmapped measure dimension XS codes to display
                    xsMeasureCount = 0;
                    foreach (string value in member.Values)
                    {
                        if (value != null
                            && info.XSMeasureDimensionConstraints.ContainsKey(value))
                        {
                            xsMeasureCount++;
                        }
                    }
                }
            }

            return xsMeasureCount == 0 ? 1 : xsMeasureCount;
        }

        #endregion
    }
}