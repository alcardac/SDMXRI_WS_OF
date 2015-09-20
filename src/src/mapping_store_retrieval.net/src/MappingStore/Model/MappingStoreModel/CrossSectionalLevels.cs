// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CrossSectionalLevels.cs" company="Eurostat">
//   Date Created : 2011-12-08
//   Copyright (c) 2011 by the European   Commission, represented by Eurostat. All rights reserved.
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   Cross Sectional level
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Estat.Sri.MappingStoreRetrieval.Model.MappingStoreModel
{
    using System;

    /// <summary>
    /// Cross Sectional level
    /// </summary>
    [Flags]
    public enum CrossSectionalLevels
    {
        /// <summary>
        /// None level
        /// </summary>
        None = 0, 

        /// <summary>
        /// DataSet level
        /// </summary>
        DataSet = 1, 

        /// <summary>
        /// Group level
        /// </summary>
        Group = 2, 

        /// <summary>
        /// Section level
        /// </summary>
        Section = 4, 

        /// <summary>
        /// Observation level
        /// </summary>
        Observation = 8
    }
}