// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MetadataflowXmlBuilder.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The metadataflow xml bean builder.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.Structureparser.Builder.XmlSerialization.V2
{
    using System;
    using System.Collections.Generic;

    using Org.Sdmx.Resources.SdmxMl.Schemas.V20.structure;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.CategoryScheme;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.MetadataStructure;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;
    using Org.Sdmxsource.Util;

    /// <summary>
    ///     The metadataflow xml bean builder.
    /// </summary>
    public class MetadataflowXmlBuilder : AbstractBuilder
    {
        #region Public Methods and Operators

        /// <summary>
        /// Build <see cref="MetadataflowType"/> from <paramref name="buildFrom"/>.
        /// </summary>
        /// <param name="buildFrom">
        /// The build from.
        /// </param>
        /// <param name="categorisations">
        /// the categorisations 
        /// </param>
        /// <returns>
        /// The <see cref="MetadataflowType"/> from <paramref name="buildFrom"/> .
        /// </returns>
        public MetadataflowType Build(IMetadataFlow buildFrom, ISet<ICategorisationObject> categorisations)
        {
            var builtObj = new MetadataflowType();
            string value = buildFrom.AgencyId;
            if (!string.IsNullOrWhiteSpace(value))
            {
                builtObj.agencyID = buildFrom.AgencyId;
            }

            string value1 = buildFrom.Id;
            if (!string.IsNullOrWhiteSpace(value1))
            {
                builtObj.id = buildFrom.Id;
            }

            if (buildFrom.Uri != null)
            {
                builtObj.uri = buildFrom.Uri;
            }
            else if (buildFrom.StructureUrl != null)
            {
                builtObj.uri = buildFrom.StructureUrl;
            }
            else if (buildFrom.ServiceUrl != null)
            {
                builtObj.uri = buildFrom.ServiceUrl;
            }

            if (ObjectUtil.ValidString(buildFrom.Urn))
            {
                builtObj.urn = buildFrom.Urn;
            }

            string value2 = buildFrom.Version;
            if (!string.IsNullOrWhiteSpace(value2))
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

            if (ObjectUtil.ValidCollection(categorisations))
            {
                /* foreach */
                foreach (ICategorisationObject currentCategoryRef in categorisations)
                {
                    var categoryRefType = new CategoryRefType();
                    builtObj.CategoryRef.Add(categoryRefType);
                    ICrossReference refBean = currentCategoryRef.CategoryReference;
                    if (refBean != null)
                    {
                        IMaintainableRefObject xref = refBean.MaintainableReference;
                        string value3 = xref.AgencyId;
                        if (!string.IsNullOrWhiteSpace(value3))
                        {
                            categoryRefType.CategorySchemeAgencyID = xref.AgencyId;
                        }

                        string value4 = xref.MaintainableId;
                        if (!string.IsNullOrWhiteSpace(value4))
                        {
                            categoryRefType.CategorySchemeID = xref.MaintainableId;
                        }

                        string value5 = xref.Version;
                        if (!string.IsNullOrWhiteSpace(value5))
                        {
                            categoryRefType.CategorySchemeVersion = xref.Version;
                        }

                        CategoryIDType idType = null;
                        IIdentifiableRefObject childRef = refBean.ChildReference;
                        int i = 0;
                        while (childRef != null)
                        {
                            if (i == 0 || idType == null)
                            {
                                idType = categoryRefType.CategoryID = new CategoryIDType();
                            }
                            else
                            {
                                idType = idType.CategoryID = new CategoryIDType();
                            }

                            idType.ID = childRef.Id;
                            childRef = childRef.ChildReference;
                            i++;
                        }

                        categoryRefType.URN = refBean.TargetUrn;
                    }
                }
            }

            if (buildFrom.MetadataStructureRef != null)
            {
                MetadataStructureRefType mdsRefType = builtObj.MetadataStructureRef = new MetadataStructureRefType();

                ICrossReference refBean0 = buildFrom.MetadataStructureRef;
                if (refBean0 != null)
                {
                    IMaintainableRefObject maintainableReference = refBean0.MaintainableReference;
                    string str4 = maintainableReference.AgencyId;
                    if (!string.IsNullOrWhiteSpace(str4))
                    {
                        mdsRefType.MetadataStructureAgencyID = maintainableReference.AgencyId;
                    }

                    string value3 = maintainableReference.MaintainableId;
                    if (!string.IsNullOrWhiteSpace(value3))
                    {
                        mdsRefType.MetadataStructureID = maintainableReference.MaintainableId;
                    }

                    string value4 = maintainableReference.Version;
                    if (!string.IsNullOrWhiteSpace(value4))
                    {
                        mdsRefType.Version = maintainableReference.Version;
                    }

                    if (ObjectUtil.ValidString(refBean0.TargetUrn))
                    {
                        mdsRefType.URN = refBean0.TargetUrn;
                    }
                }
            }

            return builtObj;
        }

        #endregion
    }
}