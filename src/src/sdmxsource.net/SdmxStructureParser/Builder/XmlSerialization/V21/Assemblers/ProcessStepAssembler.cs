// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProcessStepAssembler.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The process step bean assembler.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.Structureparser.Builder.XmlSerialization.V21.Assemblers
{
    using System.Collections.Generic;

    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Common;
    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Structure;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Process;
    using Org.Sdmxsource.Util;

    /// <summary>
    ///     The process step bean assembler.
    /// </summary>
    public class ProcessStepAssembler : NameableAssembler, IAssembler<ProcessStepType, IProcessStepObject>
    {
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
        public virtual void Assemble(ProcessStepType assembleInto, IProcessStepObject assembleFrom)
        {
            var processes = new Stack<KeyValuePair<ProcessStepType, IProcessStepObject>>();
            processes.Push(new KeyValuePair<ProcessStepType, IProcessStepObject>(assembleInto, assembleFrom));

            while (processes.Count > 0)
            {
                KeyValuePair<ProcessStepType, IProcessStepObject> pair = processes.Pop();
                assembleInto = pair.Key;
                assembleFrom = pair.Value;

                this.AssembleNameable(assembleInto, assembleFrom);

                /* foreach */
                foreach (IInputOutputObject currentInput in assembleFrom.Input)
                {
                    var inputOutputType = new InputOutputType();
                    assembleInto.Input.Add(inputOutputType);
                    this.AssembleInputOutput(currentInput, inputOutputType);
                }

                /* foreach */
                foreach (IInputOutputObject currentOutput in assembleFrom.Output)
                {
                    var inputOutputType = new InputOutputType();
                    assembleInto.Output.Add(inputOutputType);
                    this.AssembleInputOutput(currentOutput, inputOutputType);
                }

                if (assembleFrom.Computation != null)
                {
                    var computationType = new ComputationType();
                    assembleInto.Computation = computationType;
                    this.AssembleComputation(assembleFrom.Computation, computationType);
                }

                /* foreach */
                foreach (ITransition currentTranistion in assembleFrom.Transitions)
                {
                    var transitionType = new TransitionType();
                    assembleInto.Transition.Add(transitionType);
                    this.AssembleTransition(currentTranistion, transitionType);
                }

                /* foreach */
                foreach (IProcessStepObject eachProcessStepBean in assembleFrom.ProcessSteps)
                {
                    var eachProcessStep = new ProcessStepType();
                    assembleInto.ProcessStep.Add(eachProcessStep);
                    processes.Push(
                        new KeyValuePair<ProcessStepType, IProcessStepObject>(eachProcessStep, eachProcessStepBean));

                    ////this.Assemble(eachProcessStep, eachProcessStepBean);
                }
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// The assemble computation.
        /// </summary>
        /// <param name="computationBean">
        /// The computation bean.
        /// </param>
        /// <param name="computationType">
        /// The computation type.
        /// </param>
        private void AssembleComputation(IComputationObject computationBean, ComputationType computationType)
        {
            string value3 = computationBean.LocalId;
            if (!string.IsNullOrWhiteSpace(value3))
            {
                computationType.localID = computationBean.LocalId;
            }

            string value1 = computationBean.SoftwarePackage;
            if (!string.IsNullOrWhiteSpace(value1))
            {
                computationType.softwarePackage = computationBean.SoftwarePackage;
            }

            string value2 = computationBean.SoftwareLanguage;
            if (!string.IsNullOrWhiteSpace(value2))
            {
                computationType.softwareLanguage = computationBean.SoftwareLanguage;
            }

            string value = computationBean.SoftwareVersion;
            if (!string.IsNullOrWhiteSpace(value))
            {
                computationType.softwareVersion = computationBean.SoftwareVersion;
            }

            if (this.HasAnnotations(computationBean))
            {
                computationType.Annotations = new Annotations(this.GetAnnotationsType(computationBean));
            }

            this.GetTextType(computationBean.Description, computationType.Description);
        }

        /// <summary>
        /// Assemble input output.
        /// </summary>
        /// <param name="currentInput">
        /// The current input.
        /// </param>
        /// <param name="inputOutputType">
        /// The input output type.
        /// </param>
        private void AssembleInputOutput(IInputOutputObject currentInput, InputOutputType inputOutputType)
        {
            string value = currentInput.LocalId;
            if (!string.IsNullOrWhiteSpace(value))
            {
                inputOutputType.localID = currentInput.LocalId;
            }

            if (this.HasAnnotations(currentInput))
            {
                inputOutputType.Annotations = new Annotations(this.GetAnnotationsType(currentInput));
            }

            if (currentInput.StructureReference != null)
            {
                var objectReferenceType = new ObjectReferenceType();
                inputOutputType.ObjectReference = objectReferenceType;
                var objectRefType = new ObjectRefType();
                objectReferenceType.SetTypedRef(objectRefType);
                this.SetReference(objectRefType, currentInput.StructureReference);
            }
        }

        /// <summary>
        /// The assemble transition.
        /// </summary>
        /// <param name="transitionBean">
        /// The transition bean.
        /// </param>
        /// <param name="transitionType">
        /// The transition type.
        /// </param>
        private void AssembleTransition(ITransition transitionBean, TransitionType transitionType)
        {
            string value = transitionBean.LocalId;
            if (!string.IsNullOrWhiteSpace(value))
            {
                transitionType.localID = transitionBean.LocalId;
            }

            this.AssembleIdentifiable(transitionType, transitionBean);
            if (transitionBean.TargetStep != null)
            {
                var procRef = new LocalProcessStepReferenceType();
                transitionType.TargetStep = procRef;
                var xref = new LocalProcessStepRefType();
                procRef.Ref = xref;
                xref.id = transitionBean.TargetStep.GetFullIdPath(false);
            }

            if (ObjectUtil.ValidCollection(transitionBean.Condition))
            {
                transitionType.Condition = this.GetTextType(transitionBean.Condition);
            }
            else
            {
                transitionType.Condition.Add(new TextType());
            }
        }

        #endregion
    }
}