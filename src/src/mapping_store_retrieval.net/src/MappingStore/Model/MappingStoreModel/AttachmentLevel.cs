// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AttachmentLevel.cs" company="Eurostat">
//   Date Created : 2011-09-08
//   Copyright (c) 2011 by the European   Commission, represented by Eurostat. All rights reserved.
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   Attribute attachment level
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Estat.Sri.MappingStoreRetrieval.Model.MappingStoreModel
{
    /// <summary>
    /// Attribute attachment level
    /// </summary>
    public enum AttachmentLevel
    {
        /// <summary>
        /// For non attribute types. Default.
        /// </summary>
        None = 0, 

        /// <summary>
        /// Dataset level
        /// </summary>
        DataSet, 

        /// <summary>
        /// Group level.
        /// </summary>
        /// <remarks>Requires to define which groups</remarks>
        Group, 

        /// <summary>
        /// Series level
        /// </summary>
        Series, 

        /// <summary>
        /// Observation level
        /// </summary>
        Observation
    }
}