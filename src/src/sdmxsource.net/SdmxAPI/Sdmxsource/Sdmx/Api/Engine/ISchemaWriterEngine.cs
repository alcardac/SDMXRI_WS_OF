// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ISchemaWriterEngine.cs" company="Eurostat">
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

    using System.Collections.Generic;
    using System.IO;

    using Org.Sdmxsource.Sdmx.Api.Model.BaseObjects.DataStructure;

    #endregion

    /// <summary>
    /// Writes the schema in the format which is defined by this engine
    /// </summary>
    public interface ISchemaWriterEngine
    {
        #region Public Methods and Operators

        /// <summary>
        /// Generates a time-series schema in the format specified by the data type for the key family
        /// which is written to the specified OutputStream.
        /// </summary>
        /// <param name="outPutStream">
        /// The OutputStream to write the schema to  this is closed on completion
        /// </param>
        /// <param name="dsd">
        /// The DSD to generate the schema for
        /// </param>
        /// <param name="targetNamespace">
        /// The target namespace to use
        /// </param>
        /// <param name="constraintsMap">
        /// The map key is the dimension id, values is the valid codes for the dimension
        /// </param>
        void GenerateSchema(Stream outPutStream, IDataStructureObjectBase dsd, string targetNamespace, IDictionary<string, ISet<string>> constraintsMap);

        /// <summary>
        /// Generates a cross-sectional schema in the format specified by the data type for the key family
        /// which is written to the specified OutputStream.
        /// </summary>
        /// <param name="outPutStream">
        /// The OutputStream to write the schema to  this is closed on completion
        /// </param>
        /// <param name="dsd">
        /// The DSD to generate the schema for
        /// </param>
        /// <param name="targetNamespace">
        /// The target namespace to use
        /// </param>
        /// <param name="crossSectionalDimensionId">
        /// The id of the dimension that will be used at the observation level
        /// </param>
        /// <param name="constraintsMap">
        /// The map key is the dimension id, values is the valid codes for the dimension
        /// </param>
        void GenerateCrossSectionalSchema(Stream outPutStream, IDataStructureObjectBase dsd, string targetNamespace, string crossSectionalDimensionId, IDictionary<string, ISet<string>> constraintsMap);

        #endregion

    }
}
