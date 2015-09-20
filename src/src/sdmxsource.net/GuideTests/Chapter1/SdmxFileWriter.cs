// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SdmxFileWriter.cs" company="Eurostat">
//   Date Created : 2014-04-23
//   //   Copyright (c) 2014 by the European   Commission, represented by Eurostat.   All rights reserved.
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   Description of SdmxFileWriter.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace GuideTests.Chapter1
{
    using System.IO;

    using Org.Sdmxsource.Sdmx.Api.Manager.Output;
    using Org.Sdmxsource.Sdmx.Api.Model.Format;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.DataStructure;
    using Org.Sdmxsource.Sdmx.Structureparser.Manager;
    using Org.Sdmxsource.Sdmx.Util.Objects.Container;

    /// <summary>
    ///     Description of SdmxFileWriter.
    /// </summary>
    public class SdmxFileWriter
    {
        #region Fields

        /// <summary>
        /// The structures creator.
        /// </summary>
        private readonly StructuresCreator structuresCreator;

        /// <summary>
        /// The swm.
        /// </summary>
        private readonly IStructureWriterManager swm;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SdmxFileWriter"/> class.
        /// </summary>
        public SdmxFileWriter()
        {
            this.structuresCreator = new StructuresCreator();

            this.swm = new StructureWriterManager();
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The write structure to file.
        /// </summary>
        /// <param name="outputFormat">
        /// The output format.
        /// </param>
        /// <param name="outputStream">
        /// The output stream.
        /// </param>
        public void writeStructureToFile(IStructureFormat outputFormat, Stream outputStream)
        {
            ISdmxObjects sdmxObjects = new SdmxObjectsImpl();

            sdmxObjects.AddAgencyScheme(this.structuresCreator.BuildAgencyScheme());
            sdmxObjects.AddCodelist(this.structuresCreator.BuildCountryCodelist());
            sdmxObjects.AddCodelist(this.structuresCreator.BuildIndicatorCodelist());
            sdmxObjects.AddConceptScheme(this.structuresCreator.BuildConceptScheme());

            IDataStructureObject dsd = this.structuresCreator.BuildDataStructure();
            sdmxObjects.AddDataStructure(dsd);
            sdmxObjects.AddDataflow(this.structuresCreator.BuildDataflow("DF_WDI", "World Development Indicators", dsd));

            this.swm.WriteStructures(sdmxObjects, outputFormat, outputStream);
        }

        #endregion
    }
}