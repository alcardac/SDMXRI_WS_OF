// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AuthExtensions.cs" company="Eurostat">
//   Date Created : 2013-04-13
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   The dataflow authorization extensions.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Estat.Sri.MappingStoreRetrieval.Extensions
{
    using System;
    using System.Collections.Generic;

    using Estat.Sdmxsource.Extension.Manager;

    using log4net;

    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;

    /// <summary>
    ///     The dataflow authorization extensions.
    /// </summary>
    public static class AuthExtensions
    {
        #region Static Fields

        /// <summary>
        /// The _log.
        /// </summary>
        private static readonly ILog _log = LogManager.GetLogger(typeof(AuthExtensions));

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Validate the specified <paramref name="authManager"/>.
        /// </summary>
        /// <param name="authManager">
        /// The dataflow authorization manager.
        /// </param>
        /// <param name="allowedDataflows">
        /// The allowed dataflows.
        /// </param>
        /// <exception cref="ArgumentException">
        /// <paramref name="authManager"/> is null but <paramref name="allowedDataflows"/> is not
        /// </exception>
        public static void ValidateAuthManager(this IAuthSdmxMutableObjectRetrievalManager authManager, IList<IMaintainableRefObject> allowedDataflows)
        {
            if (allowedDataflows != null && authManager == null)
            {
                _log.Error(ErrorMessages.ExceptionISdmxMutableObjectAuthRetrievalManagerNotSet);
                throw new ArgumentException(ErrorMessages.ExceptionISdmxMutableObjectAuthRetrievalManagerNotSet, "allowedDataflows");
            }
        }

        /// <summary>
        /// Check if <paramref name="structureType"/> requires authentication
        /// </summary>
        /// <param name="structureType">
        /// The structure type.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public static bool NeedsAuth(this IStructureReference structureType)
        {
            return structureType != null && structureType.MaintainableStructureEnumType.NeedsAuth();
        }

        #endregion
    }
}