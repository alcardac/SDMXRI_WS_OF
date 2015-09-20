// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SdmxComponentType.cs" company="Eurostat">
//   Date Created : 2011-09-08
//   Copyright (c) 2011 by the European   Commission, represented by Eurostat. All rights reserved.
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   Enumeration of SDMX v2.0 DSD component types
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Estat.Sri.MappingStoreRetrieval.Constants
{
    /// <summary>
    /// Enumeration of SDMX v2.0 DSD component types
    /// </summary>
    public enum SdmxComponentType
    {
        /// <summary>
        /// Default/Invalid type
        /// </summary>
        None, 

        /// <summary>
        /// Dimension type
        /// </summary>
        Dimension, 

        /// <summary>
        /// Attribute type
        /// </summary>
        Attribute, 

        /// <summary>
        /// Time Dimension type
        /// </summary>
        TimeDimension, 

        /// <summary>
        /// Primary Measure type
        /// </summary>
        PrimaryMeasure, 

        /// <summary>
        /// CrossSectional Measure type
        /// </summary>
        CrossSectionalMeasure
    }
}