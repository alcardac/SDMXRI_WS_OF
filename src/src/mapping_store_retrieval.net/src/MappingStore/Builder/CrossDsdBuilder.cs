// -----------------------------------------------------------------------
// <copyright file="CrossDsdBuilder.cs" company="Eurostat">
//   Date Created : 2013-02-13
//   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
// 
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// -----------------------------------------------------------------------
namespace Estat.Sri.MappingStoreRetrieval.Builder
{
    using Org.Sdmxsource.Sdmx.Api.Builder;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.DataStructure;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.DataStructure;
    using Org.Sdmxsource.Util.Extensions;

    /// <summary>
    /// The cross DSD builder.
    /// </summary>
    internal class CrossDsdBuilder : IBuilder<ICrossSectionalDataStructureMutableObject, IDataStructureMutableObject>
    {
        /// <summary>
        /// Builds an object of type <see cref="ICrossSectionalDataStructureMutableObject"/>
        /// </summary>
        /// <param name="buildFrom">
        /// An Object to build the output object from
        /// </param>
        /// <returns>
        /// Object of type <see cref="IDataStructureMutableObject"/>
        /// </returns>
        public ICrossSectionalDataStructureMutableObject Build(IDataStructureMutableObject buildFrom)
        {
            // TODO look for an object mapper or expressions. There used to be Emit but it hasn't been updated since 2010. Automapper is way too slow.
            ICrossSectionalDataStructureMutableObject crossDsd = new CrossSectionalDataStructureMutableCore();
            crossDsd.AgencyId = buildFrom.AgencyId;
            crossDsd.Id = buildFrom.Id;
            crossDsd.Version = buildFrom.Version;
            crossDsd.Names.AddAll(buildFrom.Names);
            crossDsd.Descriptions.AddAll(buildFrom.Descriptions);
            crossDsd.Annotations.AddAll(buildFrom.Annotations);
            crossDsd.DimensionList = buildFrom.DimensionList;
            crossDsd.AttributeList = buildFrom.AttributeList;
            crossDsd.StartDate = buildFrom.StartDate;
            crossDsd.EndDate = buildFrom.EndDate;
            crossDsd.FinalStructure = buildFrom.FinalStructure;
            crossDsd.ExternalReference = buildFrom.ExternalReference;
            crossDsd.Groups.AddAll(buildFrom.Groups);
            crossDsd.MeasureList = buildFrom.MeasureList;
            crossDsd.ServiceURL = buildFrom.ServiceURL;
            crossDsd.StructureURL = buildFrom.StructureURL;
            crossDsd.Uri = buildFrom.Uri;

            return crossDsd;
        }
    }
}