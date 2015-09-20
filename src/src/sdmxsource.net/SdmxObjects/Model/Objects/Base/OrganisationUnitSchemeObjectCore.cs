// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OrganisationUnitSchemeBeanImpl.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The organisation unit scheme object core.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Base
{
    using System;
    using System.Collections.Generic;

    using Org.Sdmxsource.Sdmx.Api.Exception;

    using log4net;

    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Structure;
    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.Base;
    using Org.Sdmxsource.Util;

    /// <summary>
    ///   The organisation unit scheme object core.
    /// </summary>
    [Serializable]
    public class OrganisationUnitSchemeObjectCore :
        ItemSchemeObjectCore<IOrganisationUnit, IOrganisationUnitSchemeObject, IOrganisationUnitSchemeMutableObject, 
            IOrganisationUnitMutableObject>, 
        IOrganisationUnitSchemeObject
    {
        #region Static Fields

        /// <summary>
        ///   The log.
        /// </summary>
        private static readonly ILog Log = LogManager.GetLogger(typeof(OrganisationUnitSchemeObjectCore));

        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM ITSELF, CREATES STUB OBJECT //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////    
        /// <summary>
        /// Initializes a new instance of the <see cref="OrganisationUnitSchemeObjectCore"/> class.
        /// </summary>
        /// <param name="agencyScheme">
        /// The sdmxObject. 
        /// </param>
        /// <param name="actualLocation">
        /// The actual location. 
        /// </param>
        /// <param name="isServiceUrl">
        /// The is service url. 
        /// </param>
        private OrganisationUnitSchemeObjectCore(
            IOrganisationUnitSchemeObject agencyScheme, Uri actualLocation, bool isServiceUrl)
            : base(agencyScheme, actualLocation, isServiceUrl)
        {
            Log.Debug("Stub IOrganisationUnitSchemeObject Built");
            try
            {
                Validate();
            }
            catch (SdmxSemmanticException e)
            {
                throw new SdmxSemmanticException(e, ExceptionCode.FailValidation, this);
            }
        }
        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM MUTABLE OBJECTS             //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="OrganisationUnitSchemeObjectCore"/> class.
        /// </summary>
        /// <param name="itemMutableObject">
        /// The sdmxObject. 
        /// </param>
        public OrganisationUnitSchemeObjectCore(IOrganisationUnitSchemeMutableObject itemMutableObject)
            : base(itemMutableObject)
        {
            Log.Debug("Building IOrganisationUnitSchemeObject from Mutable Object");
            if (itemMutableObject.Items != null)
            {
                foreach (IOrganisationUnitMutableObject oumb in itemMutableObject.Items)
                {
                    this.AddInternalItem(new OrganisationUnitCore(oumb, this));
                }
            }

            if (Log.IsDebugEnabled)
            {
                Log.Debug("IOrganisationUnitSchemeObject Built " + base.Urn);
            }
            try
            {
                Validate();
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
        /// Initializes a new instance of the <see cref="OrganisationUnitSchemeObjectCore"/> class.
        /// </summary>
        /// <param name="type">
        /// The type. 
        /// </param>
        public OrganisationUnitSchemeObjectCore(OrganisationUnitSchemeType type)
            : base(type, SdmxStructureType.GetFromEnum(SdmxStructureEnumType.OrganisationUnitScheme))
        {
            Log.Debug("Building IOrganisationUnitSchemeObject from 2.1 SDMX");
            if (ObjectUtil.ValidCollection(type.Organisation))
            {
                foreach (OrganisationUnit currentType in type.Organisation)
                {
                    this.AddInternalItem(new OrganisationUnitCore(currentType.Content, this));
                }
            }

            if (Log.IsDebugEnabled)
            {
                Log.Debug("IOrganisationUnitSchemeObject Built " + this.Urn);
            }
            try
            {
                Validate();
            }
            catch (SdmxSemmanticException e)
            {
                throw new SdmxSemmanticException(e, ExceptionCode.FailValidation, this);
            }
        }

        #endregion

        #region Public Properties

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
        ///   Gets the mutable instance.
        /// </summary>
        public override IOrganisationUnitSchemeMutableObject MutableInstance
        {
            get
            {
                return new OrganisationUnitSchemeMutableCore(this);
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
        /// The sdmxObject. 
        /// </param>
        /// <param name="includeFinalProperties"> </param>
        /// <returns>
        /// The <see cref="bool"/> . 
        /// </returns>
        public override bool DeepEquals(ISdmxObject sdmxObject, bool includeFinalProperties)
        {
            if(sdmxObject == null) return false;
            if (sdmxObject.StructureType == this.StructureType)
            {
                return this.DeepEqualsInternal((IOrganisationUnitSchemeObject)sdmxObject, includeFinalProperties);
            }

            return false;
        }


        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////VALIDATION							 //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////
        private void Validate()
        {
            //CHECK FOR DUPLICATION OF URN & ILLEGAL PARENTING
            IDictionary<IOrganisationUnit, ISet<IOrganisationUnit>> parentChildMap = new Dictionary<IOrganisationUnit, ISet<IOrganisationUnit>>();
            /* foreach */
            foreach (IOrganisationUnit currentUnit in this.Items)
            {
                if (ObjectUtil.ValidString(currentUnit.ParentUnit))
                {
                    IOrganisationUnit parentUnit = GetUnit(currentUnit.ParentUnit);

                    ISet<IOrganisationUnit> children;
                    if (parentChildMap.ContainsKey(parentUnit))
                    {
                        children = parentChildMap[parentUnit];
                    }
                    else
                    {
                        children = new HashSet<IOrganisationUnit>();
                        parentChildMap.Add(parentUnit, children);
                    }
                    children.Add(currentUnit);
                    //Check that the parent code is not directly or indirectly a child of the code it is parenting

                    ISet<IOrganisationUnit> currentChildren;
                    if (parentChildMap.TryGetValue(currentUnit, out currentChildren))
                    {
                        RecurseParentMap(currentChildren, parentUnit, parentChildMap);
                    }
 
                }
            }
        }

        	private IOrganisationUnit GetUnit(string id) {
		foreach(IOrganisationUnit organisationUnitBean in this.Items) {
			if(organisationUnitBean.Id.Equals(id)) {
				return organisationUnitBean;
			}
		}
        throw new SdmxSemmanticException(ExceptionCode.CannotResolveParent, id);
	}

        private void RecurseParentMap(ISet<IOrganisationUnit> children, IOrganisationUnit parentBean, IDictionary<IOrganisationUnit, ISet<IOrganisationUnit>> parentChildMap) {
		//If the child is also a parent
		if(children != null) {
			if(children.Contains(parentBean)) {
                throw new SdmxSemmanticException(ExceptionCode.ParentRecursiveLoop, parentBean.Id);
			}
			foreach(IOrganisationUnit currentChild in children) {
                ISet<IOrganisationUnit> currentChildren;
			    if (parentChildMap.TryGetValue(currentChild, out currentChildren))
			    {
                    RecurseParentMap(currentChildren, parentBean, parentChildMap);
			    }
			}
		}
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
        /// The <see cref="IOrganisationUnitSchemeObject"/> . 
        /// </returns>
        public override IOrganisationUnitSchemeObject GetStub(Uri actualLocation, bool isServiceUrl)
        {
            return new OrganisationUnitSchemeObjectCore(this, actualLocation, isServiceUrl);
        }

        #endregion
    }
}