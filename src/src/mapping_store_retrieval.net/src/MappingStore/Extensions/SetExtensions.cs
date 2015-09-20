// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SetExtensions.cs" company="Eurostat">
//   Date Created : 2013-02-25
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   The set extensions.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Estat.Sri.MappingStoreRetrieval.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Org.Sdmxsource.Sdmx.Api.Constants;

    /// <summary>
    /// The set extensions.
    /// </summary>
    internal static class SetExtensions
    {
        #region Public Methods and Operators

        /// <summary>
        /// Get one or nothing.
        /// </summary>
        /// <param name="mutableObjects">
        /// The mutable objects.
        /// </param>
        /// <typeparam name="T">
        /// The type of the <paramref name="mutableObjects"/>
        /// </typeparam>
        /// <returns>
        /// The <typeparamref name="T"/>.
        /// </returns>
        public static T GetOneOrNothing<T>(this ISet<T> mutableObjects)
        {
            //// Note do not use SingleOrDefault
            switch (mutableObjects.Count)
            {
                case 0:
                    return default(T);
                case 1:
                    return mutableObjects.First();
                default:
                    throw new ArgumentException(ErrorMessages.MoreThanOneArtefact);
            }
        }

        /// <summary>
        /// Check if the specified <paramref name="structureEnumType"/> is in <paramref name="sdmxStructureTypes"/>. 
        /// </summary>
        /// <param name="sdmxStructureTypes">
        /// The SDMX structure types.
        /// </param>
        /// <param name="structureEnumType">
        /// The structure type as an ENUM.
        /// </param>
        /// <returns>
        /// True if <paramref name="structureEnumType"/> is in <paramref name="sdmxStructureTypes"/> or if <paramref name="sdmxStructureTypes"/> is empty; otherwise false.  
        /// </returns>
        public static bool HasStructure(this ISet<SdmxStructureType> sdmxStructureTypes, SdmxStructureEnumType structureEnumType)
        {
            return sdmxStructureTypes.Count == 0 || sdmxStructureTypes.Contains(SdmxStructureType.GetFromEnum(structureEnumType));
        }


        /// <summary>
        /// Check if the specified <paramref name="structureEnumType"/> is in <paramref name="sdmxStructureTypes"/>. 
        /// </summary>
        /// <param name="sdmxStructureTypes">
        /// The SDMX structure types.
        /// </param>
        /// <param name="structureEnumType">
        /// The structure type.
        /// </param>
        /// <returns>
        /// True if <paramref name="structureEnumType"/> is in <paramref name="sdmxStructureTypes"/> or if <paramref name="sdmxStructureTypes"/> is empty; otherwise false.  
        /// </returns>
        public static bool HasStructure(this ISet<SdmxStructureType> sdmxStructureTypes, SdmxStructureType structureEnumType)
        {
            return sdmxStructureTypes.Count == 0 || sdmxStructureTypes.Contains(structureEnumType);
        }

        #endregion
    }
}