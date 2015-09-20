// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MutableRetrievalManagerFactory.cs" company="Eurostat">
//   Date Created : 2013-04-16
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
    /// The mutable retrieval manager factory.
    /// </summary>
    public class AdvancedMutableRetrievalManagerFactory : IAdvancedMutableRetrievalManagerFactory
    {
        #region Static Fields

        /// <summary>
        /// The default factory methods.
        /// </summary>
        private static readonly IDictionary<Type, Func<object, IAdvancedSdmxMutableObjectRetrievalManager>> _factoryMethods =
            new Dictionary<Type, Func<object, IAdvancedSdmxMutableObjectRetrievalManager>>
                {
                    {
                        typeof(Database), 
                        settings => new AdvancedStructureRetriever(settings as Database)
                    }, 
                    {
                        typeof(ConnectionStringSettings), 
                        settings =>
                        new AdvancedStructureRetriever(settings as ConnectionStringSettings)
                    ////}, 
                    //// TODO Caching IAdvancedSdmxMutableObjectRetrievalManager
                    ////{
                    ////    typeof(IAdvancedSdmxMutableObjectRetrievalManager), 
                    ////    settings =>
                    ////    new CachedRetrievalManager(null, settings as ISdmxMutableObjectRetrievalManager)
                    }
                };

        #endregion

        #region Fields

        /// <summary>
        /// The user provided factory method.
        /// </summary>
        private readonly Func<object, IAdvancedSdmxMutableObjectRetrievalManager> _factoryMethod;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AdvancedMutableRetrievalManagerFactory"/> class.
        /// </summary>
        /// <param name="factoryMethod">
        /// The user provided factory method. 
        /// </param>
        public AdvancedMutableRetrievalManagerFactory(Func<object, IAdvancedSdmxMutableObjectRetrievalManager> factoryMethod)
        {
            this._factoryMethod = factoryMethod;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AdvancedMutableRetrievalManagerFactory"/> class. 
        /// </summary>
        public AdvancedMutableRetrievalManagerFactory()
        {
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Returns an instance of <see cref="IAdvancedSdmxMutableObjectRetrievalManager"/> created using the specified <paramref name="settings"/>
        /// </summary>
        /// <typeparam name="T">
        /// The type of settings
        /// </typeparam>
        /// <param name="settings">
        /// The settings.
        /// </param>
        /// <returns>
        /// The <see cref="IAdvancedSdmxMutableObjectRetrievalManager"/>.
        /// </returns>
        public IAdvancedSdmxMutableObjectRetrievalManager GetRetrievalManager<T>(T settings)
        {
            IAdvancedSdmxMutableObjectRetrievalManager manager = null;
            if (this._factoryMethod != null)
            {
                manager = this._factoryMethod(settings);
            }

            Func<object, IAdvancedSdmxMutableObjectRetrievalManager> method;
            if (_factoryMethods.TryGetValue(typeof(T), out method))
            {
                manager = method(settings);
            }

            return manager;
        }

        #endregion
    }
}