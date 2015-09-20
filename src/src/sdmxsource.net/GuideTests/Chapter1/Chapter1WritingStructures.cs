// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Chapter1WritingStructures.cs" company="Eurostat">
//   Date Created : 2014-04-23
//   //   Copyright (c) 2014 by the European   Commission, represented by Eurostat.   All rights reserved.
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   Description of Chapter1WritingStructures.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace GuideTests.Chapter1
{
    using System.IO;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Format;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model;

    /// <summary>
    ///  Description of Chapter1WritingStructures.
    /// User: sli
    /// </summary>
    public class Chapter1WritingStructures
    {
        #region Public Methods and Operators

        /// <summary>
        /// Defines the entry point of the application.
        /// </summary>
        /// <param name="args">The arguments.</param>
        public static void Main(string[] args)
        {
            var fileWriter = new SdmxFileWriter();

            // create the IStructureFormat from a standard Sdmx StructureOutputFormat
            StructureOutputFormat soFormat = StructureOutputFormat.GetFromEnum(StructureOutputFormatEnumType.SdmxV21StructureDocument);
            IStructureFormat outputFormat = new SdmxStructureFormat(soFormat);

            // create the output file
            var outputStream = new FileStream("output/structures.xml", FileMode.Create);

            // write the the structures to the output file
            fileWriter.writeStructureToFile(outputFormat, outputStream);
        }

        #endregion
    }
}