// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HierarchyAssembler.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The hierarchy hierarchicalCodelist assembler.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.Structureparser.Builder.XmlSerialization.V21.Assemblers
{
    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Structure;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Codelist;

    /// <summary>
    ///     The hierarchy hierarchicalCodelist assembler.
    /// </summary>
    public class HierarchyAssembler : NameableAssembler, IAssembler<HierarchyType, IHierarchy>
    {
        #region Fields

        /// <summary>
        ///     The hierarchical code hierarchicalCodelist assembler.
        /// </summary>
        private readonly HierarchicalCodeAssembler _hierarchicalCodeAssembler = new HierarchicalCodeAssembler();

        /// <summary>
        ///     The level assembler.
        /// </summary>
        private readonly LevelAssembler _levelAssembler = new LevelAssembler();

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
        public virtual void Assemble(HierarchyType assembleInto, IHierarchy assembleFrom)
        {
            // Populate it from inherited super
            this.AssembleNameable(assembleInto, assembleFrom);

            // Populate it using this class's specifics
            foreach (IHierarchicalCode eachCodeRefBean in assembleFrom.HierarchicalCodeObjects)
            {
                var eachHierarchicalCode = new HierarchicalCodeType();
                assembleInto.HierarchicalCode.Add(eachHierarchicalCode);
                this._hierarchicalCodeAssembler.Assemble(eachHierarchicalCode, eachCodeRefBean);
            }

            if (assembleFrom.Level != null)
            {
                this._levelAssembler.Assemble(assembleInto.Level = new LevelType(), assembleFrom.Level);
            }

            assembleInto.leveled = assembleFrom.HasFormalLevels();
        }

        #endregion
    }
}