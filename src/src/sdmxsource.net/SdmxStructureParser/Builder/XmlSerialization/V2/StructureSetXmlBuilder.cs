// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StructureSetXmlBuilder.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The structure set xml bean builder.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.Structureparser.Builder.XmlSerialization.V2
{
    using System;
    using System.Collections.Generic;

    using Org.Sdmx.Resources.SdmxMl.Schemas.V20.structure;
    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Mapping;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;
    using Org.Sdmxsource.Util;

    /// <summary>
    ///     The structure set xml bean builder.
    /// </summary>
    public class StructureSetXmlBuilder : AbstractBuilder
    {
        #region Public Methods and Operators

        /// <summary>
        /// Build <see cref="StructureSetType"/> from <paramref name="buildFrom"/>.
        /// </summary>
        /// <param name="buildFrom">
        /// The source SDMX Object.
        /// </param>
        /// <returns>
        /// The <see cref="StructureSetType"/> from <paramref name="buildFrom"/> .
        /// </returns>
        public StructureSetType Build(IStructureSetObject buildFrom)
        {
            var builtObj = new StructureSetType();

            // MAINTAINABLE ATTRIBUTES
            string str0 = buildFrom.AgencyId;
            if (!string.IsNullOrWhiteSpace(str0))
            {
                builtObj.agencyID = buildFrom.AgencyId;
            }

            string str1 = buildFrom.Id;
            if (!string.IsNullOrWhiteSpace(str1))
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
            
            builtObj.urn = buildFrom.Urn;

            string str3 = buildFrom.Version;
            if (!string.IsNullOrWhiteSpace(str3))
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

            // MAPS
            ProcessRelatedStructures(builtObj, buildFrom);
            if (buildFrom.StructureMapList != null)
            {
                /* foreach */
                foreach (IStructureMapObject each in buildFrom.StructureMapList)
                {
                    this.ProcessStructureMap(builtObj, each);
                }
            }

            if (buildFrom.CodelistMapList != null)
            {
                /* foreach */
                foreach (ICodelistMapObject each0 in buildFrom.CodelistMapList)
                {
                    this.ProcessCodelistMap(builtObj, each0);
                }
            }

            if (buildFrom.CategorySchemeMapList != null)
            {
                /* foreach */
                foreach (ICategorySchemeMapObject each1 in buildFrom.CategorySchemeMapList)
                {
                    this.ProcessCategorySchemeMap(builtObj, each1);
                }
            }

            if (buildFrom.ConceptSchemeMapList != null)
            {
                /* foreach */
                foreach (IConceptSchemeMapObject each2 in buildFrom.ConceptSchemeMapList)
                {
                    this.ProcessConceptSchemeMap(builtObj, each2);
                }
            }

            if (buildFrom.OrganisationSchemeMapList != null)
            {
                /* foreach */
                foreach (IOrganisationSchemeMapObject each3 in buildFrom.OrganisationSchemeMapList)
                {
                    this.ProcessOrganisationSchemeMap(builtObj, each3);
                }
            }

            return builtObj;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Process related structures.
        /// </summary>
        /// <param name="structureSetType">
        /// The structure set type.
        /// </param>
        /// <param name="buildFrom">
        /// The build from.
        /// </param>
        private static void ProcessRelatedStructures(StructureSetType structureSetType, IStructureSetObject buildFrom)
        {
            if (buildFrom.RelatedStructures != null)
            {
                var relatedStructuresType = new RelatedStructuresType();
                structureSetType.RelatedStructures = relatedStructuresType;
                IRelatedStructures relatedStructures = buildFrom.RelatedStructures;

                // KEY FAMILY REF
                foreach (ICrossReference xref in relatedStructures.DataStructureRef)
                {
                    var keyFamilyRefType = new KeyFamilyRefType();
                    relatedStructuresType.KeyFamilyRef.Add(keyFamilyRefType);
                    SetDataStructureRefAttributes(keyFamilyRefType, xref);
                }

                // METADATA STRUCTURE REF
                foreach (ICrossReference ref0 in relatedStructures.MetadataStructureRef)
                {
                    var refType = new MetadataStructureRefType();
                    relatedStructuresType.MetadataStructureRef.Add(refType);
                    SetMetadataStructureRefAttributes(refType, ref0);
                }

                // CONCEPT SCHEME REF
                foreach (ICrossReference ref1 in relatedStructures.ConceptSchemeRef)
                {
                    var refType2 = new ConceptSchemeRefType();
                    relatedStructuresType.ConceptSchemeRef.Add(refType2);
                    SetConceptSchemeRefAttributes(refType2, ref1);
                }

                // CATEGORY SCHEME REF
                foreach (ICrossReference ref3 in relatedStructures.CategorySchemeRef)
                {
                    var refType4 = new CategorySchemeRefType();
                    relatedStructuresType.CategorySchemeRef.Add(refType4);
                    SetCategorySchemeRefAttributes(refType4, ref3);
                }

                // ORGANISATION SCHEME REF
                foreach (ICrossReference ref5 in relatedStructures.OrgSchemeRef)
                {
                    var refType6 = new OrganisationSchemeRefType();
                    relatedStructuresType.OrganisationSchemeRef.Add(refType6);
                    SetOrganisationSchemeRefAttributes(refType6, ref5);
                }

                // HCL REF
                foreach (ICrossReference ref7 in relatedStructures.HierCodelistRef)
                {
                    var refType8 = new HierarchicalCodelistRefType();
                    relatedStructuresType.HierarchicalCodelistRef.Add(refType8);
                    SetHclRefAttributes(refType8, ref7);
                }
            }
        }

        /// <summary>
        /// Sets category id attributes.
        /// </summary>
        /// <param name="categoryIdType">
        /// The category id type.
        /// </param>
        /// <param name="categoryIds">
        /// The category id list.
        /// </param>
        private static void SetCategoryIdAttributes(CategoryIDType categoryIdType, IList<string> categoryIds)
        {
            if (categoryIds == null)
            {
                return;
            }

            int currentPos = 0;

            while (categoryIds.Count > currentPos)
            {
                categoryIdType.ID = categoryIds[currentPos];

                // FIXME TODO java bug ?
                // in 0.9.1 current pos remains the same.
                currentPos++;
                if (categoryIds.Count > currentPos)
                {
                    var subType = new CategoryIDType();
                    categoryIdType.CategoryID = subType;
                    categoryIdType = subType;
                }
            }

            // Original recursive
            ////if (categoryIds.Count > currentPos)
            ////{
            ////    categoryIdType.ID = categoryIds[currentPos];
            ////     nextPos = ++currentPos;
            ////    if (categoryIds.Count > nextPos)
            ////    {
            ////        var subType = new CategoryIDType();
            ////        categoryIdType.CategoryID = subType;
            ////        this.SetCategoryIdAttributes(subType, categoryIds, nextPos);
            ////    }
            ////}
        }

        /// <summary>
        /// The set category scheme ref attributes.
        /// </summary>
        /// <param name="refType">
        /// The ref type.
        /// </param>
        /// <param name="xref">
        /// The crossReference.
        /// </param>
        private static void SetCategorySchemeRefAttributes(CategorySchemeRefType refType, IStructureReference xref)
        {
            IMaintainableRefObject maintainableReference = xref.MaintainableReference;
            string str0 = maintainableReference.AgencyId;
            if (!string.IsNullOrWhiteSpace(str0))
            {
                refType.AgencyID = maintainableReference.AgencyId;
            }

            string str1 = maintainableReference.MaintainableId;
            if (!string.IsNullOrWhiteSpace(str1))
            {
                refType.CategorySchemeID = maintainableReference.MaintainableId;
            }

            refType.URN = xref.TargetUrn;

            string str3 = maintainableReference.Version;
            if (!string.IsNullOrWhiteSpace(str3))
            {
                refType.Version = maintainableReference.Version;
            }
        }

        /// <summary>
        /// The set codelist ref attributes.
        /// </summary>
        /// <param name="refType">
        /// The ref type.
        /// </param>
        /// <param name="xref">
        /// The crossReference.
        /// </param>
        private static void SetCodelistRefAttributes(CodelistRefType refType, IStructureReference xref)
        {
            IMaintainableRefObject maintainableReference = xref.MaintainableReference;
            string str0 = maintainableReference.AgencyId;
            if (!string.IsNullOrWhiteSpace(str0))
            {
                refType.AgencyID = maintainableReference.AgencyId;
            }

            string str1 = maintainableReference.MaintainableId;
            if (!string.IsNullOrWhiteSpace(str1))
            {
                refType.CodelistID = maintainableReference.MaintainableId;
            }

            refType.URN = xref.TargetUrn;

            string str3 = maintainableReference.Version;
            if (!string.IsNullOrWhiteSpace(str3))
            {
                refType.Version = maintainableReference.Version;
            }
        }

        /// <summary>
        /// The set concept scheme ref attributes.
        /// </summary>
        /// <param name="refType">
        /// The ref type.
        /// </param>
        /// <param name="xref">
        /// The crossReference.
        /// </param>
        private static void SetConceptSchemeRefAttributes(ConceptSchemeRefType refType, IStructureReference xref)
        {
            IMaintainableRefObject maintainableReference = xref.MaintainableReference;
            string str0 = maintainableReference.AgencyId;
            if (!string.IsNullOrWhiteSpace(str0))
            {
                refType.AgencyID = maintainableReference.AgencyId;
            }

            string str1 = maintainableReference.MaintainableId;
            if (!string.IsNullOrWhiteSpace(str1))
            {
                refType.ConceptSchemeID = maintainableReference.MaintainableId;
            }

            refType.URN = xref.TargetUrn;

            string str3 = maintainableReference.Version;
            if (!string.IsNullOrWhiteSpace(str3))
            {
                refType.Version = maintainableReference.Version;
            }
        }

        /// <summary>
        /// Sets the attributes of <paramref name="refType"/>
        /// </summary>
        /// <param name="refType">
        /// The reference type
        /// </param>
        /// <param name="crossReference">
        /// The cross-reference
        /// </param>
        private static void SetDataStructureRefAttributes(KeyFamilyRefType refType, IStructureReference crossReference)
        {
            IMaintainableRefObject maintainableReference = crossReference.MaintainableReference;
            string str0 = maintainableReference.AgencyId;
            if (!string.IsNullOrWhiteSpace(str0))
            {
                refType.KeyFamilyAgencyID = maintainableReference.AgencyId;
            }

            string str1 = maintainableReference.MaintainableId;
            if (!string.IsNullOrWhiteSpace(str1))
            {
                refType.KeyFamilyID = maintainableReference.MaintainableId;
            }

            refType.URN = crossReference.TargetUrn;

            string str3 = maintainableReference.Version;
            if (!string.IsNullOrWhiteSpace(str3))
            {
                refType.Version = maintainableReference.Version;
            }
        }

        /// <summary>
        /// Sets the attributes of <paramref name="refType"/>
        /// </summary>
        /// <param name="refType">
        /// The reference type
        /// </param>
        /// <param name="crossReference">
        /// The cross-reference
        /// </param>
        private static void SetHclRefAttributes(HierarchicalCodelistRefType refType, IStructureReference crossReference)
        {
            IMaintainableRefObject maintainableReference = crossReference.MaintainableReference;
            string value = maintainableReference.AgencyId;
            if (!string.IsNullOrWhiteSpace(value))
            {
                refType.AgencyID = maintainableReference.AgencyId;
            }

            string value1 = maintainableReference.MaintainableId;
            if (!string.IsNullOrWhiteSpace(value1))
            {
                refType.HierarchicalCodelistID = maintainableReference.MaintainableId;
            }

            if (ObjectUtil.ValidString(crossReference.TargetUrn))
            {
                refType.URN = crossReference.TargetUrn;
            }

            string str3 = maintainableReference.Version;
            if (!string.IsNullOrWhiteSpace(str3))
            {
                refType.Version = maintainableReference.Version;
            }
        }

        /// <summary>
        /// Sets the attributes of <paramref name="refType"/>
        /// </summary>
        /// <param name="refType">
        /// The reference type
        /// </param>
        /// <param name="crossReference">
        /// The cross-reference
        /// </param>
        private static void SetMetadataStructureRefAttributes(
            MetadataStructureRefType refType, IStructureReference crossReference)
        {
            IMaintainableRefObject maintainableReference = crossReference.MaintainableReference;
            string str0 = maintainableReference.AgencyId;
            if (!string.IsNullOrWhiteSpace(str0))
            {
                refType.MetadataStructureAgencyID = maintainableReference.AgencyId;
            }

            string str1 = maintainableReference.MaintainableId;
            if (!string.IsNullOrWhiteSpace(str1))
            {
                refType.MetadataStructureID = maintainableReference.MaintainableId;
            }

            refType.URN = crossReference.TargetUrn;

            string str3 = maintainableReference.Version;
            if (!string.IsNullOrWhiteSpace(str3))
            {
                refType.Version = maintainableReference.Version;
            }
        }

        /// <summary>
        /// Sets the attributes of <paramref name="refType"/>
        /// </summary>
        /// <param name="refType">
        /// The reference type
        /// </param>
        /// <param name="crossReference">
        /// The cross-reference
        /// </param>
        private static void SetOrganisationSchemeRefAttributes(
            OrganisationSchemeRefType refType, IStructureReference crossReference)
        {
            IMaintainableRefObject maintainableReference = crossReference.MaintainableReference;
            string str0 = maintainableReference.AgencyId;
            if (!string.IsNullOrWhiteSpace(str0))
            {
                refType.AgencyID = maintainableReference.AgencyId;
            }

            string str1 = maintainableReference.MaintainableId;
            if (!string.IsNullOrWhiteSpace(str1))
            {
                refType.OrganisationSchemeID = maintainableReference.MaintainableId;
            }

            refType.URN = crossReference.TargetUrn;

            string str3 = maintainableReference.Version;
            if (!string.IsNullOrWhiteSpace(str3))
            {
                refType.Version = maintainableReference.Version;
            }
        }

        /// <summary>
        /// The process category scheme map.
        /// </summary>
        /// <param name="structureSetType">
        /// The structure set type.
        /// </param>
        /// <param name="buildFrom">
        /// The build from.
        /// </param>
        private void ProcessCategorySchemeMap(StructureSetType structureSetType, ICategorySchemeMapObject buildFrom)
        {
            var catSchemeMap = new CategorySchemeMapType();
            structureSetType.CategorySchemeMap = catSchemeMap;

            string value = buildFrom.Id;
            if (!string.IsNullOrWhiteSpace(value))
            {
                catSchemeMap.id = buildFrom.Id;
            }

            IList<ITextTypeWrapper> names = buildFrom.Names;
            if (ObjectUtil.ValidCollection(names))
            {
                catSchemeMap.Name = this.GetTextType(names);
            }

            IList<ITextTypeWrapper> descriptions = buildFrom.Descriptions;
            if (ObjectUtil.ValidCollection(descriptions))
            {
                catSchemeMap.Description = this.GetTextType(descriptions);
            }

            if (buildFrom.SourceRef != null)
            {
                var catRefType = new CategorySchemeRefType();
                catSchemeMap.CategorySchemeRef = catRefType;
                SetCategorySchemeRefAttributes(catRefType, buildFrom.SourceRef);
            }

            if (buildFrom.TargetRef != null)
            {
                var catRefType0 = new CategorySchemeRefType();
                catSchemeMap.TargetCategorySchemeRef = catRefType0;
                SetCategorySchemeRefAttributes(catRefType0, buildFrom.TargetRef);
            }

            IList<ICategoryMap> categoryMapBeans = buildFrom.CategoryMaps;
            if (ObjectUtil.ValidCollection(categoryMapBeans))
            {
                /* foreach */
                foreach (ICategoryMap categoryMapBean in categoryMapBeans)
                {
                    var categoryMapType = new CategoryMapType();
                    catSchemeMap.CategoryMap.Add(categoryMapType);
                    string value1 = categoryMapBean.Alias;
                    if (!string.IsNullOrWhiteSpace(value1))
                    {
                        categoryMapType.categoryAlias = categoryMapBean.Alias;
                    }

                    if (categoryMapBean.SourceId != null)
                    {
                        var categoryIdType = new CategoryIDType();
                        categoryMapType.CategoryID = categoryIdType;
                        SetCategoryIdAttributes(categoryIdType, categoryMapBean.SourceId);
                    }

                    if (categoryMapBean.TargetId != null)
                    {
                        var categoryIdType1 = new CategoryIDType();

                        categoryMapType.TargetCategoryID = categoryIdType1;
                        SetCategoryIdAttributes(categoryIdType1, categoryMapBean.TargetId);
                    }
                }
            }

            if (this.HasAnnotations(buildFrom))
            {
                catSchemeMap.Annotations = this.GetAnnotationsType(buildFrom);
            }
        }

        /// <summary>
        /// The process codelist map.
        /// </summary>
        /// <param name="structureSetType">
        /// The structure set type.
        /// </param>
        /// <param name="buildFrom">
        /// The build from.
        /// </param>
        private void ProcessCodelistMap(StructureSetType structureSetType, IItemSchemeMapObject buildFrom)
        {
            var codelistMap = new CodelistMapType();
            structureSetType.CodelistMap = codelistMap;

            string str0 = buildFrom.Id;
            if (!string.IsNullOrWhiteSpace(str0))
            {
                codelistMap.id = buildFrom.Id;
            }

            IList<ITextTypeWrapper> names = buildFrom.Names;
            if (ObjectUtil.ValidCollection(names))
            {
                codelistMap.Name = this.GetTextType(names);
            }

            IList<ITextTypeWrapper> descriptions = buildFrom.Descriptions;
            if (ObjectUtil.ValidCollection(descriptions))
            {
                codelistMap.Description = this.GetTextType(descriptions);
            }

            if (buildFrom.SourceRef != null)
            {
                if (buildFrom.SourceRef.TargetReference.EnumType == SdmxStructureEnumType.CodeList)
                {
                    var refType = new CodelistRefType();
                    codelistMap.CodelistRef = refType;
                    SetCodelistRefAttributes(refType, buildFrom.SourceRef);
                }
                else if (buildFrom.SourceRef.TargetReference.EnumType == SdmxStructureEnumType.HierarchicalCodelist)
                {
                    var refType0 = new HierarchicalCodelistRefType();
                    codelistMap.HierarchicalCodelistRef = refType0;
                    SetHclRefAttributes(refType0, buildFrom.SourceRef);
                }
            }

            if (buildFrom.TargetRef != null)
            {
                if (buildFrom.TargetRef.TargetReference.EnumType == SdmxStructureEnumType.CodeList)
                {
                    var refType1 = new CodelistRefType();
                    codelistMap.TargetCodelistRef = refType1;
                    SetCodelistRefAttributes(refType1, buildFrom.TargetRef);
                }
                else if (buildFrom.TargetRef.TargetReference.EnumType == SdmxStructureEnumType.HierarchicalCodelist)
                {
                    var refType2 = new HierarchicalCodelistRefType();
                    codelistMap.TargetHierarchicalCodelistRef = refType2;
                    SetHclRefAttributes(refType2, buildFrom.TargetRef);
                }
            }

            IList<IItemMap> itemMapBeans = buildFrom.Items;
            if (ObjectUtil.ValidCollection(itemMapBeans))
            {
                /* foreach */
                foreach (IItemMap itemMapBean in itemMapBeans)
                {
                    var codeMaptype = new CodeMapType();
                    codelistMap.CodeMap.Add(codeMaptype);
                    string value1 = itemMapBean.SourceId;
                    if (!string.IsNullOrWhiteSpace(value1))
                    {
                        codeMaptype.MapCodeRef = itemMapBean.SourceId;
                    }

                    string value = itemMapBean.TargetId;
                    if (!string.IsNullOrWhiteSpace(value))
                    {
                        codeMaptype.MapTargetCodeRef = itemMapBean.TargetId;
                    }
                }
            }

            if (this.HasAnnotations(buildFrom))
            {
                codelistMap.Annotations = this.GetAnnotationsType(buildFrom);
            }
        }

        /// <summary>
        /// The process component.
        /// </summary>
        /// <param name="componentMapType">
        /// The component map type.
        /// </param>
        /// <param name="componentMapBean">
        /// The component map bean.
        /// </param>
        private void ProcessComponent(ComponentMapType componentMapType, IComponentMapObject componentMapBean)
        {
            if (componentMapBean.MapConceptRef != null)
            {
                componentMapType.MapConceptRef = componentMapBean.MapConceptRef;
            }

            if (componentMapBean.MapTargetConceptRef != null)
            {
                componentMapType.MapTargetConceptRef = componentMapBean.MapTargetConceptRef;
            }

            if (componentMapBean.RepMapRef != null)
            {
                if (componentMapBean.RepMapRef.ToTextFormat != null)
                {
                    var textForamtType = new TextFormatType();
                    componentMapType.ToTextFormat = textForamtType;
                    this.PopulateTextFormatType(textForamtType, componentMapBean.RepMapRef.ToTextFormat);
                }

                switch (componentMapBean.RepMapRef.ToValueType)
                {
                    case ToValue.Description:
                        componentMapType.ToValueType = ToValueTypeTypeConstants.Description;
                        break;
                    case ToValue.Name:
                        componentMapType.ToValueType = ToValueTypeTypeConstants.Name;
                        break;
                    case ToValue.Value:
                        componentMapType.ToValueType = ToValueTypeTypeConstants.Value;
                        break;
                }
            }
        }

        /// <summary>
        /// The process concept scheme map.
        /// </summary>
        /// <param name="structureSetType">
        /// The structure set type.
        /// </param>
        /// <param name="buildFrom">
        /// The build from.
        /// </param>
        private void ProcessConceptSchemeMap(StructureSetType structureSetType, IItemSchemeMapObject buildFrom)
        {
            var conceptSchemeMap = new ConceptSchemeMapType();
            structureSetType.ConceptSchemeMap = conceptSchemeMap;

            string str0 = buildFrom.Id;
            if (!string.IsNullOrWhiteSpace(str0))
            {
                conceptSchemeMap.id = buildFrom.Id;
            }

            IList<ITextTypeWrapper> names = buildFrom.Names;
            if (ObjectUtil.ValidCollection(names))
            {
                conceptSchemeMap.Name = this.GetTextType(names);
            }

            IList<ITextTypeWrapper> descriptions = buildFrom.Descriptions;
            if (ObjectUtil.ValidCollection(descriptions))
            {
                conceptSchemeMap.Description = this.GetTextType(descriptions);
            }

            if (buildFrom.SourceRef != null)
            {
                var refType = new ConceptSchemeRefType();
                conceptSchemeMap.ConceptSchemeRef = refType;
                SetConceptSchemeRefAttributes(refType, buildFrom.SourceRef);
            }

            if (buildFrom.TargetRef != null)
            {
                var refType0 = new ConceptSchemeRefType();
                conceptSchemeMap.TargetConceptSchemeRef = refType0;
                SetConceptSchemeRefAttributes(refType0, buildFrom.TargetRef);
            }

            IList<IItemMap> itemMapBeans = buildFrom.Items;
            if (ObjectUtil.ValidCollection(itemMapBeans))
            {
                /* foreach */
                foreach (IItemMap itemMapBean in itemMapBeans)
                {
                    var conceptMaptype = new ConceptMapType();
                    conceptSchemeMap.ConceptMap.Add(conceptMaptype);
                    string str1 = itemMapBean.SourceId;
                    if (!string.IsNullOrWhiteSpace(str1))
                    {
                        conceptMaptype.ConceptID = itemMapBean.SourceId;
                    }

                    string str2 = itemMapBean.TargetId;
                    if (!string.IsNullOrWhiteSpace(str2))
                    {
                        conceptMaptype.TargetConceptID = itemMapBean.TargetId;
                    }
                }
            }

            if (this.HasAnnotations(buildFrom))
            {
                conceptSchemeMap.Annotations = this.GetAnnotationsType(buildFrom);
            }
        }

        /// <summary>
        /// The process organisation scheme map.
        /// </summary>
        /// <param name="structureSetType">
        /// The structure set type.
        /// </param>
        /// <param name="buildFrom">
        /// The build from.
        /// </param>
        private void ProcessOrganisationSchemeMap(StructureSetType structureSetType, IItemSchemeMapObject buildFrom)
        {
            var orgSchemeMap = new OrganisationSchemeMapType();
            structureSetType.OrganisationSchemeMap = orgSchemeMap;

            string str0 = buildFrom.Id;
            if (!string.IsNullOrWhiteSpace(str0))
            {
                orgSchemeMap.id = buildFrom.Id;
            }

            IList<ITextTypeWrapper> names = buildFrom.Names;
            if (ObjectUtil.ValidCollection(names))
            {
                orgSchemeMap.Name = this.GetTextType(names);
            }

            IList<ITextTypeWrapper> descriptions = buildFrom.Descriptions;
            if (ObjectUtil.ValidCollection(descriptions))
            {
                orgSchemeMap.Description = this.GetTextType(descriptions);
            }

            if (buildFrom.SourceRef != null)
            {
                var refType = new OrganisationSchemeRefType();
                orgSchemeMap.OrganisationSchemeRef = refType;
                SetOrganisationSchemeRefAttributes(refType, buildFrom.SourceRef);
            }

            if (buildFrom.TargetRef != null)
            {
                var refType0 = new OrganisationSchemeRefType();
                orgSchemeMap.TargetOrganisationSchemeRef = refType0;
                SetOrganisationSchemeRefAttributes(refType0, buildFrom.TargetRef);
            }

            IList<IItemMap> itemMapBeans = buildFrom.Items;
            if (ObjectUtil.ValidCollection(itemMapBeans))
            {
                /* foreach */
                foreach (IItemMap itemMapBean in itemMapBeans)
                {
                    var orgMaptype = new OrganisationMapType();
                    orgSchemeMap.OrganisationMap.Add(orgMaptype);
                    string str1 = itemMapBean.SourceId;
                    if (!string.IsNullOrWhiteSpace(str1))
                    {
                        orgMaptype.OrganisationID = itemMapBean.SourceId;
                    }

                    string str2 = itemMapBean.TargetId;
                    if (!string.IsNullOrWhiteSpace(str2))
                    {
                        orgMaptype.TargetOrganisationID = itemMapBean.TargetId;
                    }
                }
            }

            if (this.HasAnnotations(buildFrom))
            {
                orgSchemeMap.Annotations = this.GetAnnotationsType(buildFrom);
            }
        }

        /// <summary>
        /// The process structure map.
        /// </summary>
        /// <param name="structureSetType">
        /// The structure set type.
        /// </param>
        /// <param name="buildFrom">
        /// The build from.
        /// </param>
        private void ProcessStructureMap(StructureSetType structureSetType, IStructureMapObject buildFrom)
        {
            var structureMapType = new StructureMapType();
            structureSetType.StructureMap = structureMapType;

            structureMapType.isExtension = buildFrom.Extension;

            string str0 = buildFrom.Id;
            if (!string.IsNullOrWhiteSpace(str0))
            {
                structureMapType.id = buildFrom.Id;
            }

            IList<ITextTypeWrapper> names = buildFrom.Names;
            if (ObjectUtil.ValidCollection(names))
            {
                structureMapType.Name = this.GetTextType(names);
            }

            IList<ITextTypeWrapper> descriptions = buildFrom.Descriptions;
            if (ObjectUtil.ValidCollection(descriptions))
            {
                structureMapType.Description = this.GetTextType(descriptions);
            }

            if (buildFrom.SourceRef != null)
            {
                if (buildFrom.SourceRef.TargetReference.EnumType == SdmxStructureEnumType.Dsd)
                {
                    var keyFamilyRef = new KeyFamilyRefType();
                    structureMapType.KeyFamilyRef = keyFamilyRef;
                    SetDataStructureRefAttributes(keyFamilyRef, buildFrom.SourceRef);
                }
                else if (buildFrom.SourceRef.TargetReference.EnumType == SdmxStructureEnumType.Msd)
                {
                    var refType = new MetadataStructureRefType();
                    structureMapType.MetadataStructureRef = refType;
                    SetMetadataStructureRefAttributes(refType, buildFrom.SourceRef);
                }
            }

            if (buildFrom.TargetRef != null)
            {
                if (buildFrom.TargetRef.TargetReference.EnumType == SdmxStructureEnumType.Dsd)
                {
                    var keyFamilyRef = new KeyFamilyRefType();
                    structureMapType.TargetKeyFamilyRef = keyFamilyRef;
                    SetDataStructureRefAttributes(keyFamilyRef, buildFrom.TargetRef);
                }
                else if (buildFrom.TargetRef.TargetReference.EnumType == SdmxStructureEnumType.Msd)
                {
                    var refType1 = new MetadataStructureRefType();
                    structureMapType.TargetMetadataStructureRef = refType1;
                    SetMetadataStructureRefAttributes(refType1, buildFrom.TargetRef);
                }
            }

            IList<IComponentMapObject> componentMapObjects = buildFrom.Components;
            if (ObjectUtil.ValidCollection(componentMapObjects))
            {
                /* foreach */
                foreach (IComponentMapObject componentMapBean in componentMapObjects)
                {
                    var componentMapType = new ComponentMapType();
                    structureMapType.ComponentMap.Add(componentMapType);
                    this.ProcessComponent(componentMapType, componentMapBean);
                }
            }

            if (this.HasAnnotations(buildFrom))
            {
                structureMapType.Annotations = this.GetAnnotationsType(buildFrom);
            }
        }

        #endregion
    }
}