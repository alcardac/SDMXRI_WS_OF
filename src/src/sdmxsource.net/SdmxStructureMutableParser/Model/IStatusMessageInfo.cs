// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IStatusMessageInfo.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The Status Message interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Estat.Sri.SdmxStructureMutableParser.Model
{
    using System.Collections.Generic;

    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base;

    /// <summary>
    ///     The Status Message interface.
    /// </summary>
    public interface IStatusMessageInfo
    {
        #region Public Properties

        /// <summary>
        ///     Gets the message texts.
        /// </summary>
        IList<ITextTypeWrapperMutableObject> MessageTexts { get; }

        /// <summary>
        ///     Gets or sets the status.
        /// </summary>
        Status Status { get; set; }

        #endregion
    }
}