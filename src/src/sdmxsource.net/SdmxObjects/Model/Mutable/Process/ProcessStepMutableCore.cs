// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProcessStepMutableBeanImpl.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The process step mutable core.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.Process
{
    using System;
    using System.Collections.Generic;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Process;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Process;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.Base;

    /// <summary>
    ///   The process step mutable core.
    /// </summary>
    [Serializable]
    public class ProcessStepMutableCore : NameableMutableCore, IProcessStepMutableObject
    {
        #region Fields

        /// <summary>
        ///   The _computation.
        /// </summary>
        private IComputationMutableObject _computation;

        /// <summary>
        ///   The _input.
        /// </summary>
        private IList<IInputOutputMutableObject> _input;

        /// <summary>
        ///   The _output.
        /// </summary>
        private IList<IInputOutputMutableObject> _output;

        /// <summary>
        ///   The _process steps.
        /// </summary>
        private IList<IProcessStepMutableObject> _processSteps;

        /// <summary>
        ///   The _transitions.
        /// </summary>
        private IList<ITransitionMutableObject> _transitions;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///   Initializes a new instance of the <see cref="ProcessStepMutableCore" /> class.
        /// </summary>
        public ProcessStepMutableCore()
            : base(SdmxStructureType.GetFromEnum(SdmxStructureEnumType.ProcessStep))
        {
            this._input = new List<IInputOutputMutableObject>();
            this._output = new List<IInputOutputMutableObject>();
            this._transitions = new List<ITransitionMutableObject>();
            this._processSteps = new List<IProcessStepMutableObject>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProcessStepMutableCore"/> class.
        /// </summary>
        /// <param name="objTarget">
        /// The obj target. 
        /// </param>
        public ProcessStepMutableCore(IProcessStepObject objTarget)
            : base(objTarget)
        {
            this._input = new List<IInputOutputMutableObject>();
            this._output = new List<IInputOutputMutableObject>();
            this._transitions = new List<ITransitionMutableObject>();
            this._processSteps = new List<IProcessStepMutableObject>();

            if (objTarget.Input != null)
            {
                foreach (IInputOutputObject currentIo in objTarget.Input)
                {
                    this._input.Add(new InputOutputMutableCore(currentIo));
                }
            }

            if (objTarget.Output != null)
            {
                foreach (IInputOutputObject currentIo0 in objTarget.Output)
                {
                    this._output.Add(new InputOutputMutableCore(currentIo0));
                }
            }

            // make into mutable objTarget lists
            if (objTarget.ProcessSteps != null)
            {
                foreach (IProcessStepObject processStepObject in objTarget.ProcessSteps)
                {
                    this.AddProcessStep(new ProcessStepMutableCore(processStepObject));
                }
            }

            if (objTarget.Transitions != null)
            {
                foreach (ITransition t in objTarget.Transitions)
                {
                    this.AddTransition(new TransitionMutableCore(t));
                }
            }

            if (objTarget.Computation != null)
            {
                this._computation = new ComputationMutableCore(objTarget.Computation);
            }
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets the computation.
        /// </summary>
        public IComputationMutableObject Computation
        {
            get
            {
                return this._computation;
            }
            set
            {
                this._computation = value;
            }
        }

        /// <summary>
        ///   Gets the input.
        /// </summary>
        public IList<IInputOutputMutableObject> Input
        {
            get
            {
                return this._input;
            }
        }

        /// <summary>
        ///   Gets the output.
        /// </summary>
        public IList<IInputOutputMutableObject> Output
        {
            get
            {
                return this._output;
            }
        }

        /// <summary>
        ///   Gets the process steps.
        /// </summary>
        public IList<IProcessStepMutableObject> ProcessSteps
        {
            get
            {
                return this._processSteps;
            }
        }

        /// <summary>
        ///   Gets the transitions.
        /// </summary>
        public IList<ITransitionMutableObject> Transitions
        {
            get
            {
                return this._transitions;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The add process step.
        /// </summary>
        /// <param name="processStep">
        /// The process step. 
        /// </param>
        public void AddProcessStep(IProcessStepMutableObject processStep)
        {
            this._processSteps.Add(processStep);
        }

        /// <summary>
        /// The add transition.
        /// </summary>
        /// <param name="transition">
        /// The transition. 
        /// </param>
        public void AddTransition(ITransitionMutableObject transition)
        {
            this._transitions.Add(transition);
        }

        #endregion
    }
}