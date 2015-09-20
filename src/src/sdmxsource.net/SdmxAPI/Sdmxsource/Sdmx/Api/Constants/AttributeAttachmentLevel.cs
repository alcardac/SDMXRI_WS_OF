// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AttributeAttachmentLevel.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Constants
{
    /// <summary>
    ///     Defines all the attachment levels of attributes in a key family
    /// </summary>
    public enum AttributeAttachmentLevel
    {
        /// <summary>
        ///     Null value; Can be used to check if the value is not set;
        /// </summary>
        Null = 0, 

        /// <summary>
        ///     Attribute is relevant to the entire dataset
        /// </summary>
        DataSet, 

        /// <summary>
        ///     Attribute is relevant to a Group, in which case the group identifier is needed to define the group the attribute attaches to
        /// </summary>
        Group, 

        /// <summary>
        ///     Attribute is relevant to a Group of Dimensions (no formally specified in a Group)
        /// </summary>
        DimensionGroup, 

        /// <summary>
        ///     Attribute is relevant to a single Observation
        /// </summary>
        Observation
    }
}