// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Chapter4ReadingData.cs" company="Eurostat">
//   Date Created : 2014-07-31
//   //   Copyright (c) 2014 by the European   Commission, represented by Eurostat.   All rights reserved.
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   The chapter 4 reading data.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace GuideTests.Chapter4
{
    using System;
    using System.IO;

    using Org.Sdmxsource.Sdmx.Api.Engine;
    using Org.Sdmxsource.Sdmx.Api.Factory;
    using Org.Sdmxsource.Sdmx.Api.Manager.Parse;
    using Org.Sdmxsource.Sdmx.Api.Manager.Retrieval;
    using Org.Sdmxsource.Sdmx.Api.Model;
    using Org.Sdmxsource.Sdmx.Api.Model.Data;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.DataStructure;
    using Org.Sdmxsource.Sdmx.Api.Util;
    using Org.Sdmxsource.Sdmx.DataParser.Manager;
    using Org.Sdmxsource.Sdmx.Structureparser.Manager.Parsing;
    using Org.Sdmxsource.Sdmx.StructureRetrieval.Manager;
    using Org.Sdmxsource.Util.Io;

    /// <summary>
    ///     The chapter 4 reading data.
    /// </summary>
    public class Chapter4ReadingData
    {
        #region Fields

        /// <summary>
        ///     The _data reader manager.
        /// </summary>
        private readonly IDataReaderManager _dataReaderManager;

        /// <summary>
        ///     The  <see cref="IReadableDataLocationFactory" />.
        /// </summary>
        private readonly IReadableDataLocationFactory _rdlFactory;

        /// <summary>
        ///     The _structure parsing manager.
        /// </summary>
        private readonly IStructureParsingManager _structureParsingManager;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Chapter4ReadingData"/> class.
        /// </summary>
        /// <param name="dataReaderManager">
        /// The data Reader Manager.
        /// </param>
        /// <param name="rdlFactory">
        /// The <see cref="IReadableDataLocationFactory"/>.
        /// </param>
        /// <param name="structureParsingManager">
        /// The structure Parsing Manager.
        /// </param>
        public Chapter4ReadingData(IDataReaderManager dataReaderManager, IReadableDataLocationFactory rdlFactory, IStructureParsingManager structureParsingManager)
        {
            this._dataReaderManager = dataReaderManager;
            this._rdlFactory = rdlFactory;
            this._structureParsingManager = structureParsingManager;
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The main method.
        /// </summary>
        /// <param name="args">The arguments.</param>
        public static void Main(string[] args)
        {
            // Step 1 - Create a new instance of the main class
            var main = new Chapter4ReadingData(new DataReaderManager(), new ReadableDataLocationFactory(), new StructureParsingManager());

            // Step 3 - Create a Readable Data Location from the File
            var structureFile = new FileInfo("tests/structures/chapter2/structures_full.xml");
            var dataFile = new FileInfo("tests/data/chapter3/sample_data.xml");

            main.ReadData(structureFile, dataFile);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Reads the data.
        /// </summary>
        /// <param name="structureFile">The structure file.</param>
        /// <param name="dataFile">The data file.</param>
        private void ReadData(FileInfo structureFile, FileInfo dataFile)
        {
            // Parse Structures into ISdmxObjects and build a SdmxBeanRetrievalManager
            IStructureWorkspace workspace;
            using (IReadableDataLocation rdl = this._rdlFactory.GetReadableDataLocation(structureFile))
            {
                workspace = this._structureParsingManager.ParseStructures(rdl);
            }

            ISdmxObjects beans = workspace.GetStructureObjects(false);
            ISdmxObjectRetrievalManager retreivalManager = new InMemoryRetrievalManager(beans);

            // Get the DataLocation, and from this the DataReaderEngine
            using (IReadableDataLocation dataLocation = this._rdlFactory.GetReadableDataLocation(dataFile))
            using (IDataReaderEngine dre = this._dataReaderManager.GetDataReaderEngine(dataLocation, retreivalManager))
            {
                while (dre.MoveNextDataset())
                {
                    IDataStructureObject dsd = dre.DataStructure;
                    Console.WriteLine(dsd.Name);

                    while (dre.MoveNextKeyable())
                    {
                        IKeyable currentKey = dre.CurrentKey;
                        Console.WriteLine(currentKey);
                        while (dre.MoveNextObservation())
                        {
                            IObservation obs = dre.CurrentObservation;
                            Console.WriteLine(obs);
                        }
                    }
                }
            }
        }

        #endregion
    }
}