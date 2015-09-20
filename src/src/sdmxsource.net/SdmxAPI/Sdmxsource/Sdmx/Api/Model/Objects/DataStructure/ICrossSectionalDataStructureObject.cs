// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ICrossSectionalDataStructureObject.cs" company="Eurostat">
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

    using System.Collections.Generic;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.DataStructure;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;

    #endregion

    /// <summary>
    ///     A Cross Sectional Data Structure extends DataStructure by adding cross sectional information as it was modelled in SDMX Version 2.0
    /// </summary>
    public interface ICrossSectionalDataStructureObject : IDataStructureObject
    {
        #region Public Properties

        /// <summary>
        ///     Gets a list of the cross sectional measures
        /// </summary>
        /// <value> </value>
        IList<ICrossSectionalMeasure> CrossSectionalMeasures { get; }

        /// <summary>
        ///     Gets a representation of itself in a @object which can be modified, modifications to the mutable @object
        ///     are not reflected in the instance of the IMaintainableObject.
        /// </summary>
        /// <value> </value>
        new ICrossSectionalDataStructureMutableObject MutableInstance { get; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Get the cross sectional measures that the attribute is linked to, returns an empty list if there is no cross sectional measures
        ///     defined by the attribtue.
        /// </summary>
        /// <param name="attribute">
        /// The attribute.
        /// </param>
        /// <returns>
        /// The <see cref="IList{T}"/> .
        /// </returns>
        IList<ICrossSectionalMeasure> GetAttachmentMeasures(IAttributeObject attribute);

        /// <summary>
        /// Gets the codelist reference for the dimension with the given id
        /// </summary>
        /// <param name="dimensionId">
        /// The dimension Id.
        /// </param>
        /// <returns>
        /// The <see cref="ICrossReference"/> .
        /// </returns>
        ICrossReference GetCodelistForMeasureDimension(string dimensionId);

        /// <summary>
        /// The get cross sectional attach data set.
        /// </summary>
        /// <param name="returnOnlyIfLowestLevel">
        /// The return only if lowest level.
        /// </param>
        /// <param name="returnEnumTypes">
        /// The return enum types.
        /// </param>
        /// <returns>
        /// The <see cref="IList{T}"/> .
        /// </returns>
        IList<IComponent> GetCrossSectionalAttachDataSet(
            bool returnOnlyIfLowestLevel, params SdmxStructureType[] returnEnumTypes);

        /// <summary>
        /// The get cross sectional attach group.
        /// </summary>
        /// <param name="returnOnlyIfLowestLevel">
        /// The return only if lowest level.
        /// </param>
        /// <param name="returnEnumTypes">
        /// The return enum types.
        /// </param>
        /// <returns>
        /// The <see cref="IList{T}"/> .
        /// </returns>
        IList<IComponent> GetCrossSectionalAttachGroup(
            bool returnOnlyIfLowestLevel, params SdmxStructureType[] returnEnumTypes);

        /// <summary>
        /// The get cross sectional attach observation.
        /// </summary>
        /// <param name="returnEnumTypes">
        /// The return enum types.
        /// </param>
        /// <returns>
        /// The <see cref="IList{T}"/> .
        /// </returns>
        IList<IComponent> GetCrossSectionalAttachObservation(params SdmxStructureType[] returnEnumTypes);

        /// <summary>
        /// The get cross sectional attach section.
        /// </summary>
        /// <param name="returnOnlyIfLowestLevel">
        /// The return only if lowest level.
        /// </param>
        /// <param name="returnEnumTypes">
        /// The return enum types.
        /// </param>
        /// <returns>
        /// The <see cref="IList{T}"/> .
        /// </returns>
        IList<IComponent> GetCrossSectionalAttachSection(
            bool returnOnlyIfLowestLevel, params SdmxStructureType[] returnEnumTypes);

        /// <summary>
        /// Gets the cross sectional measure with the given id.
        ///     <p/>
        ///     Gets null if no measure is found with the id.
        /// </summary>
        /// <param name="id">The id.
        /// </param>
        /// <returns>
        /// The <see cref="ICrossSectionalMeasure"/> .
        /// </returns>
        ICrossSectionalMeasure GetCrossSectionalMeasure(string id);

        /// <summary>
        /// Gets a value indicating whether the the given dimension is to be treated as the measure dimension
        /// </summary>
        /// <param name="dim">Dimensions object
        /// </param>
        /// <returns>
        /// The <see cref="bool"/> .
        /// </returns>
        bool IsMeasureDimension(IDimension dim);

        #endregion
    }
}