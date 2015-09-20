// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StructureReferenceDetail.cs" company="Eurostat">
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

    using Org.Sdmxsource.Sdmx.Api.Exception;

    #endregion

    /// <summary>
    ///   For a 2.1 REST structure query, this enumeration contains a list of all the possible reference parameters
    /// </summary>
    public enum StructureReferenceDetailEnumType
    {
        /// <summary>
        ///   Null value; Can be used to check if the value is not set;
        /// </summary>
        Null = 0, 

        /// <summary>
        ///   The none.
        /// </summary>
        None, 

        /// <summary>
        ///   The parents.
        /// </summary>
        Parents, 

        /// <summary>
        ///   The parents siblings.
        /// </summary>
        ParentsSiblings, 

        /// <summary>
        ///   The children.
        /// </summary>
        Children, 

        /// <summary>
        ///   The descendants.
        /// </summary>
        Descendants, 

        /// <summary>
        ///   The all.
        /// </summary>
        All, 

        /// <summary>
        ///   The specific.
        /// </summary>
        Specific
    }

    /// <summary>
    ///   For a 2.1 REST structure query, this enumeration contains a list of all the possible reference parameters
    /// </summary>
    public class StructureReferenceDetail : BaseConstantType<StructureReferenceDetailEnumType>
    {
        #region Static Fields

        /// <summary>
        ///   The _instances.
        /// </summary>
        private static readonly Dictionary<StructureReferenceDetailEnumType, StructureReferenceDetail> _instances =
            new Dictionary<StructureReferenceDetailEnumType, StructureReferenceDetail>
                {
                    {
                        StructureReferenceDetailEnumType.None, 
                        new StructureReferenceDetail(StructureReferenceDetailEnumType.None, "none")
                    }, 
                    {
                        StructureReferenceDetailEnumType.Parents, 
                        new StructureReferenceDetail(StructureReferenceDetailEnumType.Parents, "parents")
                    }, 
                    {
                        StructureReferenceDetailEnumType.ParentsSiblings, 
                        new StructureReferenceDetail(
                        StructureReferenceDetailEnumType.ParentsSiblings, "parentsandsiblings")
                    }, 
                    {
                        StructureReferenceDetailEnumType.Children, 
                        new StructureReferenceDetail(StructureReferenceDetailEnumType.Children, "children")
                    }, 
                    {
                        StructureReferenceDetailEnumType.Descendants, 
                        new StructureReferenceDetail(StructureReferenceDetailEnumType.Descendants, "descendants")
                    }, 
                    {
                        StructureReferenceDetailEnumType.All, 
                        new StructureReferenceDetail(StructureReferenceDetailEnumType.All, "all")
                    }, 
                    {
                        StructureReferenceDetailEnumType.Specific, 
                        new StructureReferenceDetail(StructureReferenceDetailEnumType.Specific, string.Empty)
                    }, 
                };

        #endregion

        #region Fields

        /// <summary>
        ///   The _value.
        /// </summary>
        private readonly string _value;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="StructureReferenceDetail"/> class.
        /// </summary>
        /// <param name="enumType">
        /// The structure reference type. 
        /// </param>
        /// <param name="name">
        /// The name. 
        /// </param>
        private StructureReferenceDetail(StructureReferenceDetailEnumType enumType, string name)
            : base(enumType)
        {
            this._value = name;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets all instances for this type
        /// </summary>
        public static IEnumerable<StructureReferenceDetail> Values
        {
            get
            {
                return _instances.Values;
            }
        }

        /// <summary>
        ///   Gets the value.
        /// </summary>
        public string Value
        {
            get
            {
                return this._value;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Gets the instance of <see cref="StructureReferenceDetail"/> mapped to <paramref name="enumType"/>
        /// </summary>
        /// <param name="enumType">
        /// The <c>enum</c> type 
        /// </param>
        /// <returns>
        /// the instance of <see cref="StructureReferenceDetail"/> mapped to <paramref name="enumType"/> 
        /// </returns>
        public static StructureReferenceDetail GetFromEnum(StructureReferenceDetailEnumType enumType)
        {
            StructureReferenceDetail output;
            if (_instances.TryGetValue(enumType, out output))
            {
                return output;
            }

            return null;
        }

        /// <summary>
        /// Gets the STRUCTURE_REFERENCE_DETAIL equivalent of the input string (ignores case):
        ///   <ul>
        ///     <li>none - STRUCTURE_REFERENCE_DETAIL.NONE</li>
        ///     <li>parents - STRUCTURE_REFERENCE_DETAIL.PARENTS</li>
        ///     <li><c>parentsandsiblings</c> - STRUCTURE_REFERENCE_DETAIL.PARENTS_SIBLINGS</li>
        ///     <li>children - STRUCTURE_REFERENCE_DETAIL.CHILDREN</li>
        ///     <li>descendants - STRUCTURE_REFERENCE_DETAIL.DESCENDANTS</li>
        ///     <li>all - STRUCTURE_REFERENCE_DETAIL.ALL</li>
        ///     <li>(empty string) - STRUCTURE_REFERENCE_DETAIL.SPECIFIC</li>
        ///   </ul>
        /// </summary>
        /// <param name="value">String value. </param>
        /// <returns>
        /// The <see cref="StructureReferenceDetail"/> . 
        /// </returns>
        public static StructureReferenceDetail ParseString(string value)
        {
            foreach (StructureReferenceDetail currentQueryDetail in Values)
            {
                if (currentQueryDetail.Value.Equals(value, StringComparison.OrdinalIgnoreCase))
                {
                    return currentQueryDetail;
                }
            }

            var sb = new StringBuilder();
            string concat = string.Empty;
            try
            {
                SdmxStructureType structEnumType = SdmxStructureType.ParseClass(value);
                if (!structEnumType.IsMaintainable)
                {
                    foreach (SdmxStructureType currentStructType in SdmxStructureType.Values)
                    {
                        if (currentStructType.IsMaintainable)
                        {
                            sb.Append(concat).Append(currentStructType.UrnClass.ToLowerInvariant());
                            concat = ", ";
                        }
                    }

                    throw new SdmxSemmanticException(
                        "Disallowed structure type " + structEnumType.UrnClass.ToLowerInvariant() + " allowed parameters: " + sb.ToString());
                }
            }
            catch (Exception)
            {
                foreach (StructureReferenceDetail currentQueryDetail in Values)
                {
                    sb.Append(concat + currentQueryDetail.Value);
                    concat = ", ";
                }

                throw new SdmxSemmanticException(
                    "Unknown Parameter " + value + " allowed parameters: " + sb.ToString()
                    + " or a specific structure reference such as 'codelist'");
            }

            return GetFromEnum(StructureReferenceDetailEnumType.Specific);
        }

        /// <summary>
        ///   The to string.
        /// </summary>
        /// <returns> The <see cref="string" /> . </returns>
        public override string ToString()
        {
            return this.Value;
        }

        #endregion
    }
}