// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ObjectBaseImpl.cs" company="Eurostat">
//   Date Created : 2013-08-19
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


using Org.Sdmxsource.Sdmx.Api.Exception;

namespace Org.Sdmxsource.Sdmx.Util.Objects.Container
{
    #region Using directives

    using System.Collections.Generic;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.BaseObjects;
    using Org.Sdmxsource.Sdmx.Api.Model.BaseObjects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.BaseObjects.CategoryScheme;
    using Org.Sdmxsource.Sdmx.Api.Model.BaseObjects.Codelist;
    using Org.Sdmxsource.Sdmx.Api.Model.BaseObjects.ConceptScheme;
    using Org.Sdmxsource.Sdmx.Api.Model.BaseObjects.DataStructure;
    using Org.Sdmxsource.Sdmx.Api.Model.BaseObjects.Process;
    using Org.Sdmxsource.Sdmx.Api.Model.BaseObjects.Registry;

    #endregion

    /// <summary>
    /// TODO
    /// </summary>
    public class ObjectBaseImpl : IObjectsBase
    {
        #region Fields

        /// <summary>
        /// The _categorySchemes
        /// </summary>
        private readonly ISet<ICategorySchemeObjectBase> _categorySchemes = new HashSet<ICategorySchemeObjectBase>();

	    /// <summary>
        /// The _codelists
        /// </summary>
        private readonly ISet<ICodelistObjectBase> _codelists = new HashSet<ICodelistObjectBase>();

	    /// <summary>
        /// The _conceptSchemes
        /// </summary>
        private readonly ISet<IConceptSchemeObjectBase> _conceptSchemes = new HashSet<IConceptSchemeObjectBase>();

	    /// <summary>
        /// The _dataflows
        /// </summary>
        private readonly ISet<IDataflowObjectBase> _dataflows = new HashSet<IDataflowObjectBase>();

	    /// <summary>
        /// The _hcls
        /// </summary>
        private readonly ISet<IHierarchicalCodelistObjectBase> _hcls = new HashSet<IHierarchicalCodelistObjectBase>();

	    /// <summary>
        /// The _dataStructures
        /// </summary>
        private readonly ISet<IDataStructureObjectBase> _dataStructures = new HashSet<IDataStructureObjectBase>();

	    /// <summary>
        /// The _provisionAgreement
        /// </summary>
        private readonly ISet<IProvisionAgreementObjectBase> _provisionAgreement = new HashSet<IProvisionAgreementObjectBase>();

	    /// <summary>
        /// The _processes
        /// </summary>
        private readonly ISet<IProcessObjectBase> _processes = new HashSet<IProcessObjectBase>();

	    /// <summary>
        /// The _registrations
        /// </summary>
        private readonly ISet<IRegistrationObjectBase> _registrations = new HashSet<IRegistrationObjectBase>();

        #endregion


        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectBaseImpl" /> class.
        /// </summary>
	    public ObjectBaseImpl() {}
	
        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectBaseImpl" /> class.
        /// </summary>
        /// <param name="maintainables">
        /// The maintainables.
        /// </param>
	    public ObjectBaseImpl(IEnumerable<IMaintainableObjectBase> maintainables) 
        {

		    if(maintainables != null) 
            {
			    foreach(IMaintainableObjectBase currentMaintainable in maintainables) 
                {
				    if(currentMaintainable is ICategorySchemeObjectBase) 
                    {
					    this._categorySchemes.Add((ICategorySchemeObjectBase)currentMaintainable);
				    } 
                    else if(currentMaintainable is ICodelistObjectBase) 
                    {
					    this._codelists.Add((ICodelistObjectBase)currentMaintainable);
				    } 
                    else if(currentMaintainable is IConceptSchemeObjectBase) 
                    {
					    this._conceptSchemes.Add((IConceptSchemeObjectBase)currentMaintainable);
				    } 
                    else if(currentMaintainable is IDataflowObjectBase) 
                    {
					    this._dataflows.Add((IDataflowObjectBase)currentMaintainable);
				    } 
                    else if(currentMaintainable is IHierarchicalCodelistObjectBase) 
                    {
					    this._hcls.Add((IHierarchicalCodelistObjectBase)currentMaintainable);
				    } 
                    else if(currentMaintainable is IDataStructureObjectBase) 
                    {
					    this._dataStructures.Add((IDataStructureObjectBase)currentMaintainable);
				    } 
                    else if(currentMaintainable.BuiltFrom.StructureType == SdmxStructureType.GetFromEnum(SdmxStructureEnumType.ProvisionAgreement)) 
                    {
					    this._provisionAgreement.Add((IProvisionAgreementObjectBase)currentMaintainable);
				    } 
                    else if(currentMaintainable is IProcessObjectBase) 
                    {
					    this._processes.Add((IProcessObjectBase)currentMaintainable);
				    } 
                    else if(currentMaintainable is IRegistrationObjectBase) 
                    {
					    this._registrations.Add((IRegistrationObjectBase)currentMaintainable);
				    } 
			    }
		    }
	    }
	    
