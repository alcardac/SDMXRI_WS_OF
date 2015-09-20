// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataflowReferenceInfo.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The dataflow reference info.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Estat.Sri.SdmxStructureMutableParser.Model
{
    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Registry;

    /// <summary>
    ///     The dataflow reference info.
    /// </summary>
    public class DataflowReferenceInfo : ReferenceInfo, IDataflowReferenceInfo
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DataflowReferenceInfo" /> class.
        /// </summary>
        public DataflowReferenceInfo()
            : base(SdmxStructureEnumType.Dataflow)
        {
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets or sets the constraint.
        /// </summary>
        public IContentConstraintMutableObject Constraint { get; set; }

        #endregion
    }
}