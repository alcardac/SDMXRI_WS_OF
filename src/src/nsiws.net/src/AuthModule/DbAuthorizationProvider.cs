// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DbAuthorizationProvider.cs" company="Eurostat">
//   Date Created : 2011-06-16
//   Copyright (c) 2011 by the European   Commission, represented by Eurostat. All rights reserved.
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   Implementation of the <see cref="IAuthorizationProvider" /> interface.
//   This implementation will retrieve the allowed dataflows from a database using a configurable sql query
//   The connection string and the sql query are specified in the .config file
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Estat.Nsi.AuthModule
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data;
    using System.Data.Common;
    using System.Globalization;
    using System.Linq;

    using Estat.Sri.MappingStoreRetrieval.Manager;

    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;
    using Org.Sdmxsource.Sdmx.Util.Objects.Reference;

    /// <summary>
    /// Implementation of the <see cref="IAuthorizationProvider"/> interface. 
    /// This implementation will retrieve the allowed dataflows from a database using a configurable SQL query
    /// The connection string and the SQL query are specified in the .config file
    /// </summary>
    public class DbAuthorizationProvider : IAuthorizationProvider
    {
        #region Constants and Fields

        /// <summary>
        /// The authorization database
        /// </summary>
        private readonly Database _database;

        /// <summary>
        /// The list of allowed dataflows. The map consists of IMaintainableRefObject.id to DataflowRefBeans
        /// </summary>
        private readonly IDictionary<string, List<IMaintainableRefObject>> _dataflowIdSet =
            new Dictionary<string, List<IMaintainableRefObject>>(StringComparer.Ordinal);

        /// <summary>
        /// The list of allowed dataflows. The map consists of IMaintainableRefObject to DataflowRefBean.Id
        /// </summary>
        private readonly IDictionary<IMaintainableRefObject, string> _dataflowSet =
           new Dictionary<IMaintainableRefObject, string>(new DataflowCompare());

        /// <summary>
        /// The SQL query as specified in the config file
        /// </summary>
        private readonly string _selectQuery;

        /// <summary>
        /// The last <see cref="IUser"/>
        /// </summary>
        private IUser _lastUser;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DbAuthorizationProvider"/> class. 
        /// Initialize a new instance of the <see cref="DbAuthorizationProvider"/> class
        /// </summary>
        public DbAuthorizationProvider()
        {
            // check configuration
            AuthUtils.ValidateConfig(ConfigManager.Instance.Config.DBAuth, this.GetType());
            AuthUtils.ValidateConfig(ConfigManager.Instance.Config.DBAuth.Authorization, this.GetType());

            string connectionStringName = ConfigManager.Instance.Config.DBAuth.Authorization.ConnectionStringName;
            if (string.IsNullOrEmpty(connectionStringName))
            {
                connectionStringName = ConfigManager.Instance.Config.DBAuth.Authentication != null
                                           ? ConfigManager.Instance.Config.DBAuth.Authentication.ConnectionStringName
                                           : null;
            }

            if (string.IsNullOrEmpty(connectionStringName))
            {
                throw new AuthConfigurationException(Errors.DbAuthMissingConnectionStringName);
            }

            var connectionStringSettings = ConfigurationManager.ConnectionStrings[connectionStringName];
            if (connectionStringSettings == null)
            {
                throw new AuthConfigurationException(Errors.DbAuthMissingConnectionStringName);
            }

            this._selectQuery = ConfigManager.Instance.Config.DBAuth.Authorization.Sql;
            if (
                !AuthUtils.ValidateContains(
                    this._selectQuery, 
                    DbConstants.UserMacro, 
                    DbConstants.DataflowIdMacro, 
                    DbConstants.DataflowVersionMacro, 
                    DbConstants.DataflowAgencyIdMacro))
            {
                string missingMessage = string.Format(
                    CultureInfo.CurrentCulture, 
                    Errors.DbAuthMissingConnectionStringName, 
                    DbConstants.DataflowIdMacro, 
                    DbConstants.DataflowVersionMacro, 
                    DbConstants.DataflowAgencyIdMacro, 
                    DbConstants.UserMacro);
                throw new AuthConfigurationException(
                    string.Format(
                        CultureInfo.CurrentCulture, Errors.DbAuthInvalidSqlQuery, this.GetType().Name, missingMessage));
            }

            this._database = new Database(connectionStringSettings);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DbAuthorizationProvider"/> class. 
        /// Initialize a new instance of the <see cref="DbAuthorizationProvider"/> class with the specified <see cref="IUser"/>.
        /// This constructor calls <see cref="SetUser"/>
        /// </summary>
        /// <param name="user">
        /// The <see cref="IUser"/>
        /// </param>
        public DbAuthorizationProvider(IUser user)
            : this()
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            this.SetUser(user);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Check if there is at least one dataflow with the specified ID in list of allowed dataflows
        /// </summary>
        /// <param name="user">
        /// The <see cref="IUser"/> to check
        /// </param>
        /// <param name="dataflowId">
        /// The dataflow ID
        /// </param>
        /// <returns>
        /// True if there is at least one dataflow with the specified ID in list of allowed dataflows. Else false
        /// </returns>
        public bool AccessControl(IUser user, string dataflowId)
        {
            this.SetUser(user);
            return this._dataflowIdSet.ContainsKey(dataflowId);
        }

        /// <summary>
        /// Check if there is a dataflow in the list of allowed dataflows which matches the id, version and agencyId of the specified <see cref="IMaintainableRefObject"/> 
        /// </summary>
        /// <param name="user">
        /// The <see cref="IUser"/> to check
        /// </param>
        /// <param name="dataflowRef">
        /// The <see cref="IMaintainableRefObject"/> to check
        /// </param>
        /// <returns>
        /// True if there is a dataflow in the list of allowed dataflows which matches the id, version and agencyId of the specified <see cref="IMaintainableRefObject"/> 
        /// </returns>
        public bool AccessControl(IUser user, IMaintainableRefObject dataflowRef)
        {
            this.SetUser(user);
            return this._dataflowSet.ContainsKey(dataflowRef)
                   || this._dataflowSet.Any(
                       pair =>
                       pair.Key.MaintainableId.Equals(dataflowRef.MaintainableId) && (!pair.Key.HasAgencyId() || pair.Key.AgencyId.Equals(dataflowRef.AgencyId))
                       && (!pair.Key.HasVersion() || pair.Key.Version.Equals(dataflowRef.Version)));
        }

        /// <summary>
        /// Get the collection of allowed dataflows
        /// </summary>
        /// <param name="user">
        /// The <see cref="IUser"/> to check
        /// </param>
        /// <returns>
        /// The list of dataflows for the <see cref="IUser"/> 
        /// </returns>
        public ICollection<IMaintainableRefObject> GetDataflows(IUser user)
        {
            this.SetUser(user);
            return this._dataflowSet.Keys;
        }

        /// <summary>
        /// Get the collection of allowed dataflows with the specific dataflow id
        /// </summary>
        /// <param name="user">
        /// The <see cref="IUser"/> to check
        /// </param>
        /// <param name="dataflowId">
        /// The dataflow id
        /// </param>
        /// <returns>
        /// The list of dataflows for the <see cref="IUser"/> 
        /// </returns>
        public IEnumerable<IMaintainableRefObject> GetDataflows(IUser user, string dataflowId)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            if (dataflowId == null)
            {
                return new MaintainableRefObjectImpl[0];
            }

            this.SetUser(user);
            List<IMaintainableRefObject> dataflowRefBeans;
            if (!this._dataflowIdSet.TryGetValue(dataflowId, out dataflowRefBeans))
            {
                return new MaintainableRefObjectImpl[0];
            }

            return dataflowRefBeans;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Retrieve allowed dataflows for user from the database
        /// </summary>
        /// <param name="user">
        /// The user
        /// </param>
        protected void RetrieveAllowedDataFlows(IUser user)
        {
            this._dataflowSet.Clear();
            this._dataflowIdSet.Clear();
            string userParam = this._database.BuildParameterName(DbConstants.UserParamName);

            string sql =
                this._selectQuery.Replace(DbConstants.UserMacro, userParam).Replace(
                    DbConstants.DataflowIdMacro, DbConstants.DataflowIdField).Replace(
                        DbConstants.DataflowVersionMacro, DbConstants.DataflowVersionField).Replace(
                            DbConstants.DataflowAgencyIdMacro, DbConstants.DataflowAgencyIdField);

            using (DbCommand command = this._database.GetSqlStringCommand(sql))
            {
                this._database.AddInParameter(command, DbConstants.UserParamName, DbType.String, user.UserName);
                using (IDataReader reader = this._database.ExecuteReader(command))
                {
                    int idIdx = reader.GetOrdinal(DbConstants.DataflowIdField);
                    int versionIdx = reader.GetOrdinal(DbConstants.DataflowVersionField);
                    int agencyIdx = reader.GetOrdinal(DbConstants.DataflowAgencyIdField);
                    while (reader.Read())
                    {
                        IMaintainableRefObject dataflowRefBean = new MaintainableRefObjectImpl
                            {
                                MaintainableId = AuthUtils.ConvertDBValue<string>(reader.GetValue(idIdx)), 
                                AgencyId = AuthUtils.ConvertDBValue<string>(reader.GetValue(agencyIdx)), 
                                Version = AuthUtils.ConvertDBValue<string>(reader.GetValue(versionIdx))
                            };
                        if (!this._dataflowSet.ContainsKey(dataflowRefBean))
                        {
                            this._dataflowSet.Add(dataflowRefBean, dataflowRefBean.MaintainableId);
                            List<IMaintainableRefObject> dataflowRefBeans;
                            if (!this._dataflowIdSet.TryGetValue(dataflowRefBean.MaintainableId, out dataflowRefBeans))
                            {
                                dataflowRefBeans = new List<IMaintainableRefObject>();
                                this._dataflowIdSet.Add(dataflowRefBean.MaintainableId, dataflowRefBeans);
                            }

                            dataflowRefBeans.Add(dataflowRefBean);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Set the <see cref="IUser"/>. 
        /// </summary>
        /// <param name="user">
        /// The <see cref="IUser"/>
        /// </param>
        private void SetUser(IUser user)
        {
            if (this._lastUser == null
                || !(this._lastUser.Equals(user) || this._lastUser.UserName.Equals(user.UserName)))
            {
                this.RetrieveAllowedDataFlows(user);
                this._lastUser = user;
            }
        }

        #endregion
    }
}