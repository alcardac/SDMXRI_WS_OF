// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IMaintainableObject.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Model.Objects.Base
{
    #region Using directives

    using System;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base;

    #endregion

    /// <summary>
    ///     A IMaintainableObject is a top level Object (contains no parents), it has a reference to an Agency <c>getAgencyId()</c>
    ///     and has a <b>mandatory id</b> and a <b>mandatory version</b>, defaulting to 1.0.
    ///     <p />
    ///     The unique identifier of a maintainable artefact is the AgencyId, Id and Version.
    ///     <p />
    ///     Each maintainable artefact can create a mutable representation of itself (<c>getMutableInstance()</c>)
    ///     and can also return a stub representation of itself <c>getStub()</c>.
    /// </summary>
    public interface IMaintainableObject : INameableObject, IComparable<IMaintainableObject>
    {
        #region Public Properties

        /// <summary>
        ///     Gets the agency id that is responsible for maintaining this maintainable artifact
        /// </summary>
        /// <value> </value>
        string AgencyId { get; }

        /// <summary>
        ///     Gets the end date of this maintainable artifact, returns null if there is no endDate
        ///     <p />
        /// </summary>
        /// <value> </value>
        ISdmxDate EndDate { get; }

        /// <summary>
        ///     Gets a representation of itself in a Object which can be modified, modifications to the mutable Object
        ///     are not reflected in the instance of the IMaintainableObject.
        /// </summary>
        /// <value> </value>
        IMaintainableMutableObject MutableInstance { get; }

        /// <summary>
        ///     Gets the serviceURL attribute indicates the Uri of an SDMX SOAP web service from which the details of the object can be retrieved.
        ///     Note that this can be a registry or and SDMX structural metadata repository,
        ///     as they both implement that same web service interface.
        ///     Optional.
        /// </summary>
        /// <value> </value>
        Uri ServiceUrl { get; }

        /// <summary>
        ///     Gets or sets the start date of this maintainable artifact, returns null if there is no startDate
        ///     <p />
        /// </summary>
        /// <value> </value>
        ISdmxDate StartDate { get; set; }

        /// <summary>
        ///     Gets the structureURL attribute indicates the Uri of a SDMX-ML structure message
        ///     (in the same version as the source document) in which the externally referenced object is contained.
        ///     Note that this my be a Uri of an SDMX <c>RESTful</c> web service which will return the referenced object.
        ///     Optional.
        /// </summary>
        /// <value> </value>
        Uri StructureUrl { get; }

        /// <summary>
        ///     Gets the version of this maintainable artifact, default version is 1.0
        ///     <p />
        ///     Version is a integer value with period '.' separators between integers, for example 1.2.3.19
        /// </summary>
        /// <value> </value>
        string Version { get; }

        /// <summary>
        ///     Gets TERTIARY_BOOL.TRUE if the structure is marked as final, meaning the structure can not be modified
        /// </summary>
        TertiaryBool IsFinal { get; }

        /// <summary>
        ///     Gets TERTIARY_BOOL.TRUE if this maintainable artifact is externally referenced
        /// </summary>
        TertiaryBool IsExternalReference { get; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Gets a stub reference of itself.
        ///     <p/>
        ///     A stub Object only contains Maintainable and Identifiable Attributes, not the composite Objects that are
        ///     contained within the Maintainable
        /// </summary>
        /// <returns>
        /// The <see cref="IMaintainableObject"/> .
        /// </returns>
        /// <param name="actualLocation">
        /// the Uri indicating where the full structure can be returned from
        /// </param>
        /// <param name="isServiceUrl">
        /// if true this Uri will be present on the serviceURL attribute, otherwise it will be treated as a structureURL attribute
        /// </param>
        IMaintainableObject GetStub(Uri actualLocation, bool isServiceUrl);

        #endregion
    }
}