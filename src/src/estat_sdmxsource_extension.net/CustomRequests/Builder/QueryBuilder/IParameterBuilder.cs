// -----------------------------------------------------------------------
// <copyright file="IParameterBuilder.cs" company="EUROSTAT">
//   Date Created : 2014-10-30
//   Copyright (c) 2014 by the European   Commission, represented by Eurostat.   All rights reserved.
// 
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// -----------------------------------------------------------------------
namespace Estat.Sri.CustomRequests.Builder.QueryBuilder
{
    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Query;

    /// <summary>
    /// The builder model for Parameter types.
    /// </summary>
    internal interface IParameterBuilder
    {
        /// <summary>
        /// Adds the dimension.
        /// </summary>
        /// <param name="dimension">The dimension.</param>
        void AddDimension(DimensionValueType dimension);

        /// <summary>
        /// Adds the attribute.
        /// </summary>
        /// <param name="attributeValueType">Type of the attribute value.</param>
        void AddAttribute(AttributeValueType attributeValueType);

        /// <summary>
        /// Populates the AND parameter.
        /// </summary>
        /// <param name="andParameter">The and parameter.</param>
        void PopulateAndParameter(DataParametersAndType andParameter);
    }
}