// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IdentifiableAssembler.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The identifiable bean assembler.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.Structureparser.Builder.XmlSerialization.V21.Assemblers
{
    using System;

    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Common;
    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Structure;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Util;

    /// <summary>
    ///     The identifiable bean assembler.
    /// </summary>
    public abstract class IdentifiableAssembler : AbstractAssembler
    {
        #region Public Methods and Operators

        /// <summary>
        /// The assemble identifiable.
        /// </summary>
        /// <param name="assembleInto">
        /// The assemble into.
        /// </param>
        /// <param name="assembleFrom">
        /// The assemble from.
        /// </param>
        public void AssembleIdentifiable(IdentifiableType assembleInto, IIdentifiableObject assembleFrom)
        {
            if (this.HasAnnotations(assembleFrom))
            {
                assembleInto.Annotations = new Annotations(this.GetAnnotationsType(assembleFrom));
            }

            string str0 = assembleFrom.Id;
            if (!string.IsNullOrWhiteSpace(str0))
            {
                assembleInto.id = assembleFrom.Id;
            }

            if (assembleFrom.Uri != null)
            {
                assembleInto.uri = assembleFrom.Uri;
            }

            assembleInto.urn = assembleFrom.Urn;
        }

        #endregion
    }
}