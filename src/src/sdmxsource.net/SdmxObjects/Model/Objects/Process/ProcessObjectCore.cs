// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProcessBeanImpl.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The process object core.
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
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Process;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Process;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.Process;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Base;

    /// <summary>
    ///   The process object core.
    /// </summary>
    [Serializable]
    public class ProcessObjectCore : MaintainableObjectCore<IProcessObject, IProcessMutableObject>, IProcessObject
    {
        #region Static Fields

        /// <summary>
        ///   The log.
        /// </summary>
        private static readonly ILog Log = LogManager.GetLogger(typeof(ProcessObjectCore));

        #endregion

        #region Fields

        /// <summary>
        ///   The _process steps.
        /// </summary>
        private readonly IList<IProcessStepObject> _processSteps;

        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM ITSELF, CREATES STUB OBJECT //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////    

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM MUTABLE OBJECTS             //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ProcessObjectCore"/> class.
        /// </summary>
        /// <param name="processMutableObject">
        /// The iprocess. 
        /// </param>
        /// <exception cref="SdmxSemmanticException"> Throws SdmxSemmanticException.
        /// </exception>
        /// <exception cref="SdmxSemmanticException">
        /// Throws Validate exception.
        /// </exception>
        public ProcessObjectCore(IProcessMutableObject processMutableObject)
            : base(processMutableObject)
        {
            this._processSteps = new List<IProcessStepObject>();
            Log.Debug("Building IProcessObject from Mutable Object");
            try
            {
                if (processMutableObject.ProcessSteps != null)
                {
                    foreach (IProcessStepMutableObject processStep in processMutableObject.ProcessSteps)
                    {
                        this._processSteps.Add(new ProcessStepCore(this, processStep));
                    }
                }
            }
            catch (Exception th)
            {
                throw new SdmxSemmanticException(th, ExceptionCode.ObjectStructureConstructionError, this);
            }

            try
            {
                this.Validate();
            }
            catch (SdmxSemmanticException e)
            {
                throw new SdmxSemmanticException(e, ExceptionCode.FailValidation, this);
            }

            if (Log.IsDebugEnabled)
            {
                Log.Debug("IProcessObject Built " + this.Urn);
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM V2.1 SCHEMA                 //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Initializes a new instance of the <see cref="ProcessObjectCore"/> class.
        /// </summary>
        /// <param name="process">
        /// The process. 
        /// </param>
        /// <exception cref="SdmxSemmanticException"> Throws SdmxSemmanticException.
        /// </exception>
        /// <exception cref="SdmxSemmanticException">
        /// Throws Validate exception.
        /// </exception>
        public ProcessObjectCore(ProcessType process)
            : base(process, SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Process))
        {
            this._processSteps = new List<IProcessStepObject>();
            Log.Debug("Building IProcessObject from 2.1 SDMX");
            try
            {
                if (process.ProcessStep != null)
                {
                    foreach (ProcessStepType processStep in process.ProcessStep)
                    {
                        this._processSteps.Add(new ProcessStepCore(this, processStep));
                    }
                }
            }
            catch (Exception th)
            {
                throw new SdmxSemmanticException(th, ExceptionCode.ObjectStructureConstructionError, this);
            }

            try
            {
                this.Validate();
            }
            catch (SdmxSemmanticException e)
            {
                throw new SdmxSemmanticException(e, ExceptionCode.FailValidation, this);
            }

            if (Log.IsDebugEnabled)
            {
                Log.Debug("IProcessObject Built " + this.Urn);
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM V2 SCHEMA                 //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Initializes a new instance of the <see cref="ProcessObjectCore"/> class.
        /// </summary>
        /// <param name="process">
        /// The agencyScheme. 
        /// </param>
        /// <exception cref="SdmxSemmanticException"> Throws SdmxSemmanticException.
        /// </exception>
        /// <exception cref="SdmxSemmanticException">
        /// Throws Validate exception.
        /// </exception>
        public ProcessObjectCore(Org.Sdmx.Resources.SdmxMl.Schemas.V20.structure.ProcessType process)
            : base(
                process, 
                SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Process), 
                process.validTo, 
                process.validFrom, 
                process.version, 
                CreateTertiary(process.isFinal), 
                process.agencyID, 
                process.id, 
                process.uri, 
                process.Name, 
                process.Description, 
                CreateTertiary(process.isExternalReference), 
                process.Annotations)
        {
            this._processSteps = new List<IProcessStepObject>();
            Log.Debug("Building IProcessObject from 2.0 SDMX");
            try
            {
                if (process.ProcessStep != null)
                {
                    foreach (Org.Sdmx.Resources.SdmxMl.Schemas.V20.structure.ProcessStepType processStep in
                        process.ProcessStep)
                    {
                        this._processSteps.Add(new ProcessStepCore(this, processStep));
                    }
                }
            }
            catch (SdmxSemmanticException ex)
            {
                throw new SdmxSemmanticException(ex, ExceptionCode.ObjectStructureConstructionError, this.Urn);
            }
            catch (Exception th)
            {
                throw new SdmxException(th, ExceptionCode.ObjectStructureConstructionError, this.Urn);
            }

            try
            {
                this.Validate();
            }
            catch (SdmxSemmanticException e)
            {
                throw new SdmxSemmanticException(e, ExceptionCode.FailValidation, this);
            }

            if (Log.IsDebugEnabled)
            {
                Log.Debug("IProcessObject Built " + this.Urn);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProcessObjectCore"/> class.
        /// </summary>
        /// <param name="agencyScheme">
        /// The agencyScheme. 
        /// </param>
        /// <param name="actualLocation">
        /// The actual location. 
        /// </param>
        /// <param name="isServiceUrl">
        /// The is service url. 
        /// </param>
        private ProcessObjectCore(IProcessObject agencyScheme, Uri actualLocation, bool isServiceUrl)
            : base(agencyScheme, actualLocation, isServiceUrl)
        {
            this._processSteps = new List<IProcessStepObject>();
            Log.Debug("Stub IProcessObject Built");
        }

        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////DEEP EQUALS                             //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////
        #region Public Properties

        /// <summary>
        ///   Gets the mutable instance.
        /// </summary>
        public override IProcessMutableObject MutableInstance
        {
            get
            {
                return new ProcessMutableCore(this);
            }
        }

        /// <summary>
        /// Gets the Urn
        /// </summary>
        public sealed override Uri Urn
        {
            get
            {
                return base.Urn;
            }
        }

        /// <summary>
        ///   Gets the process steps.
        /// </summary>
        public virtual IList<IProcessStepObject> ProcessSteps
        {
            get
            {
                return new List<IProcessStepObject>(this._processSteps);
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The deep equals.
        /// </summary>
        /// <param name="sdmxObject">
        /// The agencyScheme. 
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
                var that = (IProcessObject)sdmxObject;
                if (!this.Equivalent(this._processSteps, that.ProcessSteps, includeFinalProperties))
                {
                    return false;
                }

                return this.DeepEqualsInternal(that, includeFinalProperties);
            }

            return false;
        }

        /// <summary>
        /// The get stub.
        /// </summary>
        /// <param name="actualLocation">
        /// The actual location. 
        /// </param>
        /// <param name="isServiceUrl">
        /// The is service url. 
        /// </param>
        /// <returns>
        /// The <see cref="IProcessObject"/> . 
        /// </returns>
        public override IProcessObject GetStub(Uri actualLocation, bool isServiceUrl)
        {
            return new ProcessObjectCore(this, actualLocation, isServiceUrl);
        }

        #endregion

        #region Methods

        /// <summary>
        /// The populate process step sets.
        /// </summary>
        /// <param name="processSteps0">
        /// The process steps 0. 
        /// </param>
        /// <param name="processStepIds">
        /// The process step ids. 
        /// </param>
        /// <param name="processStepReferences">
        /// The process step references. 
        /// </param>
        private static void PopulateProcessStepSets(
            IList<IProcessStepObject> processSteps0, ISet<string> processStepIds, ISet<string> processStepReferences)
        {
            var stack = new Stack<IList<IProcessStepObject>>();
            stack.Push(processSteps0);
            while (stack.Count > 0)
            {
                processSteps0 = stack.Pop();
                foreach (IProcessStepObject processStep in processSteps0)
                {
                    foreach (ITransition transition in processStep.Transitions)
                    {
                        processStepReferences.Add(transition.TargetStep.Id);
                    }

                    processStepIds.Add(processStep.GetFullIdPath(false));
                    stack.Push(processStep.ProcessSteps);
                }
            }
        }

        /// <summary>
        ///   The validate.
        /// </summary>
        /// <exception cref="SdmxSemmanticException">Throws Validate exception.</exception>
        private void Validate()
        {
            ISet<string> processStepReferences = new HashSet<string>(); // Process Step References
            ISet<string> processStepIds = new HashSet<string>(); // Process Step Ids

            PopulateProcessStepSets(this._processSteps, processStepIds, processStepReferences);
            if (!processStepIds.IsSupersetOf(processStepReferences))
            {
                foreach (string currentReference in processStepReferences)
                {
                    if (!processStepIds.Contains(currentReference))
                    {
                        throw new SdmxSemmanticException(
                            "Transition references non existent process step '" + currentReference + "'");
                    }
                }
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
          base.AddToCompositeSet(this._processSteps, composites);
          return composites;
       }

        #endregion
    }
}