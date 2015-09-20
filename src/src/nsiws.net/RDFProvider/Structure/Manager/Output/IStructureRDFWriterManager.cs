// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IStructureWriterManager.cs" company="ISTAT">
//   Date Created : 2014-09-19
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace RDFProvider.Structure.Manager.Output
{
    #region Using directives

    using System.IO;

    using Org.Sdmxsource.Sdmx.Api.Model.Format;
    using Org.Sdmxsource.Sdmx.Api.Model.Header;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;

    #endregion


    public interface IStructureRDFWriterManager
    {
        #region Public Methods and Operators

        void RDFWriteStructures(ISdmxObjects sdmxObjects, Stream outPutStream);
        

        #endregion
    }
}
