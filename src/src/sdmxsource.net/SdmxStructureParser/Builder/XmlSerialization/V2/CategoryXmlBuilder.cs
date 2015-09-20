// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CategoryXmlBuilder.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The category xml bean builder.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.Structureparser.Builder.XmlSerialization.V2
{
    using System;
    using System.Collections.Generic;

    using Org.Sdmx.Resources.SdmxMl.Schemas.V20.structure;
    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.CategoryScheme;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;
    using Org.Sdmxsource.Util;

    /// <summary>
    ///     The category xml bean builder.
    /// </summary>
    public class CategoryXmlBuilder : AbstractBuilder
    {
        #region Public Methods and Operators

        /// <summary>
        /// Build <see cref="CategoryType"/> from <paramref name="buildFrom"/>.
        /// </summary>
        /// <param name="buildFrom">
        /// The build from.
        /// </param>
        /// <returns>
        /// The <see cref="CategoryType"/> from <paramref name="buildFrom"/> .
        /// </returns>
        public CategoryType Build(IReportingCategoryObject buildFrom)
        {
            return this.BuildCommon(buildFrom, null);
        }

        /// <summary>
        /// Build <see cref="CategorySchemeType"/> from <paramref name="buildFrom"/>.
        /// </summary>
        /// <param name="buildFrom">
        /// The build from.
        /// </param>
        /// <param name="categorisations">
        /// The set of categorisations
        /// </param>
        /// <returns>
        /// The <see cref="CategorySchemeType"/> from <paramref name="buildFrom"/> .
        /// </returns>
        public CategoryType Build(ICategoryObject buildFrom, ISet<ICategorisationObject> categorisations)
        {
            return this.BuildCommon(buildFrom, categorisations);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Populate categorisations to <paramref name="builtObj"/> if <paramref name="categorisations"/> is not null
        /// </summary>
        /// <param name="buildFrom">
        /// The source object.
        /// </param>
        /// <param name="categorisations">
        /// The categorisations.
        /// </param>
        /// <param name="builtObj">
        /// The destination, to build
        /// </param>
        private static void PopulatedCategorisations(
            IIdentifiableObject buildFrom, IEnumerable<ICategorisationObject> categorisations, CategoryType builtObj)
        {
            if (categorisations != null)
            {
                /* foreach */
                foreach (ICategorisationObject cat in categorisations)
                {
                    ICrossReference catRef = cat.CategoryReference;
                    if (catRef.IsMatch(buildFrom))
                    {
                        switch (cat.StructureReference.TargetReference.EnumType)
                        {
                            case SdmxStructureEnumType.Dataflow:
                                {
                                    var dataflowRefType = new DataflowRefType();
                                    builtObj.DataflowRef.Add(dataflowRefType);

                                    IMaintainableRefObject maintainableReference =
                                        cat.StructureReference.MaintainableReference;
                                    string str1 = maintainableReference.AgencyId;
                                    if (!string.IsNullOrWhiteSpace(str1))
                                    {
                                        dataflowRefType.AgencyID = maintainableReference.AgencyId;
                                    }

                                    string str3 = maintainableReference.MaintainableId;
                                    if (!string.IsNullOrWhiteSpace(str3))
                                    {
                                        dataflowRefType.DataflowID = maintainableReference.MaintainableId;
                                    }

                                    string str4 = maintainableReference.Version;
                                    if (!string.IsNullOrWhiteSpace(str4))
                                    {
                                        dataflowRefType.Version = maintainableReference.Version;
                                    }

                                    dataflowRefType.URN = cat.StructureReference.TargetUrn;
                                }

                                break;
                            case SdmxStructureEnumType.MetadataFlow:
                                {
                                    var metdataflowRefType = new MetadataflowRefType();
                                    builtObj.MetadataflowRef.Add(metdataflowRefType);
                                    IMaintainableRefObject mdfRef = cat.StructureReference.MaintainableReference;
                                    string str1 = mdfRef.AgencyId;
                                    if (!string.IsNullOrWhiteSpace(str1))
                                    {
                                        metdataflowRefType.AgencyID = mdfRef.AgencyId;
                                    }

                                    string str3 = mdfRef.MaintainableId;
                                    if (!string.IsNullOrWhiteSpace(str3))
                                    {
                                        metdataflowRefType.MetadataflowID = mdfRef.MaintainableId;
                                    }

                                    string str4 = mdfRef.Version;
                                    if (!string.IsNullOrWhiteSpace(str4))
                                    {
                                        metdataflowRefType.Version = mdfRef.Version;
                                    }

                                    metdataflowRefType.URN = cat.StructureReference.TargetUrn;
                                }

                                break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Build <see cref="CategoryType"/> from <paramref name="buildFrom"/>.
        /// </summary>
        /// <typeparam name="T">
        /// The type of <paramref name="buildFrom"/>
        /// </typeparam>
        /// <param name="buildFrom">
        /// The build from.
        /// </param>
        /// <param name="categorisations">
        /// The set of categorisations can be null
        /// </param>
        /// <returns>
        /// The <see cref="CategoryType"/> from <paramref name="buildFrom"/> .
        /// </returns>
        private CategoryType BuildCommon<T>(T buildFrom, ISet<ICategorisationObject> categorisations)
            where T : IHierarchicalItemObject<T>
        {
            var pairs = new Stack<KeyValuePair<CategoryType, T>>();
            var root = new CategoryType();
            pairs.Push(new KeyValuePair<CategoryType, T>(root, buildFrom));

            while (pairs.Count > 0)
            {
                KeyValuePair<CategoryType, T> kv = pairs.Pop();
                buildFrom = kv.Value;
                CategoryType builtObj = kv.Key;

                this.PopulateCommon(buildFrom, builtObj);

                PopulatedCategorisations(buildFrom, categorisations, builtObj);

                IList<T> currentReportingCategories = buildFrom.Items;
                if (ObjectUtil.ValidCollection(currentReportingCategories))
                {
                    foreach (T currentReportingCategory in currentReportingCategories)
                    {
                        var item = new CategoryType();
                        builtObj.Category.Add(item);
                        pairs.Push(new KeyValuePair<CategoryType, T>(item, currentReportingCategory));
                    }
                }
            }

            return root;
        }

        /// <summary>
        /// Populate common properties of <paramref name="builtObj"/> from <paramref name="buildFrom"/>.
        /// </summary>
        /// <param name="buildFrom">
        /// The source
        /// </param>
        /// <param name="builtObj">
        /// The destination
        /// </param>
        private void PopulateCommon(INameableObject buildFrom, CategoryType builtObj)
        {
            string str1 = buildFrom.Id;
            if (!string.IsNullOrWhiteSpace(str1))
            {
                builtObj.id = str1;
            }

            if (buildFrom.Uri != null)
            {
                builtObj.uri = buildFrom.Uri;
            }

            if (ObjectUtil.ValidString(buildFrom.Urn))
            {
                builtObj.urn = buildFrom.Urn;
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
        }

        #endregion
    }
}