// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NameableAssembler.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The nameable bean assembler.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.Structureparser.Builder.XmlSerialization.V21.Assemblers
{
    using System.Collections.Generic;

    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Structure;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Util;

    /// <summary>
    ///     The nameable bean assembler.
    /// </summary>
    public class NameableAssembler : IdentifiableAssembler
    {
        #region Public Methods and Operators

        /// <summary>
        /// Assemble SDMX nameable artefact.
        /// </summary>
        /// <param name="assembleInto">
        /// The assemble into.
        /// </param>
        /// <param name="assembleFrom">
        /// The assemble from.
        /// </param>
        public void AssembleNameable(NameableType assembleInto, INameableObject assembleFrom)
        {
            this.AssembleIdentifiable(assembleInto, assembleFrom);
            IList<ITextTypeWrapper> names = assembleFrom.Names;
            if (ObjectUtil.ValidCollection(names))
            {
                this.GetTextType(names, assembleInto.Name);
            }

            IList<ITextTypeWrapper> descriptions = assembleFrom.Descriptions;
            if (ObjectUtil.ValidCollection(descriptions))
            {
                this.GetTextType(descriptions, assembleInto.Description);
            }
        }

        #endregion
    }
}