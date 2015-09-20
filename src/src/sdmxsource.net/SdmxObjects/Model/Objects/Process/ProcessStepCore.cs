// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProcessStepBeanImpl.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The process step core.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Process
{
    using System;
    using System.Collections.Generic;

    using log4net;

    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Structure;
    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Exception;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Process;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Process;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Base;
    using Org.Sdmxsource.Util.Extensions;

    /// <summary>
    ///   The process step core.
    /// </summary>
    [Serializable]
    public class ProcessStepCore : NameableCore, IProcessStepObject
    {
        #region Static Fields

        /// <summary>
        ///   The log.
        /// </summary>
        private static readonly ILog LOG = LogManager.GetLogger(typeof(ProcessStepCore));

        #endregion

        #region Fields

        /// <summary>
        ///   The computation.
        /// </summary>
        private readonly IComputationObject computation;

        /// <summary>
        ///   The input.
        /// </summary>
        private readonly IList<IInputOutputObject> input;

        /// <summary>
        ///   The output.
        /// </summary>
        private readonly IList<IInputOutputObject> output;

        /// <summary>
        ///   The process steps.
        /// </summary>
        private readonly IList<IProcessStepObject> processSteps;

        /// <summary>
        ///   The transitions.
        /// </summary>
        private readonly IList<ITransition> transitions;

        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM MUTABLE OBJECTS             //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ProcessStepCore"/> class.
        /// </summary>
        /// <param name="parent">
        /// The parent. 
        /// </param>
        /// <param name="processStepMutableObject">
        /// The iprocess. 
        /// </param>
        /// <exception cref="SdmxSemmanticException">
        /// Throws Validate exception.
        /// </exception>
        public ProcessStepCore(IIdentifiableObject parent, IProcessStepMutableObject processStepMutableObject)
            : base(processStepMutableObject, parent)
        {
            this.input = new List<IInputOutputObject>();
            this.output = new List<IInputOutputObject>();
            this.transitions = new List<ITransition>();
            this.processSteps = new List<IProcessStepObject>();
            if (processStepMutableObject.Input != null)
            {
                foreach (IInputOutputMutableObject currentIo in processStepMutableObject.Input)
                {
                    this.input.Add(new InputOutputCore(this, currentIo));
                }
            }

            if (processStepMutableObject.Output != null)
            {
                foreach (IInputOutputMutableObject currentIo0 in processStepMutableObject.Output)
                {
                    this.output.Add(new InputOutputCore(this, currentIo0));
                }
            }

            if (processStepMutableObject.Computation != null)
            {
                this.computation = new ComputationCore(this, processStepMutableObject.Computation);
            }

            if (processStepMutableObject.Transitions != null)
            {
                foreach (ITransitionMutableObject mutable in processStepMutableObject.Transitions)
                {
                    this.transitions.Add(new TransitionCore(mutable, this));
                }
            }

            if (processStepMutableObject.ProcessSteps != null)
            {
                foreach (IProcessStepMutableObject mutable1 in processStepMutableObject.ProcessSteps)
                {
                    this.processSteps.Add(new ProcessStepCore(this, mutable1));
                }
            }

            try
            {
                this.Validate();
            }
            catch (SdmxSemmanticException e)
            {
                throw new SdmxSemmanticException(e, ExceptionCode.FailValidation, this);
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM V2.1 SCHEMA                 //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Initializes a new instance of the <see cref="ProcessStepCore"/> class.
        /// </summary>
        /// <param name="parent">
        /// The parent. 
        /// </param>
        /// <param name="process">
        /// The process. 
        /// </param>
        /// <exception cref="SdmxSemmanticException">
        /// Throws Validate exception.
        /// </exception>
        public ProcessStepCore(INameableObject parent, ProcessStepType process)
            : base(process, SdmxStructureType.GetFromEnum(SdmxStructureEnumType.ProcessStep), parent)
        {
            this.input = new List<IInputOutputObject>();
            this.output = new List<IInputOutputObject>();
            this.transitions = new List<ITransition>();
            this.processSteps = new List<IProcessStepObject>();

            if (process.Input != null)
            {
                foreach (InputOutputType currentIo in process.Input)
                {
                    this.input.Add(new InputOutputCore(this, currentIo));
                }
            }

            if (process.Output != null)
            {
                foreach (InputOutputType currentIo0 in process.Output)
                {
                    this.output.Add(new InputOutputCore(this, currentIo0));
                }
            }

            if (process.Computation != null)
            {
                this.computation = new ComputationCore(this, process.Computation);
            }

            if (process.ProcessStep != null)
            {
                foreach (ProcessStepType processStep in process.ProcessStep)
                {
                    this.processSteps.Add(new ProcessStepCore(this, processStep));
                }
            }

            if (process.Transition != null)
            {
                foreach (TransitionType trans in process.Transition)
                {
                    ITransition transitionCore = new TransitionCore(trans, this);
                    this.transitions.Add(transitionCore);
                }
            }

            try
            {
                this.Validate();
            }
            catch (SdmxSemmanticException e)
            {
                throw new SdmxSemmanticException(e, ExceptionCode.FailValidation, this);
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM V2 SCHEMA                 //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Initializes a new instance of the <see cref="ProcessStepCore"/> class.
        /// </summary>
        /// <param name="parent">
        /// The parent. 
        /// </param>
        /// <param name="process">
        /// The process. 
        /// </param>
        /// <exception cref="SdmxSemmanticException">
        /// Throws Validate exception.
        /// </exception>
        public ProcessStepCore(
            INameableObject parent, Org.Sdmx.Resources.SdmxMl.Schemas.V20.structure.ProcessStepType process)
            : base(
                process, 
                SdmxStructureType.GetFromEnum(SdmxStructureEnumType.ProcessStep), 
                process.id, 
                default(Uri), 
                process.Name, 
                process.Description, 
                process.Annotations, 
                parent)
        {
            this.input = new List<IInputOutputObject>();
            this.output = new List<IInputOutputObject>();
            this.transitions = new List<ITransition>();
            this.processSteps = new List<IProcessStepObject>();

            if (process.Input != null)
            {
                LOG.Warn("Input items not supported for SDMX V2.0. These items will be discarded");
            }

            if (process.Output != null)
            {
                LOG.Warn("Input items not supported for SDMX V2.0. These items will be discarded");
            }

            if (process.Computation != null)
            {
                this.computation = new ComputationCore(this, process);
            }

            if (process.ProcessStep != null)
            {
                foreach (
                    Org.Sdmx.Resources.SdmxMl.Schemas.V20.structure.ProcessStepType processStep in process.ProcessStep)
                {
                    this.processSteps.Add(new ProcessStepCore(this, processStep));
                }
            }

            if (process.Transition != null)
            {
                foreach (Org.Sdmx.Resources.SdmxMl.Schemas.V20.structure.TransitionType trans in process.Transition)
                {
                    ITransition transitionCore = new TransitionCore(trans, this);
                    this.transitions.Add(transitionCore);
                }
            }

            try
            {
                this.Validate();
            }
            catch (SdmxSemmanticException e)
            {
                throw new SdmxSemmanticException(e, ExceptionCode.FailValidation, this);
            }
        }

        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////DEEP EQUALS                             //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////
        #region Public Properties

        /// <summary>
        ///   Gets the all text types.
        /// </summary>
        public override IList<ITextTypeWrapper> AllTextTypes
        {
            get
            {
                IList<ITextTypeWrapper> allTextTypes = base.AllTextTypes;
                if (this.computation != null)
                {
                    this.computation.Description.AddAll(allTextTypes);
                }

                return allTextTypes;
            }
        }

        /// <summary>
        ///   Gets the computation.
        /// </summary>
        public virtual IComputationObject Computation
        {
            get
            {
                return this.computation;
            }
        }

        /// <summary>
        ///   Gets the input.
        /// </summary>
        public virtual IList<IInputOutputObject> Input
        {
            get
            {
                return new List<IInputOutputObject>(this.input);
            }
        }

        /// <summary>
        ///   Gets the output.
        /// </summary>
        public virtual IList<IInputOutputObject> Output
        {
            get
            {
                return new List<IInputOutputObject>(this.output);
            }
        }

        /// <summary>
        ///   Gets the process steps.
        /// </summary>
        public virtual IList<IProcessStepObject> ProcessSteps
        {
            get
            {
                return new List<IProcessStepObject>(this.processSteps);
            }
        }

        /// <summary>
        ///   Gets the transitions.
        /// </summary>
        public virtual IList<ITransition> Transitions
        {
            get
            {
                return new List<ITransition>(this.transitions);
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The deep equals.
        /// </summary>
        /// <param name="sdmxObject">
        /// The sdmxObject. 
        /// </param>
        /// <param name="includeFinalProperties"> </param>
        /// <returns>
        /// The <see cref="bool"/> . 
        /// </returns>
        public override bool DeepEquals(ISdmxObject sdmxObject, bool includeFinalProperties)
        {
            if (sdmxObject == null) return false;
            if (sdmxObject.StructureType == this.StructureType)
            {
                var that = (IProcessStepObject)sdmxObject;
                if (!this.Equivalent(this.input, that.Input, includeFinalProperties))
                {
                    return false;
                }

                if (!this.Equivalent(this.output, that.Output, includeFinalProperties))
                {
                    return false;
                }

                if (!this.Equivalent(this.transitions, that.Transitions, includeFinalProperties))
                {
                    return false;
                }

                if (!this.Equivalent(this.processSteps, that.ProcessSteps, includeFinalProperties))
                {
                    return false;
                }

                if (!this.Equivalent(this.computation, that.Computation, includeFinalProperties))
                {
                    return false;
                }

                return this.DeepEqualsNameable(that, includeFinalProperties);
            }

            return false;
        }

        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////VALIDATION                             //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////
        #region Methods

        /// <summary>
        ///   The validate.
        /// </summary>
        private void Validate()
        {
            // NO VALIDATION REQUIRED
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////COMPOSITES		                     //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////
       
        /// <summary>
        ///   Get composites internal.
        /// </summary>
        protected override ISet<ISdmxObject> GetCompositesInternal() 
        {
          ISet<ISdmxObject> composites = base.GetCompositesInternal();
          base.AddToCompositeSet(this.input, composites);
          base.AddToCompositeSet(this.output, composites);
          base.AddToCompositeSet(this.transitions, composites);
          base.AddToCompositeSet(this.processSteps, composites);
          base.AddToCompositeSet(this.computation, composites);
          return composites;
        }

        #endregion
    }
}