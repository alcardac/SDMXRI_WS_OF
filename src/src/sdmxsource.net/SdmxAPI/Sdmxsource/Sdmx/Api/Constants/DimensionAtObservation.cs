// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DimensionAtObservation.cs" company="Eurostat">
//   Date Created : 2013-08-19
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Constants
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using Org.Sdmxsource.Sdmx.Api.Constants.InterfaceConstant;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public enum DimensionAtObservationEnumType
    {
        /// <summary>
        /// All
        /// </summary>
        All,
        /// <summary>
        /// Time
        /// </summary>
        Time
    }

     /// <summary>
     /// 
     /// </summary>
     public sealed class DimensionAtObservation : BaseConstantType<DimensionAtObservationEnumType>
     {
         private readonly string _value;

         /// <summary>
         /// The instances.
         /// </summary>
         private static readonly IDictionary<DimensionAtObservationEnumType, DimensionAtObservation> Instances =
             new Dictionary<DimensionAtObservationEnumType, DimensionAtObservation>
                 {
                     {
                         DimensionAtObservationEnumType.All,
                         new DimensionAtObservation(DimensionAtObservationEnumType.All, "AllDimensions")
                     },
                     {
                         DimensionAtObservationEnumType.Time,
                         new DimensionAtObservation(
                         DimensionAtObservationEnumType.Time, DimensionObject.TimeDimensionFixedId)
                     }
                 };

         /// <summary>
         /// 
         /// </summary>
         /// <param name="enumType"></param>
         /// <param name="value"></param>
         public DimensionAtObservation(DimensionAtObservationEnumType enumType, string value)
             : base(enumType)
         {
             this._value = value;
         }


         /// <summary>
         ///     Gets all instances
         /// </summary>
         public static IEnumerable<DimensionAtObservation> Values
         {
             get
             {
                 return Instances.Values;
             }
         }

         /// <summary>
         /// The value
         /// </summary>
         public string Value
         {
             get
             {
                 return this._value;
             }
         }

         #region Public Methods and Operators

         /// <summary>
         /// Gets the instance of <see cref="DimensionAtObservation"/> mapped to <paramref name="enumType"/>
         /// </summary>
         /// <param name="enumType">
         /// The <c>enum</c> type
         /// </param>
         /// <returns>
         /// the instance of <see cref="DimensionAtObservation"/> mapped to <paramref name="enumType"/>
         /// </returns>
         public static DimensionAtObservation GetFromEnum(DimensionAtObservationEnumType enumType)
         {
             DimensionAtObservation output;
             if (Instances.TryGetValue(enumType, out output))
             {
                 return output;
             }

             return null;
         }

         #endregion

     }
}
