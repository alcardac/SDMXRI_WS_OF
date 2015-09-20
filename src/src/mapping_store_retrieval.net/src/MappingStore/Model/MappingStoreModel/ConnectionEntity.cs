// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConnectionEntity.cs" company="Eurostat">
//   Date Created : 2011-09-08
//   Copyright (c) 2011 by the European   Commission, represented by Eurostat. All rights reserved.
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   This is the class representation of a dissemination database connection
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Estat.Sri.MappingStoreRetrieval.Model.MappingStoreModel
{
    /// <summary>
    /// This is the class representation of a dissemination database connection
    /// </summary>
    public class ConnectionEntity : PersistentEntityBase
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ConnectionEntity"/> class. 
        /// Initializes a new instance of the <see cref="PersistentEntityBase"/> class.
        /// </summary>
        /// <param name="sysId">
        /// The unique entity identifier
        /// </param>
        public ConnectionEntity(long sysId)
            : base(sysId)
        {
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the database owner.
        /// </summary>
        /// <value>
        /// The database owner.
        /// </value>
        public string DBOwner { get; set; }
        
        /// <summary>
        /// Gets or sets the database ADO.NET connection string
        /// </summary>
        public string AdoConnectionString { get; set; }

        /// <summary>
        /// Gets or sets the database name
        /// </summary>
        public string DBName { get; set; }

        /// <summary>
        /// Gets or sets the database user password
        /// </summary>
        public string DBPassword { get; set; }

        /// <summary>
        /// Gets or sets the database server port
        /// </summary>
        public int DBPort { get; set; }

        /// <summary>
        /// Gets or sets the database server
        /// </summary>
        public string DBServer { get; set; }

        /// <summary>
        /// Gets or sets the database type
        /// </summary>
        public string DBType { get; set; }

        /// <summary>
        /// Gets or sets the database user
        /// </summary>
        public string DBUser { get; set; }

        /// <summary>
        /// Gets or sets the database JDBC connection string
        /// </summary>
        public string JdbcConnectionString { get; set; }

        /// <summary>
        /// Gets or sets the connection name
        /// </summary>
        public string Name { get; set; }

        #endregion
    }
}