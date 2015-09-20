// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataQueryFactory.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The data query factory implementation.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.SdmxQueryBuilder.Factory
{
    #region Using Directives

    using System.Xml.Linq;

    using Org.Sdmxsource.Sdmx.Api.Builder;
    using Org.Sdmxsource.Sdmx.Api.Factory;
    using Org.Sdmxsource.Sdmx.Api.Model.Format;
    using Org.Sdmxsource.Sdmx.Api.Model.Header;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Header;
    using Org.Sdmxsource.Sdmx.SdmxQueryBuilder.Builder;
    using Org.Sdmxsource.Sdmx.SdmxQueryBuilder.Model;
    using Org.Sdmxsource.Sdmx.Structureparser.Builder.XmlSerialization.V2;
    
    #endregion

    /// <summary>
    /// The data query factory implementation.
    /// </summary>
    public class DataQueryFactory : IDataQueryFactory
    {
        #region Constants

        /// <summary>
        ///     The unknown id.
        /// </summary>
        private const string UnknownId = "UNKNOWN";

        #endregion


        #region Fields

        private readonly DataQueryBuilderRest _dataQueryBuilderRest = new DataQueryBuilderRest();

        /// <summary>
        /// The _default header
        /// </summary>
        private readonly IHeader _defaultHeader;

        #endregion


        /// <summary>
        /// Initializes a new instance of the <see cref="DataQueryFactory"/> class.
        /// </summary>
        /// <param name="defaultHeader">The default header.</param>
        public DataQueryFactory(IHeader defaultHeader)
        {
            //// TODO header factory.
            this._defaultHeader = defaultHeader;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataQueryFactory"/> class.
        /// </summary>
        public DataQueryFactory()
        {
            this._defaultHeader = new HeaderImpl(UnknownId, UnknownId);
        }

        #region Public Methods and Operators

        /// <summary>
        /// Returns a DataQueryBuilder only if this factory understands the DataQueryFormat.  If the format is unknown, null will be returned
        /// </summary>
        /// <typeparam name="T">
        /// Generic type parameter
        /// </typeparam>
        /// <param name="format">
        /// Format
        /// </param>
        /// <returns>
        /// DataQueryBuilder is this factory knows how to build this query format, or null if it doesn't
        /// </returns>
        public IDataQueryBuilder<T> GetDataQueryBuilder<T>(IDataQueryFormat<T> format)
        {
            if (format is RestQueryFormat)
            {
                return (IDataQueryBuilder<T>)_dataQueryBuilderRest;
            }

            if (format is QueryMessageV2Format)
            {
                IDataQueryBuilder<XDocument> dataQueryBuilder =
                    new XDocumentDataQueryBuilderV2(this._defaultHeader, new StructureHeaderXmlBuilder());
                return dataQueryBuilder as IDataQueryBuilder<T>;
            }

            return null;
        }

        #endregion
    }
}
