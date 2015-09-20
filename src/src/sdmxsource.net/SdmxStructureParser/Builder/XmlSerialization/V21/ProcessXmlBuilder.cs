// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProcessXmlBuilder.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The process xml bean builder.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.Structureparser.Builder.XmlSerialization.V21
{
    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Structure;
    using Org.Sdmxsource.Sdmx.Api.Builder;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Process;
    using Org.Sdmxsource.Sdmx.Structureparser.Builder.XmlSerialization.V21.Assemblers;

    /// <summary>
    ///     The process xml bean builder.
    /// </summary>
    public class ProcessXmlBuilder : MaintainableAssembler, IBuilder<ProcessType, IProcessObject>
    {
        #region Fields

        /// <summary>
        ///     The process step bean assembler bean.
        /// </summary>
        private readonly ProcessStepAssembler _processStepAssemblerBean = new ProcessStepAssembler();

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Build a <see cref="ProcessType"/> from <paramref name="buildFrom"/> and return it
        /// </summary>
        /// <param name="buildFrom">
        /// The <see cref="IProcessObject"/>
        /// </param>
        /// <returns>
        /// The <see cref="ProcessType"/>.
        /// </returns>
        public virtual ProcessType Build(IProcessObject buildFrom)
        {
            // Create outgoing build
            var builtObj = new ProcessType();

            // Populate it from inherited super
            this.AssembleMaintainable(builtObj, buildFrom);

            // Populate it using this class's specifics
            foreach (IProcessStepObject eachProcessStepBean in buildFrom.ProcessSteps)
            {
                var newProcessStepType = new ProcessStepType();
                builtObj.ProcessStep.Add(newProcessStepType);
                this._processStepAssemblerBean.Assemble(newProcessStepType, eachProcessStepBean);
            }

            return builtObj;
        }

        #endregion
    }
}