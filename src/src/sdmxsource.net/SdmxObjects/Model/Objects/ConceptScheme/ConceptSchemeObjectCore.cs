// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConceptSchemeBeanImpl.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The concept scheme object core.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.ConceptScheme
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using Org.Sdmxsource.Sdmx.Api.Constants.InterfaceConstant;

    using log4net;

    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Structure;
    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Exception;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.ConceptScheme;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.ConceptScheme;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.ConceptScheme;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Base;
    using Org.Sdmxsource.Util;

    using ConceptType = Org.Sdmx.Resources.SdmxMl.Schemas.V20.structure.ConceptType;
    using TextType = Org.Sdmx.Resources.SdmxMl.Schemas.V20.common.TextType;

    /// <summary>
    ///   The concept scheme object core.
    /// </summary>
    [Serializable]
    public sealed class ConceptSchemeObjectCore :
        ItemSchemeObjectCore<IConceptObject, IConceptSchemeObject, IConceptSchemeMutableObject, IConceptMutableObject>, 
        IConceptSchemeObject
    {
        #region Static Fields

        /// <summary>
        ///   The log.
        /// </summary>
        private static readonly ILog Log = LogManager.GetLogger(typeof(IConceptSchemeObject));
        [NonSerialized()]
        private IDictionary<string, IConceptObject> itemById = new Dictionary<string, IConceptObject>();

        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM ITSELF, CREATES STUB OBJECT //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////    

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM MUTABLE OBJECTS             //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ConceptSchemeObjectCore"/> class.
        /// </summary>
        /// <param name="conceptScheme">
        /// The concept scheme. 
        /// </param>
        /// <exception cref="SdmxSemmanticException"> Throws SdmxSemmanticException.
        /// </exception>
        /// <exception cref="SdmxSemmanticException">
        /// Throws Validate exception.
        /// </exception>
        public ConceptSchemeObjectCore(IConceptSchemeMutableObject conceptScheme)
            : base(conceptScheme)
        {
            Log.Debug("Building IConceptSchemeObject from Mutable Object");
            try
            {
                if (conceptScheme.Items != null)
                {
                    foreach (IConceptMutableObject concept in conceptScheme.Items)
                    {
                        this.AddInternalItem(new ConceptCore(this, concept));
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
            catch (Exception ex)
            {
                throw new SdmxException(ex, ExceptionCode.FailValidation, this);
            }

            if (Log.IsDebugEnabled)
            {
                Log.Debug("IConceptSchemeObject Built " + this);
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM V2.1 SCHEMA                 //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Initializes a new instance of the <see cref="ConceptSchemeObjectCore"/> class.
        /// </summary>
        /// <param name="conceptScheme">
        /// The agencyScheme. 
        /// </param>
        /// <exception cref="SdmxSemmanticException"> Throws SdmxSemmanticException.
        /// </exception>
        /// <exception cref="SdmxSemmanticException">
        /// Throws Validate exception.
        /// </exception>
        public ConceptSchemeObjectCore(ConceptSchemeType conceptScheme)
            : base(conceptScheme, SdmxStructureType.GetFromEnum(SdmxStructureEnumType.ConceptScheme))
        {
            Log.Debug("Building IConceptSchemeObject from 2.1 SDMX");
            try
            {
                foreach (Concept currentItem in conceptScheme.Item)
                {
                    this.AddInternalItem(new ConceptCore(this, currentItem.Content));
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
            catch (Exception ex)
            {
                throw new SdmxException(ex, ExceptionCode.FailValidation, this);
            }

            if (Log.IsDebugEnabled)
            {
                Log.Debug("IConceptSchemeObject Built " + this);
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM V2 SCHEMA                 //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Initializes a new instance of the <see cref="ConceptSchemeObjectCore"/> class.
        /// </summary>
        /// <param name="conceptScheme">
        /// The agencyScheme. 
        /// </param>
        /// <exception cref="SdmxSemmanticException"> Throws SdmxSemmanticException.
        /// </exception>
        /// <exception cref="SdmxSemmanticException">
        /// Throws Validate exception.
        /// </exception>
        public ConceptSchemeObjectCore(Org.Sdmx.Resources.SdmxMl.Schemas.V20.structure.ConceptSchemeType conceptScheme)
            : base(
                conceptScheme, 
                SdmxStructureType.GetFromEnum(SdmxStructureEnumType.ConceptScheme), 
                conceptScheme.validTo, 
                conceptScheme.validFrom, 
                conceptScheme.version, 
                CreateTertiary(conceptScheme.isFinal), 
                conceptScheme.agencyID, 
                conceptScheme.id, 
                conceptScheme.uri, 
                conceptScheme.Name, 
                conceptScheme.Description, 
                CreateTertiary(conceptScheme.isExternalReference), 
                conceptScheme.Annotations)
        {
            Log.Debug("Building IConceptSchemeObject from 2.0 SDMX");
            try
            {
                foreach (ConceptType currentItem in conceptScheme.Concept)
                {
                    this.AddInternalItem(new ConceptCore(this, currentItem));
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
            catch (Exception ex)
            {
                throw new SdmxException(ex, ExceptionCode.FailValidation, this);
            }

            if (Log.IsDebugEnabled)
            {
                Log.Debug("IConceptSchemeObject Built " + this);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConceptSchemeObjectCore"/> class. 
        ///   Default Scheme
        /// </summary>
        /// <param name="concepts">
        /// The concepts. 
        /// </param>
        /// <param name="agencyId">
        /// The agency Id. 
        /// </param>
        public ConceptSchemeObjectCore(IList<ConceptType> concepts, string agencyId)
            : base(
                null, 
                SdmxStructureType.GetFromEnum(SdmxStructureEnumType.ConceptScheme), 
                null, 
                null, 
                ConceptSchemeObject.DefaultSchemeVersion, 
                TertiaryBool.GetFromEnum(TertiaryBoolEnumType.False), 
                agencyId, 
                ConceptSchemeObject.DefaultSchemeId, 
                null, 
                DefaultName, 
                null, 
                TertiaryBool.GetFromEnum(TertiaryBoolEnumType.False), 
                null)
        {
            Log.Debug("Building IConceptSchemeObject from Stand Alone 2.0 Concepts");
            try
            {
                foreach (ConceptType currentItem in concepts)
                {
                    if (!currentItem.agencyID.Equals(this.AgencyId))
                    {
                        var sb = new StringBuilder("Attempting to create Default Concept Scheme from v1.0 List of concepts, and was provided with ");
                        sb.Append("a concept that reference different agency reference ('");
                        sb.Append(currentItem.agencyID);
                        sb.Append("') to the scheme agency ('");
                        sb.Append(this.AgencyId);
                        sb.Append("')");
                        throw new SdmxSemmanticException(ExceptionCode.FailValidation, sb.ToString());
                    }

                    this.AddInternalItem(new ConceptCore(this, currentItem));
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
            catch (Exception ex)
            {
                throw new SdmxException(ex, ExceptionCode.FailValidation, this);
            }

            if (Log.IsDebugEnabled)
            {
                Log.Debug("IConceptSchemeObject Built " + this);
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM V1 SCHEMA                 //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Initializes a new instance of the <see cref="ConceptSchemeObjectCore"/> class. 
        ///   Default Scheme
        /// </summary>
        /// <param name="agencyId">
        /// The agency Id. 
        /// </param>
        /// <param name="concepts">
        /// The concepts. 
        /// </param>
        public ConceptSchemeObjectCore(
            string agencyId, IList<Org.Sdmx.Resources.SdmxMl.Schemas.V10.structure.ConceptType> concepts)
            : base(
                null, 
                SdmxStructureType.GetFromEnum(SdmxStructureEnumType.ConceptScheme), 
                null, 
                null, 
                ConceptSchemeObject.DefaultSchemeVersion, 
                TertiaryBool.GetFromEnum(TertiaryBoolEnumType.False), 
                agencyId, 
                ConceptSchemeObject.DefaultSchemeId, 
                null, 
                DefaultName, 
                null, 
                TertiaryBool.GetFromEnum(TertiaryBoolEnumType.False), 
                null)
        {
            Log.Debug("Building IConceptSchemeObject from 1.0 SDMX");
            try
            {
                foreach (Org.Sdmx.Resources.SdmxMl.Schemas.V10.structure.ConceptType currentItem in concepts)
                {
                    if (!currentItem.agency.Equals(this.AgencyId))
                    {
                        var sb = new StringBuilder("Attempting to create Default Concept Scheme from v1.0 List of concepts, and was provided with ");
                        sb.Append("a concept that reference different agency reference ('");
                        sb.Append("') to the scheme agency ('");
                        sb.Append(this.AgencyId);
                        sb.Append("')");

                        throw new SdmxSemmanticException(ExceptionCode.FailValidation, sb.ToString());
                    }

                    this.AddInternalItem(new ConceptCore(this, currentItem));
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
            catch (Exception ex)
            {
                throw new SdmxException(ex, ExceptionCode.FailValidation, this);
            }

            if (Log.IsDebugEnabled)
            {
                Log.Debug("IConceptSchemeObject Built " + this);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConceptSchemeObjectCore"/> class.
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
        private ConceptSchemeObjectCore(IConceptSchemeObject agencyScheme, Uri actualLocation, bool isServiceUrl)
            : base(agencyScheme, actualLocation, isServiceUrl)
        {
            Log.Debug("Stub IConceptSchemeObject Built");
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets a value indicating whether default scheme.
        /// </summary>
        public bool DefaultScheme
        {
            get
            {
                return this.Id.Equals(ConceptSchemeObject.DefaultSchemeId);
            }
        }

        /// <summary>
        ///   Gets the mutable instance.
        /// </summary>
        public override IConceptSchemeMutableObject MutableInstance
        {
            get
            {
                return new ConceptSchemeMutableCore(this);
            }
        }

        #endregion

        #region Properties

        /// <summary>
        ///   Gets the default name.
        /// </summary>
        private static IList<TextType> DefaultName
        {
            get
            {
                IList<TextType> returnList = new List<TextType>();

                var tt = new TextType { TypedValue = ConceptSchemeObject.DefaultSchemeName };
                returnList.Add(tt);
                return returnList;
            }
        }

        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////DEEP VALIDATION                         //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////
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
                return this.DeepEqualsInternal((IConceptSchemeObject)sdmxObject, includeFinalProperties);
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
        /// The <see cref="IConceptSchemeObject"/> . 
        /// </returns>
        public override IConceptSchemeObject GetStub(Uri actualLocation, bool isServiceUrl)
        {
            return new ConceptSchemeObjectCore(this, actualLocation, isServiceUrl);
        }

        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////VALIDATION                             //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////
        #region Methods

        /// <summary>
        /// The validate id.
        /// </summary>
        /// <param name="startWithIntAllowed">
        /// The start with int allowed. 
        /// </param>
        protected internal override void ValidateId(bool startWithIntAllowed)
        {
            // Not allowed to start with an integer
            base.ValidateId(false);
        }

        /// <summary>
        /// Get item by id.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        public IConceptObject GetItemById(string id)
        {
            if (itemById == null || itemById.Count == 0)
            {
                itemById = new Dictionary<string, IConceptObject>();
                foreach (IConceptObject currentConcept in base.Items)
                {
                    itemById.Add(currentConcept.Id, currentConcept);
                }
            }

            return itemById[id];
        }

        /// <summary>
        /// The i get concept.
        /// </summary>
        /// <param name="concepts">
        /// The concepts. 
        /// </param>
        /// <param name="id">
        /// The id. 
        /// </param>
        /// <returns>
        /// The <see cref="IConceptObject"/> . 
        /// </returns>
        /// <exception cref="SdmxSemmanticException">
        /// Throws Validate exception.
        /// </exception>
        private IConceptObject IGetConcept(IList<IConceptObject> concepts, string id)
        {
            foreach (IConceptObject current in concepts)
            {
                if (current.Id.Equals(id))
                {
                    return current;
                }
            }

            throw new SdmxSemmanticException(ExceptionCode.CannotResolveParent, id);
        }

        /// <summary>
        /// Recurses the map checking the children of each child, if one of the children is the parent code, then an excetpion is thrown
        /// </summary>
        /// <param name="children">Set of children object.
        /// </param>
        /// <param name="parent">
        /// The parent. 
        /// </param>
        /// <param name="parentChildMap">Parent list. </param>
        private void RecurseParentMap(
            ISet<IConceptObject> children, 
            IConceptObject parent, 
            IDictionary<IConceptObject, ISet<IConceptObject>> parentChildMap)
        {
            // If the child is also a parent
            if (children != null)
            {
                if (children.Contains(parent))
                {
                    throw new SdmxSemmanticException(ExceptionCode.ParentRecursiveLoop, parent.Id);
                }

                foreach (IConceptObject currentChild in children)
                {
                    this.RecurseParentMap(parentChildMap[currentChild], parent, parentChildMap);
                }
            }
        }

        /// <summary>
        ///   The validate.
        /// </summary>
        /// <exception cref="SdmxSemmanticException">Throws Validate exception.</exception>
        private void Validate()
        {
            var urns = new HashSet<Uri>();
            if (this.Id.Equals(ConceptSchemeObject.DefaultSchemeId))
            {
                if (!this.Version.Equals(ConceptSchemeObject.DefaultSchemeVersion))
                {
                    throw new SdmxSemmanticException(
                        ExceptionCode.FailValidation,
                        ConceptSchemeObject.DefaultSchemeId + " can only be version " + ConceptSchemeObject.DefaultSchemeVersion);
                }

                if (this.IsFinal.IsTrue)
                {
                    throw new SdmxSemmanticException(
                        ExceptionCode.FailValidation, 
                        ConceptSchemeObject.DefaultSchemeId + " can not be made final");
                }
            }

            if (this.Items != null)
            {
                var parentChildMap = new Dictionary<IConceptObject, ISet<IConceptObject>>();

                foreach (IConceptObject concept in this.Items)
                {
                    if (urns.Contains(concept.Urn))
                    {
                        throw new SdmxSemmanticException(ExceptionCode.DuplicateUrn, concept.Urn);
                    }

                    urns.Add(concept.Urn);
                    try
                    {
                        if (!string.IsNullOrWhiteSpace(concept.ParentConcept))
                        {
                            IConceptObject parent = this.IGetConcept(this.Items, concept.ParentConcept);
                            ISet<IConceptObject> children;
                            if (parentChildMap.ContainsKey(parent))
                            {
                                children = parentChildMap[parent];
                            }
                            else
                            {
                                children = new HashSet<IConceptObject>();
                                parentChildMap.Add(parent, children);
                            }

                            children.Add(concept);
                            ISet<IConceptObject> childSet = null;
                            if (parentChildMap.TryGetValue(concept, out childSet))
                            {
                                // Check that the parent code is not directly or indirectly a child of the code it is parenting
                                this.RecurseParentMap(childSet, parent, parentChildMap);    
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
                }
            }
        }

        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////GETTERS                                 //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////
    }
}