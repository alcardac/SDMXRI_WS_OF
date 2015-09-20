// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ObservationAction.cs" company="Eurostat">
//   Date Created : 2013-08-19
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
    using System.Collections.Generic;
    using System.Text;

    using Org.Sdmxsource.Sdmx.Api.Exception;

    #endregion


    /// <summary>
    ///     For a 2.1 REST data query, this enum contains a list of the parameters.
    ///     <p />
    ///     Observable action; possible options are:
    ///     <ul>
    ///      <li>active - Active observations, regardless of when they were added or updated will be returned</li>
    ///      <li>added - Newly added observations will be returned</li>
    ///      <li>updated - Only updated observations will be returned</li>
    ///      <li>deleted - Only deleted observations will be returned</li>
    ///     </ul>
    /// </summary>
    public enum ObservationActionEnumType
    {
        /// <summary>
        ///     Null value; Can be used to check if the value is not set;
        /// </summary>
        Null = 0,

        /// <summary>
        ///     Active observations, regardless of when they were added or updated will be returned
        /// </summary>
        Active,

        /// <summary>
        ///     Newly added observations will be returned
        /// </summary>
        Added,

        /// <summary>
        ///     Only updated observations will be returned
        /// </summary>
        Updated,

        /// <summary>
        ///     Only deleted observations will be returned
        /// </summary>
        Deleted
    }

    /// <summary>
    /// The observable action
    /// </summary>
    public class ObservationAction : BaseConstantType<ObservationActionEnumType>
    {

        #region Static Fields

        /// <summary>
        ///     The _instances.
        /// </summary>
        private static readonly Dictionary<ObservationActionEnumType, ObservationAction> Instances =
            new Dictionary<ObservationActionEnumType, ObservationAction>
                {
                    {
                        ObservationActionEnumType.Active, 
                        new ObservationAction(
                        ObservationActionEnumType.Active, 
                        "active")
                    }, 
                    {
                        ObservationActionEnumType.Added, 
                        new ObservationAction(
                        ObservationActionEnumType.Added, 
                        "added")
                    }, 
                    {
                        ObservationActionEnumType.Updated, 
                        new ObservationAction(
                        ObservationActionEnumType.Updated, 
                        "updated")
                    }, 
                    {
                        ObservationActionEnumType.Deleted, 
                        new ObservationAction(
                        ObservationActionEnumType.Deleted, 
                        "deleted")
                    }
                };

        #endregion

        #region Fields

        /// <summary>
        ///     The _obsAction.
        /// </summary>
        private readonly string _obsAction;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ObservationAction"/> class.
        /// </summary>
        /// <param name="enumType">
        /// The enum type
        /// </param>
        /// <param name="obsAction">
        /// The obsAction
        /// </param>
        private ObservationAction(ObservationActionEnumType enumType, string obsAction)
            : base(enumType)
        {
            this._obsAction = obsAction;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets the instances of <see cref="ObservationAction" />
        /// </summary>
        public static IEnumerable<ObservationAction> Values
        {
            get
            {
                return Instances.Values;
            }
        }

        /// <summary>
        ///     Gets the obsAction.
        /// </summary>
        public string ObsAction
        {
            get
            {
                return this._obsAction;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Gets the instance of <see cref="ObservationAction"/> mapped to <paramref name="enumType"/>
        /// </summary>
        /// <param name="enumType">
        /// The <c>enum</c> type
        /// </param>
        /// <returns>
        /// the instance of <see cref="ObservationAction"/> mapped to <paramref name="enumType"/>
        /// </returns>
        public static ObservationAction GetFromEnum(ObservationActionEnumType enumType)
        {
            ObservationAction output;
            if (Instances.TryGetValue(enumType, out output))
            {
                return output;
            }

            return null;
        }

        /// <summary>
        /// Parses the string.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The <see cref="ObservationAction" /> .
        /// </returns>
        /// <exception cref="Org.Sdmxsource.Sdmx.Api.Exception.SdmxSemmanticException">Unknown Parameter  + value +  allowed parameters:  + sb.ToString()</exception>
        /// <exception cref="SdmxSemmanticException">Throws Validation Exception</exception>
        public static ObservationAction ParseString(string value)
        {
            foreach (ObservationAction currentQueryDetail in Values)
            {
                if (currentQueryDetail.ObsAction.Equals(value, StringComparison.OrdinalIgnoreCase))
                {
                    return currentQueryDetail;
                }
            }

            var sb = new StringBuilder();
            string concat = string.Empty;

            foreach (ObservationAction currentQueryDetail in Values)
            {
                sb.Append(concat + currentQueryDetail.ObsAction);
                concat = ", ";
            }

            throw new SdmxSemmanticException("Unknown Parameter " + value + " allowed parameters: " + sb.ToString());

        }

        #endregion

    }
}
