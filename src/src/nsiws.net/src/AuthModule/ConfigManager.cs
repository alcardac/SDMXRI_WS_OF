// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConfigManager.cs" company="Eurostat">
//   Date Created : 2011-06-16
//   Copyright (c) 2011 by the European   Commission, represented by Eurostat. All rights reserved.
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   Configuration manager for this module
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Estat.Nsi.AuthModule
{
    using System.Configuration;

    /// <summary>
    /// Configuration manager for this module
    /// </summary>
    public class ConfigManager
    {
        #region Constants and Fields

        /// <summary>
        /// The singleton instance
        /// </summary>
        private static readonly ConfigManager _instance = new ConfigManager();

        /// <summary>
        /// The configuration section
        /// </summary>
        private readonly AuthConfigSection _config;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Prevents a default instance of the <see cref="ConfigManager"/> class from being created. 
        /// Initialize a new instance of the <see cref="ConfigManager"/> class
        /// </summary>
        private ConfigManager()
        {
            this._config = (AuthConfigSection)ConfigurationManager.GetSection("estat.nsi.ws.config/auth");
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the singleton instance
        /// </summary>
        public static ConfigManager Instance
        {
            get
            {
                return _instance;
            }
        }

        /// <summary>
        /// Gets the current configuration section
        /// </summary>
        public AuthConfigSection Config
        {
            get
            {
                return this._config;
            }
        }

        #endregion
    }
}