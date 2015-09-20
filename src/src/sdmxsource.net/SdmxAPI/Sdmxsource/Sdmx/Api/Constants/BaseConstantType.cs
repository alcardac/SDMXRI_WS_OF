// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BaseConstantType.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Constants
{
    #region Using directives

    using System;

    #endregion

    /// <summary>
    /// Base class for Constant classes which glues them with the specified <typeparamref name="T"/>
    /// </summary>
    /// <typeparam name="T">
    /// The <c>enum</c> type
    /// </typeparam>
    public abstract class BaseConstantType<T>
        where T : struct, IConvertible
    {
        #region Fields

        /// <summary>
        ///     The <c>enum</c> value.
        /// </summary>
        private readonly T _enumType;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseConstantType{T}"/> class.
        /// </summary>
        /// <param name="enumType">
        /// The <c>enum</c> Type.
        /// </param>
        protected BaseConstantType(T enumType)
        {
            this._enumType = enumType;
            if (!typeof(T).IsEnum)
            {
                throw new ArgumentException("T must be an enumerated type");
            }
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets the <c>enum</c> value
        /// </summary>
        public T EnumType
        {
            get
            {
                return this._enumType;
            }
        }

        #endregion

        public T ToEnumType()
        {
            return this.EnumType;
        }

        /// <summary>
        /// Attributes the specified constant type.
        /// </summary>
        /// <param name="constantType">Type of the constant.</param>
        /// <returns></returns>
        public static implicit operator T(BaseConstantType<T> constantType)
        {
            return constantType == null ? default(T) : constantType.EnumType;
        }
    }
}