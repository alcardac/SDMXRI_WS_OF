// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CrossMutableRetrievalManagerFactory.cs" company="Eurostat">
//   Date Created : 2013-06-15
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   The mutable retrieval manager factory.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Estat.Sri.MappingStoreRetrieval.Factory
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;

    using Estat.Sri.MappingStoreRetrieval.Manager;

    using Org.Sdmxsource.Sdmx.Api.Manager.Retrieval.Mutable;

    /// <summary>
    ///     The mutable retrieval manager factory.
    /// </summary>
    public class CrossMutableRetrievalManagerFactory : ICrossRetrievalManagerFactory
    {
        #region Static Fields

        /// <summary>
        ///     The default factory methods.
        /// </summary>
        private static readonly IDictionary<Type, Func<object, ISdmxMutableObjectRetrievalManager, ICrossReferenceMutableRetrievalManager>> _factoryMethods =
            new Dictionary<Type, Func<object, ISdmxMutableObjectRetrievalManager, ICrossReferenceMutableRetrievalManager>>
                {
                    {
                        typeof(Database), 
                        (settings, retrievalManager) =>
                        new CrossReferenceRetrievalManager(
                            retrievalManager, settings as Database)
                    }, 
                    {
                        typeof(ConnectionStringSettings), 
                        (settings, retrievalManager) =>
                        new CrossReferenceRetrievalManager(
                            retrievalManager, settings as ConnectionStringSettings)
                    }, 
                };

        #endregion

        #region Fields

        /// <summary>
        ///     The user provided factory method.
        /// </summary>
        private readonly Func<object, ISdmxMutableObjectRetrievalManager, ICrossReferenceMutableRetrievalManager> _factoryMethod;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CrossMutableRetrievalManagerFactory"/> class. 
        /// </summary>
        /// <param name="factoryMethod">
        /// The user provided factory method.
        /// </param>
        public CrossMutableRetrievalManagerFactory(Func<object, ISdmxMutableObjectRetrievalManager, ICrossReferenceMutableRetrievalManager> factoryMethod)
        {
            this._factoryMethod = factoryMethod;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CrossMutableRetrievalManagerFactory"/> class. 
        /// </summary>
        public CrossMutableRetrievalManagerFactory()
        {
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Returns an instance of <see cref="ISdmxMutableObjectRetrievalManager"/> created using the specified
        ///     <paramref name="settings"/>
        /// </summary>
        /// <typeparam name="T">
        /// The type of settings
        /// </typeparam>
        /// <param name="settings">
        /// The settings.
        /// </param>
        /// <param name="retrievalManager">
        /// The retrieval Manager.
        /// </param>
        /// <returns>
        /// The <see cref="ISdmxMutableObjectRetrievalManager"/>.
        /// </returns>
        public ICrossReferenceMutableRetrievalManager GetCrossRetrievalManager<T>(T settings, ISdmxMutableObjectRetrievalManager retrievalManager)
        {
            ICrossReferenceMutableRetrievalManager manager = null;
            if (this._factoryMethod != null)
            {
                manager = this._factoryMethod(settings, retrievalManager);
            }

            Func<object, ISdmxMutableObjectRetrievalManager, ICrossReferenceMutableRetrievalManager> method;
            if (_factoryMethods.TryGetValue(typeof(T), out method))
            {
                manager = method(settings, retrievalManager);
            }

            return manager;
        }

        #endregion
    }
}