// -----------------------------------------------------------------------
// <copyright file="ComplexDataQueryExtension.cs" company="EUROSTAT">
//   Date Created : 2014-10-30
//   Copyright (c) 2014 by the European   Commission, represented by Eurostat.   All rights reserved.
// 
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// -----------------------------------------------------------------------
namespace Estat.Sri.CustomRequests.Extension
{
    using System.Collections.Generic;
    using System.Linq;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Data.Query.Complex;

    /// <summary>
    /// The complex data query extension.
    /// </summary>
    public static class ComplexDataQueryExtension
    {
        /// <summary>
        /// Gets a value to determine if we should use the AND operator.
        /// </summary>
        /// <param name="selection">The selection.</param>
        /// <returns><c>true</c> if the values should be <c>AND'ed</c>; otherwise false</returns>
        public static bool ShouldUseAnd(this IComplexDataQuerySelection selection)
        {
            if (selection == null)
            {
                return false;
            }

            var values = selection.HasMultipleValues() ? selection.Values : new HashSet<IComplexComponentValue>() { selection.Value };
            return values.All(value => value.OrderedOperator != null && value.OrderedOperator == OrderedOperatorEnumType.NotEqual) || values.All(value => value.TextSearchOperator != null && value.TextSearchOperator == TextSearchEnumType.NotEqual);
        }
    }
}