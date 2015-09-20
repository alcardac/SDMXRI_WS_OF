// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TimeDimensionXmlAssembler.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The time dimension xml assembler.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.Structureparser.Builder.XmlSerialization.V21.Assemblers
{
    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Structure;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.DataStructure;

    /// <summary>
    ///     The time dimension xml assembler.
    /// </summary>
    public class TimeDimensionXmlAssembler : IAssembler<TimeDimensionType, IDimension>
    {
        #region Fields

        /// <summary>
        ///     The component assembler.
        /// </summary>
        private readonly ComponentAssembler<TimeDimensionRepresentationType> _componentAssembler =
            new ComponentAssembler<TimeDimensionRepresentationType>();

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The assemble.
        /// </summary>
        /// <param name="assembleInto">
        /// The assemble into.
        /// </param>
        /// <param name="assembleFrom">
        /// The assemble from.
        /// </param>
        public virtual void Assemble(TimeDimensionType assembleInto, IDimension assembleFrom)
        {
            this._componentAssembler.AssembleComponent(assembleInto, assembleFrom);
            assembleInto.position = assembleFrom.Position;
        }

        #endregion
    }
}