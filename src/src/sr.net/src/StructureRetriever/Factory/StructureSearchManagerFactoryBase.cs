// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StructureSearchManagerFactoryBase.cs" company="Eurostat">
//   Date Created : 2013-09-23
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   The structure search manager base.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Estat.Nsi.StructureRetriever.Factory
{
    using System;
    using System.Collections.Generic;

    using Estat.Sri.MappingStoreRetrieval.Extensions;

    using Org.Sdmxsource.Sdmx.Api.Constants;

    /// <summary>
    /// The structure search manager base.
    /// </summary>
    /// <typeparam name="TStructureSearch">
    /// The structure search type
    /// </typeparam>
    public abstract class StructureSearchManagerFactoryBase<TStructureSearch> : IStructureSearchManagerFactory<TStructureSearch>
    {
        #region Fields

        /// <summary>
        ///     The user provided factory method.
        /// </summary>
        private readonly Func<object, TStructureSearch> _factoryMethod;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="StructureSearchManagerFactoryBase{TStructureSearch}"/> class.
        /// </summary>
        /// <param name="factoryMethod">
        /// The factory method.
        /// </param>
        protected StructureSearchManagerFactoryBase(Func<object, TStructureSearch> factoryMethod)
        {
            this._factoryMethod = factoryMethod;
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Returns an instance of the structure search manager created using the specified
        ///     <paramref name="settings"/>
        /// </summary>
        /// <typeparam name="T">
        /// The type of the settings
        /// </typeparam>
        /// <param name="settings">
        /// The settings.
        /// </param>
        /// <param name="sdmxSchemaVersion">
        /// The SDMX Schema Version.
        /// </param>
        /// <returns>
        /// The structure search manager.
        /// </returns>
        public TStructureSearch GetStructureSearchManager<T>(T settings, SdmxSchema sdmxSchemaVersion)
        {
            TStructureSearch manager = default(TStructureSearch);
            if (this._factoryMethod != null)
            {
                manager = this._factoryMethod(settings);
            }

            var isDefault = manager.IsDefault();
            if (isDefault)
            {
                Func<object, SdmxSchema, TStructureSearch> method;
                if (this.GetFactories().TryGetValue(typeof(T), out method))
                {
                    manager = method(settings, sdmxSchemaVersion);
                }
            }

            return manager;
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Gets the factories.
        /// </summary>
        /// <returns>The type based dictionary.</returns>
        protected abstract IDictionary<Type, Func<object, SdmxSchema, TStructureSearch>> GetFactories();

        #endregion
    }
}