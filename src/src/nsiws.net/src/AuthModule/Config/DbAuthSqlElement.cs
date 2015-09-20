// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DBAuthSqlElement.cs" company="Eurostat">
//   Date Created : 2011-09-07
//   Copyright (c) 2011 by the European   Commission, represented by Eurostat. All rights reserved.
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   Abstract class for DbAuth* elements which contains the common configuration properties
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Estat.Nsi.AuthModule.Config
{
    using System.Configuration;

    /// <summary>
    /// Abstract class for DbAuth* elements which contains the common configuration properties
    /// </summary>
    public abstract class DBAuthSqlElement : ConfigurationElement
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the ConnectionString Name. Must exist a connectionString with this name at connectionStrings section
        /// </summary>
        [ConfigurationProperty("connectionStringName")]
        public string ConnectionStringName
        {
            get
            {
                return (string)this["connectionStringName"];
            }

            set
            {
                this["connectionStringName"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the SQL Query with macros
        /// </summary>
        [ConfigurationProperty("sql", IsRequired = true)]
        public string Sql
        {
            get
            {
                return (string)this["sql"];
            }

            set
            {
                this["sql"] = value;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Check if the specified value is empty
        /// </summary>
        /// <param name="value">
        /// The sql query string
        /// </param>
        /// <exception cref="AuthConfigurationException">
        /// See the <see cref="Errors.DbAuthValidateMissingSql"/>
        /// </exception>
        public static void SqlValidate(object value)
        {
            var sql = value as string;
            if (string.IsNullOrEmpty(sql))
            {
                throw new AuthConfigurationException(Errors.DbAuthValidateMissingSql);
            }
        }

        #endregion
    }
}