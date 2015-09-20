// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IStructureValidationManager.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The StructureValidationManager interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.Structureparser.Manager.Parsing
{
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;

    /// <summary>
    ///     The StructureValidationManager interface.
    /// </summary>
    public interface IStructureValidationManager
    {
        #region Public Methods and Operators

        /// <summary>
        /// Validates a structure is compliant to the 2.1 SDMX schema, by writing it out as SDMX and validating against the schema
        /// </summary>
        /// <param name="maintainableBean">
        /// The maintainable object
        /// </param>
        void ValidateStructure(IMaintainableObject maintainableBean);

        #endregion
    }
}