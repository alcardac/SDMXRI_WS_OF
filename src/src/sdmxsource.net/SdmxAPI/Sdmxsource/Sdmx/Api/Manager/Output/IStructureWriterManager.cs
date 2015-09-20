// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IStructureWriterManager.cs" company="Eurostat">
//   Date Created : 2013-08-19
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Manager.Output
{
    #region Using directives

    using System.IO;

    using Org.Sdmxsource.Sdmx.Api.Model.Format;
    using Org.Sdmxsource.Sdmx.Api.Model.Header;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;

    #endregion

    /// <summary>
    /// The structure writing manager is responsible for writing SdmxBean objects to an output stream as SDMX / EDI documents.
    /// <p/>
    /// The Interface gives options for the type of SDMX document to be output 
    /// </summary>
    public interface IStructureWriterManager
    {
        #region Public Methods and Operators

        /// <summary>
        /// Writes the contents of the beans out to the output stream in the format specified.
        /// <p/>
        /// Will write the header information contained within the SdmxBeans container if there is a header present.  If the header
        /// is not present a default header will be created
        /// </summary>
        /// <param name="sdmxObjects">
        /// The objects to write to the output stream
        /// </param>
        /// <param name="outputFormat">
        /// The output format of the message (required)
        /// </param>
        /// <param name="outputStream">
        /// The stream to write to, the stream is closed on completion, this can be null if not required (i.e the outputFormat
        /// may contain a writer)
        /// </param>
        void WriteStructures(ISdmxObjects sdmxObjects, IStructureFormat outputFormat, Stream outputStream);

        /// <summary>
        /// Writes the contents of the bean out to the output stream in the version specified.
        /// </summary>
        /// <param name="maintainableObject">
        /// The objects to write to the output stream
        /// </param>
        /// <param name="header">
        /// Header can be null, if null will create a default header
        /// </param>
        /// <param name="outputFormat">
        /// The output format of the message (required)
        /// </param>
        /// <param name="outputStream">
        /// The stream to write to, the stream is NOT closed on completion, this can be null if not required (i.e the outputFormat
        /// may contain a writer)
        /// </param>
        void WriteStructure(IMaintainableObject maintainableObject, IHeader header, IStructureFormat outputFormat, Stream outputStream);
        
        #endregion
    }
}
