// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TransitionBeanImpl.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The transition core.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Process
{
    using System;
    using System.Collections.Generic;

    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Structure;
    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Exception;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Process;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Util;
    using Org.Sdmxsource.Util;
    using Org.Sdmxsource.Util.Extensions;

    /// <summary>
    ///   The transition core.
    /// </summary>
    [Serializable]
    public class TransitionCore : IdentifiableCore, ITransition
    {
        #region Fields

        /// <summary>
        ///   The condition.
        /// </summary>
        private readonly IList<ITextTypeWrapper> condition;

        /// <summary>
        ///   The local id.
        /// </summary>
        private readonly string localId;

        /// <summary>
        ///   The target step.
        /// </summary>
        private readonly string targetStep;

        /// <summary>
        ///   The process step.
        /// </summary>
        private IProcessStepObject processStep; // Referenced from targetStep

        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM MUTABLE OBJECTS              //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TransitionCore"/> class.
        /// </summary>
        /// <param name="itemMutableObject">
        /// The sdmxObject. 
        /// </param>
        /// <param name="parent">
        /// The parent. 
        /// </param>
        public TransitionCore(ITransitionMutableObject itemMutableObject, ISdmxStructure parent)
            : base(itemMutableObject, parent)
        {
            this.condition = new List<ITextTypeWrapper>();
            this.targetStep = itemMutableObject.TargetStep;
            if (itemMutableObject.Conditions != null)
            {
                foreach (ITextTypeWrapperMutableObject textTypeWrapperMutableObject in itemMutableObject.Conditions)
                {
                    if (!string.IsNullOrWhiteSpace(textTypeWrapperMutableObject.Value))
                    {
                        this.condition.Add(new TextTypeWrapperImpl(textTypeWrapperMutableObject, this));
                    }
                }
            }

            this.localId = itemMutableObject.LocalId;
            this.Validate();
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM V2.1 SCHEMA                 //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Initializes a new instance of the <see cref="TransitionCore"/> class.
        /// </summary>
        /// <param name="transition">
        /// The transition. 
        /// </param>
        /// <param name="parent">
        /// The parent. 
        /// </param>
        public TransitionCore(TransitionType transition, ISdmxStructure parent)
            : base(transition, SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Transition), parent)
        {
            this.condition = new List<ITextTypeWrapper>();
            if (transition.TargetStep != null)
            {
                this.targetStep = RefUtil.CreateLocalIdReference(transition.TargetStep);
            }

            if (transition.Condition != null)
            {
                this.condition = TextTypeUtil.WrapTextTypeV21(transition.Condition, this);
            }

            this.localId = transition.localID;
            this.Validate();
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM V2 SCHEMA                 //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Initializes a new instance of the <see cref="TransitionCore"/> class.
        /// </summary>
        /// <param name="trans">
        /// The trans. 
        /// </param>
        /// <param name="parent">
        /// The parent. 
        /// </param>
        public TransitionCore(
            Org.Sdmx.Resources.SdmxMl.Schemas.V20.structure.TransitionType trans, ISdmxStructure parent)
            : base(GenerateId(), SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Transition), parent)
        {
            this.condition = new List<ITextTypeWrapper>();
            this.targetStep = trans.TargetStep;
            if (trans.Condition != null)
            {
                this.condition.Add(new TextTypeWrapperImpl(trans.Condition, this));
            }
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets the all text types.
        /// </summary>
        public override IList<ITextTypeWrapper> AllTextTypes
        {
            get
            {
                IList<ITextTypeWrapper> returnList = base.AllTextTypes;
                this.condition.AddAll(returnList);
                return returnList;
            }
        }

        /// <summary>
        ///   Gets the condition.
        /// </summary>
        public virtual IList<ITextTypeWrapper> Condition
        {
            get
            {
                return new List<ITextTypeWrapper>(this.condition);
            }
        }

        /// <summary>
        ///   Gets the local id.
        /// </summary>
        public virtual string LocalId
        {
            get
            {
                return this.localId;
            }
        }

        /// <summary>
        ///   Gets the target step.
        /// </summary>
        public virtual IProcessStepObject TargetStep
        {
            get
            {
                if (this.targetStep != null && this.processStep == null)
                {
                    this.VerifyProcessSteps();
                }

                return this.processStep;
            }
        }

        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////DEEP EQUALS                             //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////
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
                var that = (ITransition)sdmxObject;
                if (!this.Equivalent(this.condition, that.Condition, includeFinalProperties))
                {
                    return false;
                }

                if (!ObjectUtil.Equivalent(this.targetStep, that.TargetStep))
                {
                    return false;
                }

                if (!ObjectUtil.Equivalent(this.localId, that.LocalId))
                {
                    return false;
                }

                return this.DeepEqualsInternal(that, includeFinalProperties);
            }

            return false;
        }

        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////VALIDATE                 //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////
        #region Methods

        /// <summary>
        ///   Method to be called after construction of Process, as a Transition may be referencing a Process Step that has not yet been built due to nesting
        /// </summary>
        protected internal void VerifyProcessSteps()
        {
            var parentProcess = (IProcessObject)this.MaintainableParent;
            this.SetTargetStep(
                parentProcess.ProcessSteps, this.targetStep.Split(new[] { "\\." }, StringSplitOptions.None), 0);
        }

        /// <summary>
        ///   The generate id.
        /// </summary>
        /// <returns> The <see cref="string" /> . </returns>
        private static string GenerateId()
        {
            return Guid.NewGuid().ToString();
        }

        /// <summary>
        /// Walks through Processes and sub processes to set the IProcessStepObject referenced from the targetStep
        /// </summary>
        /// <param name="processSteps">List of processes.
        /// </param>
        /// <param name="targetStepSplit">
        /// The target Step Split. 
        /// </param>
        /// <param name="currentPosition">
        /// The current Position. 
        /// </param>
        private void SetTargetStep(IList<IProcessStepObject> processSteps, string[] targetStepSplit, int currentPosition)
        {
            foreach (IProcessStepObject currentProcessStep in processSteps)
            {
                if (currentProcessStep.Id.Equals(targetStepSplit[currentPosition]))
                {
                    int nextPos = currentPosition + 1;
                    if (targetStepSplit.Length > nextPos)
                    {
                        this.SetTargetStep(currentProcessStep.ProcessSteps, targetStepSplit, nextPos);
                        return;
                    }
                    else
                    {
                        this.processStep = currentProcessStep;
                        return;
                    }
                }
            }

            throw new SdmxSemmanticException(
                "Can not resolve reference to ProcessStep with reference '" + this.targetStep + "'");
        }

        /// <summary>
        ///   The validate.
        /// </summary>
        /// <exception cref="SdmxSemmanticException">Throws Validate exception.</exception>
        private void Validate()
        {
            if (string.IsNullOrWhiteSpace(this.targetStep))
            {
                throw new SdmxSemmanticException("Transition is missing mandatory 'Target Step'");
            }
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
        base.AddToCompositeSet(this.condition, composites);
        return composites;
      }

        #endregion
    }
}