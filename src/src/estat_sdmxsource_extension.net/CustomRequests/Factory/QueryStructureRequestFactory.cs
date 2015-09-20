// --------------------------------------------------------------------------------------------------------------------
// <copyright file="QueryStructureRequestFactory.cs" company="Eurostat">
//   Date Created : 2013-03-28
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   The query structure request factory.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Estat.Sri.CustomRequests.Factory
{
    using System.Xml.Linq;

    using Estat.Sri.CustomRequests.Builder;
    using Estat.Sri.CustomRequests.Model;

    using Org.Sdmxsource.Sdmx.Api.Model.Format;

    /// <summary>
    /// The query structure request factory.
    /// </summary>
    public class QueryStructureRequestFactory : IQueryStructureRequestFactory
    {
        #region Fields

        /// <summary>
        /// The _request builder.
        /// </summary>
        private readonly IQueryStructureRequestBuilder<XDocument> _requestBuilder;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="QueryStructureRequestFactory"/> class.
        /// </summary>
        /// <param name="requestBuilder">
        /// The request builder.
        /// </param>
        public QueryStructureRequestFactory(IQueryStructureRequestBuilder<XDocument> requestBuilder)
        {
            this._requestBuilder = requestBuilder;
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Returns a <see cref="IQueryStructureRequestBuilder{T}"/> only if this factory understands the
        ///     <see cref="IStructureQueryFormat{T}"/>
        ///     .  If the format is unknown, null will be returned
        /// </summary>
        /// <typeparam name="T">
        /// generic type parameter
        /// </typeparam>
        /// <param name="format">
        /// The <see cref="IStructureQueryFormat{T}"/>.
        /// </param>
        /// <returns>
        /// <see cref="IQueryStructureRequestBuilder{T}"/> if this factory knows how to build this query format, or null if it doesn't
        /// </returns>
        public IQueryStructureRequestBuilder<T> GetStructureQueryBuilder<T>(IStructureQueryFormat<T> format)
        {
            if (format is QueryStructureRequestFormat)
            {
                return this._requestBuilder as IQueryStructureRequestBuilder<T>;
            }

            return null;
        }

        #endregion
    }
}