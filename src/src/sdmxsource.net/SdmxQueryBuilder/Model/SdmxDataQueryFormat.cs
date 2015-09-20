// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SdmxDataQueryFormat.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The sdmxdata query format implementation.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.SdmxQueryBuilder.Model
{

    #region Using Directives

    using System;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Format;

    #endregion

    /// <summary>
    /// TODO
    /// </summary>
    /// <typeparam name="T">
    /// The type of the output
    /// </typeparam>
    public class SdmxDataQueryFormat<T> : IDataQueryFormat<T>
    {

        #region Fields

        private readonly SdmxSchema _version;

        #endregion


        #region Constructors and Destructors

        /// <summary>
        /// Creates SdmxData query format from a version
        /// </summary>
        /// <param name="version">
        /// The version
        /// </param>
        public SdmxDataQueryFormat(SdmxSchema version)
        {
            this._version = version;

            if (version == null)
            {
                throw new ArgumentNullException("version");
            }
        }

        #endregion


        #region Public Properties

        /// <summary>
        /// Gets the version
        /// </summary>
        public SdmxSchema Version
        {
            get
            {
                return _version;
            }
        }

        #endregion


        #region Public Methods and Operators

        /// <summary>
        /// Get the version string
        /// </summary>
        /// <returns>
        /// The version string
        /// </returns>
        public override string ToString()
        {
            return this._version.ToString();
        }

        #endregion

    }
}
