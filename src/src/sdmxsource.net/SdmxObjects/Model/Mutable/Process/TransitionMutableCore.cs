// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TransitionMutableBeanImpl.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The transition mutable core.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.Process
{
    using System;
    using System.Collections.Generic;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Process;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.Base;

    /// <summary>
    ///   The transition mutable core.
    /// </summary>
    [Serializable]
    public class TransitionMutableCore : IdentifiableMutableCore, ITransitionMutableObject
    {
        #region Fields

        /// <summary>
        ///   The condition.
        /// </summary>
        private readonly IList<ITextTypeWrapperMutableObject> _conditions;

        /// <summary>
        ///   The local id.
        /// </summary>
        private string _localId;

        /// <summary>
        ///   The target step.
        /// </summary>
        private string _targetStep;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///   Initializes a new instance of the <see cref="TransitionMutableCore" /> class.
        /// </summary>
        public TransitionMutableCore()
            : base(SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Transition))
        {
            this._conditions = new List<ITextTypeWrapperMutableObject>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TransitionMutableCore"/> class.
        /// </summary>
        /// <param name="transitionType">
        /// The transition type. 
        /// </param>
        public TransitionMutableCore(ITransition transitionType)
            : base(transitionType)
        {
            this._conditions = new List<ITextTypeWrapperMutableObject>();
            this._targetStep = transitionType.TargetStep.Id;
            if (transitionType.Condition != null)
            {
                foreach (ITextTypeWrapper textTypeWrapper in transitionType.Condition)
                {
                    this._conditions.Add(new TextTypeWrapperMutableCore(textTypeWrapper));
                }
            }

            this._localId = transitionType.LocalId;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets the condition.
        /// </summary>
        public virtual IList<ITextTypeWrapperMutableObject> Conditions
        {
            get
            {
                return this._conditions;
            }
        }

        /// <summary>
        ///   Gets or sets the local id.
        /// </summary>
        public virtual string LocalId
        {
            get
            {
                return this._localId;
            }

            set
            {
                this._localId = value;
            }
        }

        /// <summary>
        ///   Gets or sets the target step.
        /// </summary>
        public virtual string TargetStep
        {
            get
            {
                return this._targetStep;
            }

            set
            {
                this._targetStep = value;
            }
        }

        #endregion
    }
}