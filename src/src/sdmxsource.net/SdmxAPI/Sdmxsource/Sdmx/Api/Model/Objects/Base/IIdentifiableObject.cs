// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IIdentifiableObject.cs" company="Eurostat">
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
    using System.Collections.Generic;

    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;

    #endregion

    /// <summary>
    ///     An Identifiable Object describes a Object which contains a unique identifier (urn) and can therefore
    ///     be identified uniquely.
    /// </summary>
    public interface IIdentifiableObject : IAnnotableObject, ISdmxStructure
    {
        #region Public Properties

        /// <summary>
        ///     Gets a list of all the underlying text types for this identifiable (does not recurse down to children).
        ///     <p />
        ///     Gets an empty list if there are no text types for this identifiable
        /// </summary>
        /// <value> </value>
        IList<ITextTypeWrapper> AllTextTypes { get; }

        /// <summary>
        ///     Gets the id for this component, this is a mandatory field and will never be null
        /// </summary>
        /// <value> </value>
        string Id { get; }

        /// <summary>
        ///     Gets the URI for this component, returns null if there is no URI.
        ///     <p />
        ///     URI describes where additional information can be found for this component, this is guaranteed to return
        ///     a value if the structure is a <c>IMaintainableObject</c> and isExternalReference is true
        /// </summary>
        /// <value> </value>
        Uri Uri { get; }

        /// <summary>
        ///     Gets the URN for this component.  The URN is unique to this instance and is a computable generated value based on
        ///     other attributes set within the component.
        /// </summary>
        /// <value> </value>
         Uri Urn { get; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Builds a IStructureReference that is a representation of this IIdentifiableObject as a reference.  The
        ///     returned IStructureReference can be used to uniquely identify this identifiable @object.
        /// </summary>
        /// <value> The &lt; see cref= &quot; IStructureReference &quot; / &gt; . </value>
        IStructureReference AsReference { get; }

        /// <summary>
        /// Gets a period separated id of this identifiable, starting from the non-maintainable top level ancestor to this identifiable.
        ///     <p/>
        ///     For example, if this is Category A living as a child of Category AA, then this method will return AA.A (not the category scheme id is not present in this identifier)
        ///     <p/>
        ///     Gets null if this is a maintainable Object
        /// </summary>
        /// <param name="includeDifferentTypes">Include different types.
        /// </param>
        /// <returns>
        /// The <see cref="string"/> .
        /// </returns>
        string GetFullIdPath(bool includeDifferentTypes);

        #endregion
    }
}