	    /// <summary>
	    /// Initializes a new instance of the <see cref="ObjectBaseImpl" /> class.
	    /// </summary>
	    /// <param name="objectsBase">
	    /// The objects base
	    /// </param>
	    public ObjectBaseImpl(params IObjectsBase[] objectsBase) 
        {
		    foreach(IObjectsBase currentObjectBase in objectsBase) 
            {
			    this._categorySchemes.UnionWith(currentObjectBase.CategorySchemes);
			    this._codelists.UnionWith(currentObjectBase.Codelists);
			    this._conceptSchemes.UnionWith(currentObjectBase.ConceptSchemes);
			    this._dataflows.UnionWith(currentObjectBase.Dataflows);
			    this._hcls.UnionWith(currentObjectBase.HierarchicalCodelists);
			    this._dataStructures.UnionWith(currentObjectBase.DataStructures);
			    this._provisionAgreement.UnionWith(currentObjectBase.Provisions);
			    this._processes.UnionWith(currentObjectBase.Processes);
			    this._registrations.UnionWith(currentObjectBase.Registartions);
		    }		
	    }
	
        #endregion


        #region Public Properties

        /// <summary>
        ///     Gets a <b>copy</b> of the IMaintainableObjectBase Set.  Gets an empty set if no MaintainableBaseObjects are stored in this container
        /// </summary>
        /// <value> </value>
	    public ISet<IMaintainableObjectBase> AllMaintainables
        {
            get
            {
		        ISet<IMaintainableObjectBase> returnSet = new HashSet<IMaintainableObjectBase>();

		        returnSet.UnionWith(this._categorySchemes);
		        returnSet.UnionWith(this._codelists);
		        returnSet.UnionWith(this._conceptSchemes);
		        returnSet.UnionWith(this._dataflows);
		        returnSet.UnionWith(this._hcls);
		        returnSet.UnionWith(this._dataStructures);
		        returnSet.UnionWith(this._provisionAgreement);
		        returnSet.UnionWith(this._processes);
		        returnSet.UnionWith(this._registrations);

		        return returnSet;
	        }
        }

        /// <summary>
        ///     Gets a <b>copy</b> of the ICategorySchemeObjectBase Set.  Gets an empty set if no CategorySchemeBaseObjects are stored in this container
        /// </summary>
        /// <value> </value>
	    public ISet<ICategorySchemeObjectBase> CategorySchemes 
        {
            get
            {
		        return new HashSet<ICategorySchemeObjectBase>(this._categorySchemes);
            }
	    }

	    /// <summary>
        ///     Gets a <b>copy</b> of the ICodelistObjectBase Set.  Gets an empty set if no CodelistBaseObjects are stored in this container
        /// </summary>
        /// <value> </value>
	    public ISet<ICodelistObjectBase> Codelists 
        {
		    get
            {
		        return new HashSet<ICodelistObjectBase>(this._codelists);
            }
	    }

	    /// <summary>
        ///     Gets a <b>copy</b> of the IConceptSchemeObjectBase Set.  Gets an empty set if no ConceptSchemeBaseObjects are stored in this container
        /// </summary>
        /// <value> </value>
	    public ISet<IConceptSchemeObjectBase> ConceptSchemes 
        {
		    get
            {
		        return new HashSet<IConceptSchemeObjectBase>(this._conceptSchemes);
            }
	    }

