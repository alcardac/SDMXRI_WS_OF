// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IQueryBeanBuilder.cs" company="EUROSTAT">
//   TODO
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

/////// Copyright (c) 2012 Metadata Technology Ltd.
/////// All rights reserved. This program and the accompanying materials
/////// are made available under the terms of the GNU Public License v3.0
/////// which accompanies this distribution, and is available at
/////// http://www.gnu.org/licenses/gpl.html
/////// This file is part of the SDMX Component Library.
/////// The SDMX Component Library is free software: you can redistribute it and/or modify
/////// it under the terms of the GNU General Public License as published by
/////// the Free Software Foundation, either version 3 of the License, or
/////// (at your option) any later version.
/////// The SDMX Component Library is distributed in the hope that it will be useful,
/////// but WITHOUT ANY WARRANTY; without even the implied warranty of
/////// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
/////// GNU General Public License for more details.
/////// You should have received a copy of the GNU General Public License
/////// along with The SDMX Component Library If not, see <http://www.gnu.org/licenses/>.
/////// Contributors:
/////// Metadata Technology - initial API and implementation
/////// </summary>
///////

namespace Org.Sdmxsource.Sdmx.Structureparser.Builder.Query
{
    using System.Collections.Generic;

    using Org.Sdmx.Resources.SdmxMl.Schemas.V10.message;
    using Org.Sdmx.Resources.SdmxMl.Schemas.V20.registry;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;

    /// <summary>
    /// Builds Query reference objects from SDMX query messages.
    /// The SDMX queries can be structure queries, registration queries or provision queries
    /// </summary>
    public interface IQueryBeanBuilder
    {
        #region Public Methods and Operators

        /// <summary>
        /// Builds a list of structure references from a version 2.0 registry query structure request message
        /// </summary>
        /// <param name="queryStructureRequests">
        /// The Structure query
        /// </param>
        /// <returns>
        /// list of structure references
        /// </returns>
        IList<IStructureReference> Build(QueryStructureRequestType queryStructureRequests);

        /// <summary>
        /// Builds a list of provision references from a version 2.0 registry query registration request message
        /// </summary>
        /// <param name="queryRegistrationRequestType">
        /// The query Registration Request Type.
        /// </param>
        /// <returns>
        /// provision references
        /// </returns>
        IStructureReference Build(QueryRegistrationRequestType queryRegistrationRequestType);

        /// <summary>
        /// Builds a list of provision references from a version 2.1 registry query registration request message
        /// </summary>
        /// <param name="queryRegistrationRequestType">
        /// Query registration request type
        /// </param>
        /// <returns>
        /// provision references
        /// </returns>
        IStructureReference Build(
            Org.Sdmx.Resources.SdmxMl.Schemas.V21.Registry.QueryRegistrationRequestType queryRegistrationRequestType);

        /// <summary>
        /// Builds a list of provision references from a version 2.0 registry query provision request message
        /// </summary>
        /// <param name="queryProvisionRequestType">
        /// The query for provision
        /// </param>
        /// <returns>
        /// provision references
        /// </returns>
        IStructureReference Build(QueryProvisioningRequestType queryProvisionRequestType);

        /// <summary>
        /// Builds a list of structure references from a version 1.0 query message
        /// </summary>
        /// <param name="queryMessage">
        /// The query message
        /// </param>
        /// <returns>
        /// list of structure references
        /// </returns>
        IList<IStructureReference> Build(QueryMessageType queryMessage);

        /// <summary>
        /// Builds a list of structure references from a version 2.0 query message
        /// </summary>
        /// <param name="queryMessage">
        /// The query message
        /// </param>
        /// <returns>
        /// list of structure references
        /// </returns>
        IList<IStructureReference> Build(Org.Sdmx.Resources.SdmxMl.Schemas.V20.message.QueryMessageType queryMessage);

        #endregion
    }
}