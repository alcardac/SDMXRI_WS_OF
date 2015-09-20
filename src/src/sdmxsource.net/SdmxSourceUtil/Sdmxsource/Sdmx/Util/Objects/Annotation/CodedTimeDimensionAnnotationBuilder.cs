// -----------------------------------------------------------------------
// <copyright file="CodedTimeDimensionAnnotationBuilder.cs" company="EUROSTAT">
//   Date Created : 2014-12-09
//   Copyright (c) 2014 by the European   Commission, represented by Eurostat.   All rights reserved.
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// -----------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.Util.Objects.Annotation
{
    using System.Globalization;

    using Org.Sdmxsource.Sdmx.Api.Exception;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;

    /// <summary>
    /// The coded time dimension annotation builder.
    /// </summary>
    /// <typeparam name="TImpl">
    /// The concrete implementation of <see cref="IAnnotationMutableObject"/>
    /// </typeparam>
    public class CodedTimeDimensionAnnotationBuilder<TImpl> : IAnnotationBuilder<IMaintainableRefObject>
        where TImpl : IAnnotationMutableObject, new()
    {
        #region Public Methods and Operators

        /// <summary>
        /// Builds an <see cref="IAnnotationMutableObject"/> from the specified <paramref name="buildFrom"/>
        /// </summary>
        /// <param name="buildFrom">
        /// An Object to build the output object from
        /// </param>
        /// <returns>
        /// The <see cref="IAnnotableMutableObject"/>
        /// </returns>
        /// <exception cref="SdmxException">
        /// - If anything goes wrong during the build process
        /// </exception>
        public IAnnotationMutableObject Build(IMaintainableRefObject buildFrom)
        {
            var impl = new TImpl { Title = AnnotationConstant.CodeTimeDimensionTitle, Type = string.Format(CultureInfo.InvariantCulture, "{0}:{1}({2})", buildFrom.AgencyId, buildFrom.MaintainableId, buildFrom.Version) };
            impl.AddText("en", "The TimeDimension has coded representation. But SDMX v2.1 does not allow coded representation for TimeDimenions.");
            return impl;
        }

        #endregion
    }
}