        /// <summary>
        ///     Gets a <b>copy</b> of the KeyFamilyBaseObjects Set.  Gets an empty set if no KeyFamilyBaseObjects are stored in this container
        /// </summary>
        /// <value> </value>
	    public ISet<IDataStructureObjectBase> DataStructures
        {
		    get
            {
		        return new HashSet<IDataStructureObjectBase>(this._dataStructures);
            }
	    }

	    /// <summary>
        ///     Gets a <b>copy</b> of the IDataflowObjectBase Set.  Gets an empty set if no DataflowBaseObjects are stored in this container
        /// </summary>
        /// <value> </value>
	    public ISet<IDataflowObjectBase> Dataflows 
        {
		    get
            {
		        return new HashSet<IDataflowObjectBase>(this._dataflows);
            }
	    }

	    /// <summary>
        ///     Gets a <b>copy</b> of the IHierarchicalCodelistObjectBase Set.  Gets an empty set if no HierarchicalCodelistBaseObjects are stored in this container
        /// </summary>
        /// <value> </value>
	    public ISet<IHierarchicalCodelistObjectBase> HierarchicalCodelists
        {
		    get
            {
		        return new HashSet<IHierarchicalCodelistObjectBase>(this._hcls);
            }
	    }
	
	    /// <summary>
        ///     Gets a <b>copy</b> of the IProcessObjectBase Set.  Gets an empty set if no ProcessBaseObjects are stored in this container
        /// </summary>
        /// <value> </value>
	    public ISet<IProcessObjectBase> Processes
        {
		    get
            {
		        return new HashSet<IProcessObjectBase>(this._processes);
            }
	    }

        /// <summary>
        ///     Gets a <b>copy</b> of the IProvisionAgreementObjectBase Set.  Gets an empty set if no ProvisionAgreementBaseObjects are stored in this container
        /// </summary>
        /// <value> </value>
	    public ISet<IProvisionAgreementObjectBase> Provisions 
        {
		    get
            {
		        return new HashSet<IProvisionAgreementObjectBase>(this._provisionAgreement);
            }
	    }

	    /// <summary>
        ///     Gets a <b>copy</b> of the IRegistrationObjectBase Set.  Gets an empty set if no RegistrationBaseObjects are stored in this container
        /// </summary>
        /// <value> </value>
	    public ISet<IRegistrationObjectBase> Registartions 
        {
		    get
            {
		        return new HashSet<IRegistrationObjectBase>(this._registrations);
            }
	    }

        #endregion


        #region Public Methods and Operators

        /// <summary>
        /// Add a ICategorySchemeObjectBase to this container, overwrite if one already exists with the same URN
        /// </summary>
        /// <param name="categorySchemeObject">The object. </param>
	    public void AddCategoryScheme(ICategorySchemeObjectBase categorySchemeObject)
        {
		    if(categorySchemeObject != null)
            {
			    this._categorySchemes.Remove(categorySchemeObject);
			    this._categorySchemes.Add(categorySchemeObject);
		    }
	    }

        /// <summary>
        /// Add a ICodelistObjectBase to this container, overwrite if one already exists with the same URN
        /// </summary>
        /// <param name="codelistObject">Codelist object. </param>
	    public void AddCodelist(ICodelistObjectBase codelistObject)
        {
		    if(codelistObject != null)
            {
			    this._codelists.Remove(codelistObject);
			    this._codelists.Add(codelistObject);
		    }
	    }

	    /// <summary>
        /// Add a IConceptSchemeObjectBase to this container, overwrite if one already exists with the same URN
        /// </summary>
        /// <param name="conceptSchemeObject"> ConceptScheme object </param>
	    public void AddConceptScheme(IConceptSchemeObjectBase conceptSchemeObject)
        {
		    if(conceptSchemeObject != null)
            {
			    this._conceptSchemes.Remove(conceptSchemeObject);
			    this._conceptSchemes.Add(conceptSchemeObject);
		    }
	    }

