// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IDataParseManager.cs" company="Eurostat">
//   Date Created : 2014-07-22
//   //   Copyright (c) 2014 by the European   Commission, represented by Eurostat.   All rights reserved.
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   The data Manager is responsible for transforming data to adhere to one SMDX schema type to another.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.DataParser.Manager
{
    #region Using directives

    using System.Collections.Generic;
    using System.IO;

    using Org.Sdmxsource.Sdmx.Api.Manager.Retrieval;
    using Org.Sdmxsource.Sdmx.Api.Model.Data;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.DataStructure;
    using Org.Sdmxsource.Sdmx.Api.Util;

    #endregion

    /// <summary>
    ///     The data Manager is responsible for transforming data to adhere to one SMDX schema type to another.
    /// </summary>
    public interface IDataParseManager
    {
        #region Public Methods and Operators

        /// <summary>
        /// Performs a transform from one data format to another data format.
        /// </summary>
        /// <param name="sourceData">
        /// The location of the input stream which reads the data to transform
        /// </param>
        /// <param name="outPutStream">
        /// OutputStream to write the transform results to
        /// </param>
        /// <param name="dataFormat">
        /// The format to transform to
        /// </param>
        /// <param name="retrievalManager">
        /// RetrievalManager used to get any data structures needed to interpret the data
        /// </param>
        void PerformTransform(IReadableDataLocation sourceData, Stream outPutStream, IDataFormat dataFormat, ISdmxObjectRetrievalManager retrievalManager);

        /// <summary>
        /// Performs a transform from one data format to another data format.
        /// </summary>
        /// <param name="sourceData">
        /// The location of the input stream which reads the data to transform
        /// </param>
        /// <param name="outPutStream">
        /// OutputStream to write the transform results to
        /// </param>
        /// <param name="dataFormat">
        /// The format to transform to
        /// </param>
        /// <param name="dsd">
        /// The Structure object that specifies the data format
        /// </param>
        /// <param name="dataflowObject">
        /// DataflowObject (optional) provides extra information about the data
        /// </param>
        void PerformTransform(IReadableDataLocation sourceData, Stream outPutStream, IDataFormat dataFormat, IDataStructureObject dsd, IDataflowObject dataflowObject);

        /// <summary>
        /// Performs a transform from one data format to another data format.
        /// </summary>
        /// <param name="sourceData">
        /// The location of the input stream which reads the data to transform
        /// </param>
        /// <param name="dsdLocation">
        /// The location of the structure used to transform the data
        /// </param>
        /// <param name="outPutStream">
        /// OutputStream to write the transform results to
        /// </param>
        /// <param name="dataFormat">
        /// The format to transform to
        /// </param>
        void PerformTransform(IReadableDataLocation sourceData, IReadableDataLocation dsdLocation, Stream outPutStream, IDataFormat dataFormat);

        /// <summary>
        /// Performs a transform from one data format to another data format.
        /// </summary>
        /// <param name="sourceData">
        /// SourceData giving access to the input stream which reads the data to transform
        /// </param>
        /// <param name="dataFormat">
        /// The format to transform to
        /// </param>
        /// <param name="objectRetrievalManager">
        /// The object retrieval manager
        /// </param>
        /// <returns>
        /// The readable data location
        /// </returns>
        IReadableDataLocation PerformTransform(IReadableDataLocation sourceData, IDataFormat dataFormat, ISdmxObjectRetrievalManager objectRetrievalManager);

        /// <summary>
        /// Performs a transform from one data format to another data format.
        /// </summary>
        /// <param name="sourceData">
        /// The source data
        /// </param>
        /// <param name="dataFormat">
        /// The data format
        /// </param>
        /// <param name="dsd">
        /// The data structure object
        /// </param>
        /// <param name="dataflowObject">
        /// The data flow object
        /// </param>
        /// <returns>
        /// The readable data location
        /// </returns>
        IReadableDataLocation PerformTransform(IReadableDataLocation sourceData, IDataFormat dataFormat, IDataStructureObject dsd, IDataflowObject dataflowObject);

        /// <summary>
        /// Performs the transform and split.
        /// </summary>
        /// <param name="sourceData">
        /// The source data
        /// </param>
        /// <param name="dsdLocation">
        /// The readable data location
        /// </param>
        /// <param name="dataFormat">
        /// The data format
        /// </param>
        /// <returns>
        /// The list of readable data location
        /// </returns>
        IList<IReadableDataLocation> PerformTransformAndSplit(IReadableDataLocation sourceData, IReadableDataLocation dsdLocation, IDataFormat dataFormat);

        /// <summary>
        /// Performs the transform and split.
        /// </summary>
        /// <param name="sourceData">
        /// The source data
        /// </param>
        /// <param name="dataFormat">
        /// The data format
        /// </param>
        /// <param name="objectRetrievalManager">
        /// The object retrieval manager
        /// </param>
        /// <returns>
        /// The list of readable data location
        /// </returns>
        IList<IReadableDataLocation> PerformTransformAndSplit(IReadableDataLocation sourceData, IDataFormat dataFormat, ISdmxObjectRetrievalManager objectRetrievalManager);

        #endregion
    }
}