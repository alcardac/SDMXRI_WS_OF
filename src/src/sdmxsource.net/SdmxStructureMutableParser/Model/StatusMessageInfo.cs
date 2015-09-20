// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StatusMessageInfo.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The status message info.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Estat.Sri.SdmxStructureMutableParser.Model
{
    using System.Collections.Generic;

    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base;

    /// <summary>
    ///     The status message info.
    /// </summary>
    public class StatusMessageInfo : IStatusMessageInfo
    {
        /// <summary>
        /// The _message texts.
        /// </summary>
        private readonly IList<ITextTypeWrapperMutableObject> _messageTexts = new List<ITextTypeWrapperMutableObject>();

        #region Public Properties

        /// <summary>
        ///     Gets the message texts.
        /// </summary>
        public IList<ITextTypeWrapperMutableObject> MessageTexts
        {
            get
            {
                return this._messageTexts;
            }
        }

        /// <summary>
        ///     Gets or sets the status.
        /// </summary>
        public Status Status { get; set; }

        #endregion
    }
}