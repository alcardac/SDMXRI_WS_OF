// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ParameterBuilderOr.cs" company="EUROSTAT">
//   Date Created : 2014-10-31
//   //   Copyright (c) 2014 by the European   Commission, represented by Eurostat.   All rights reserved.
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   The parameter builder for OR.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Estat.Sri.CustomRequests.Builder.QueryBuilder
{
    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Query;

    /// <summary>
    ///     The parameter builder for OR.
    /// </summary>
    internal class ParameterBuilderOr : IParameterBuilder
    {
        #region Fields

        /// <summary>
        ///     The _data parameters or type
        /// </summary>
        private readonly DataParametersOrType _dataParametersOrType;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="ParameterBuilderOr" /> class.
        /// </summary>
        public ParameterBuilderOr()
        {
            this._dataParametersOrType = new DataParametersOrType();
        }

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
            this._dataParametersOrType.AttributeValue.Add(attributeValueType);
        }

        /// <summary>
        /// Adds the dimension.
        /// </summary>
        /// <param name="dimension">
        /// The dimension.
        /// </param>
        public void AddDimension(DimensionValueType dimension)
        {
            this._dataParametersOrType.DimensionValue.Add(dimension);
        }

        /// <summary>
        /// Populates the AND parameter.
        /// </summary>
        /// <param name="andParameter">
        /// The and parameter.
        /// </param>
        public void PopulateAndParameter(DataParametersAndType andParameter)
        {
            andParameter.Or.Add(this._dataParametersOrType);
        }

        #endregion
    }
}