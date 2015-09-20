// -----------------------------------------------------------------------
// <copyright file="IDataStructureFromCrossBuilder.cs" company="Eurostat">
//   Date Created : 2013-02-07
//   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
// 
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// -----------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.SdmxObjects.Util
{
    using Org.Sdmxsource.Sdmx.Api.Builder;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.DataStructure;

    /// <summary>
    /// The Mutable <see cref="ICrossSectionalDataStructureMutableObject"/> to <see cref="IDataStructureMutableObject"/> builder interface.
    /// </summary>
    public interface IDataStructureFromCrossBuilder : IBuilder<IDataStructureMutableObject,ICrossSectionalDataStructureMutableObject>
    { 
    }
}