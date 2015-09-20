// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AbstractRetrievalManager.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The abstract retrieval manager.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.StructureRetrieval.Manager
{
    using System;

    using Org.Sdmxsource.Sdmx.Api.Manager.Retrieval;

    /// <summary>
    ///     The abstract retrieval manager.
    /// </summary>
    public abstract class AbstractRetrievalManager
    {
        #region Fields

        /// <summary>
        ///     The _sdmx object retrieval manager.
        /// </summary>
        private readonly ISdmxObjectRetrievalManager _sdmxObjectRetrievalManager;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AbstractRetrievalManager"/> class.
        /// </summary>
        /// <param name="sdmxObjectRetrievalManager">
        /// The sdmx object retrieval manager.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="sdmxObjectRetrievalManager"/> is null
        /// </exception>
        protected AbstractRetrievalManager(ISdmxObjectRetrievalManager sdmxObjectRetrievalManager)
        {
            if (sdmxObjectRetrievalManager == null)
            {
                throw new ArgumentNullException("sdmxObjectRetrievalManager");
            }

            this._sdmxObjectRetrievalManager = sdmxObjectRetrievalManager;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets the sdmx object retrieval manager.
        /// </summary>
        public ISdmxObjectRetrievalManager SdmxObjectRetrievalManager
        {
            get
            {
                return this._sdmxObjectRetrievalManager;
            }
        }

        #endregion
    }
}