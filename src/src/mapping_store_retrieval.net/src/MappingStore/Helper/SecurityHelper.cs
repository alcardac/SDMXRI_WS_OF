// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SecurityHelper.cs" company="Eurostat">
//   Date Created : 2013-02-12
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   Helper class for adding allowed dataflows as clauses to prepared statements
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Estat.Sri.MappingStoreRetrieval.Helper
{
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Globalization;
    using System.Text;

    using Estat.Sri.MappingStoreRetrieval.Constants;
    using Estat.Sri.MappingStoreRetrieval.Extensions;
    using Estat.Sri.MappingStoreRetrieval.Manager;

    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;
    using Org.Sdmxsource.Sdmx.Util.Objects.Reference;

    /// <summary>
    ///     Helper class for adding allowed dataflows as clauses to prepared statements
    /// </summary>
    internal static class SecurityHelper
    {
        #region Public Methods and Operators

        /// <summary>
        /// Checks if there is a .
        /// </summary>
        /// <param name="allowedDataflows">
        /// The allowed dataflows.
        /// </param>
        /// <param name="requestedObject">
        /// The requested object.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public static bool Contains(ICollection<IMaintainableRefObject> allowedDataflows, IMaintainableRefObject requestedObject)
        {
            if (allowedDataflows == null)
            {
                return true;
            }

            foreach (IMaintainableRefObject allowedDataflow in allowedDataflows)
            {
                if ((!requestedObject.HasMaintainableId() || string.Equals(allowedDataflow.MaintainableId, requestedObject.MaintainableId))
                    && ((!allowedDataflow.HasVersion() || !requestedObject.HasVersion() || string.Equals(requestedObject.Version, allowedDataflow.Version))
                        && (!allowedDataflow.HasAgencyId() || !requestedObject.HasAgencyId() || string.Equals(requestedObject.AgencyId, allowedDataflow.AgencyId))))
                {
                    return true;
                }
            }

            return false;
        }

        #endregion

        /// <summary>
        /// Add where clauses and populate <paramref name="parameters"/>
        /// </summary>
        /// <param name="maintainableRef">
        ///     The maintainable reference which may contain ID, AGENCY ID and/or VERSION.
        /// </param>
        /// <param name="database">
        ///     The database.
        /// </param>
        /// <param name="sqlCommand">
        ///     The SQL command.
        /// </param>
        /// <param name="parameters">
        ///     The parameters.
        /// </param>
        /// <param name="allowedDataflows">
        ///     The allowed dataflows.
        /// </param>
        /// <param name="whereState">the current state of the WHERE clause in <paramref name="sqlCommand"/></param>
        /// <returns>
        /// The <paramref name="parameters"/>
        /// </returns>
        public static IList<DbParameter> AddWhereClauses(IMaintainableRefObject maintainableRef, Database database, StringBuilder sqlCommand, IList<DbParameter> parameters, ICollection<IMaintainableRefObject> allowedDataflows, WhereState whereState)
        {
            if (allowedDataflows == null)
            {
                return parameters;
            }

            maintainableRef = maintainableRef ?? new MaintainableRefObjectImpl();
            switch (whereState)
            {
                case WhereState.Nothing:
                    sqlCommand.Append(" WHERE (");
                    break;
                case WhereState.Where:
                    break;
                case WhereState.And:
                    sqlCommand.Append(" AND (");
                    break;
            }

            int lastClause = sqlCommand.Length;
            int count = 0;
            bool addedClauses = false;
            foreach (IMaintainableRefObject allowedDataflow in allowedDataflows)
            {
                // TODO check if allowed dataflow id is mandatory. If not we need to change this.
                if (!maintainableRef.HasMaintainableId() || string.Equals(maintainableRef.MaintainableId, allowedDataflow.MaintainableId))
                {
                    string countString = count.ToString(CultureInfo.InvariantCulture);
                    sqlCommand.Append("(");
                    
                    // id 
                    string idParam = ParameterNameConstants.IdParameter + countString;
                    sqlCommand.AppendFormat(" A.ID = {0} ", database.BuildParameterName(idParam));
                    parameters.Add(database.CreateInParameter(idParam, DbType.String, allowedDataflow.MaintainableId));
                    
                    // version
                    var versionParameters = allowedDataflow.GenerateVersionParameters(database, parameters, "A.VERSION", versionNumber => string.Format(CultureInfo.InvariantCulture, "{0}{1}_{2}", ParameterNameConstants.VersionParameter, versionNumber, countString));
                    if (versionParameters.Length > 0)
                    {
                        sqlCommand.AppendFormat(CultureInfo.InvariantCulture, "AND {0}", versionParameters);
                    }
                    
                    // agency
                    if (allowedDataflow.HasAgencyId())
                    {
                        string name = ParameterNameConstants.AgencyParameter + countString;
                        sqlCommand.AppendFormat(" AND A.AGENCY = {0} ", database.BuildParameterName(name));
                        parameters.Add(database.CreateInParameter(name, DbType.String, allowedDataflow.AgencyId));
                    }

                    sqlCommand.Append(" ) ");
                    lastClause = sqlCommand.Length;
                    sqlCommand.Append(" OR ");
                    count++;
                    addedClauses = true;
                }
            }

            sqlCommand.Length = lastClause;

            if (!addedClauses)
            {
                sqlCommand.Append("'ACCESS' = 'DENIED'");
            }

            sqlCommand.Append(" ) ");

            return parameters;
        }
    }
}