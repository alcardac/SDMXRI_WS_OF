// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DatasetAction.cs" company="Eurostat">
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
    using System.Collections.Generic;
    using System.Text;

    #endregion

    /// <summary>
    ///     Defines all the actions that are available in the header of a SDMX message
    /// </summary>
    public enum DatasetActionEnumType
    {
        /// <summary>
        ///     Null value; Can be used to check if the value is not set;
        /// </summary>
        Null = 0, 

        /// <summary>
        ///     Message is of type Append
        /// </summary>
        Append, 

        /// <summary>
        ///     Message is of type Replace
        /// </summary>
        Replace, 

        /// <summary>
        ///     Message is of type Delete
        /// </summary>
        Delete, 

        /// <summary>
        ///     Message is of type Information
        /// </summary>
        Information
    }

    /// <summary>
    ///     Defines all the actions that are available in the header of a SDMX message
    /// </summary>
    public class DatasetAction : BaseConstantType<DatasetActionEnumType>
    {
        #region Static Fields

        /// <summary>
        ///     The _instances.
        /// </summary>
        private static readonly Dictionary<DatasetActionEnumType, DatasetAction> Instances =
            new Dictionary<DatasetActionEnumType, DatasetAction>
                {
                    {
                        DatasetActionEnumType.Append, 
                        new DatasetAction(
                        DatasetActionEnumType.Append, "Append")
                    }, 
                    {
                        DatasetActionEnumType.Replace, 
                        new DatasetAction(
                        DatasetActionEnumType.Replace, "Replace")
                    }, 
                    {
                        DatasetActionEnumType.Delete, 
                        new DatasetAction(
                        DatasetActionEnumType.Delete, "Delete")
                    }, 
                    {
                        DatasetActionEnumType.Information, 
                        new DatasetAction(
                        DatasetActionEnumType.Information, 
                        "Information")
                    }
                };

        #endregion

        #region Fields

        /// <summary>
        ///     The _action.
        /// </summary>
        private readonly string _action;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DatasetAction"/> class.
        /// </summary>
        /// <param name="enumType"> The enum type. </param>
        /// <param name="action">The action. </param>
        private DatasetAction(DatasetActionEnumType enumType, string action)
            : base(enumType)
        {
            this._action = action;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets the values.
        /// </summary>
        public static IEnumerable<DatasetAction> Values
        {
            get
            {
                return Instances.Values;
            }
        }

        /// <summary>
        ///     Gets the action.
        /// </summary>
        public string Action
        {
            get
            {
                return this._action;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// From an action (Append, Update, Replace, Delete, or Information) will return the DATASET_ACTION enum equivalent,
        ///     The input string is case insensitive, and 'update' treats as Append
        /// </summary>
        /// <param name="action">The action. </param>
        /// <returns>
        /// The <see cref="DatasetAction"/> .
        /// </returns>
        public static DatasetAction GetAction(string action)
        {
            if (action == null)
            {
                throw new ArgumentNullException("action");
            }

            if (action.Equals("update", StringComparison.OrdinalIgnoreCase))
            {
                return GetFromEnum(DatasetActionEnumType.Append);
            }

            foreach (DatasetAction currentAction in Values)
            {
                if (currentAction.Action.Equals(action, StringComparison.OrdinalIgnoreCase))
                {
                    return currentAction;
                }
            }

            string concat = string.Empty;
            var sb = new StringBuilder();

            foreach (DatasetAction currentArgument in Values)
            {
                sb.Append(concat);
                sb.Append(currentArgument.Action);
                concat = ", ";
            }

            throw new ArgumentException("Unknown Dataset Action, allowed values are  '" + sb + "'");
        }

        /// <summary>
        /// Implicit conversion from <see cref="DatasetActionEnumType"/> to <see cref="DatasetAction"/>
        /// </summary>
        /// <param name="enumType">The <see cref="DatasetActionEnumType"/>.</param>
        /// <returns>
        /// the instance of <see cref="DatasetAction"/> mapped to <paramref name="enumType"/>
        /// </returns>
        public static implicit operator DatasetAction(DatasetActionEnumType enumType)
        {
            return GetFromEnum(enumType);
        }

        /// <summary>
        /// Gets the instance of <see cref="DatasetAction"/> mapped to <paramref name="enumType"/>
        /// </summary>
        /// <param name="enumType">
        /// The <c>enum</c> type
        /// </param>
        /// <returns>
        /// the instance of <see cref="DatasetAction"/> mapped to <paramref name="enumType"/>
        /// </returns>
        public static DatasetAction GetFromEnum(DatasetActionEnumType enumType)
        {
            DatasetAction output;
            if (Instances.TryGetValue(enumType, out output))
            {
                return output;
            }

            return null;
        }

        #endregion
    }
}