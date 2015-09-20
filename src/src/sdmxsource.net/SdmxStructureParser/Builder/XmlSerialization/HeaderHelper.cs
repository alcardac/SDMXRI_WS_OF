// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HeaderHelper.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The header helper.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.Structureparser.Builder.XmlSerialization
{
    using Org.Sdmxsource.Sdmx.Api.Manager.Retrieval;
    using Org.Sdmxsource.Util;

    /// <summary>
    ///     The header helper.
    /// </summary>
    public class HeaderHelper
    {
        #region Constants

        /// <summary>
        ///     The Unknown id.
        /// </summary>
        private const string UnknownId = "Unknown";

        #endregion

        #region Static Fields

        /// <summary>
        ///     The instance.
        /// </summary>
        private static readonly HeaderHelper _instance = new HeaderHelper();

        #endregion

        #region Fields

        /// <summary>
        ///     The header manager. TODO
        /// </summary>
        private readonly IHeaderRetrievalManager _headerManager = null;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Prevents a default instance of the <see cref="HeaderHelper" /> class from being created.
        /// </summary>
        private HeaderHelper()
        {
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets the instance.
        /// </summary>
        public static HeaderHelper Instance
        {
            get
            {
                return _instance;
            }
        }

        /// <summary>
        ///     Gets the receiver Id for this message
        /// </summary>
        public string ReceiverId
        {
            get
            {
                if (this._headerManager != null && this._headerManager.Header != null
                    && ObjectUtil.ValidCollection(this._headerManager.Header.Receiver))
                {
                    return this._headerManager.Header.Receiver[0].Id;
                }

                return UnknownId;
            }
        }

        /// <summary>
        ///     Gets the sender Id for this message
        /// </summary>
        public string SenderId
        {
            get
            {
                if (this._headerManager != null && this._headerManager.Header != null)
                {
                    string retVal = this._headerManager.Header.Sender.Id;
                    if (retVal != null)
                    {
                        return retVal;
                    }
                }

                return UnknownId;
            }
        }

        #endregion
    }
}