// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IStructureReference.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference
{
    #region Using directives

    using System;
    using System.Collections.Generic;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;

    #endregion

    /// <summary>
    ///     A IStructureReference is used to reference an identifiable artifact using a combination of reference parameters.
    ///     <p />
    ///     If all reference parameters are present then the reference is for a single identifiable item, if any are missing, then this represents
    ///     a wildcard parameter / or ALL.
    ///     <p />
    ///     SWSDMX_STRUCTURE_TYPE is a mandatory reference parameter, all others are optional
    /// </summary>
    public interface IStructureReference : IMaintainableRefObject
    {
        #region Public Properties

        /// <summary>
        ///     Gets the child artifact that is being referenced, returns null if there is no child artifact
        /// </summary>
        /// <value> </value>
        IIdentifiableRefObject ChildReference { get; }

        /// <summary>
        /// Gets the full id of the identifiable reference.  This is a dot '.' separated id which consists of the parent identifiable ids and the target.
        /// If the referenced structure is a Sub-Agency then this will include the parent Agency ids, and the id of the target agency.  If this reference
        /// is referencing a maintainable then null will be returned.  If there is only one child identifiable, then the id of that identifable will be returned, with no '.' seperators.
        /// </summary>
        /// <value> </value>
        string FullId { get; }

        /// <summary>
        ///     Gets a string array of any child ids (obtained from getChildReference()), returns null if getChildReference() is null
        /// </summary>
        /// <value> </value>
        IList<string> IdentifiableIds { get; }

        /// <summary>
        ///     Gets the reference parameters as a maintainable reference
        /// </summary>
        /// <value> </value>
        IMaintainableRefObject MaintainableReference { get; }

        /// <summary>
        ///     Gets the SDMX Structure that is being referenced at the top level (maintainable level) by this reference @object.
        ///     <p />
        ///     Any child references will further refine the structure type that is being referenced.
        /// </summary>
        /// <value> </value>
        SdmxStructureType MaintainableStructureEnumType { get; }

        /// <summary>
        ///     Gets the URN of the maintainable structure that is being referenced, returns null if there is no URN available
        /// </summary>
         Uri MaintainableUrn { get; }

        /// <summary>
        ///     Gets the SDMX Structure that is being referenced by this reference @object.
        /// </summary>
        /// <value> </value>
        SdmxStructureType TargetReference { get; }

        /// <summary>
        ///     Gets the URN of the target structure that is being referenced, returns null if there is no URN available
        /// </summary>
         Uri TargetUrn { get; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Creates a copy of this @object
        /// </summary>
        /// <returns>
        ///     The <see cref="IStructureReference" /> .
        /// </returns>
        IStructureReference CreateCopy();

        /// <summary>
        /// Gets the matched Identifiable Object from this reference, returns the original Maintainable if this is a Maintainable reference that matches the maintainable.  Gets null if no match
        ///     is found
        /// </summary>
        /// <param name="maintainableObject"> The maintainable object. </param>
        /// <returns>
        /// The <see cref="IIdentifiableObject"/> .
        /// </returns>
        IIdentifiableObject GetMatch(IMaintainableObject maintainableObject);

        /// <summary>
        ///     Gets a value indicating whether the getChildReference returns a value
        /// </summary>
        /// <returns>
        ///     The <see cref="bool" /> .
        /// </returns>
        bool HasChildReference();

        /// <summary>
        ///     Gets a value indicating whether the getMaintainableUrn returns a value
        /// </summary>
        /// <returns>
        ///     The <see cref="bool" /> .
        /// </returns>
        bool HasMaintainableUrn();

        /// <summary>
        ///     Gets a value indicating whether the getTargetUrn returns a value
        /// </summary>
        /// <returns>
        ///     The <see cref="bool" /> .
        /// </returns>
        bool HasTargetUrn();

        /// <summary>
        /// Gets a value indicating whether the reference matches the IMaintainableObject, or one of its indentifiable composites.
        ///     <p/>
        ///     This @object does not require all reference parameters to be set, this method will return true if all the parameters that are set match
        ///     the @object passed in.  If this reference is referencing an Identifiable composite, then the maintainable's identifiable composites will also be
        ///     checked to determine if this is a match.
        /// </summary>
        /// <param name="reference"> The reference object. </param>
        /// <returns>
        /// The <see cref="bool"/> .
        /// </returns>
        bool IsMatch(IMaintainableObject reference);

        #endregion
    }
}