        /// <summary>
        /// Add a IDataStructureObjectBase to this container, overwrite if one already exists with the same URN
        /// </summary>
        /// <param name="dataStructureObject">Datastructure object. </param>
	    public void AddDataStructure(IDataStructureObjectBase dataStructureObject)
        {
		    if(dataStructureObject != null)
            {
			    this._dataStructures.Remove(dataStructureObject);
			    this._dataStructures.Add(dataStructureObject);
		    }
	    }

	    /// <summary>
        /// Add a IDataflowObjectBase to this container, overwrite if one already exists with the same URN
        /// </summary>
        /// <param name="dataflowObject">Dataflow Object </param>
	    public void AddDataflow(IDataflowObjectBase dataflowObject)
        {
		    if(dataflowObject != null)
            {
			    this._dataflows.Remove(dataflowObject);
			    this._dataflows.Add(dataflowObject);
		    }
	    }

	    /// <summary>
        /// Add a IHierarchicalCodelistObjectBase to this container, overwrite if one already exists with the same URN
        /// </summary>
        /// <param name="hierarchicalCodelistObject"> HierarchicalCodelist Object </param>
	    public void AddHierarchicalCodelist(IHierarchicalCodelistObjectBase hierarchicalCodelistObject)
        {
		    if(hierarchicalCodelistObject != null)
            {
			    this._hcls.Remove(hierarchicalCodelistObject);
			    this._hcls.Add(hierarchicalCodelistObject);
		    }
	    }
	
        /// <summary>
        /// The add maintainable.
        /// </summary>
        /// <param name="maintainableObject">
        /// The categorySchemeObject.
        /// </param>
	    public void AddMaintainable(IMaintainableObjectBase maintainableObject)
        {
		    switch(maintainableObject.BuiltFrom.StructureType.EnumType) 
            {
		        case SdmxStructureEnumType.CategoryScheme : 
                    AddCategoryScheme((ICategorySchemeObjectBase)maintainableObject);
		            break;

		        case SdmxStructureEnumType.CodeList : 
                    AddCodelist((ICodelistObjectBase)maintainableObject);
		            break;

		        case SdmxStructureEnumType.ConceptScheme : 
                    AddConceptScheme((IConceptSchemeObjectBase)maintainableObject);
		            break;

		        case SdmxStructureEnumType.Dataflow : 
                    AddDataflow((IDataflowObjectBase)maintainableObject);
		            break;

		        case SdmxStructureEnumType.HierarchicalCodelist : 
                    AddHierarchicalCodelist((IHierarchicalCodelistObjectBase)maintainableObject);
		            break;

		        case SdmxStructureEnumType.Dsd : 
                    AddDataStructure((IDataStructureObjectBase)maintainableObject);
		            break;

		        case SdmxStructureEnumType.Process : 
                    AddProcess((IProcessObjectBase)maintainableObject);
		            break;

		        case SdmxStructureEnumType.ProvisionAgreement : 
                    AddHierarchicalCodelist((IHierarchicalCodelistObjectBase)maintainableObject);
		            break;

		        case SdmxStructureEnumType.Registration : 
                    AddRegistration((IRegistrationObjectBase)maintainableObject);
		            break;

		        default: 
                    throw new SdmxNotImplementedException("SuperBeansImpl.addMaintainable of type : " + maintainableObject.BuiltFrom.StructureType.StructureType);
		    }
	    }

	    /// <summary>
        /// Add a IProcessObjectBase to this container, overwrite if one already exists with the same URN
        /// </summary>
        /// <param name="processObject"> Process Object </param>
	    public void AddProcess(IProcessObjectBase processObject)
        {
		    if(processObject != null)
            {
			    this._processes.Remove(processObject);	
			    this._processes.Add(processObject);	
		    }
	    }

        /// <summary>
        /// Add a IProvisionAgreementObjectBase to this container, overwrite if one already exists with the same URN
        /// </summary>
        /// <param name="provisionAgreementObject"> ProvisionAgreement Object </param>
	    public void AddProvision(IProvisionAgreementObjectBase provisionAgreementObject)
        {
		    if(provisionAgreementObject != null)
            {
			    this._provisionAgreement.Remove(provisionAgreementObject);	
			    this._provisionAgreement.Add(provisionAgreementObject);	
		    }
	    }
	
