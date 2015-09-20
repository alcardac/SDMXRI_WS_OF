// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MetadataStructureDefinitionBeanImpl.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The metadata structure definition object core.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.MetadataStructure
{
    using System;
    using System.Collections.Generic;

    using log4net;

    using Org.Sdmx.Resources.SdmxMl.Schemas.V20.structure;
    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Structure;
    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Exception;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.MetadataStructure;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.MetadataStructure;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.MetadataStructure;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Base;
    using Org.Sdmxsource.Util;

    using ReportStructureType = Org.Sdmx.Resources.SdmxMl.Schemas.V20.structure.ReportStructureType;

    /// <summary>
    ///   The metadata structure definition object core.
    /// </summary>
    [Serializable]
    public class MetadataStructureDefinitionObjectCore :
        MaintainableObjectCore<IMetadataStructureDefinitionObject, IMetadataStructureDefinitionMutableObject>, 
        IMetadataStructureDefinitionObject
    {
        #region Static Fields

        /// <summary>
        ///   The log.
        /// </summary>
        private static readonly ILog Log = LogManager.GetLogger(typeof(MetadataStructureDefinitionObjectCore));

        #endregion

        #region Fields

        /// <summary>
        ///   The metadata target.
        /// </summary>
        private readonly IList<IMetadataTarget> metadataTarget;

        /// <summary>
        ///   The report structures.
        /// </summary>
        private readonly IList<IReportStructure> reportStructures;

        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM ITSELF, CREATES STUB OBJECT //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////    

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM MUTABLE OBJECTS             //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MetadataStructureDefinitionObjectCore"/> class.
        /// </summary>
        /// <param name="itemMutableObject">
        /// The agencyScheme. 
        /// </param>
        /// <exception cref="SdmxSemmanticException"> Throws SdmxSemmanticException.
        /// </exception>
        /// <exception cref="SdmxSemmanticException">
        /// Throws Validate exception.
        /// </exception>
        public MetadataStructureDefinitionObjectCore(IMetadataStructureDefinitionMutableObject itemMutableObject)
            : base(itemMutableObject)
        {
            this.reportStructures = new List<IReportStructure>();
            this.metadataTarget = new List<IMetadataTarget>();
            Log.Debug("Building IMetadataStructureDefinitionObject from Mutable Object");
            try
            {
                if (itemMutableObject.ReportStructures != null)
                {
                    foreach (IReportStructureMutableObject current in itemMutableObject.ReportStructures)
                    {
                        this.reportStructures.Add(new ReportStructureCore(this, current));
                    }
                }

                if (itemMutableObject.MetadataTargets != null)
                {
                    foreach (IMetadataTargetMutableObject m in itemMutableObject.MetadataTargets)
                    {
                        this.metadataTarget.Add(new MetadataTargetCore(this, m));
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
                Log.Debug("IMetadataStructureDefinitionObject Built " + this.Urn);
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM V2.1 SCHEMA                 //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Initializes a new instance of the <see cref="MetadataStructureDefinitionObjectCore"/> class.
        /// </summary>
        /// <param name="metadataStructure">
        /// The agencyScheme. 
        /// </param>
        /// <exception cref="SdmxSemmanticException"> Throws SdmxSemmanticException.
        /// </exception>
        /// <exception cref="SdmxSemmanticException">
        /// Throws Validate exception.
        /// </exception>
        public MetadataStructureDefinitionObjectCore(MetadataStructureType metadataStructure)
            : base(metadataStructure, SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Msd))
        {
            this.reportStructures = new List<IReportStructure>();
            this.metadataTarget = new List<IMetadataTarget>();
            Log.Debug("Building IMetadataStructureDefinitionObject from 2.1 SDMX");
            try
            {
                if (metadataStructure.MetadataStructureComponents != null)
                {
                    if (metadataStructure.MetadataStructureComponents.MetadataTarget != null)
                    {
                        foreach (MetadataTarget currentMetadataTarget in metadataStructure.MetadataStructureComponents.MetadataTarget)
                        {
                            this.metadataTarget.Add(new MetadataTargetCore(currentMetadataTarget.Content, this));
                        }
                    }

                    if (metadataStructure.MetadataStructureComponents.ReportStructure != null)
                    {
                        foreach (ReportStructure currentRs in metadataStructure.MetadataStructureComponents.ReportStructure)
                        {
                            this.reportStructures.Add(new ReportStructureCore(this, currentRs.Content));
                        }
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
                Log.Debug("IMetadataStructureDefinitionObject Built " + this.Urn);
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM V2 SCHEMA                 //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Initializes a new instance of the <see cref="MetadataStructureDefinitionObjectCore"/> class.
        /// </summary>
        /// <param name="metadataStructureDefinition">
        /// The agencyScheme. 
        /// </param>
        /// <exception cref="SdmxSemmanticException"> Throws SdmxSemmanticException.
        /// </exception>
        /// <exception cref="SdmxSemmanticException">
        /// Throws Validate exception.
        /// </exception>
        public MetadataStructureDefinitionObjectCore(MetadataStructureDefinitionType metadataStructureDefinition)
            : base(
                metadataStructureDefinition, 
                SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Msd), 
                metadataStructureDefinition.validTo, 
                metadataStructureDefinition.validFrom, 
                metadataStructureDefinition.version, 
                CreateTertiary(metadataStructureDefinition.isFinal), 
                metadataStructureDefinition.agencyID, 
                metadataStructureDefinition.id, 
                metadataStructureDefinition.uri, 
                metadataStructureDefinition.Name, 
                metadataStructureDefinition.Description, 
                CreateTertiary(metadataStructureDefinition.isExternalReference), 
                metadataStructureDefinition.Annotations)
        {
            this.reportStructures = new List<IReportStructure>();
            this.metadataTarget = new List<IMetadataTarget>();
            Log.Debug("Building IMetadataStructureDefinitionObject from 2.0 SDMX");
            try
            {
                // add any Target Identifier as MetadataTarget
                var targetIdentifiers = metadataStructureDefinition.TargetIdentifiers;
                if (targetIdentifiers != null)
                {
                    IMetadataTarget fullTarget = null;
                    if (targetIdentifiers.FullTargetIdentifier != null)
                    {
                        fullTarget = new MetadataTargetCore(targetIdentifiers.FullTargetIdentifier, this);
                        this.metadataTarget.Add(fullTarget);
                    }

                    if (targetIdentifiers.PartialTargetIdentifier != null)
                    {
                        foreach (var partialTargetIdentifierType in targetIdentifiers.PartialTargetIdentifier)
                        {
                            this.metadataTarget.Add(new MetadataTargetCore(partialTargetIdentifierType,fullTarget,this));
                        }
                    }
                }

                // FUNC 2.1 do we support backward compatability here???
                if (metadataStructureDefinition.ReportStructure != null)
                {
                    foreach (ReportStructureType currentRs in metadataStructureDefinition.ReportStructure)
                    {
                        this.reportStructures.Add(new ReportStructureCore(this, currentRs));
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
            catch (SdmxSemmanticException ex1)
            {
                throw new SdmxSemmanticException(ex1, ExceptionCode.ObjectStructureConstructionError, this.Urn);
            }
            catch (Exception th1)
            {
                throw new SdmxException(th1, ExceptionCode.ObjectStructureConstructionError, this.Urn);
            }

            if (Log.IsDebugEnabled)
            {
                Log.Debug("IMetadataStructureDefinitionObject Built " + this.Urn);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MetadataStructureDefinitionObjectCore"/> class.
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
        private MetadataStructureDefinitionObjectCore(
            IMetadataStructureDefinitionObject agencyScheme, Uri actualLocation, bool isServiceUrl)
            : base(agencyScheme, actualLocation, isServiceUrl)
        {
            this.reportStructures = new List<IReportStructure>();
            this.metadataTarget = new List<IMetadataTarget>();
            Log.Debug("Stub IMetadataStructureDefinitionObject Built");
        }

        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////VALIDATION                             //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////
        #region Public Properties

        /// <summary>
        ///   Gets the metadata targets.
        /// </summary>
        public virtual IList<IMetadataTarget> MetadataTargets
        {
            get
            {
                return new List<IMetadataTarget>(this.metadataTarget);
            }
        }

        /// <summary>
        ///   Gets the mutable instance.
        /// </summary>
        public override IMetadataStructureDefinitionMutableObject MutableInstance
        {
            get
            {
                return new MetadataStructureDefinitionMutableCore(this);
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
        ///   Gets the report structures.
        /// </summary>
        public virtual IList<IReportStructure> ReportStructures
        {
            get
            {
                return new List<IReportStructure>(this.reportStructures);
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
            if (sdmxObject.StructureType == this.StructureType)
            {
                var that = (IMetadataStructureDefinitionObject)sdmxObject;
                if (!this.Equivalent(this.reportStructures, that.ReportStructures, includeFinalProperties))
                {
                    return false;
                }

                if (!this.Equivalent(this.metadataTarget, that.MetadataTargets, includeFinalProperties))
                {
                    return false;
                }

                return this.DeepEqualsInternal(that, includeFinalProperties);
            }

            return false;
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////GETTERS                                 //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////

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
        /// The <see cref="IMetadataStructureDefinitionObject"/> . 
        /// </returns>
        public override IMetadataStructureDefinitionObject GetStub(Uri actualLocation, bool isServiceUrl)
        {
            return new MetadataStructureDefinitionObjectCore(this, actualLocation, isServiceUrl);
        }

        #endregion

        #region Methods

        /// <summary>
        ///   The validate.
        /// </summary>
        /// <exception cref="SdmxSemmanticException">Throws Validate exception.</exception>
        private void Validate()
        {
            if (!this.IsExternalReference.IsTrue)
            {
                if (!ObjectUtil.ValidCollection(this.metadataTarget))
                {
                    throw new SdmxSemmanticException("Metadata CategorisationStructure Definition requires at least one Metadata Target");
                }

                if (!ObjectUtil.ValidCollection(this.reportStructures))
                {
                    throw new SdmxSemmanticException(
                        "Metadata CategorisationStructure Definition requires at least one Report CategorisationStructure");
                }
            }

            ISet<string> metadataTargetIds = new HashSet<string>();

            foreach (IMetadataTarget currentTarget in this.metadataTarget)
            {
                metadataTargetIds.Add(currentTarget.Id);
            }

            foreach (IReportStructure currentReportStructure in this.reportStructures)
            {
                foreach (string currentTarget0 in currentReportStructure.TargetMetadatas)
                {
                    if (!metadataTargetIds.Contains(currentTarget0))
                    {
                        throw new SdmxSemmanticException(
                            "Report CategorisationStructure references undefined metadata target '" + currentTarget0 + "'");
                    }
                }
            }

            // FUNC 2.1 Validation
            // Set<string> identiferComponentId = new HashSet<string>();
            // if(targetIdentifiers != null) {
            // IFullTargetIdentifier Ifti = targetIdentifiers.getFullTargetIdentifier();
            // if(Ifti == null) {
            // throw new SdmxSemmanticException(ExceptionCode.ObjectMissingRequiredElement, this.structureType, "FullTargetIdentifier");
            // }
            // Set<string> identifierComponent = new HashSet<string>();
            // Set<string> identifierComponentUrns = new HashSet<string>();
            // if(Ifti.getIdentifierComponents() != null) {
            // for(IIdentifierComponent Iic : Ifti.getIdentifierComponents()) {
            // if(identifierComponentUrns.contains(Iic.getUrn())) {
            // throw new SdmxSemmanticException(ExceptionCode.DuplicateUrn, Iic.getUrn());
            // }
            // identifierComponentUrns.add(Iic.getUrn());
            // identifierComponent.add(Iic.getId());
            // }
            // }
            // identiferComponentId.add(Ifti.getId());
            // if(targetIdentifiers.getPartialTargetIdentifiers() != null) {
            // Set<string> partialTargetIdentifierUrns = new HashSet<string>();
            // for(IPartialTargetIdentifier Ipti : targetIdentifiers.getPartialTargetIdentifiers()) {
            // if(partialTargetIdentifierUrns.contains(Ipti.getUrn())) {
            // throw new SdmxSemmanticException(ExceptionCode.DuplicateUrn, Ipti.getUrn());
            // }
            // partialTargetIdentifierUrns.add(Ipti.getUrn());
            // if(Ifti.getId().equals(Ipti.getId())) {
            // throw new SdmxSemmanticException(ExceptionCode.PartialTargetIdDuplicatesFullTargetId, Ipti.getId()); 
            // }
            // identiferComponentId.add(Ipti.getId());
            // if(Ipti.getIdentifierComponentRef() != null) {
            // for(string ftiRef : Ipti.getIdentifierComponentRef()) {
            // if(!identifierComponent.contains(ftiRef)) {
            // throw new SdmxSemmanticException(ExceptionCode.ReferenceError, SdmxStructureType.GetFromEnum(SdmxStructureEnumType.FullTargetIdentifier), SdmxStructureType.GetFromEnum(SdmxStructureEnumType.PartialTargetIdentifier), Ipti); 
            // }
            // }
            // }
            // }
            // }
            // }
            // //VALIDATE REPORT STRUCTURES
            // Set<string> conceptIds;
            // Set<string> reportStructureUrns = new HashSet<string>();
            // for(IReportStructure Irs : reportStructures) {
            // if(reportStructureUrns.contains(Irs.getUrn())) {
            // throw new SdmxSemmanticException(ExceptionCode.DuplicateUrn, Irs.getUrn());
            // }
            // reportStructureUrns.add(Irs.getUrn());
            // conceptIds = new HashSet<string>();
            // try {
            // if(!identiferComponentId.contains(Irs.getTarget())) {
            // throw new SdmxSemmanticException(ExceptionCode.ReportStructureInvalidIdentifierReference, Irs.getTarget()); 
            // }
            // if(Irs.getMetadataAttributes() == null || Irs.getMetadataAttributes().size() == 0) {
            // throw new SdmxSemmanticException(ExceptionCode.ObjectMissingRequiredElement, this.getUrn(), "MetadataAttribute");
            // }
            // for(IMetadataAttributeObject Ima : Irs.getMetadataAttributes()) {
            // string conceptId = Ima.getConceptRef().getId();
            // if(conceptIds.contains(conceptId)) {
            // throw new SdmxSemmanticException(ExceptionCode.DuplicateConcept, Ima.getUrn());
            // }
            // conceptIds.add(conceptId);
            // }
            // } catch(ValidationException e) {
            // throw new SdmxSemmanticException(e, ExceptionCode.FAIL_VALIDATION, new Object[] {Irs.getUrn()});
            // }
            // }
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
         base.AddToCompositeSet(this.reportStructures, composites);
         base.AddToCompositeSet(this.metadataTarget, composites);
         return composites;
      }

        #endregion
    }
}