// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataflowWriterV2.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The dataflow writer v 2.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Estat.Sri.SdmxStructureMutableParser.Engine.V2
{
    using System.Collections.Generic;
    using System.Xml;

    using Estat.Sri.SdmxParseBase.Model;
    using Estat.Sri.SdmxXmlConstants;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.CategoryScheme;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.DataStructure;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;
    using Org.Sdmxsource.Sdmx.Api.Util;
    using Org.Sdmxsource.Sdmx.Util.Objects.Reference;
    using Org.Sdmxsource.Util.Collections;

    /// <summary>
    ///     The dataflow writer v 2.
    /// </summary>
    internal class DataflowWriterV2 : StructureWriterBaseV2, IMutableWriter
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DataflowWriterV2"/> class.
        /// </summary>
        /// <param name="writer">
        /// The writer.
        /// </param>
        /// <param name="namespaces">
        /// The namespaces.
        /// </param>
        public DataflowWriterV2(XmlWriter writer, SdmxNamespaces namespaces)
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
            this.WriteDataflows(structure.Dataflows, structure.Categorisations);
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
        /// <returns>
        /// The categorisation dictionary
        /// </returns>
        private static IDictionaryOfLists<IMaintainableRefObject, ICategorisationMutableObject> BuildCategorisationMap(
            IEnumerable<ICategorisationMutableObject> categorisations)
        {
            var categorisationMap = new DictionaryOfLists<IMaintainableRefObject, ICategorisationMutableObject>();
            foreach (ICategorisationMutableObject categorisation in categorisations)
            {
                IStructureReference structureReference = categorisation.StructureReference;
                switch (structureReference.MaintainableStructureEnumType.EnumType)
                {
                    case SdmxStructureEnumType.Dataflow:
                        IList<ICategorisationMutableObject> dataflowCategories;
                        IMaintainableRefObject dataflowReference = structureReference.MaintainableReference;
                        if (!categorisationMap.TryGetValue(dataflowReference, out dataflowCategories))
                        {
                            dataflowCategories = new List<ICategorisationMutableObject>();
                            categorisationMap.Add(dataflowReference, dataflowCategories);
                        }

                        dataflowCategories.Add(categorisation);

                        break;
                }
            }

            return categorisationMap;
        }

        /// <summary>
        /// Write the CategoryID element from the given CategoryIDBean object
        /// </summary>
        /// <param name="categoryID">
        /// The CategoryIDBean object to write
        /// </param>
        private void WriteCategoryID(IIdentifiableRefObject categoryID)
        {
            int open = 0;

            IIdentifiableRefObject current = categoryID;
            while (current != null)
            {
                this.WriteStartElement(this.DefaultPrefix, ElementNameTable.CategoryID);
                open++;
                this.TryToWriteElement(this.DefaultPrefix, ElementNameTable.ID, current.Id);
                current = current.ChildReference;
            }

            while (open > 0)
            {
                this.WriteEndElement();
                open--;
            }
        }

        /// <summary>
        /// Write the CategoryRef element from the given CategoryRefBean object
        /// </summary>
        /// <param name="categoryRef">
        /// The CategoryRefBean object to write
        /// </param>
        private void WriteCategoryRef(IStructureReference categoryRef)
        {
            IMaintainableRefObject categorySchemeRef = categoryRef.MaintainableReference;
            IIdentifiableRefObject category = categoryRef.ChildReference;
            this.WriteStartElement(this.DefaultPrefix, ElementNameTable.CategoryRef);
            this.TryToWriteElement(this.DefaultPrefix, ElementNameTable.URN, categoryRef.TargetUrn);
            this.TryToWriteElement(this.DefaultPrefix, ElementNameTable.CategorySchemeID, categorySchemeRef.MaintainableId);
            this.TryToWriteElement(this.DefaultPrefix, ElementNameTable.CategorySchemeAgencyID, categorySchemeRef.AgencyId);
            this.TryToWriteElement(this.DefaultPrefix, ElementNameTable.CategorySchemeVersion, categorySchemeRef.Version);
            this.WriteCategoryID(category);
            this.WriteEndElement();
        }

        /// <summary>
        /// Write dataflows
        /// </summary>
        /// <param name="dataflows">
        /// The dataflows to write
        /// </param>
        /// <param name="categorisations">
        /// The categorisations.
        /// </param>
        private void WriteDataflows(
            IEnumerable<IDataflowMutableObject> dataflows, IEnumerable<ICategorisationMutableObject> categorisations)
        {
            IDictionaryOfLists<IMaintainableRefObject, ICategorisationMutableObject> categorisationMap =
                BuildCategorisationMap(categorisations);

            this.WriteStartElement(this.RootNamespace, ElementNameTable.Dataflows);
            foreach (IDataflowMutableObject dataflow in dataflows)
            {
                this.WriteMaintainableArtefact(ElementNameTable.Dataflow, dataflow);
                if (dataflow.DataStructureRef != null)
                {
                    this.WriteKeyFamilyRef(dataflow.DataStructureRef);
                }

                var dataflowReference = new MaintainableRefObjectImpl(dataflow.AgencyId, dataflow.Id, dataflow.Version);

                foreach (KeyValuePair<IMaintainableRefObject, IList<ICategorisationMutableObject>> catRef in
                    categorisationMap)
                {
                    if (dataflowReference.Equals(catRef.Key))
                    {
                        foreach (ICategorisationMutableObject categorisation in catRef.Value)
                        {
                            this.WriteCategoryRef(categorisation.CategoryReference);
                        }
                    }
                }

                this.WriteAnnotations(ElementNameTable.Annotations, dataflow.Annotations);
                this.WriteEndElement();
            }

            this.WriteEndElement();
        }

        /// <summary>
        /// Write the KeyFamilyRef element from the given KeyFamilyRefBean object
        /// </summary>
        /// <param name="keyfamilyRef">
        /// The KeyFamilyRefBean object to write
        /// </param>
        private void WriteKeyFamilyRef(IStructureReference keyfamilyRef)
        {
            // ReSharper restore SuggestBaseTypeForParameter
            this.WriteStartElement(this.DefaultPrefix, ElementNameTable.KeyFamilyRef);
            this.TryToWriteElement(this.DefaultPrefix, ElementNameTable.URN, keyfamilyRef.MaintainableUrn);
            IMaintainableRefObject reference = keyfamilyRef.MaintainableReference;
            this.TryToWriteElement(this.DefaultPrefix, ElementNameTable.KeyFamilyID, reference.MaintainableId);
            this.TryToWriteElement(this.DefaultPrefix, ElementNameTable.KeyFamilyAgencyID, reference.AgencyId);
            this.TryToWriteElement(this.DefaultPrefix, ElementNameTable.Version, reference.Version);

            this.WriteEndElement();
        }

        #endregion
    }
}