	    /// <summary>
        /// Add a IRegistrationObjectBase to this container, overwrite if one already exists with the same URN
        /// </summary>
        /// <param name="registrationObject">Registration Object </param>
	    public void AddRegistration(IRegistrationObjectBase registrationObject)
        {
		    if(registrationObject != null)
            {
			    this._registrations.Remove(registrationObject);
			    this._registrations.Add(registrationObject);	
		    }
	    }

        /// <summary>
        /// Merges the super
        /// </summary>
        /// <param name="objectsBase"> objects Base </param>
	    public void Merge(IObjectsBase objectsBase) 
        {
		    foreach(IMaintainableObjectBase currentObjectBase in objectsBase.AllMaintainables) 
            {
			    AddMaintainable(currentObjectBase);
		    }
	    }

        /// <summary>
        /// Remove the given ICategorySchemeObjectBase from this container, do nothing if it is not in this container
        /// </summary>
        /// <param name="categorySchemeObject">CategoryScheme Object </param>
	    public void RemoveCategoryScheme(ICategorySchemeObjectBase categorySchemeObject)
        {
		    this._categorySchemes.Remove(categorySchemeObject);
	    }

	    /// <summary>
        /// Remove the given ICategorySchemeObjectBase from this container, do nothing if it is not in this container
        /// </summary>
        /// <param name="codelistObject">codelist Object </param>
	    public void RemoveCodelist(ICodelistObjectBase codelistObject)
        {
		    this._codelists.Remove(codelistObject);
	    }

	    /// <summary>
        /// Remove the given ICategorySchemeObjectBase from this container, do nothing if it is not in this container
        /// </summary>
        /// <param name="conceptSchemeObject">ConceptScheme Object </param>
	    public void RemoveConceptScheme(IConceptSchemeObjectBase conceptSchemeObject)
        {
		    this._conceptSchemes.Remove(conceptSchemeObject);
	    }

        /// <summary>
        /// Remove the given IDataStructureObjectBase from this container, do nothing if it is not in this container
        /// </summary>
        /// <param name="dataStructureObject">DataStructure Object </param>
	    public void RemoveDataStructure(IDataStructureObjectBase dataStructureObject)
        {
		    this._dataStructures.Remove(dataStructureObject);
	    }

	    /// <summary>
        /// Remove the given IDataflowObjectBase from this container, do nothing if it is not in this container
        /// </summary>
        /// <param name="dataflowObject"> Dataflow Object </param>
	    public void RemoveDataflow(IDataflowObjectBase dataflowObject)
        {
		    this._dataflows.Remove(dataflowObject);
	    }

	    /// <summary>
        /// Remove the given IHierarchicalCodelistObjectBase from this container, do nothing if it is not in this container
        /// </summary>
        /// <param name="hierarchicalCodelistObject">HierarchicalCodelist Object </param>
	    public void RemoveHierarchicalCodelist(IHierarchicalCodelistObjectBase hierarchicalCodelistObject)
        {
		    this._hcls.Remove(hierarchicalCodelistObject);
	    }

        /// <summary>
        /// Remove the given IProcessObjectBase from this container, do nothing if it is not in this container
        /// </summary>
        /// <param name="processObject">Process Object </param>
	    public void RemoveProcess(IProcessObjectBase processObject)
        {
		    this._processes.Remove(processObject);
	    }

	    /// <summary>
        /// Remove the given IProvisionAgreementObjectBase from this container, do nothing if it is not in this container
        /// </summary>
        /// <param name="provisionAgreementObject">ProvisionAgreement Object </param>
	    public void RemoveProvision(IProvisionAgreementObjectBase provisionAgreementObject)
        {
		    this._provisionAgreement.Remove(provisionAgreementObject);
	    }

        /// <summary>
        /// Remove the given IRegistrationObjectBase from this container, do nothing if it is not in this container
        /// </summary>
        /// <param name="registrationObject">Registration Object </param>
        public void RemoveRegistration(IRegistrationObjectBase registrationObject)
        {
            this._registrations.Remove(registrationObject);
	    }

        #endregion
    }
}
