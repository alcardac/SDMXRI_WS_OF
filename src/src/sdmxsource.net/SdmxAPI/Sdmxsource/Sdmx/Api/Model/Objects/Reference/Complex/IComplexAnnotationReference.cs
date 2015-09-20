// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IComplexAnnotationReference.cs" company="Eurostat">
//   Date Created : 2013-08-19
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference.Complex
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// ComplexAnnotationReference is used to reference annotations
    /// </summary>
    public interface IComplexAnnotationReference
    {
        /// <summary>
        /// Gets a reference parameter to the annotation type - this can be null
        /// </summary>
        IComplexTextReference TypeReference { get; }

        /// <summary>
        /// Gets a reference parameter to the annotation title - this can be null
        /// </summary>
        IComplexTextReference TitleReference { get; }

        /// <summary>
        /// Gets a reference parameter to the annotation text - this can be null
        /// </summary>
        IComplexTextReference TextReference { get; }
    }
}
