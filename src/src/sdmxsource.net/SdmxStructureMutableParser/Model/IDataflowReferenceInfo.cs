// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IDataflowReferenceInfo.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The Dataflow Reference interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Estat.Sri.SdmxStructureMutableParser.Model
{
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Registry;

    /// <summary>
    ///     The Dataflow Reference interface.
    /// </summary>
    public interface IDataflowReferenceInfo : IReferenceInfo
    {
        #region Public Properties

        /// <summary>
        ///     Gets or sets the constraint.
        /// </summary>
        IContentConstraintMutableObject Constraint { get; set; }

        #endregion
    }
}