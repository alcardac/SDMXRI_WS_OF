// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IDataStructureObject.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Model.Objects.DataStructure
{
    #region Using directives

    using System;
    using System.Collections.Generic;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.DataStructure;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;

    #endregion

    /// <summary>
    ///     A Data Structure also known as a Data Structure Definition (DSD) or KeyFamily describes the dimensionality
    ///     of the data.
    ///     <p />
    ///     A Data Structure contains Dimensions, Attributes, Groups, and a Measure. Each of these components is
    ///     held in its respective identifiable list container (DimensionList, AttributeList, MeasureList),
    ///     however the DataStructureObject exposes the methods to obtain the Dimensions, Attributes, Measure, and Groups
    ///     without having to navigate through the list.
    /// </summary>
    public interface IDataStructureObject : IMaintainableObject, IConstrainableObject
    {
        #region Public Properties

        /// <summary>
        ///     Gets the Attribute List.  The Attribute List is an identifiable container for the DataStructure's attributes.
        /// </summary>
        /// <value> </value>
        IAttributeList AttributeList { get; }

        /// <summary>
        ///     Gets the attributes that belong to this DataStructure.
        ///     If the DataStructure has no attributes then an empty list is returned.
        /// </summary>
        /// <value> </value>
        IList<IAttributeObject> Attributes { get; }

        /// <summary>
        /// Returns a list of all components that belong to this DSD
        /// </summary>
        /// <value> </value>
        IList<IComponent> Components { get; }

        /// <summary>
        ///     Gets a subset of the DataStructure's attributes.
        ///     Gets the attributes with Attachment Level of DataSet.
        ///     <p />
        ///     If there are no attributes that match this criteria then an empty list is returned
        /// </summary>
        /// <value> </value>
        IList<IAttributeObject> DatasetAttributes { get; }

        /// <summary>
        ///     Gets a subset of the DataStructure's attributes.
        ///     Gets the attributes with Attachment Level of DimensionGroup.
        ///     If there are no attributes that match this criteria then an empty list is returned.
        /// </summary>
        /// <value> </value>
        IList<IAttributeObject> DimensionGroupAttributes { get; }

        /// <summary>
        ///     Gets the Dimensions List.  The Dimensions List is an identifiable container for the DataStructure's dimensions.
        /// </summary>
        /// <value> </value>
        IDimensionList DimensionList { get; }

        /// <summary>
        ///     Gets the dimension that is playing the role of frequency. Gets <c>null</c> if no dimension is playing the role of frequency.
        /// </summary>
        /// <value> </value>
        IDimension FrequencyDimension { get; }

        /// <summary>
        ///     Gets a subset of the DataStructure's attributes.
        ///     Gets the attributes with Attachment Level of Group.
        ///     If there are no attributes that match this criteria then an empty list is returned.
        /// </summary>
        /// <value> </value>
        IList<IAttributeObject> GroupAttributes { get; }

        /// <summary>
        ///     Gets the list of groups that belongs to this DataStructure. If there are no groups then an empty list is returned.
        /// </summary>
        /// <value> </value>
        IList<IGroup> Groups { get; }

        /// <summary>
        ///     Gets the Measure List.  The Measure List is an identifiable container for the DataStructure's measure.
        /// </summary>
        /// <value> </value>
        IMeasureList MeasureList { get; }

        /// <summary>
        ///     Gets a representation of itself in a @object which can be modified, modifications to the mutable @object
        ///     are not reflected in the instance of the IMaintainableObject.
        /// </summary>
        /// <value> </value>
        new IDataStructureMutableObject MutableInstance { get; }

        /// <summary>
        ///     Gets a subset of the DataStructure's attributes.
        ///     Gets the attributes with Attachment Level of Observation when time is at the observation level.
        ///     If there are no attributes that match this criteria then an empty list is returned.
        /// </summary>
        /// <value> </value>
        IList<IAttributeObject> ObservationAttributes { get; }

        /// <summary>
        ///     Gets the primary measure that belongs to this DataStructure.
        ///     A DataStructure must have a primary measure so this method will always return a value.
        /// </summary>
        /// <value> </value>
        IPrimaryMeasure PrimaryMeasure { get; }

        /// <summary>
        ///     Gets the time dimension that belongs to this DataStructure.
        ///     <p />
        ///     If there is no time dimension then <c>null</c> is returned
        /// </summary>
        /// <value> </value>
        IDimension TimeDimension { get; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Gets the attribute with the given id, returns <c>null</c> if no such attribute exists.
        /// </summary>
        /// <param name="id">The attribute id.
        /// </param>
        /// <returns>
        /// The <see cref="IAttributeObject"/> .
        /// </returns>
        IAttributeObject GetAttribute(string id);

        /// <summary>
        /// Gets the component with the given id.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="IComponent"/> .
        /// </returns>
        IComponent GetComponent(string id);

        /// <summary>
        /// Gets the dimension with the given Id.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="IDimension"/> .
        /// </returns>
        IDimension GetDimension(string id);

        /// <summary>
        /// Gets the series attribute with the given id, returns <c>null</c> if no such attribute exists.
        /// </summary>
        /// <param name="id">The id. </param>
        /// <returns>IAttributeObject instance</returns>
        IAttributeObject GetDimensionGroupAttribute(string id);

        /// <summary>
        /// The get dimensions.
        /// </summary>
        /// <param name="include"> The include. </param>
        /// <returns>List of dimenstions </returns>
        IList<IDimension> GetDimensions(params SdmxStructureEnumType[] include);

        /// <summary>
        /// Gets the group with the given id that belongs to this DataStructure.
        ///     <p/>
        ///     If there are no groups with the given id then <c>null</c> will be returned.
        /// </summary>
        /// <param name="groupId">The group id. </param>
        /// <returns>
        /// The <see cref="IGroup"/> .
        /// </returns>
        IGroup GetGroup(string groupId);

        /// <summary>
        /// Gets the group attribute with the given id, returns <c>null</c> if no such attribute exists.
        /// </summary>
        /// <param name="id">The id. </param>
        /// <returns>
        /// The <see cref="IAttributeObject"/> .
        /// </returns>
        IAttributeObject GetGroupAttribute(string id);

        /// <summary>
        /// Gets a subset of the DataStructure's attributes.
        ///     Gets the attributes with Attachment Level of Group, that are attached to the group with the given id.
        ///     If there are no attributes that match this criteria then an empty list is returned.
        /// </summary>
        /// <param name="groupId"> 
        /// The group id of the group to return the attributes for
        /// </param>
        /// <param name="includeDimensionGroups">
        /// If true, this will include attributes which are attached to a dimension group, which group the same dimensions as the 
        /// group with the given id
        /// </param>
        /// <returns>
        /// List of attributes 
        /// </returns>
        /// <exception cref="ArgumentException">
        /// If the groupId does not match any groups for this Data Structure
        /// </exception>
        IList<IAttributeObject> GetGroupAttributes(string groupId, bool includeDimensionGroups);

        /// <summary>
        /// Gets the observation attribute with the given id, returns <c>null</c> if no such attribute exists.
        /// </summary>
        /// <param name="id">The id.
        /// </param>
        /// <returns>
        /// The <see cref="IAttributeObject"/> .
        /// </returns>
        IAttributeObject GetObservationAttribute(string id);

        /// <summary>
        /// Gets a subset of the DataStructure's attributes.
        ///     Gets the attributes with Attachment Level of Observation,
        ///     or an Attachment level of DimenisonGroup, where one of the dimensions in the group is the one at the cross sectional level.
        ///     If there are no attributes that match this criteria then an empty list is returned.
        /// </summary>
        /// <param name="crossSectionalConcept">
        /// The cross Sectional Concept.
        /// </param>
        /// <returns>
        /// The <see cref="IList{T}"/> .
        /// </returns>
        IList<IAttributeObject> GetObservationAttributes(string crossSectionalConcept);

        /// <summary>
        /// Gets a subset of the DataStructure's attributes.
        ///     Gets the attributes with Attachment Level of DimensionGroup,
        ///     where none of the dimensions referenced in the group are the cross sectional attribute.
        ///     If there are no attributes that match this criteria then an empty list is returned.
        /// </summary>
        /// <param name="crossSectionalConcept">
        /// The cross Sectional Concept.
        /// </param>
        /// <returns>
        /// The <see cref="IList{T}"/> .
        /// </returns>
        IList<IAttributeObject> GetSeriesAttributes(string crossSectionalConcept);

        /// <summary>
        /// Gets a stub reference of itself.
        ///     A stub @object only contains Maintainable and Identifiable Attributes, not the composite Objects that are
        ///     contained within the Maintainable.
        /// </summary>
        /// <returns>
        /// The <see cref="IDataStructureObject"/> .
        /// </returns>
        /// <param name="actualLocation">
        /// the Uri indicating where the full structure can be returned from
        /// </param>
        /// <param name="isServiceUrl">
        /// if true this Uri will be present on the serviceURL attribute, otherwise it will be treated as a structureURL attribute
        /// </param>
        new IDataStructureObject GetStub(Uri actualLocation, bool isServiceUrl);

        /// <summary>
        ///     Gets a value indicating whether the there is a dimension that is playing the role of frequency.
        /// </summary>
        /// <returns>
        ///     The <see cref="bool" /> .
        /// </returns>
        bool HasFrequencyDimension();

        /// <summary>
        /// Returns false if this data structure is not compatible with the target schema version.
        /// <p/>
        /// For SDMX Version 1.0 all dimensions had to be coded.
        /// <p/>
        /// For SDMX Version 1.0 and 2.0 each components had to reference a concept with a unique id
        /// <p/>
        /// For EDI ?
        /// </summary>
        /// <param name="schemaVersion">
        /// The schema version
        /// </param>
        /// <returns>
        /// The boolean
        /// </returns>
        bool IsCompatible(SdmxSchema schemaVersion);
 
        #endregion
    }
}