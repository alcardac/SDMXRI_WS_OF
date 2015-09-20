// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SDMXObjectUtil.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Util.Objects
{
    using Org.Sdmxsource.Sdmx.Api.Constants;

    /// <summary>
    /// The sdmx object util.
    /// </summary>
    public class SdmxObjectUtil
    {
        #region Static Fields

        /// <summary>
        /// The _false value.
        /// </summary>
        private static readonly TertiaryBool _falseValue = TertiaryBool.GetFromEnum(TertiaryBoolEnumType.False);

        /// <summary>
        /// The _true value.
        /// </summary>
        private static readonly TertiaryBool _trueValue = TertiaryBool.GetFromEnum(TertiaryBoolEnumType.True);

        /// <summary>
        /// The _unset value.
        /// </summary>
        private static readonly TertiaryBool _unsetValue = TertiaryBool.GetFromEnum(TertiaryBoolEnumType.Unset);

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Create tertiary from the specified <paramref name="isSet"/> and <paramref name="valueRen"/>
        /// </summary>
        /// <param name="isSet">
        /// The is set.
        /// </param>
        /// <param name="valueRen">
        /// The value.
        /// </param>
        /// <returns>
        /// The <see cref="TertiaryBool"/>.
        /// </returns>
        public static TertiaryBool CreateTertiary(bool isSet, bool valueRen)
        {
            if (!isSet)
            {
                return _unsetValue;
            }

            return valueRen ? _trueValue : _falseValue;
        }

        /// <summary>
        /// Create tertiary from the specified  and <paramref name="valueRen"/>
        /// </summary>
        /// <param name="valueRen">
        /// The value.
        /// </param>
        /// <returns>
        /// The corresponding <see cref="TertiaryBool"/> if <paramref name="valueRen"/> is not null; otherwise <see cref="TertiaryBoolEnumType.Unset"/>.
        /// </returns>
        public static TertiaryBool CreateTertiary(bool? valueRen)
        {
            if (!valueRen.HasValue)
            {
                return _unsetValue;
            }

            return valueRen.Value ? _trueValue : _falseValue;
        }

        #endregion
    }
}