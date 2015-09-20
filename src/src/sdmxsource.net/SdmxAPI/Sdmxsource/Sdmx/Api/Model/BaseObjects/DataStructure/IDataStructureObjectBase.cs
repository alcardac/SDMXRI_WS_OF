// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IDataStructureObjectBase.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Model.BaseObjects.DataStructure
{
    #region Using directives

    using System;
    using System.Collections.Generic;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.BaseObjects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.BaseObjects.Codelist;
    using Org.Sdmxsource.Sdmx.Api.Model.BaseObjects.ConceptScheme;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.DataStructure;

    #endregion

    /// <summary>
    ///     A DataStructure identifies the dimensionality of a dataset in terms of the codelists and concepts used.  It also specifies the
    ///     attributes, measures and groups that can be used in the dataset.
    /// </summary>
    public interface IDataStructureObjectBase : IMaintainableObjectBase
    {
        #region Public Properties

        /// <summary>
        ///     Gets a list of all the attributes in this DataStructure.
        ///     If there are no attributes then an empty list will be returned.
        /// </summary>
        /// <value> list of attributes </value>
        IList<IAttributeObjectBase> Attributes { get; }

        /// <summary>
        ///     Gets a set of all the components used within this DataStructure.
        /// </summary>
        /// <value> </value>
        ISet<IComponentObjectBase> Components { get; }

        /// <summary>
        ///     Gets the i data structure object.
        /// </summary>
        [Obsolete]
        IDataStructureObject DataStructureObject { get; }

        /// <summary>
        ///     Gets a subset of the key family attributes.
        ///     Gets the attributes with Attachment Level of DataSet.
        ///     If no such attributes exist then an empty list will be returned.
        /// </summary>
        /// <value> list of attributes </value>
        IList<IAttributeObjectBase> DatasetAttributes { get; }

        /// <summary>
        ///     Gets a list of all the dimensions in the DataStructure.
        ///     This does not include the primary measure dimension.
        ///     If there are no dimensions then an empty list will be returned.
        /// </summary>
        /// <value> List of dimensions </value>
        IList<IDimensionObjectBase> Dimensions { get; }

        /// <summary>
        ///     Gets a set of all the attributes attached to any group in the DataStructure.
        ///     If no such attributes exist then an empty list will be returned.
        /// </summary>
        /// <value> list of attributes </value>
        ISet<IAttributeObjectBase> GroupAttributes { get; }

        /// <summary>
        ///     Gets all the groups in the DataStructure.
        ///     If there are no groups an empty list will be returned.
        /// </summary>
        /// <value> List of groups </value>
        IList<IGroupObjectBase> Groups { get; }

        /// <summary>
        ///     Gets a subset of the DataStructure attributes.
        ///     Gets the attributes with Attachment Level of Observation.
        ///     If no such attributes exist then an empty list will be returned.
        /// </summary>
        /// <value> list of attributes </value>
        IList<IAttributeObjectBase> ObservationAttributes { get; }

        /// <summary>
        ///     Gets the primary measure for this DataStructure.
        ///     If there is no primary measure then null will be returned.
        /// </summary>
        /// <value> list of IPrimaryMeasureObjectBase </value>
        IPrimaryMeasureObjectBase PrimaryMeasure { get; }

        /// <summary>
        ///     Gets a set of all the codelists referenced within this DataStructure.
        /// </summary>
        /// <value> </value>
        ISet<ICodelistObjectBase> ReferencedCodelists { get; }

        /// <summary>
        ///     Gets a set of all the concepts referenced within this DataStructure.
        /// </summary>
        /// <value> </value>
        ISet<IConceptObjectBase> ReferencedConcepts { get; }

        /// <summary>
        ///     Gets a subset of the DataStructure attributes.
        ///     Gets the attributes with Attachment Level of Series.
        /// </summary>
        /// <value> list of attributes </value>
        IList<IAttributeObjectBase> SeriesAttributes { get; }

        /// <summary>
        ///     Gets the time dimension from this DataStructure.
        /// </summary>
        /// <value> The time dimension @object </value>
        IDimensionObjectBase TimeDimension { get; }

        /// <summary>
        /// 
        /// </summary>
        new IDataStructureObject BuiltFrom { get; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Gets a list of group identifiers, to which a group level attribute attaches.
        /// </summary>
        /// <param name="id">
        /// id of the attribute
        /// </param>
        /// <returns>
        /// list of group identifiers
        /// </returns>
        string GetAttributeAttachmentGroup(string id);

        /// <summary>
        /// Gets a referenced codelist from a component with the given id.
        ///     If there are no components in the key family that have reference to the given concept
        ///     id or if the referenced component is uncoded then null will be returned.
        /// </summary>
        /// <param name="componentId">
        /// the id by which to refer to the codelist.
        /// </param>
        /// <returns>
        /// the codelist referred to by the component id.
        /// </returns>
        ICodelistObjectBase GetCodelistByComponentId(string componentId);

        /// <summary>
        /// Gets a component from an id.
        ///     If no such concept exists, null will be returned.
        /// </summary>
        /// <param name="componentId">
        /// the id by which to refer to the component.
        /// </param>
        /// <returns>
        /// the component which references the specified id.
        /// </returns>
        IComponentObjectBase GetComponentById(string componentId);

        /// <summary>
        /// Gets a dimension, that is referenced by the specified id.
        ///     If no such dimension exists for this DataStructure then null will be returned.
        /// </summary>
        /// <param name="dimensionId">
        /// the id that the dimension has reference to
        /// </param>
        /// <returns>
        /// the dimension which references the specified id.
        /// </returns>
        IDimensionObjectBase GetDimensionById(string dimensionId);

        /// <summary>
        /// The get dimensions.
        /// </summary>
        /// <param name="include">
        /// The include.
        /// </param>
        /// <returns>
        /// The <see cref="IList{T}"/> .
        /// </returns>
        IList<IDimensionObjectBase> GetDimensions(params SdmxStructureType[] include);

        /// <summary>
        /// Gets the group with the given id.
        ///     <p/>
        ///     If no groups exist or no groups have the id, then null will be returned.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The group. 
        /// </returns>
        IGroupObjectBase GetGroup(string id);

        /// <summary>
        /// Gets a subset of the DataStructure attributes.
        ///     Gets the attributes with Attachment Level of Group, where the group id is the id given.
        ///     If no such attributes exist then an empty list will be returned.
        /// </summary>
        /// <param name="groupId">
        /// The group id of the group to return the attributes for
        /// </param>
        /// <param name="includeDimensionGroups">
        /// If true, this will include attributes which are attached to a dimension group, which group the same dimensions as the 
        /// group with the given id
        /// </param>
        /// <returns>
        /// list of attributes
        /// </returns>
        /// <exception cref="ArgumentException">
        /// If the groupId does not match any groups for this Data Structure
        /// </exception>
        IList<IAttributeObjectBase> GetGroupAttributes(string groupId, bool includeDimensionGroups);

        /// <summary>
        /// Gets the group identifier that matches the dimensions set supplied .
        /// </summary>
        /// <param name="dimensions">
        /// a set containing the complete collection of dimensions to be matched with a group
        /// </param>
        /// <returns>
        /// the identification of the group which has the same dimensions as the supplied set
        /// </returns>
        string GetGroupId(ISet<string> dimensions);

        #endregion
    }
}