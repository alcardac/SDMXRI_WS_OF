// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataflowXmlBuilder.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The dataflow xml bean builder.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.Structureparser.Builder.XmlSerialization.V2
{
    using System;
    using System.Collections.Generic;

    using Org.Sdmx.Resources.SdmxMl.Schemas.V20.structure;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.CategoryScheme;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.DataStructure;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;
    using Org.Sdmxsource.Util;

    /// <summary>
    ///     The dataflow xml bean builder.
    /// </summary>
    public class DataflowXmlBuilder : AbstractBuilder
    {
        #region Public Methods and Operators

        /// <summary>
        /// Build <see cref="DataflowType"/> from <paramref name="buildFrom"/>.
        /// </summary>
        /// <param name="buildFrom">
        /// The build from.
        /// </param>
        /// <param name="categorisations">
        /// The categorisations 
        /// </param>
        /// <returns>
        /// The <see cref="DataflowType"/> from <paramref name="buildFrom"/> .
        /// </returns>
        public DataflowType Build(IDataflowObject buildFrom, ISet<ICategorisationObject> categorisations)
        {
            var builtObj = new DataflowType();
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
                builtObj.uri = buildFrom.StructureUrl;
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
                        IMaintainableRefObject maintainableReference = refBean.MaintainableReference;
                        string value3 = maintainableReference.AgencyId;
                        if (!string.IsNullOrWhiteSpace(value3))
                        {
                            categoryRefType.CategorySchemeAgencyID = maintainableReference.AgencyId;
                        }

                        string value4 = maintainableReference.MaintainableId;
                        if (!string.IsNullOrWhiteSpace(value4))
                        {
                            categoryRefType.CategorySchemeID = maintainableReference.MaintainableId;
                        }

                        string value5 = maintainableReference.Version;
                        if (!string.IsNullOrWhiteSpace(value5))
                        {
                            categoryRefType.CategorySchemeVersion = maintainableReference.Version;
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

                        if (ObjectUtil.ValidString(refBean.TargetUrn))
                        {
                            categoryRefType.URN = refBean.TargetUrn;
                        }
                    }
                }
            }

            if (buildFrom.DataStructureRef != null)
            {
                KeyFamilyRefType keyFamilyRefType = builtObj.KeyFamilyRef = new KeyFamilyRefType();
                ICrossReference dataStructureRef = buildFrom.DataStructureRef;
                IMaintainableRefObject refBean0 = dataStructureRef.MaintainableReference;
                string value3 = refBean0.AgencyId;
                if (!string.IsNullOrWhiteSpace(value3))
                {
                    keyFamilyRefType.KeyFamilyAgencyID = refBean0.AgencyId;
                }

                string value4 = refBean0.MaintainableId;
                if (!string.IsNullOrWhiteSpace(value4))
                {
                    keyFamilyRefType.KeyFamilyID = refBean0.MaintainableId;
                }

                string value5 = refBean0.Version;
                if (!string.IsNullOrWhiteSpace(value5))
                {
                    keyFamilyRefType.Version = refBean0.Version;
                }

                if (ObjectUtil.ValidString(dataStructureRef.TargetUrn))
                {
                    keyFamilyRefType.URN = dataStructureRef.TargetUrn;
                }
            }

            return builtObj;
        }

        #endregion
    }
}