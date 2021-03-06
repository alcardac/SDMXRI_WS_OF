// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AuthMutableStructureSearchManagerFactory.cs" company="Eurostat">
//   Date Created : 2013-04-16
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   The auth mutable structure search manager factory.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Estat.Nsi.StructureRetriever.Factory
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;

    using Estat.Nsi.StructureRetriever.Manager;
    using Estat.Sdmxsource.Extension.Manager;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Manager.Retrieval.Mutable;

    /// <summary>
    /// The authorization mutable structure search manager factory.
    /// </summary>
    public class AuthMutableStructureSearchManagerFactory : StructureSearchManagerFactoryBase<IAuthMutableStructureSearchManager>, IAuthMutableStructureSearchManagerFactory
    {
        #region Static Fields

        /// <summary>
        ///     The default factory methods.
        /// </summary>
        private static readonly IDictionary<Type, Func<object, SdmxSchema, IAuthMutableStructureSearchManager>> _factoryMethods =
            new Dictionary<Type, Func<object, SdmxSchema, IAuthMutableStructureSearchManager>> { { typeof(ConnectionStringSettings), OnConnectionStringSettings } };

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthMutableStructureSearchManagerFactory"/> class.
        /// </summary>
        /// <param name="factoryMethod">
        /// The factory method.
        /// </param>
        public AuthMutableStructureSearchManagerFactory(Func<object, IAuthMutableStructureSearchManager> factoryMethod) : base(factoryMethod)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthMutableStructureSearchManagerFactory"/> class.
        /// </summary>
        public AuthMutableStructureSearchManagerFactory() : this(null)
        {
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Gets the factories.
        /// </summary>
        /// <returns>The type based dictionary.</returns>
        protected override IDictionary<Type, Func<object, SdmxSchema, IAuthMutableStructureSearchManager>> GetFactories()
        {
            return _factoryMethods;
        }

        /// <summary>
        /// The on connection string settings factory method.
        /// </summary>
        /// <param name="settings">
        /// The settings.
        /// </param>
        /// <param name="sdmxSchema">
        /// The SDMX schema.
        /// </param>
        /// <returns>
        /// The <see cref="IMutableStructureSearchManager"/>.
        /// </returns>
        private static IAuthMutableStructureSearchManager OnConnectionStringSettings(object settings, SdmxSchema sdmxSchema)
        {
            var connectionStringSettings = settings as ConnectionStringSettings;
            if (settings == null)
            {
                return null;
            }

            SdmxSchemaEnumType sdmxSchemaEnumType = sdmxSchema != null ? sdmxSchema.EnumType : SdmxSchemaEnumType.Null;
            switch (sdmxSchemaEnumType)
            {
                case SdmxSchemaEnumType.VersionTwo:
                    return new AuthStructureRetrieverAvailableDataV20(connectionStringSettings);
                case SdmxSchemaEnumType.VersionTwoPointOne:
                    return new AuthStructureRetrieverV21(connectionStringSettings);
                default:
                    return new AuthMutableStructureSearchManager(connectionStringSettings);
            }
        }

        #endregion
    }
}