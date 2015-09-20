// -----------------------------------------------------------------------
// <copyright file="IQueryStructureRequestFactory.cs" company="Eurostat">
//   Date Created : 2013-03-27
//   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
// 
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// -----------------------------------------------------------------------
namespace Estat.Sri.CustomRequests.Factory
{
    using Estat.Sri.CustomRequests.Builder;

    using Org.Sdmxsource.Sdmx.Api.Model.Format;

    /// <summary>
    /// A  query structure request factory is a factory which is responsible fore creating a builder that can build
    /// a <c>QueryStructureRequest</c> message in the format defined by the <see cref="IStructureQueryFormat{T}"/> 
    /// </summary>
    public interface IQueryStructureRequestFactory
    {
        /// <summary>
        /// Returns a <see cref="IQueryStructureRequestBuilder{T}"/> only if this factory understands the <see cref="IStructureQueryFormat{T}"/>.  If the format is unknown, null will be returned
        /// </summary>
        /// <typeparam name="T">generic type parameter</typeparam>
        /// <param name="format">The <see cref="IStructureQueryFormat{T}"/>.</param>
        /// <returns><see cref="IQueryStructureRequestBuilder{T}"/> if this factory knows how to build this query format, or null if it doesn't</returns>
        IQueryStructureRequestBuilder<T> GetStructureQueryBuilder<T>(IStructureQueryFormat<T> format);
    }
}