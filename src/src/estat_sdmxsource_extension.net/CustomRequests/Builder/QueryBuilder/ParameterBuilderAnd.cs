// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ParameterBuilderAnd.cs" company="EUROSTAT">
//   Date Created : 2014-10-31
//   //   Copyright (c) 2014 by the European   Commission, represented by Eurostat.   All rights reserved.
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   The parameter builder for AND.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Estat.Sri.CustomRequests.Builder.QueryBuilder
{
    using System.Collections.Generic;

    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Query;
    using Org.Sdmxsource.Util.Extensions;

    /// <summary>
    ///     The parameter builder for AND.
    /// </summary>
    internal class ParameterBuilderAnd : IParameterBuilder
    {
        #region Fields

        /// <summary>
        ///     The _attributes
        /// </summary>
        private readonly List<AttributeValueType> _attributes = new List<AttributeValueType>();

        /// <summary>
        ///     The _dimensions
        /// </summary>
        private readonly List<DimensionValueType> _dimensions = new List<DimensionValueType>();

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Adds the attribute.
        /// </summary>
        /// <param name="attributeValueType">
        /// Type of the attribute value.
        /// </param>
        public void AddAttribute(AttributeValueType attributeValueType)
        {
            this._attributes.Add(attributeValueType);
        }

        /// <summary>
        /// Adds the dimension.
        /// </summary>
        /// <param name="dimension">
        /// The dimension.
        /// </param>
        public void AddDimension(DimensionValueType dimension)
        {
            this._dimensions.Add(dimension);
        }

        /// <summary>
        /// Populates the and parameter.
        /// </summary>
        /// <param name="andParameter">
        /// The and parameter.
        /// </param>
        public void PopulateAndParameter(DataParametersAndType andParameter)
        {
            andParameter.DimensionValue.AddAll(this._dimensions);
            andParameter.AttributeValue.AddAll(this._attributes);
        }

        #endregion
    }
}