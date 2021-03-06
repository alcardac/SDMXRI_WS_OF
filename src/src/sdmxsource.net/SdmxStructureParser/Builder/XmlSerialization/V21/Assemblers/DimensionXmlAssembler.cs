// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DimensionXmlAssembler.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The dimension xml assembler.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.Structureparser.Builder.XmlSerialization.V21.Assemblers
{
    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Common;
    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Structure;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.DataStructure;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;

    /// <summary>
    ///     The dimension xml assembler.
    /// </summary>
    public class DimensionXmlAssembler : IAssembler<DimensionType, IDimension>
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
        public virtual void Assemble(DimensionType assembleInto, IDimension assembleFrom)
        {
            this._componentAssembler.AssembleComponent(assembleInto, assembleFrom);

            if (assembleFrom.ConceptRole != null)
            {
                foreach (ICrossReference currentConceptRole in assembleFrom.ConceptRole)
                {
                    var conceptRef = new ConceptReferenceType();
                    assembleInto.ConceptRole.Add(conceptRef);
                    var conceptRefType = new ConceptRefType();
                    conceptRef.SetTypedRef(conceptRefType);
                    this._componentAssembler.SetReference(conceptRefType, currentConceptRole);
                }
            }

            assembleInto.position = assembleFrom.Position;
        }

        #endregion
    }
}