// -----------------------------------------------------------------------
// <copyright file="ProcessCrossReferenceUpdater.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Structureparser.Engine.Reversion
{
    using System;
    using System.Collections.Generic;

    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Process;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Process;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;

    /// <summary>
    ///   TODO: Update summary.
    /// </summary>
    public class ProcessCrossReferenceUpdater : IProcessCrossReferenceUpdater
    {
        /// <summary>
        /// The update references.
        /// </summary>
        /// <param name="maintianable">
        /// The maintianable.
        /// </param>
        /// <param name="updateReferences">
        /// The update references.
        /// </param>
        /// <param name="newVersionNumber">
        /// The new version number.
        /// </param>
        /// <returns>
        /// The <see cref="IProcessObject"/>.
        /// </returns>
        public IProcessObject UpdateReferences(
            IProcessObject maintianable,
            IDictionary<IStructureReference, IStructureReference> updateReferences,
            string newVersionNumber)
        {
            IProcessMutableObject processMutableBean = maintianable.MutableInstance;
            processMutableBean.Version = newVersionNumber;
            this.UpdateProcessSteps(processMutableBean.ProcessSteps, updateReferences);
            return processMutableBean.ImmutableInstance;
        }

        /// <summary>
        /// The update process steps.
        /// </summary>
        /// <param name="processSteps">
        /// The process steps.
        /// </param>
        /// <param name="updateReferences">
        /// The update references.
        /// </param>
        private void UpdateProcessSteps(
            IList<IProcessStepMutableObject> processSteps,
            IDictionary<IStructureReference, IStructureReference> updateReferences)
        {
            if (processSteps != null)
            {
                foreach (IProcessStepMutableObject currentProcessStep in processSteps)
                {
                    this.UpdateProcessSteps(currentProcessStep.ProcessSteps, updateReferences);
                    this.UpdateInputOutput(currentProcessStep.Input, updateReferences);
                    this.UpdateInputOutput(currentProcessStep.Output, updateReferences);
                }
            }
        }

        /// <summary>
        /// The update input output.
        /// </summary>
        /// <param name="inputOutput">
        /// The input output.
        /// </param>
        /// <param name="updateReferences">
        /// The update references.
        /// </param>
        private void UpdateInputOutput(
            IList<IInputOutputMutableObject> inputOutput,
            IDictionary<IStructureReference, IStructureReference> updateReferences)
        {
            if (inputOutput != null)
            {
                foreach (IInputOutputMutableObject currentInputOutput in inputOutput)
                {
                    IStructureReference updateTo = updateReferences[currentInputOutput.StructureReference];
                    if (updateTo != null)
                    {
                        currentInputOutput.StructureReference = updateTo;
                    }
                }
            }
        }
    }
}
