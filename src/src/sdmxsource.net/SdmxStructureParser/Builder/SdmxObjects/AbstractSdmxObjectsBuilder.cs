// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AbstractSdmxObjectsBuilder.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The abstract sdmx beans builder.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.Structureparser.Builder.SdmxObjects
{
    using System;
    using System.Collections.Generic;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Exception;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;

    /// <summary>
    ///     The abstract sdmx beans builder.
    /// </summary>
    public abstract class AbstractSdmxObjectsBuilder
    {
        #region Methods

        /// <summary>
        /// Adds a bean to the container as long as the urn of the bean to add is not contained in the set of URNs.
        ///     <p/>
        ///     If successfully added, will add the bean urn to the set of urns
        /// </summary>
        /// <param name="beans">
        /// container to add to
        /// </param>
        /// <param name="urns">
        /// to exclude
        /// </param>
        /// <param name="bean">
        /// to add to the beans container
        /// </param>
        protected internal void AddIfNotDuplicateURN(ISdmxObjects beans, ISet<Uri> urns, IIdentifiableObject bean)
        {
            if (!urns.Add(bean.Urn))
            {
                throw new SdmxSemmanticException(ExceptionCode.DuplicateUrn, bean.Urn);
            }

            beans.AddIdentifiable(bean);
        }

        #endregion
    }
}