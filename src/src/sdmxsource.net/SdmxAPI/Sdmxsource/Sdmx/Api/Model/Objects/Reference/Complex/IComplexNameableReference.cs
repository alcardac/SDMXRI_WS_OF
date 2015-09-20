// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IComplexNameableReference.cs" company="Eurostat">
//   Date Created : 2013-08-19
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference.Complex
{
    #region Using directives

    using Org.Sdmxsource.Sdmx.Api.Constants;

    #endregion


    /// <summary>
    /// Represents a complex namable reference
    /// </summary>
    public interface IComplexNameableReference
    {
        #region Public Properties

        /// <summary>
        /// Gets the structure type that is being referenced, this can not be null
        /// </summary>
        SdmxStructureType ReferencedStructureType { get; }

        /// <summary>
        /// Gets an annotation reference
        /// </summary>
        IComplexAnnotationReference AnnotationReference { get; }

        /// <summary>
        /// Gets a name reference
        /// </summary>
        /// <returns></returns>
        IComplexTextReference NameReference { get; }

        /// <summary>
        /// Gets a description reference for a NameableObject
        /// </summary>
        IComplexTextReference DescriptionReference { get; }

        #endregion
    }
}
