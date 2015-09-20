// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ISchemaGenerationManager.cs" company="EUROSTAT">
// Copyright (c) 2012 Metadata Technology Ltd.
// All rights reserved. This program and the accompanying materials
// are made available under the terms of the GNU Public License v3.0
// which accompanies this distribution, and is available at
// http://www.gnu.org/licenses/gpl.html
// 
// This file is part of the SDMX Component Library.
// 
// The SDMX Component Library is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// The SDMX Component Library is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with The SDMX Component Library If not, see <http://www.gnu.org/licenses/>.
// 
// Contributors:
//     Metadata Technology - initial API and implementation
// </copyright>
// <summary>
//   TODO
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Estat.Sri.SdmxDataParser.Manager
{
    #region Using directives

    using System.Collections.Generic;
    using System.IO;
    using Org.Sdmxsource.Sdmx.Api.Constants;

    #endregion


    /// <summary>
    /// TODO
    /// </summary>
    public interface ISchemaGenerationManager
    {
        #region Public Methods and Operator

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
        /// <param name="schema">
        /// The schema type to generate
        /// </param>
        /// <param name="constraintsMap">
        /// The dictionary key is the dimension id, values is the valid codes for the dimension
        /// </param>
        void GenerateSchema(Stream outPutStream, 
                            DataStructureSuperBean dsd, 
                            string targetNamespace, 
                            DataType schema, 
                            IDictionary<string, ISet<string>> constraintsMap);
	
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
	    /// <param name="schema">
        /// The schema type to generate
	    /// </param>
	    /// <param name="crossSectionalDimensionId">
        /// The id of the dimension that will be used at the observation level
	    /// </param>
	    /// <param name="constraintsMap">
        /// The dictionary key is the dimension id, values is the valid codes for the dimension
	    /// </param>
        void GenerateCrossSectionalSchema(Stream outPutStream, 
                                          DataStructureSuperBean dsd, 
                                          string targetNamespace, 
                                          DataType schema, 
                                          string crossSectionalDimensionId, 
                                          IDictionary<string, ISet<string>> constraintsMap);

        #endregion
    }
}
