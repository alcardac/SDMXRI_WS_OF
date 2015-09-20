// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AssignmentStatus.cs" company="Eurostat">
//   Date Created : 2011-09-08
//   Copyright (c) 2011 by the European   Commission, represented by Eurostat. All rights reserved.
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   Attribute Assignment status
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Estat.Sri.MappingStoreRetrieval.Model.MappingStoreModel
{
    /// <summary>
    /// Attribute Assignment status
    /// </summary>
    public enum AssignmentStatus
    {
        /// <summary>
        /// For non-attribute types. Default.
        /// </summary>
        None = 0, 

        /// <summary>
        /// Providing attribute value is mandatory 
        /// </summary>
        Mandatory, 

        /// <summary>
        /// Providing attribute value is optional
        /// </summary>
        Conditional
    }
}