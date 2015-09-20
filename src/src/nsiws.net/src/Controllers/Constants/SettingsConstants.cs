// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SettingsConstants.cs" company="Eurostat">
//   Date Created : 2013-10-11
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   Constant values used in <see cref="SettingsManager" />
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Estat.Sri.Ws.Controllers.Constants
{
    using Estat.Sri.Ws.Controllers.Manager;

    /// <summary>
    ///     Constant values used in <see cref="SettingsManager" />
    /// </summary>
    internal static class SettingsConstants
    {
        #region Constants

        /// <summary>
        ///     The web.config appSettings variable name for overriding the 32bit binary folder.
        ///     Default is set at <see cref="DefaultBin32" />
        /// </summary>
        public const string Bin32 = "bin32";

        /// <summary>
        ///     The web.config appSettings variable name for overriding the 64bit binary folder.
        ///     Default is set at <see cref="DefaultBin64" />
        /// </summary>
        public const string Bin64 = "bin64";

        /// <summary>
        ///     Default 32bit directory name
        /// </summary>
        public const string DefaultBin32 = "win32";

        /// <summary>
        ///     Default 64bit directory name
        /// </summary>
        public const string DefaultBin64 = "x64";

        /// <summary>
        ///     The web.config appSettings variable name for the default DDB Oracle Provider
        /// </summary>
        public const string DefaultDdbOracleProvider = "defaultDDBOracleProvider";

        /// <summary>
        ///     The default prefix
        /// </summary>
        public const string DefaultPrefix = "defaultPrefix";

        /// <summary>
        ///     DLL extension with dot.
        /// </summary>
        public const string DllExtension = ".dll";

        /// <summary>
        ///     The web.config appSettings variable name for log level
        /// </summary>
        public const string LogLevel = "logLevel";

        /// <summary>
        ///     The web.config appSettings variable name for log template
        /// </summary>
        public const string LogTemplate = "logTemplateFormat";

        /// <summary>
        ///     The Name of the mapping store connection string. The default value is MappingStoreServer
        /// </summary>
        public const string MappingStoreConnectionName = "MappingStoreServer";

        /// <summary>
        ///     The path environment setting
        /// </summary>
        public const string PathEnvironment = "path";

        /// <summary>
        ///     The additional path setting
        /// </summary>
        public const string PathSetting = "path";

        /// <summary>
        ///     The web.config appSettings variable name for overriding the PlatformSpecificAssemblies separated by comma
        ///     Default is set at <see cref="SettingsManager._platformAssemblies" />
        /// </summary>
        public const string PlatformSpecificAssemblies = "PlatformSpecificAssemblies";

        /// <summary>
        ///     Sqlite data provider
        /// </summary>
        public const string SqlLiteDataProvider = "System.Data.SQLite";

        /// <summary>
        ///     Virtual bin folder in the app directory
        /// </summary>
        public const string VirtualBin = "~/bin";

        #endregion
    }
}