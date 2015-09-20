// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IStructureWriterEngine.cs" company="Eurostat">
//   Date Created : 2013-08-19
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Engine
{
    #region Using directives

    using Org.Sdmxsource.Sdmx.Api.Model.Objects;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;

    #endregion

    /// <summary>
    ///     Classes that support this interface can write one or many structures.
    ///     <p />
    ///     The location and format that the structures are written to and in, are implementation dependent.
    /// </summary>
    public interface IStructureWriterEngine
    {
        #region Public Methods and Operators

        /// <summary>
        /// Writes the @maintainableObject out to the output location in the format specified by the implementation
        /// </summary>
        /// <param name="maintainableObject">
        /// The maintainableObject.
        /// </param>
        void WriteStructure(IMaintainableObject maintainableObject);

        /// <summary>
        /// Writes the sdmxObjects to the output location in the format specified by the implementation
        /// </summary>
        /// <param name="sdmxObjects">SDMX objects
        /// </param>
        void WriteStructures(ISdmxObjects sdmxObjects);

        #endregion
    }
}