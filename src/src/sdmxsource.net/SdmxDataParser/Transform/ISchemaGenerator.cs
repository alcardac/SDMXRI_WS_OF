// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ISchemaGenerator.cs" company="EUROSTAT">
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
//   Creates a SDMX Schema based on the DataStructure and the valid codes
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Estat.Sri.SdmxDataParser.Transform
{
    #region Using directives

    using System.Collections.Generic;
    using System.IO;

    using Org.Sdmxsource.Sdmx.Api.Constants;

    #endregion


    /// <summary>
    /// Creates a SDMX Schema based on the DataStructure and the valid codes
    /// </summary>
    public interface ISchemaGenerator
    {
        #region Public Methods and Operators

        /// <summary>
        /// Creates a schema
        /// </summary>
        /// <param name="outPutStream">
        /// Output stream to write to - this is closed on completion
        /// </param>
        /// <param name="targetNamespace">
        /// The target namespace
        /// </param>
        /// <param name="targetSchemaVersion">
        /// The target schema version
        /// </param>
        /// <param name="keyFamily">
        /// The key family
        /// </param>
        /// <param name="validCodes">
        /// The vali codes
        /// </param>
        void Transform(Stream outPutStream, 
                       string targetNamespace, 
			           SdmxSchema targetSchemaVersion, 
                       DataStructureSuperBean keyFamily, 
                       IDictionary<string, ISet<string>> validCodes);

        #endregion
    }
}
