// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TertiaryBool.cs" company="Eurostat">
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

    using System.Collections.Generic;

    #endregion

    /// <summary>
    ///     An alternative to Boolean which supplements the true and false values with a third value of "UNSET".
    /// </summary>
    public enum TertiaryBoolEnumType
    {
        /// <summary>
        ///     Null value; Can be used to check if the value is not set;
        /// </summary>
        Null = 0, 

        /// <summary>
        ///     The true.
        /// </summary>
        True, 

        /// <summary>
        ///     The false.
        /// </summary>
        False, 

        /// <summary>
        ///     The unset.
        /// </summary>
        Unset
    }

    /// <summary>
    ///     An alternative to Boolean which supplements the true and false values with a third value of "UNSET".
    /// </summary>
    public class TertiaryBool : BaseConstantType<TertiaryBoolEnumType>
    {
        #region Static Fields

        /// <summary>
        ///     The instances.
        /// </summary>
        private static readonly Dictionary<TertiaryBoolEnumType, TertiaryBool> Instances =
            new Dictionary<TertiaryBoolEnumType, TertiaryBool>
                {
                    {
                        TertiaryBoolEnumType.True, 
                        new TertiaryBool(TertiaryBoolEnumType.True, true)
                    }, 
                    {
                        TertiaryBoolEnumType.False, 
                        new TertiaryBool(TertiaryBoolEnumType.False, false)
                    }, 
                    {
                        TertiaryBoolEnumType.Unset, 
                        new TertiaryBool(TertiaryBoolEnumType.Unset, false)
                    }
                };

        #endregion

        #region Fields

        /// <summary>
        ///     The _is true.
        /// </summary>
        private readonly bool _isTrue;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TertiaryBool"/> class.
        /// </summary>
        /// <param name="enumType">
        /// The enum type.
        /// </param>
        /// <param name="isTrue">
        /// The is true.
        /// </param>
        private TertiaryBool(TertiaryBoolEnumType enumType, bool isTrue)
            : base(enumType)
        {
            this._isTrue = isTrue;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets all instances for this type
        /// </summary>
        public static IEnumerable<TertiaryBool> Values
        {
            get
            {
                return Instances.Values;
            }
        }

        /// <summary>
        ///     Gets a value indicating whether is true.
        /// </summary>
        public bool IsTrue
        {
            get
            {
                return this._isTrue;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Gets the instance of <see cref="TertiaryBool"/> mapped to <paramref name="enumType"/>
        /// </summary>
        /// <param name="enumType">
        /// The <c>enum</c> type
        /// </param>
        /// <returns>
        /// the instance of <see cref="TertiaryBool"/> mapped to <paramref name="enumType"/>
        /// </returns>
        public static TertiaryBool GetFromEnum(TertiaryBoolEnumType enumType)
        {
            TertiaryBool output;
            if (Instances.TryGetValue(enumType, out output))
            {
                return output;
            }

            return null;
        }

        /// <summary>
        /// The parse boolean.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <returns>
        /// The <see cref="TertiaryBool"/> .
        /// </returns>
        public static TertiaryBool ParseBoolean(bool? value)
        {
            /* $$$   boolean null? */
            if (value == null)
            {
                return GetFromEnum(TertiaryBoolEnumType.Unset);
            }

            if (value == true)
            {
                return GetFromEnum(TertiaryBoolEnumType.True);
            }

            return GetFromEnum(TertiaryBoolEnumType.False);
        }

        /// <summary>
        ///     The is set.
        /// </summary>
        /// <returns>
        ///     The <see cref="bool" /> .
        /// </returns>
        public bool IsSet()
        {
            return this.EnumType != TertiaryBoolEnumType.Unset;
        }

        /// <summary>
        ///     The is unset.
        /// </summary>
        /// <returns>
        ///     The <see cref="bool" /> .
        /// </returns>
        public bool IsUnset()
        {
            return this.EnumType == TertiaryBoolEnumType.Unset;
        }

        #endregion
    }
}