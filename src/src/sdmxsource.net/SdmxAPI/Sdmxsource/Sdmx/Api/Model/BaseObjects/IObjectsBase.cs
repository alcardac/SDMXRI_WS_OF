// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IObjectsBase.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Model.BaseObjects
{
    #region Using directives

    using System.Collections.Generic;

    using Org.Sdmxsource.Sdmx.Api.Model.BaseObjects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.BaseObjects.CategoryScheme;
    using Org.Sdmxsource.Sdmx.Api.Model.BaseObjects.Codelist;
    using Org.Sdmxsource.Sdmx.Api.Model.BaseObjects.ConceptScheme;
    using Org.Sdmxsource.Sdmx.Api.Model.BaseObjects.DataStructure;
    using Org.Sdmxsource.Sdmx.Api.Model.BaseObjects.Process;
    using Org.Sdmxsource.Sdmx.Api.Model.BaseObjects.Registry;

    #endregion
    /// <summary>
    ///     BaseObjects is a container for objects of type <b>BaseObjects</b>
    /// </summary>
    public interface IObjectsBase
    {
        // ADD
        #region Public Properties

        /// <summary>
        ///     Gets a <b>copy</b> of the IMaintainableObjectBase Set.  Gets an empty set if no MaintainableBaseObjects are stored in this container
        /// </summary>
        /// <value> </value>
        ISet<IMaintainableObjectBase> AllMaintainables { get; }

        /// <summary>
        ///     Gets a <b>copy</b> of the ICategorySchemeObjectBase Set.  Gets an empty set if no CategorySchemeBaseObjects are stored in this container
        /// </summary>
        /// <value> </value>
        ISet<ICategorySchemeObjectBase> CategorySchemes { get; }

        /// <summary>
        ///     Gets a <b>copy</b> of the ICodelistObjectBase Set.  Gets an empty set if no CodelistBaseObjects are stored in this container
        /// </summary>
        /// <value> </value>
        ISet<ICodelistObjectBase> Codelists { get; }

        /// <summary>
        ///     Gets a <b>copy</b> of the IConceptSchemeObjectBase Set.  Gets an empty set if no ConceptSchemeBaseObjects are stored in this container
        /// </summary>
        /// <value> </value>
        ISet<IConceptSchemeObjectBase> ConceptSchemes { get; }

        /// <summary>
        ///     Gets a <b>copy</b> of the KeyFamilyBaseObjects Set.  Gets an empty set if no KeyFamilyBaseObjects are stored in this container
        /// </summary>
        /// <value> </value>
        ISet<IDataStructureObjectBase> DataStructures { get; }

        /// <summary>
        ///     Gets a <b>copy</b> of the IDataflowObjectBase Set.  Gets an empty set if no DataflowBaseObjects are stored in this container
        /// </summary>
        /// <value> </value>
        ISet<IDataflowObjectBase> Dataflows { get; }

        /// <summary>
        ///     Gets a <b>copy</b> of the IHierarchicalCodelistObjectBase Set.  Gets an empty set if no HierarchicalCodelistBaseObjects are stored in this container
        /// </summary>
        /// <value> </value>
        ISet<IHierarchicalCodelistObjectBase> HierarchicalCodelists { get; }

        /// <summary>
        ///     Gets a <b>copy</b> of the IProcessObjectBase Set.  Gets an empty set if no ProcessBaseObjects are stored in this container
        /// </summary>
        /// <value> </value>
        ISet<IProcessObjectBase> Processes { get; }

        /// <summary>
        ///     Gets a <b>copy</b> of the IProvisionAgreementObjectBase Set.  Gets an empty set if no ProvisionAgreementBaseObjects are stored in this container
        /// </summary>
        /// <value> </value>
        ISet<IProvisionAgreementObjectBase> Provisions { get; }

        /// <summary>
        ///     Gets a <b>copy</b> of the IRegistrationObjectBase Set.  Gets an empty set if no RegistrationBaseObjects are stored in this container
        /// </summary>
        /// <value> </value>
        ISet<IRegistrationObjectBase> Registartions { get; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Add a ICategorySchemeObjectBase to this container, overwrite if one already exists with the same URN
        /// </summary>
        /// <param name="categorySchemeObject">The object. </param>
        void AddCategoryScheme(ICategorySchemeObjectBase categorySchemeObject);

        /// <summary>
        /// Add a ICodelistObjectBase to this container, overwrite if one already exists with the same URN
        /// </summary>
        /// <param name="codelistObject">Codelist object. </param>
        void AddCodelist(ICodelistObjectBase codelistObject);

        /// <summary>
        /// Add a IConceptSchemeObjectBase to this container, overwrite if one already exists with the same URN
        /// </summary>
        /// <param name="conceptSchemeObject"> ConceptScheme object </param>
        void AddConceptScheme(IConceptSchemeObjectBase conceptSchemeObject);

        /// <summary>
        /// Add a IDataStructureObjectBase to this container, overwrite if one already exists with the same URN
        /// </summary>
        /// <param name="dataStructureObject">Datastructure object. </param>
        void AddDataStructure(IDataStructureObjectBase dataStructureObject);

        /// <summary>
        /// Add a IDataflowObjectBase to this container, overwrite if one already exists with the same URN
        /// </summary>
        /// <param name="dataflowObject">Dataflow Object </param>
        void AddDataflow(IDataflowObjectBase dataflowObject);

        /// <summary>
        /// Add a IHierarchicalCodelistObjectBase to this container, overwrite if one already exists with the same URN
        /// </summary>
        /// <param name="hierarchicalCodelistObject"> HierarchicalCodelist Object </param>
        void AddHierarchicalCodelist(IHierarchicalCodelistObjectBase hierarchicalCodelistObject);

        /// <summary>
        /// The add maintainable.
        /// </summary>
        /// <param name="maintainableObject">
        /// The categorySchemeObject.
        /// </param>
        void AddMaintainable(IMaintainableObjectBase maintainableObject);

        /// <summary>
        /// Add a IProcessObjectBase to this container, overwrite if one already exists with the same URN
        /// </summary>
        /// <param name="processObject"> Process Object </param>
        void AddProcess(IProcessObjectBase processObject);

        /// <summary>
        /// Add a IProvisionAgreementObjectBase to this container, overwrite if one already exists with the same URN
        /// </summary>
        /// <param name="provisionAgreementObject"> ProvisionAgreement Object </param>
        void AddProvision(IProvisionAgreementObjectBase provisionAgreementObject);

        /// <summary>
        /// Add a IRegistrationObjectBase to this container, overwrite if one already exists with the same URN
        /// </summary>
        /// <param name="registrationObject">Registration Object </param>
        void AddRegistration(IRegistrationObjectBase registrationObject);

        /// <summary>
        /// Merges the super
        /// </summary>
        /// <param name="objectsBase"> objects Base </param>
        void Merge(IObjectsBase objectsBase);

        // REMOVE

        /// <summary>
        /// Remove the given ICategorySchemeObjectBase from this container, do nothing if it is not in this container
        /// </summary>
        /// <param name="categorySchemeObject">CategoryScheme Object </param>
        void RemoveCategoryScheme(ICategorySchemeObjectBase categorySchemeObject);

        /// <summary>
        /// Remove the given ICategorySchemeObjectBase from this container, do nothing if it is not in this container
        /// </summary>
        /// <param name="codelistObject">codelist Object </param>
        void RemoveCodelist(ICodelistObjectBase codelistObject);

        /// <summary>
        /// Remove the given ICategorySchemeObjectBase from this container, do nothing if it is not in this container
        /// </summary>
        /// <param name="conceptSchemeObject">ConceptScheme Object </param>
        void RemoveConceptScheme(IConceptSchemeObjectBase conceptSchemeObject);

        /// <summary>
        /// Remove the given IDataStructureObjectBase from this container, do nothing if it is not in this container
        /// </summary>
        /// <param name="dataStructureObject">DataStructure Object </param>
        void RemoveDataStructure(IDataStructureObjectBase dataStructureObject);

        /// <summary>
        /// Remove the given IDataflowObjectBase from this container, do nothing if it is not in this container
        /// </summary>
        /// <param name="dataflowObject"> Dataflow Object </param>
        void RemoveDataflow(IDataflowObjectBase dataflowObject);

        /// <summary>
        /// Remove the given IHierarchicalCodelistObjectBase from this container, do nothing if it is not in this container
        /// </summary>
        /// <param name="hierarchicalCodelistObject">HierarchicalCodelist Object </param>
        void RemoveHierarchicalCodelist(IHierarchicalCodelistObjectBase hierarchicalCodelistObject);

        /// <summary>
        /// Remove the given IProcessObjectBase from this container, do nothing if it is not in this container
        /// </summary>
        /// <param name="processObject">Process Object </param>
        void RemoveProcess(IProcessObjectBase processObject);

        /// <summary>
        /// Remove the given IProvisionAgreementObjectBase from this container, do nothing if it is not in this container
        /// </summary>
        /// <param name="provisionAgreementObject">ProvisionAgreement Object </param>
        void RemoveProvision(IProvisionAgreementObjectBase provisionAgreementObject);

        /// <summary>
        /// Remove the given IRegistrationObjectBase from this container, do nothing if it is not in this container
        /// </summary>
        /// <param name="registrationObject">Registration Object </param>
        void RemoveRegistration(IRegistrationObjectBase registrationObject);

        #endregion
    }
}