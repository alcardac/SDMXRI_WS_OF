// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SdmxObjectsV1Builder.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The sdmx beans v 1 builder.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.Structureparser.Builder.SdmxObjects
{
    using System;
    using System.Collections.Generic;

    using Org.Sdmx.Resources.SdmxMl.Schemas.V10.message;
    using Org.Sdmx.Resources.SdmxMl.Schemas.V10.structure;
    using Org.Sdmxsource.Sdmx.Api.Builder;
    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Constants.InterfaceConstant;
    using Org.Sdmxsource.Sdmx.Api.Exception;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Header;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Codelist;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.ConceptScheme;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.DataStructure;
    using Org.Sdmxsource.Sdmx.Util.Objects.Container;

    using StructureType = Org.Sdmx.Resources.SdmxMl.Schemas.V10.message.StructureType;

    /// <summary>
    ///     The sdmx beans v 1 builder.
    /// </summary>
    public class SdmxObjectsV1Builder : AbstractSdmxObjectsBuilder, IBuilder<ISdmxObjects, Structure>
    {
        #region Public Methods and Operators

        /// <summary>
        /// Builds an <see cref="ISdmxObjects"/> object from the specified <paramref name="structuresDoc"/>
        /// </summary>
        /// <param name="structuresDoc">
        /// An <see cref="Structure"/> to build the output object from
        /// </param>
        /// <returns>
        /// an <see cref="ISdmxObjects"/> object from the specified <paramref name="structuresDoc"/>
        /// </returns>
        /// <exception cref="BuilderException">
        /// - If anything goes wrong during the build process
        /// </exception>
        public virtual ISdmxObjects Build(Structure structuresDoc)
        {
            var urns = new HashSet<Uri>();
            StructureType structures = structuresDoc.Content;
            var beans = new SdmxObjectsImpl(new HeaderImpl(structures.Header));

            if (structures.CodeLists != null && structures.CodeLists.CodeList != null)
            {
                /* foreach */
                foreach (CodeListType currentType in structures.CodeLists.CodeList)
                {
                    try
                    {
                        this.AddIfNotDuplicateURN(beans, urns, new CodelistObjectCore(currentType));
                    }
                    catch (Exception th)
                    {
                        throw new MaintainableObjectException(
                            th,
                            SdmxStructureType.GetFromEnum(SdmxStructureEnumType.CodeList),
                            currentType.agency,
                            currentType.id,
                            currentType.version);
                    }
                }
            }

            IDictionary<string, IList<ConceptType>> conceptAgencyMap = new Dictionary<string, IList<ConceptType>>();
            if (structures.Concepts != null && structures.Concepts.Concept != null)
            {
                /* foreach */
                foreach (ConceptType currentType0 in structures.Concepts.Concept)
                {
                    IList<ConceptType> concepts;
                    if (!conceptAgencyMap.TryGetValue(currentType0.agency, out concepts))
                    {
                        concepts = new List<ConceptType>();
                        conceptAgencyMap.Add(currentType0.agency, concepts);
                    }

                    concepts.Add(currentType0);
                }
            }

            /* foreach */
            foreach (KeyValuePair<string, IList<ConceptType>> currentConceptAgency in conceptAgencyMap)
            {
                try
                {
                    this.AddIfNotDuplicateURN(
                        beans, urns, new ConceptSchemeObjectCore(currentConceptAgency.Key, currentConceptAgency.Value));
                }
                catch (Exception th1)
                {
                    throw new MaintainableObjectException(
                        th1,
                        SdmxStructureType.GetFromEnum(SdmxStructureEnumType.ConceptScheme),
                        currentConceptAgency.Key,
                        ConceptSchemeObject.DefaultSchemeId,
                        ConceptSchemeObject.DefaultSchemeVersion);
                }
            }

            if (structures.KeyFamilies != null && structures.KeyFamilies.KeyFamily != null)
            {
                /* foreach */
                foreach (KeyFamilyType currentType2 in structures.KeyFamilies.KeyFamily)
                {
                    try
                    {
                        this.AddIfNotDuplicateURN(beans, urns, new DataStructureObjectCore(currentType2));
                    }
                    catch (Exception th3)
                    {
                        throw new MaintainableObjectException(
                            th3,
                            SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Dsd),
                            currentType2.agency,
                            currentType2.id,
                            currentType2.version);
                    }
                }
            }

            return beans;
        }

        #endregion
    }
}