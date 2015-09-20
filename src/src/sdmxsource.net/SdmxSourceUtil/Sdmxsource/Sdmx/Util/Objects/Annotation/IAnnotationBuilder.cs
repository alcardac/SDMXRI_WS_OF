// -----------------------------------------------------------------------
// <copyright file="IAnnotationBuilder.cs" company="EUROSTAT">
//   Date Created : 2014-12-09
//   Copyright (c) 2014 by the European   Commission, represented by Eurostat.   All rights reserved.
// 
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// -----------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.Util.Objects.Annotation
{
    using Org.Sdmxsource.Sdmx.Api.Builder;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base;

    /// <summary>
    /// The AnnotationBuilder interface.
    /// </summary>
    /// <typeparam name="T">
    /// The input type.
    /// </typeparam>
    public interface IAnnotationBuilder<in T> : IBuilder<IAnnotationMutableObject, T>
    {
    }
}