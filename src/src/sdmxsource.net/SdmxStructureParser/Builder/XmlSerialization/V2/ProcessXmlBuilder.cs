// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProcessXmlBuilder.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The process xml codelistRef builder.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.Structureparser.Builder.XmlSerialization.V2
{
    using System;
    using System.Collections.Generic;

    using Org.Sdmx.Resources.SdmxMl.Schemas.V20.structure;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Process;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Util;
    using Org.Sdmxsource.Util;
    using Org.Sdmxsource.Util.Extensions;

    /// <summary>
    ///     The process xml codelistRef builder.
    /// </summary>
    public class ProcessXmlBuilder : AbstractBuilder
    {
        #region Public Methods and Operators

        /// <summary>
        /// Build <see cref="ProcessType"/> from <paramref name="buildFrom"/>.
        /// </summary>
        /// <param name="buildFrom">
        /// The build from.
        /// </param>
        /// <returns>
        /// The <see cref="ProcessType"/> from <paramref name="buildFrom"/> .
        /// </returns>
        public ProcessType Build(IProcessObject buildFrom)
        {
            var builtObj = new ProcessType();

            // MAINTAINABLE ATTRIBTUES
            string value = buildFrom.AgencyId;
            if (!string.IsNullOrWhiteSpace(value))
            {
                builtObj.agencyID = buildFrom.AgencyId;
            }

            string value1 = buildFrom.Id;
            if (!string.IsNullOrWhiteSpace(value1))
            {
                builtObj.id = buildFrom.Id;
            }

            if (buildFrom.Uri != null)
            {
                builtObj.uri = buildFrom.Uri;
            }
            else if (buildFrom.StructureUrl != null)
            {
                builtObj.uri = buildFrom.StructureUrl;
            }
            else if (buildFrom.ServiceUrl != null)
            {
                builtObj.uri = buildFrom.StructureUrl;
            }

            if (ObjectUtil.ValidString(buildFrom.Urn))
            {
                builtObj.urn = buildFrom.Urn;
            }

            string value2 = buildFrom.Version;
            if (!string.IsNullOrWhiteSpace(value2))
            {
                builtObj.version = buildFrom.Version;
            }

            if (buildFrom.StartDate != null)
            {
                builtObj.validFrom = buildFrom.StartDate;
            }

            if (buildFrom.EndDate != null)
            {
                builtObj.validTo = buildFrom.EndDate;
            }

            IList<ITextTypeWrapper> names = buildFrom.Names;
            if (ObjectUtil.ValidCollection(names))
            {
                builtObj.Name = this.GetTextType(names);
            }

            IList<ITextTypeWrapper> descriptions = buildFrom.Descriptions;
            if (ObjectUtil.ValidCollection(descriptions))
            {
                builtObj.Description = this.GetTextType(descriptions);
            }

            if (this.HasAnnotations(buildFrom))
            {
                builtObj.Annotations = this.GetAnnotationsType(buildFrom);
            }

            if (buildFrom.IsExternalReference.IsSet())
            {
                builtObj.isExternalReference = buildFrom.IsExternalReference.IsTrue;
            }

            if (buildFrom.IsFinal.IsSet())
            {
                builtObj.isFinal = buildFrom.IsFinal.IsTrue;
            }

            /* foreach */
            foreach (IProcessStepObject processStep in buildFrom.ProcessSteps)
            {
                var processStepType = new ProcessStepType();
                builtObj.ProcessStep.Add(processStepType);
                this.ProcessProcessStep(processStep, processStepType);
            }

            return builtObj;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Process <paramref name="buildFrom"/>
        /// </summary>
        /// <param name="buildFrom">
        /// The <see cref="IProcessStepObject"/> to build from
        /// </param>
        /// <param name="builtObj">
        /// The output
        /// </param>
        private void ProcessProcessStep(IProcessStepObject buildFrom, ProcessStepType builtObj)
        {
            string value = buildFrom.Id;
            if (!string.IsNullOrWhiteSpace(value))
            {
                builtObj.id = buildFrom.Id;
            }

            IList<ITextTypeWrapper> names = buildFrom.Names;
            if (ObjectUtil.ValidCollection(names))
            {
                builtObj.Name = this.GetTextType(names);
            }

            IList<ITextTypeWrapper> descriptions = buildFrom.Descriptions;
            if (ObjectUtil.ValidCollection(descriptions))
            {
                builtObj.Description = this.GetTextType(descriptions);
            }

            IList<IInputOutputObject> inputObjects = buildFrom.Input;
            if (ObjectUtil.ValidCollection(inputObjects))
            {
                /* foreach */
                foreach (IInputOutputObject input in inputObjects)
                {
                    if (input.StructureReference != null)
                    {
                        builtObj.Input.Add(
                            XmlobjectsEnumUtil.GetSdmxObjectIdType(input.StructureReference.TargetReference));
                    }
                }
            }

            IList<IInputOutputObject> outputObjects = buildFrom.Output;
            if (ObjectUtil.ValidCollection(outputObjects))
            {
                /* foreach */
                foreach (IInputOutputObject outputObject in outputObjects)
                {
                    if (outputObject.StructureReference != null)
                    {
                        builtObj.Input.Add(
                            XmlobjectsEnumUtil.GetSdmxObjectIdType(outputObject.StructureReference.TargetReference));
                    }
                }
            }

            if (buildFrom.Computation != null)
            {
                builtObj.Computation.AddAll(this.GetTextType(buildFrom.Computation.Description));
            }

            IList<ITransition> transitions = buildFrom.Transitions;
            if (ObjectUtil.ValidCollection(transitions))
            {
                /* foreach */
                foreach (ITransition transition in transitions)
                {
                    var transitionType = new TransitionType();
                    builtObj.Transition.Add(transitionType);
                    if (transition.TargetStep != null)
                    {
                        transitionType.TargetStep = transition.TargetStep.Id;
                    }

                    IList<ITextTypeWrapper> textTypeWrappers = transition.Condition;
                    if (ObjectUtil.ValidCollection(textTypeWrappers))
                    {
                        transitionType.Condition = this.GetTextType(textTypeWrappers[0]);
                    }
                }
            }

            IList<IProcessStepObject> processStepObjects = buildFrom.ProcessSteps;
            if (ObjectUtil.ValidCollection(processStepObjects))
            {
                /* foreach */
                foreach (IProcessStepObject processStep in processStepObjects)
                {
                    var newProcessStepType = new ProcessStepType();
                    builtObj.ProcessStep.Add(newProcessStepType);
                    this.ProcessProcessStep(processStep, newProcessStepType);
                }
            }

            if (this.HasAnnotations(buildFrom))
            {
                builtObj.Annotations = this.GetAnnotationsType(buildFrom);
            }
        }

        #endregion
    }
}