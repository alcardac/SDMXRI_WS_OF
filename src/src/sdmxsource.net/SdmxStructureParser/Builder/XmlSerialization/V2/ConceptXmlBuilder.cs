// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConceptXmlBuilder.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The concept xml bean builder.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.Structureparser.Builder.XmlSerialization.V2
{
    using System;
    using System.Collections.Generic;

    using Org.Sdmx.Resources.SdmxMl.Schemas.V20.structure;
    using Org.Sdmxsource.Sdmx.Api.Builder;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.ConceptScheme;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;
    using Org.Sdmxsource.Util;

    /// <summary>
    ///     The concept xml bean builder.
    /// </summary>
    public class ConceptXmlBuilder : AbstractBuilder, IBuilder<ConceptType, IConceptObject>
    {
        #region Public Methods and Operators

        /// <summary>
        /// Build <see cref="ConceptType"/> from <paramref name="buildFrom"/>.
        /// </summary>
        /// <param name="buildFrom">
        /// The build from.
        /// </param>
        /// <returns>
        /// The <see cref="ConceptType"/> from <paramref name="buildFrom"/> .
        /// </returns>
        public virtual ConceptType Build(IConceptObject buildFrom)
        {
            var builtObj = new ConceptType();

            string str0 = buildFrom.Id;
            if (!string.IsNullOrWhiteSpace(str0))
            {
                builtObj.id = buildFrom.Id;
            }

            if (buildFrom.Uri != null)
            {
                builtObj.uri = buildFrom.Uri;
            }

            builtObj.urn = buildFrom.Urn;

            IList<ITextTypeWrapper> names = buildFrom.Names;
            if (ObjectUtil.ValidCollection(names))
            {
                builtObj.Name = this.GetTextType(names);
            }

            IList<ITextTypeWrapper> descriptions = buildFrom.Descriptions;
            if (ObjectUtil.ValidCollection(descriptions))
            {
                builtObj.Description = this.GetTextType(descriptions);
            }

            if (this.HasAnnotations(buildFrom))
            {
                builtObj.Annotations = this.GetAnnotationsType(buildFrom);
            }

            string str3 = buildFrom.ParentConcept;
            if (!string.IsNullOrWhiteSpace(str3))
            {
                builtObj.parent = buildFrom.ParentConcept;
            }

            string str2 = buildFrom.ParentAgency;
            if (!string.IsNullOrWhiteSpace(str2))
            {
                builtObj.parentAgency = buildFrom.ParentAgency;
            }

            if (buildFrom.CoreRepresentation != null)
            {
                if (buildFrom.CoreRepresentation.Representation != null)
                {
                    IMaintainableRefObject maintRef =
                        buildFrom.CoreRepresentation.Representation.MaintainableReference;
                    builtObj.coreRepresentation = maintRef.MaintainableId;
                    builtObj.coreRepresentationAgency = maintRef.AgencyId;
                }

                if (buildFrom.CoreRepresentation.TextFormat != null)
                {
                    var textFormatType = new TextFormatType();
                    this.PopulateTextFormatType(textFormatType, buildFrom.CoreRepresentation.TextFormat);
                    builtObj.TextFormat = textFormatType;
                }
            }

            return builtObj;
        }

        #endregion
    }
}