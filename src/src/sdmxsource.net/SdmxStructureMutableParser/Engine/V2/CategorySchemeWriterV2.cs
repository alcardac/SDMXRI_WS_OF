// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CategorySchemeWriterV2.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The category scheme writer v 2.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Estat.Sri.SdmxStructureMutableParser.Engine.V2
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml;

    using Estat.Sri.SdmxParseBase.Model;
    using Estat.Sri.SdmxXmlConstants;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.CategoryScheme;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;
    using Org.Sdmxsource.Sdmx.Api.Util;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.Base;
    using Org.Sdmxsource.Sdmx.Util.Extension;
    using Org.Sdmxsource.Sdmx.Util.Objects.Reference;
    using Org.Sdmxsource.Util.Collections;

    /// <summary>
    ///     The category scheme writer v 2.
    /// </summary>
    internal class CategorySchemeWriterV2 : StructureWriterBaseV2, IMutableWriter
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CategorySchemeWriterV2"/> class.
        /// </summary>
        /// <param name="writer">
        /// The writer.
        /// </param>
        /// <param name="namespaces">
        /// The namespaces.
        /// </param>
        public CategorySchemeWriterV2(XmlWriter writer, SdmxNamespaces namespaces)
            : base(writer, namespaces)
        {
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Write.the specified <paramref name="structure"/>
        /// </summary>
        /// <param name="structure">
        /// The structure.
        /// </param>
        public void Write(IMutableObjects structure)
        {
            this.WriteCategorySchemes(structure.CategorySchemes, structure.Categorisations);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Builds the categorisation dictionary where dataflow <see cref="IMaintainableRefObject"/> are keys and lists of
        ///     <see cref="ICategorisationMutableObject"/>
        ///     the value.
        /// </summary>
        /// <param name="categorisations">
        /// The categorisations.
        /// </param>
        /// <param name="itemScheme">
        /// The Category Scheme
        /// </param>
        /// <returns>
        /// The categorisation dictionary
        /// </returns>
        private static IDictionaryOfLists<string, ICategorisationMutableObject> BuildCategorisationMap(
            IEnumerable<ICategorisationMutableObject> categorisations, IMaintainableMutableObject itemScheme)
        {
            var categorisationMap = new DictionaryOfLists<string, ICategorisationMutableObject>(StringComparer.Ordinal);
            var categorySchemeRef = new MaintainableRefObjectImpl(
                itemScheme.AgencyId, itemScheme.Id, itemScheme.Version);
            foreach (ICategorisationMutableObject categorisation in categorisations)
            {
                IStructureReference structureReference = categorisation.StructureReference;
                switch (structureReference.MaintainableStructureEnumType.EnumType)
                {
                    case SdmxStructureEnumType.Dataflow:
                        IStructureReference categoryRef = categorisation.CategoryReference;
                        if (categorySchemeRef.Equals(categoryRef.MaintainableReference))
                        {
                            IList<ICategorisationMutableObject> categoryDataflows;
                            string categoryId = categoryRef.ChildReference.GetLastId();
                            if (!categorisationMap.TryGetValue(categoryId, out categoryDataflows))
                            {
                                categoryDataflows = new List<ICategorisationMutableObject>();
                                categorisationMap.Add(categoryId, categoryDataflows);
                            }

                            categoryDataflows.Add(categorisation);
                        }

                        break;
                }
            }

            return categorisationMap;
        }

        /// <summary>
        /// Traverse the category hierarchy tree and write each category starting from
        ///     the given ICategoryMutableObject object.
        /// </summary>
        /// <param name="item">
        /// The root ICategoryMutableObject object
        /// </param>
        /// <param name="categorisationMap">
        /// The categorisation map
        /// </param>
        private void TraverseCategoryTree(
            ICategoryMutableObject item, IDictionary<string, IList<ICategorisationMutableObject>> categorisationMap)
        {
            var parents = new Stack<ICategoryMutableObject>();
            var open = new Dictionary<ICategoryMutableObject, Queue<ICategoryMutableObject>>();
            this.WriteCategory(item, categorisationMap);

            parents.Push(item);
            open.Add(item, new Queue<ICategoryMutableObject>(item.Items));
            while (parents.Count > 0)
            {
                ICategoryMutableObject parent = parents.Peek();
                Queue<ICategoryMutableObject> remainingCategories = open[parent];
                while (remainingCategories.Count > 0)
                {
                    ICategoryMutableObject current = remainingCategories.Dequeue();
                    this.WriteCategory(current, categorisationMap);
                    parents.Push(current);
                    remainingCategories = new Queue<ICategoryMutableObject>(current.Items);
                    open.Add(current, remainingCategories);

                    // parent = parents.Peek();
                }

                parent = parents.Pop();
                this.WriteAnnotations(ElementNameTable.Annotations, parent.Annotations);
                this.WriteEndElement();
            }
        }

        /// <summary>
        /// Write the Category element from the given ICategoryMutableObject and call <see cref="WriteDataflowRef"/> to write the
        ///     dataflowRef.
        /// </summary>
        /// <param name="item">
        /// The ICategoryMutableObject object to write
        /// </param>
        /// <param name="categorisationMap">
        /// The categorisation map
        /// </param>
        private void WriteCategory(
            IItemMutableObject item, IDictionary<string, IList<ICategorisationMutableObject>> categorisationMap)
        {
            // handle categories without name
            if (item.Names.Count == 0)
            {
                item.Names.Add(new TextTypeWrapperMutableCore { Value = item.Id, Locale = "en" });
            }

            this.WriteItem(ElementNameTable.Category, item);
            IList<ICategorisationMutableObject> dataflowList;
            if (categorisationMap.TryGetValue(item.Id, out dataflowList))
            {
                foreach (ICategorisationMutableObject dataflow in dataflowList)
                {
                    this.WriteDataflowRef(dataflow.StructureReference);
                }
            }
        }

        /// <summary>
        /// Write categories schemes and call <see cref="TraverseCategoryTree"/> for writing Categories
        /// </summary>
        /// <param name="categorySchemes">
        /// The category Schemes.
        /// </param>
        /// <param name="categorisations">
        /// The categorisations.
        /// </param>
        private void WriteCategorySchemes(
            IEnumerable<ICategorySchemeMutableObject> categorySchemes, 
            IEnumerable<ICategorisationMutableObject> categorisations)
        {
            this.WriteStartElement(this.RootNamespace, ElementNameTable.CategorySchemes);
            ICategorisationMutableObject[] categorisationMutableObjects =
                categorisations as ICategorisationMutableObject[] ?? categorisations.ToArray();
            foreach (ICategorySchemeMutableObject itemScheme in categorySchemes)
            {
                IDictionaryOfLists<string, ICategorisationMutableObject> categorisationMap =
                    BuildCategorisationMap(categorisationMutableObjects, itemScheme);
                this.WriteMaintainableArtefact(ElementNameTable.CategoryScheme, itemScheme);
                foreach (ICategoryMutableObject item in itemScheme.Items)
                {
                    this.TraverseCategoryTree(item, categorisationMap);
                }

                this.WriteAnnotations(ElementNameTable.Annotations, itemScheme.Annotations);
                this.WriteEndElement();
            }

            this.WriteEndElement();
        }

        /// <summary>
        /// Write the DataflowRef element from the given DataflowRefBean object
        /// </summary>
        /// <param name="dataflowRef">
        /// The DataflowRefBean object to write
        /// </param>
        private void WriteDataflowRef(IStructureReference dataflowRef)
        {
            this.WriteStartElement(this.DefaultPrefix, ElementNameTable.DataflowRef);
            this.TryToWriteElement(this.DefaultPrefix, ElementNameTable.URN, dataflowRef.MaintainableUrn);
            this.TryToWriteElement(this.DefaultPrefix, ElementNameTable.AgencyID, dataflowRef.MaintainableReference.AgencyId);
            this.TryToWriteElement(
                this.DefaultPrefix, ElementNameTable.DataflowID, dataflowRef.MaintainableReference.MaintainableId);
            this.TryToWriteElement(this.DefaultPrefix, ElementNameTable.Version, dataflowRef.MaintainableReference.Version);
            
            this.WriteEndElement();
        }

        #endregion
    }
}