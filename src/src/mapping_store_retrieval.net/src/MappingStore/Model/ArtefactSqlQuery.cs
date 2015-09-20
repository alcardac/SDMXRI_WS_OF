// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ArtefactSqlQuery.cs" company="Eurostat">
//   Date Created : 2013-02-08
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   The artefact SQL query.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Estat.Sri.MappingStoreRetrieval.Model
{
    using Estat.Sri.MappingStoreRetrieval.Builder;

    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;
    using Org.Sdmxsource.Sdmx.Util.Objects.Reference;

    /// <summary>
    /// The artefact SQL Query.
    /// </summary>
    internal class ArtefactSqlQuery : SqlQueryBase
    {
        #region Fields

        /// <summary>
        /// The artefact SQL builder.
        /// </summary>
        private static readonly ArtefactSqlBuilder _artefactSqlBuilder = new ArtefactSqlBuilder();

        /// <summary>
        ///     The _maintainable ref.
        /// </summary>
        private readonly IMaintainableRefObject _maintainableRef;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ArtefactSqlQuery"/> class.
        /// </summary>
        /// <param name="tableInfo">
        /// The table Info.
        /// </param>
        /// <param name="maintainableRef">
        /// The maintainable reference which may contain ID, AGENCY ID and/or VERSION
        /// </param>
        public ArtefactSqlQuery(TableInfo tableInfo, IMaintainableRefObject maintainableRef)
            : this(_artefactSqlBuilder.Build(tableInfo), maintainableRef)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ArtefactSqlQuery"/> class.
        /// </summary>
        /// <param name="queryInfo">
        /// The query info.
        /// </param>
        /// <param name="maintainableRef">
        /// The maintainable reference which may contain ID, AGENCY ID and/or VERSION
        /// </param>
        public ArtefactSqlQuery(SqlQueryInfo queryInfo, IMaintainableRefObject maintainableRef)
            : base(queryInfo)
        {
            this._maintainableRef = maintainableRef ?? new MaintainableRefObjectImpl();
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets the maintainable reference.
        /// </summary>
        public IMaintainableRefObject MaintainableRef
        {
            get
            {
                return this._maintainableRef;
            }
        }

        #endregion

    }
}