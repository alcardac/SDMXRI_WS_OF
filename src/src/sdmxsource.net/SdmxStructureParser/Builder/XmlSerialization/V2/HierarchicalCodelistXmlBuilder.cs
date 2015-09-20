// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HierarchicalCodelistXmlBuilder.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The hierarchical codelist xml bean builder.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.Structureparser.Builder.XmlSerialization.V2
{
    using System;
    using System.Collections.Generic;

    using Org.Sdmx.Resources.SdmxMl.Schemas.V20.structure;
    using Org.Sdmxsource.Sdmx.Api.Builder;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Codelist;
    using Org.Sdmxsource.Util;

    /// <summary>
    ///     The hierarchical codelist xml bean builder.
    /// </summary>
    public class HierarchicalCodelistXmlBuilder : AbstractBuilder, 
                                                  IBuilder<HierarchicalCodelistType, IHierarchicalCodelistObject>
    {
        #region Fields

        /// <summary>
        ///     The codelist ref xml bean builder.
        /// </summary>
        private readonly CodelistRefXmlBuilder _codelistRefXmlBuilder = new CodelistRefXmlBuilder();

        /// <summary>
        ///     The hierarchy xml bean builder.
        /// </summary>
        private readonly HierarchyXmlBuilder _hierarchyXmlBuilder = new HierarchyXmlBuilder();

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Build <see cref="HierarchicalCodelistType"/> from <paramref name="buildFrom"/>.
        /// </summary>
        /// <param name="buildFrom">
        /// The build from.
        /// </param>
        /// <returns>
        /// The <see cref="HierarchicalCodelistType"/> from <paramref name="buildFrom"/> .
        /// </returns>
        public virtual HierarchicalCodelistType Build(IHierarchicalCodelistObject buildFrom)
        {
            var builtObj = new HierarchicalCodelistType();
            string value1 = buildFrom.AgencyId;
            if (!string.IsNullOrWhiteSpace(value1))
            {
                builtObj.agencyID = buildFrom.AgencyId;
            }

            string value2 = buildFrom.Id;
            if (!string.IsNullOrWhiteSpace(value2))
            {
                builtObj.id = buildFrom.Id;
            }

            if (buildFrom.Uri != null)
            {
                builtObj.uri = buildFrom.Uri;
            }

            if (ObjectUtil.ValidString(buildFrom.Urn))
            {
                builtObj.urn = buildFrom.Urn;
            }

            string value = buildFrom.Version;
            if (!string.IsNullOrWhiteSpace(value))
            {
                builtObj.version = buildFrom.Version;
            }

            if (buildFrom.StartDate != null)
            {
                builtObj.validFrom = buildFrom.StartDate.Date;
            }

            if (buildFrom.EndDate != null)
            {
                builtObj.validTo = buildFrom.EndDate.Date;
            }

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

            if (buildFrom.IsExternalReference.IsSet())
            {
                builtObj.isExternalReference = buildFrom.IsExternalReference.IsTrue;
            }

            if (buildFrom.IsFinal.IsSet())
            {
                builtObj.isFinal = buildFrom.IsFinal.IsTrue;
            }

            IList<IHierarchy> hierarchyBeans = buildFrom.Hierarchies;
            if (ObjectUtil.ValidCollection(hierarchyBeans))
            {
                /* foreach */
                foreach (IHierarchy hierarchyBean in hierarchyBeans)
                {
                    builtObj.Hierarchy.Add(this._hierarchyXmlBuilder.Build(hierarchyBean));
                }
            }

            IList<ICodelistRef> codelistRefBeans = buildFrom.CodelistRef;
            if (ObjectUtil.ValidCollection(codelistRefBeans))
            {
                /* foreach */
                foreach (ICodelistRef codelistRefBean in codelistRefBeans)
                {
                    builtObj.CodelistRef.Add(this._codelistRefXmlBuilder.Build(codelistRefBean));
                }
            }

            return builtObj;
        }

        #endregion
    }
}