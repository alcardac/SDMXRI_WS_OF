// -----------------------------------------------------------------------
// <copyright file="StructureReferenceFromMutableBuilder.cs" company="Eurostat">
//   Date Created : 2013-02-25
//   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
// 
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// -----------------------------------------------------------------------
namespace Estat.Sri.MappingStoreRetrieval.Builder
{
    using System;

    using Org.Sdmxsource.Sdmx.Api.Builder;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;
    using Org.Sdmxsource.Sdmx.Util.Objects.Reference;

    /// <summary>
    ///  Builds a <see cref="IStructureReference"/> from a <see cref="IMaintainableMutableObject"/>.
    /// </summary>
    public class StructureReferenceFromMutableBuilder : IBuilder<IStructureReference, IMaintainableMutableObject>
    {
        /// <summary>
        /// Builds an object of type <see cref="IStructureReference"/> from the specified <paramref name="buildFrom"/>
        /// </summary>
        /// <param name="buildFrom">
        /// An Object to build the output object from
        /// </param>
        /// <returns>
        /// Object of type <see cref="IStructureReference"/>
        /// </returns>
        public IStructureReference Build(IMaintainableMutableObject buildFrom)
        {
            if (buildFrom == null)
            {
                throw new ArgumentNullException("buildFrom");
            }

            return new StructureReferenceImpl(buildFrom.AgencyId, buildFrom.Id, buildFrom.Version, buildFrom.StructureType);
        }
    }
}