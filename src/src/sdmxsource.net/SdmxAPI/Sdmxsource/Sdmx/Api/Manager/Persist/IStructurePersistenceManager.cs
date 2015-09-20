// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IStructurePersistenceManager.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Manager.Persist
{
    #region Using directives

    using Org.Sdmxsource.Sdmx.Api.Model.Objects;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;

    #endregion

    /// <summary>
    ///     Manages the persistence of structures
    /// </summary>
    public interface IStructurePersistenceManager
    {
        #region Public Methods and Operators

        /// <summary>
        /// Saves the maintainable 
        /// </summary>
        /// <param name="maintainable"></param>
        void SaveStructure(IMaintainableObject maintainable);

        /// <summary>
        /// Saves the maintainable structures in the supplied sdmxObjects
        /// </summary>
        /// <param name="sdmxObjects"> SDMX objects
        /// </param>
        void SaveStructures(ISdmxObjects sdmxObjects);

        /// <summary>
        /// Deletes the maintainable structures in the supplied sdmxObjects
        /// </summary>
        /// <param name="sdmxObjects">SDMX objects
        /// </param>
        void DeleteStructures(ISdmxObjects sdmxObjects);

        /// <summary>
        /// Deletes the maintainable structures in the supplied objects
        /// </summary>
        /// <param name="maintainable"></param>
        void DeleteStructure(IMaintainableObject maintainable);


        #endregion
    }
}