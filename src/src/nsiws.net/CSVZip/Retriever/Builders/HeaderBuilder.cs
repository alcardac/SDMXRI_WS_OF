// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HeaderBuilder.cs" company="Eurostat">
//   Date Created : 2011-12-15
//   Copyright (c) 2011 by the European   Commission, represented by Eurostat. All rights reserved.
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   Build a <see cref="HeaderBean" />
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace CSVZip.Retriever.Builders
{
    using System;

    using CSVZip.Retriever.Model;

    using log4net;
    using Org.Sdmxsource.Sdmx.Api.Model.Header;
    using Estat.Sri.MappingStoreRetrieval.Engine;
    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Estat.Nsi.DataRetriever.Properties;

    /// <summary>
    /// Build a <see cref="HeaderBean"/>
    /// </summary>
    internal class HeaderBuilder : IBuilder<IHeader, DataRetrievalInfo>
    {
        #region Constants and Fields

        /// <summary>
        ///   The sigleton instance
        /// </summary>
        private static readonly HeaderBuilder _instance = new HeaderBuilder();

        private static readonly ILog Logger = LogManager.GetLogger(typeof(HeaderBuilder));
        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///   Prevents a default instance of the <see cref="HeaderBuilder" /> class from being created.
        /// </summary>
        private HeaderBuilder()
        {
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets the singleton instance of this class
        /// </summary>
        public static HeaderBuilder Instance
        {
            get
            {
                return _instance;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// The method that builds a <see cref="HeaderBean"/> from the specified <paramref name="info"/>
        /// </summary>
        /// <param name="info">
        /// The current Data retrieval state 
        /// </param>
        /// <returns>
        /// The <see cref="HeaderBean"/> 
        /// </returns>
        public IHeader Build(DataRetrievalInfo info)
        {
            IHeader header = null;
            try
            {

                HeaderRetrieverEngine headerRetrieverEngine = new HeaderRetrieverEngine(info.ConnectionStringSettings);
                header = headerRetrieverEngine.GetHeader(info.MappingSet.Dataflow, null, null);


                // was something returned?
                if (header == null)
                {
                    header = info.DefaultHeader;
                    if (header != null)
                    {
                        Logger.Info(Resources.No_header_information_in_the_Mapping_Store);
                    }
                    else
                    {
                        throw new DataRetrieverException(Resources.ErrorNoHeader,
                            SdmxErrorCode.GetFromEnum(SdmxErrorCodeEnumType.NoResultsFound));
                    }
                }

                //header.Truncated = info.IsTruncated;
            }
            catch (DataRetrieverException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new DataRetrieverException(ex,
                    SdmxErrorCode.GetFromEnum(SdmxErrorCodeEnumType.InternalServerError),
                    Resources.DataRetriever_GetMessageHeader_Error_populating_header);
            }

            return header;
        }

        #endregion
    }
}