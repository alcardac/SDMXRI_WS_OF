// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IStructureWriterEngine.cs" company="ISTAT">
//   Date Created : 2014-09-19
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace RDFProvider.Structure.Engine
{
    #region Using directives

    using Org.Sdmxsource.Sdmx.Api.Model.Objects;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;

    #endregion

    public interface IStructureRDFWriterEngine
    {
        void RDFWriteStructures(ISdmxObjects sdmxObjects);
        
    }
}