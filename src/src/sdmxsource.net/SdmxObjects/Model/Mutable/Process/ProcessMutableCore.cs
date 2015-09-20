// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProcessMutableBeanImpl.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The process mutable core.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.Process
{
    using System;
    using System.Collections.Generic;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Process;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Process;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.Base;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Process;

    /// <summary>
    ///   The process mutable core.
    /// </summary>
    [Serializable]
    public class ProcessMutableCore : MaintainableMutableCore<IProcessObject>, IProcessMutableObject
    {
        #region Fields

        /// <summary>
        ///   The process steps.
        /// </summary>
        private readonly IList<IProcessStepMutableObject> _processSteps;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///   Initializes a new instance of the <see cref="ProcessMutableCore" /> class.
        /// </summary>
        public ProcessMutableCore()
            : base(SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Process))
        {
            this._processSteps = new List<IProcessStepMutableObject>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProcessMutableCore"/> class.
        /// </summary>
        /// <param name="objTarget">
        /// The obj target. 
        /// </param>
        public ProcessMutableCore(IProcessObject objTarget)
            : base(objTarget)
        {
            this._processSteps = new List<IProcessStepMutableObject>();

            // make into mutable objTarget list
            if (objTarget.ProcessSteps != null)
            {
                foreach (IProcessStepObject processStepObject in objTarget.ProcessSteps)
                {
                    this.AddProcessStep(new ProcessStepMutableCore(processStepObject));
                }
            }
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets the immutable instance.
        /// </summary>
        public override IProcessObject ImmutableInstance
        {
            get
            {
                return new ProcessObjectCore(this);
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

        #endregion
    }
}