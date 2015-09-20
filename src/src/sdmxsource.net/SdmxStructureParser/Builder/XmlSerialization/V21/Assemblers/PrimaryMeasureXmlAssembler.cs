// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PrimaryMeasureXmlAssembler.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The primary measure xml assembler.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.Structureparser.Builder.XmlSerialization.V21.Assemblers
{
    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Structure;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.DataStructure;

    /// <summary>
    ///     The primary measure xml assembler.
    /// </summary>
    public class PrimaryMeasureXmlAssembler : IAssembler<PrimaryMeasureType, IPrimaryMeasure>
    {
        #region Fields

        /// <summary>
        ///     The component assembler.
        /// </summary>
        private readonly ComponentAssembler<SimpleDataStructureRepresentationType> _componentAssembler =
            new ComponentAssembler<SimpleDataStructureRepresentationType>();

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
        public virtual void Assemble(PrimaryMeasureType assembleInto, IPrimaryMeasure assembleFrom)
        {
            this._componentAssembler.AssembleComponent(assembleInto, assembleFrom);
        }

        #endregion
    }
}