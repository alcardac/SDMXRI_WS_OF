// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IDataStructureMutableObject.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Model.Mutable.DataStructure
{
    #region Using directives

    using System.Collections.Generic;

    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.DataStructure;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;

    #endregion

    /// <summary>
    ///     The DataStructureMutableObject interface.
    /// </summary>
    public interface IDataStructureMutableObject : IMaintainableMutableObject
    {
        #region Public Properties

        /// <summary>
        ///     Gets or sets the attribute list.
        /// </summary>
        IAttributeListMutableObject AttributeList { get; set; }

        /// <summary>
        /// Gets the attributes.this may return null or an empty list if none exist
        /// </summary>
        /// <value>
        /// The attributes.
        /// </value>
        IList<IAttributeMutableObject> Attributes { get; }

        /// <summary>
        ///     Gets or sets the dimension list.
        /// </summary>
        IDimensionListMutableObject DimensionList { get; set; }

        /// <summary>
        ///     Gets the dimensions.
        /// </summary>
        IList<IDimensionMutableObject> Dimensions { get; }

        /// <summary>
        ///     Gets the groups.
        /// </summary>
        IList<IGroupMutableObject> Groups { get; }

        /// <summary>
        ///     Gets a representation of itself in a @object which can not be modified, modifications to the mutable @object
        ///     are not reflected in the returned instance of the IMaintainableObject.
        /// </summary>
        /// <value> </value>
        new IDataStructureObject ImmutableInstance { get; }

        /// <summary>
        ///     Gets or sets the measure list.
        /// </summary>
        IMeasureListMutableObject MeasureList { get; set; }

        /// <summary>
        ///    Gets or sets the primary measure.
        /// </summary>
        IPrimaryMeasureMutableObject PrimaryMeasure { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The add attribute.
        /// </summary>
        /// <param name="attribute">
        /// The attribute.
        /// </param>
        void AddAttribute(IAttributeMutableObject attribute);

        /// <summary>
        /// Adds the attribute.
        /// </summary>
        /// <param name="conceptRef">The concept reference</param>
        /// <param name="codelistRef">The code list reference</param>
        /// <returns>
        /// The attribute
        /// </returns>
        IAttributeMutableObject AddAttribute(IStructureReference conceptRef, IStructureReference codelistRef);

        /// <summary>
        /// Adds the dimension.
        /// </summary>
        /// <param name="dimension">The dimension.</param>
        void AddDimension(IDimensionMutableObject dimension);

        /// <summary>
        /// Adds the dimension.
        /// </summary>
        /// <param name="conceptRef">The concept preference.</param>
        /// <param name="codelistRef">The codelist preference.</param>
        /// <returns>The dimension that was created.</returns>
        IDimensionMutableObject AddDimension(IStructureReference conceptRef, IStructureReference codelistRef);

        /// <summary>
        /// Adds the primary measure.
        /// </summary>
        /// <param name="conceptRef">The concept preference.</param>
        /// <returns>The primary measure created.</returns>
        IPrimaryMeasureMutableObject AddPrimaryMeasure(IStructureReference conceptRef);

        /// <summary>
        /// Adds the group.
        /// </summary>
        /// <param name="group">The group.</param>
        void AddGroup(IGroupMutableObject group);

        /// <summary>
        /// Removes the component  (dimension or attribute) with the given id
        /// </summary>
        /// <param name="id">The unique identifier.</param>
        /// <returns>this instance</returns>
        IDataStructureMutableObject RemoveComponent(string id);

        /// <summary>
        /// Gets the dimension.
        /// </summary>
        /// <param name="id">The unique identifier.</param>
        /// <returns>Returns the dimension with the given id, or null if there is no match.</returns>
        IDimensionMutableObject GetDimension(string id);

        /// <summary>
        /// Gets the attribute.
        /// </summary>
        /// <param name="id">The unique identifier.</param>
        /// <returns> Returns the attribute with the given id, or null if there is no match.</returns>
        IAttributeMutableObject GetAttribute(string id);

        #endregion
    }
}