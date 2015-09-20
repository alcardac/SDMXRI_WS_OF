// --------------------------------------------------------------------------------------------------------------------
// <copyright file="QueryStructureRequestBuilderManager.cs" company="Eurostat">
//   Date Created : 2013-03-28
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   The query structure request builder manager.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Estat.Sri.CustomRequests.Manager
{
    using System.Collections.Generic;

    using Estat.Sri.CustomRequests.Builder;
    using Estat.Sri.CustomRequests.Factory;

    using Org.Sdmxsource.Sdmx.Api.Model.Format;
    using Org.Sdmxsource.Sdmx.Api.Model.Header;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;

    /// <summary>
    ///     The query structure request builder manager.
    /// </summary>
    public class QueryStructureRequestBuilderManager : IQueryStructureRequestBuilderManager
    {
        #region Fields

        /// <summary>
        /// The factories.
        /// </summary>
        private readonly IQueryStructureRequestFactory[] _factories;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="QueryStructureRequestBuilderManager"/> class.
        /// </summary>
        public QueryStructureRequestBuilderManager()
            : this((IHeader)null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="QueryStructureRequestBuilderManager"/> class.
        /// </summary>
        /// <param name="header">
        /// The header.
        /// </param>
        public QueryStructureRequestBuilderManager(IHeader header)
        {
            this._factories = new IQueryStructureRequestFactory[] { new QueryStructureRequestFactory(new QueryStructureRequestBuilderV2(header)) };
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="QueryStructureRequestBuilderManager"/> class.
        /// </summary>
        /// <param name="factories">
        /// The factories.
        /// </param>
        public QueryStructureRequestBuilderManager(IQueryStructureRequestFactory[] factories)
        {
            this._factories = factories;
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Builds a query structure request in the requested format
        /// </summary>
        /// <param name="structureReferences">
        ///     The list of <see cref="IStructureReference"/> to build a representation of <c>QueryStructureRequest</c>
        /// </param>
        /// <param name="structureQueryFormat">
        ///     The required format.
        /// </param>
        /// <param name="resolveReferences">
        /// Set to <c>True</c> to resolve references.
        /// </param>
        /// <typeparam name="T">
        /// Generic type parameter.
        /// </typeparam>
        /// <returns>
        /// Representation of query in the desired format.
        /// </returns>
        public T BuildStructureQuery<T>(IEnumerable<IStructureReference> structureReferences, IStructureQueryFormat<T> structureQueryFormat, bool resolveReferences)
        {
            for (int i = 0; i < this._factories.Length; i++)
            {
                var factory = this._factories[i];
                var builder = factory.GetStructureQueryBuilder(structureQueryFormat);
                if (builder != null)
                {
                    return builder.BuildStructureQuery(structureReferences, resolveReferences);
                }
            }

            return default(T);
        }

        #endregion
    }
}