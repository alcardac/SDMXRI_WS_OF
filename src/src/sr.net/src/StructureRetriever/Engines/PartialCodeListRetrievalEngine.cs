// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PartialCodeListRetrievalEngine.cs" company="Eurostat">
//   Date Created : 2012-03-28
//   Copyright (c) 2012 by the European   Commission, represented by Eurostat. All rights reserved.
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   This <see cref="ICodeListRetrievalEngine" /> retrieves partial codelists from DDB and Mastore.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Estat.Nsi.StructureRetriever.Engines
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Data;
    using System.Data.Common;

    using Estat.Nsi.StructureRetriever.Builders;
    using Estat.Nsi.StructureRetriever.Model;
    using Estat.Sri.MappingStoreRetrieval.Engine.Mapping;

    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Codelist;
    using Org.Sdmxsource.Sdmx.Util.Objects.Reference;

    /// <summary>
    /// This <see cref="ICodeListRetrievalEngine"/> retrieves partial codelists from DDB and Mastore.
    /// </summary>
    internal class PartialCodeListRetrievalEngine : ICodeListRetrievalEngine
    {
        #region Public Methods and Operators

        /// <summary>
        /// Retrieve Codelist
        /// </summary>
        /// <param name="info">
        /// The current StructureRetrieval state 
        /// </param>
        /// <returns>
        /// A <see cref="CodeListBean"/> 
        /// </returns>
        public ICodelistMutableObject GetCodeList(StructureRetrievalInfo info)
        {
            if (info.RequestedComponentInfo == null)
            {
                return null;
            }

            var codesSet = new Dictionary<string, object>();
            IComponentMapping cmap = info.RequestedComponentInfo.ComponentMapping;
            using (DbConnection ddbConnection = DDbConnectionBuilder.Instance.Build(info))
            {
                using (DbCommand cmd = ddbConnection.CreateCommand())
                {
                    cmd.CommandText = info.SqlQuery;
                    cmd.CommandTimeout = 0;
                    using (IDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string dsdCode = cmap.MapComponent(reader);
                            if (dsdCode != null && !codesSet.ContainsKey(dsdCode))
                            {
                                codesSet.Add(dsdCode, null);
                            }
                        }
                    }
                }
            }

            if (codesSet.Count > 0)
            {
                var subset = new List<string>(codesSet.Keys);
                ISet<ICodelistMutableObject> codeLists =
                    info.MastoreAccess.GetMutableCodelistObjects(
                        new MaintainableRefObjectImpl(info.CodelistRef.AgencyId, 
                                                        info.CodelistRef.MaintainableId,
                                                        info.CodelistRef.Version
                                                        ),
                                                        subset);

                return CodeListHelper.GetFirstCodeList(codeLists);
            }

            return null;
        }

        #endregion
    }
}