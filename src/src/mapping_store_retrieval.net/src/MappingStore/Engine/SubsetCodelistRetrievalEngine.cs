// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SubsetCodelistRetrievalEngine.cs" company="Eurostat">
//   Date Created : 2013-05-30
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   The subset codelist retrieval engine.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Estat.Sri.MappingStoreRetrieval.Engine
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Globalization;
    using System.Text;

    using Estat.Sri.MappingStoreRetrieval.Config;
    using Estat.Sri.MappingStoreRetrieval.Constants;
    using Estat.Sri.MappingStoreRetrieval.Helper;
    using Estat.Sri.MappingStoreRetrieval.Manager;
    using Estat.Sri.MappingStoreRetrieval.Model;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Codelist;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.Codelist;

    /// <summary>
    ///     The subset codelist retrieval engine.
    /// </summary>
    internal class SubsetCodelistRetrievalEngine : CodeListRetrievalEngine
    {
        #region Fields

        /// <summary>
        ///     The _max SQL parameters.
        /// </summary>
        private readonly int _maxSqlParameters;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SubsetCodelistRetrievalEngine"/> class.
        /// </summary>
        /// <param name="mappingStoreDb">
        /// The mapping store DB.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="mappingStoreDb"/> is null
        /// </exception>
        public SubsetCodelistRetrievalEngine(Database mappingStoreDb)
            : this(mappingStoreDb, 1000)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SubsetCodelistRetrievalEngine"/> class.
        /// </summary>
        /// <param name="mappingStoreDb">
        /// The mapping store DB.
        /// </param>
        /// <param name="maxSqlParameters">
        /// The max SQL Parameters.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="mappingStoreDb"/> is null
        /// </exception>
        public SubsetCodelistRetrievalEngine(Database mappingStoreDb, int maxSqlParameters)
            : base(mappingStoreDb)
        {
            this._maxSqlParameters = maxSqlParameters;
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Retrieve the <see cref="ICodelistMutableObject"/> from Mapping Store.
        /// </summary>
        /// <param name="maintainableRef">
        /// The maintainable reference which may contain ID, AGENCY ID and/or VERSION.
        /// </param>
        /// <param name="detail">
        /// The <see cref="StructureQueryDetail"/> which controls if the output will include details or not.
        /// </param>
        /// <param name="subset">
        /// The subset.
        /// </param>
        /// <returns>
        /// The <see cref="ISet{ICodelistMutableObject}"/>.
        /// </returns>
        public ISet<ICodelistMutableObject> Retrieve(IMaintainableRefObject maintainableRef, ComplexStructureQueryDetailEnumType detail, IList<string> subset)
        {
            var sqlQuery = new ArtefactSqlQuery(this.SqlQueryInfoForAll, maintainableRef);
            Func<ICodelistMutableObject, long, ICodelistMutableObject> hashMethod = (itemSchemeBean, parentSysId) => this.FillCodesHash(itemSchemeBean, parentSysId, subset);
            Func<ICodelistMutableObject, long, ICodelistMutableObject> whereInMethod = (itemSchemeBean, parentSysId) => this.FillCodes(itemSchemeBean, parentSysId, subset);
            Func<ICodelistMutableObject, long, ICodelistMutableObject> selected = whereInMethod;

            switch (this.MappingStoreDb.ProviderName)
            {
                case MappingStoreDefaultConstants.SqlServerProvider:

                    // SQL server performs very bad with 'IN'
                    selected = hashMethod;

                    break;
                case MappingStoreDefaultConstants.MySqlProvider:
                    if (subset.Count > 2000)
                    {
                        selected = hashMethod;
                    }

                    break;
                case MappingStoreDefaultConstants.OracleProvider:
                case MappingStoreDefaultConstants.OracleProviderOdp:
                    if (subset.Count > 4000)
                    {
                        selected = hashMethod;
                    }

                    break;
                default:
                    if (subset.Count > 1000)
                    {
                        selected = hashMethod;
                    }

                    break;
            }

            return this.RetrieveArtefacts(sqlQuery, detail, retrieveDetails: selected);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Get the Codes
        /// </summary>
        /// <param name="itemScheme">
        /// The parent <see cref="ICodelistMutableObject"/>
        /// </param>
        /// <param name="parentSysId">
        /// The parent ItemScheme primary key in Mapping Store
        /// </param>
        /// <param name="subset">
        /// The list of items to retrieve
        /// </param>
        /// <returns>
        /// The <see cref="ICodelistMutableObject"/>.
        /// </returns>
        private ICodelistMutableObject FillCodes(ICodelistMutableObject itemScheme, long parentSysId, ICollection<string> subset)
        {
            var allItems = new Dictionary<long, ICodeMutableObject>();
            var orderedItems = new List<KeyValuePair<long, ICodeMutableObject>>();
            var childItems = new Dictionary<long, long>();

            var sql = new StringBuilder(this.ItemSqlQueryInfo.QueryFormat);
            SqlHelper.AddWhereClause(sql, WhereState.And, " I.ID IN (");
            int position = sql.Length;
            int maxParameters = Math.Min(this._maxSqlParameters, subset.Count);
            var parameters = new DbParameter[maxParameters];
            var parameterNames = new string[maxParameters];
            var orderBy = this.ItemSqlQueryInfo.OrderBy;
            if (string.IsNullOrWhiteSpace(orderBy))
            {
                orderBy = string.Empty;
            }

            var stack = new Stack<string>(subset);

            using (DbConnection connection = this.MappingStoreDb.CreateConnection())
            {
                connection.Open();
                using (DbCommand command = connection.CreateCommand())
                {
                    var codelistParameter = command.CreateParameter();
                    codelistParameter.DbType = DbType.Int64;
                    codelistParameter.ParameterName = ParameterNameConstants.IdParameter;
                    codelistParameter.Value = parentSysId;

                    for (int i = 0; i < parameters.Length; i++)
                    {
                        var dbParameter = command.CreateParameter();
                        dbParameter.DbType = DbType.String;
                        string parameterName = string.Format(CultureInfo.InvariantCulture, "{0}{1}", ParameterNameConstants.CodeParameterName, i);
                        dbParameter.ParameterName = parameterName;
                        parameters[i] = dbParameter;
                        parameterNames[i] = this.MappingStoreDb.BuildParameterName(parameterName);
                    }

                    while (stack.Count > 0)
                    {
                        sql.Length = position;

                        maxParameters = Math.Min(stack.Count, this._maxSqlParameters);

                        for (int i = 0; i < maxParameters; i++)
                        {
                            sql.AppendFormat(CultureInfo.InvariantCulture, "{0},", parameterNames[i]);
                        }

                        sql.Length--;
                        sql.Append(")");
                        sql.AppendFormat(" {0}", orderBy);
                        command.Parameters.Clear();
                        command.CommandText = sql.ToString();
                        command.Parameters.Add(codelistParameter);

                        for (int i = 0; i < maxParameters; i++)
                        {
                            parameters[i].Value = stack.Pop();
                            command.Parameters.Add(parameters[i]);
                        }

                        this.ReadItems(allItems, orderedItems, command, childItems);
                    }
                }
            }

            this.FillParentItems(itemScheme, childItems, allItems, orderedItems);

            return itemScheme;
        }

        /// <summary>
        /// Get the Codes
        /// </summary>
        /// <param name="itemSchemeBean">
        /// The parent <see cref="ICodelistMutableObject"/>
        /// </param>
        /// <param name="parentSysId">
        /// The parent ItemScheme primary key in Mapping Store
        /// </param>
        /// <param name="subset">
        /// The list of items to retrieve
        /// </param>
        /// <returns>
        /// The <see cref="ICodelistMutableObject"/>.
        /// </returns>
        private ICodelistMutableObject FillCodesHash(ICodelistMutableObject itemSchemeBean, long parentSysId, IList<string> subset)
        {
            var allItems = new Dictionary<long, ICodeMutableObject>();
            var orderedItems = new List<KeyValuePair<long, ICodeMutableObject>>();
            var childItems = new Dictionary<long, long>();

            // TODO convert to Set<> in .NET 3.5
            var subsetSet = new HashSet<string>(StringComparer.Ordinal);
            for (int i = 0; i < subset.Count; i++)
            {
                subsetSet.Add(subset[i]);
            }

            using (DbCommand command = this.ItemCommandBuilder.Build(new ItemSqlQuery(this.ItemSqlQueryInfo, parentSysId)))
            {
                using (IDataReader dataReader = this.MappingStoreDb.ExecuteReader(command))
                {
                    int sysIdIdx = dataReader.GetOrdinal("SYSID");
                    int idIdx = dataReader.GetOrdinal("ID");
                    int parentIdx = dataReader.GetOrdinal("PARENT");
                    int txtIdx = dataReader.GetOrdinal("TEXT");
                    int langIdx = dataReader.GetOrdinal("LANGUAGE");
                    int typeIdx = dataReader.GetOrdinal("TYPE");
                    while (dataReader.Read())
                    {
                        string id = DataReaderHelper.GetString(dataReader, idIdx);
                        if (subsetSet.Contains(id))
                        {
                            long sysId = DataReaderHelper.GetInt64(dataReader, sysIdIdx);
                            ICodeMutableObject item;
                            if (!allItems.TryGetValue(sysId, out item))
                            {
                                item = new CodeMutableCore { Id = id };

                                orderedItems.Add(new KeyValuePair<long, ICodeMutableObject>(sysId, item));
                                allItems.Add(sysId, item);
                                long parentItemId = DataReaderHelper.GetInt64(dataReader, parentIdx);
                                if (parentItemId > long.MinValue)
                                {
                                    childItems.Add(sysId, parentItemId);
                                }
                            }

                            ReadLocalisedString(item, typeIdx, txtIdx, langIdx, dataReader);
                        }
                    }
                }
            }

            this.FillParentItems(itemSchemeBean, childItems, allItems, orderedItems);
            return itemSchemeBean;
        }

        #endregion
    }
}