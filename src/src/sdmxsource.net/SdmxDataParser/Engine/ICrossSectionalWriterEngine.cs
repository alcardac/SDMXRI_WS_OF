// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ICrossSectionalWriterEngine.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.DataParser.Engine
{
    using Org.Sdmxsource.Sdmx.Api.Engine;
    using Org.Sdmxsource.Sdmx.Api.Model.Header;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.DataStructure;

    /// <summary>
    ///     The interface for SDMX Cross Sectional Data Writers
    /// </summary>
    /// <example>
    ///     A sample in C# for <see cref="ICrossSectionalWriterEngine" />
    ///     <code source="..\ReUsingExamples\DataWriting\ReUsingCrossWriter.cs" lang="cs" />
    /// </example>
    public interface ICrossSectionalWriterEngine : IWriterEngine
    {
        #region Public Methods and Operators

        /// <summary>
        /// Starts a dataset with the data conforming to the <paramref name="dsd"/>
        /// </summary>
        /// <param name="dataflow">
        /// The <see cref="IDataflowObject"/>
        /// </param>
        /// <param name="dsd">
        /// The <see cref="ICrossSectionalDataStructureObject"/>
        /// </param>
        /// <param name="header">
        /// The Dataset attributes
        /// </param>
        /// <exception cref="System.ArgumentNullException">
        /// if the <paramref name="dsd"/> is null
        /// </exception>
        void StartDataset(IDataflowObject dataflow, ICrossSectionalDataStructureObject dsd, IDatasetHeader header);

        /// <summary>
        ///     Start a Cross Sectional Section
        /// </summary>
        void StartSection();

        /// <summary>
        ///     Start a Cross Sectional group
        /// </summary>
        void StartXSGroup();

        /// <summary>
        /// Write a Cross Sectional Measure with <paramref name="measure"/> and <paramref name="value"/>
        /// </summary>
        /// <param name="measure">
        /// The measure code
        /// </param>
        /// <param name="value">
        /// The measure value
        /// </param>
        void StartXSObservation(string measure, string value);

        /// <summary>
        /// Write an <paramref name="attribute"/> and the <paramref name="value"/>
        /// </summary>
        /// <param name="attribute">
        /// The attribute concept id
        /// </param>
        /// <param name="value">
        /// The value
        /// </param>
        void WriteAttributeValue(string attribute, string value);

        /// <summary>
        /// Write a Cross Sectional section <paramref name="key"/> and the <paramref name="value"/>
        /// </summary>
        /// <param name="key">
        /// The key. i.e. the dimension
        /// </param>
        /// <param name="value">
        /// The value
        /// </param>
        void WriteSectionKeyValue(string key, string value);

        /// <summary>
        /// Write a Cross Sectional DataSet <paramref name="key"/> and the <paramref name="value"/>
        /// </summary>
        /// <param name="key">
        /// The key. i.e. the dimension
        /// </param>
        /// <param name="value">
        /// The value
        /// </param>
        void WriteDataSetKeyValue(string key, string value);

        /// <summary>
        /// Write a Cross Sectional Group <paramref name="key"/> and the <paramref name="value"/>
        /// </summary>
        /// <param name="key">
        /// The key. i.e. the dimension
        /// </param>
        /// <param name="value">
        /// The value
        /// </param>
        void WriteXSGroupKeyValue(string key, string value);

        /// <summary>
        /// Write a Cross Sectional measure <paramref name="key"/> and the <paramref name="value"/>
        /// </summary>
        /// <param name="key">
        /// The key. i.e. the dimension
        /// </param>
        /// <param name="value">
        /// The value
        /// </param>
        void WriteXSObservationKeyValue(string key, string value);

        /// <summary>
        /// Closes the writer. Makes to close any opened XSObservation/Section/XSGroup and write the closing dataset/message elements.
        /// </summary>
        void Close();

        #endregion
    }
}