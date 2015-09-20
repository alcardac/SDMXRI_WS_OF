// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StructureXmlBuilder.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The structure xml bean builder.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.Structureparser.Builder.XmlSerialization.V1
{
    using System.Collections.Generic;

    using Org.Sdmx.Resources.SdmxMl.Schemas.V10.message;
    using Org.Sdmx.Resources.SdmxMl.Schemas.V10.structure;
    using Org.Sdmxsource.Sdmx.Api.Builder;
    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Exception;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Codelist;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.ConceptScheme;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.DataStructure;
    using Org.Sdmxsource.Sdmx.Structureparser.Builder.XmlSerialization.Registry.Response.V1;

    using StructureType = Org.Sdmx.Resources.SdmxMl.Schemas.V10.message.StructureType;

    /// <summary>
    ///     The structure xml bean builder.
    /// </summary>
    public class StructureXmlBuilder : IBuilder<Structure, ISdmxObjects>
    {
        #region Fields

        /// <summary>
        ///     The agency xml bean builder.
        /// </summary>
        private readonly AgencyXmlBuilder _agencyXmlBuilder = new AgencyXmlBuilder();

        /// <summary>
        ///     The codelist xml bean builder.
        /// </summary>
        private readonly CodelistXmlBuilder _codelistXmlBuilder = new CodelistXmlBuilder();

        /// <summary>
        ///     The concept xml bean builder.
        /// </summary>
        private readonly ConceptXmlBuilder _conceptXmlBuilder = new ConceptXmlBuilder();

        /// <summary>
        ///     The data structure xml bean builder.
        /// </summary>
        private readonly DataStructureXmlBuilder _dataStructureXmlBuilder = new DataStructureXmlBuilder();

        /// <summary>
        ///     The structure header xml bean builder.
        /// </summary>
        private readonly StructureHeaderXmlBuilder _structureHeaderXmlBuilder = new StructureHeaderXmlBuilder();

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The build.
        /// </summary>
        /// <param name="buildFrom">
        /// The build from.
        /// </param>
        /// <returns>
        /// The <see cref="Structure"/>.
        /// </returns>
        public virtual Structure Build(ISdmxObjects buildFrom)
        {
            // Validate the structures in the sdmxObjects file are supported by SDMX at version 1.0
            ValidateSupport(buildFrom);
            var doc = new Structure();
            StructureType returnType = doc.Content;
            HeaderType headerType;
            if (buildFrom.Header != null)
            {
                headerType = this._structureHeaderXmlBuilder.Build(buildFrom.Header);
                returnType.Header = headerType;
            }
            else
            {
                headerType = new HeaderType();
                returnType.Header = headerType;

                V1Helper.SetHeader(headerType, buildFrom);
            }

            // GET CODELISTS
            if (buildFrom.Codelists.Count > 0)
            {
                var codeListsType = new CodeListsType();
                returnType.CodeLists = codeListsType;

                /* foreach */
                foreach (ICodelistObject codelistBean in buildFrom.Codelists)
                {
                    codeListsType.CodeList.Add(this._codelistXmlBuilder.Build(codelistBean));
                }
            }

            // CONCEPT SCHEMES
            if (buildFrom.ConceptSchemes.Count > 0)
            {
                var conceptsType = new ConceptsType();
                returnType.Concepts = conceptsType;

                /* foreach */
                foreach (IConceptSchemeObject conceptSchemeBean in buildFrom.ConceptSchemes)
                {
                    /* foreach */
                    foreach (IConceptObject conceptBean in conceptSchemeBean.Items)
                    {
                        conceptsType.Concept.Add(this._conceptXmlBuilder.Build(conceptBean));
                    }
                }
            }

            // KEY FAMILY
            if (buildFrom.DataStructures.Count > 0)
            {
                var keyFamiliesType = new KeyFamiliesType();
                returnType.KeyFamilies = keyFamiliesType;

                /* foreach */
                foreach (IDataStructureObject currentBean in buildFrom.DataStructures)
                {
                    keyFamiliesType.KeyFamily.Add(this._dataStructureXmlBuilder.Build(currentBean));
                }
            }

            // AGENCIES
            if (buildFrom.Agencies.Count > 0)
            {
                var agencies = new AgenciesType();
                returnType.Agencies = agencies;

                foreach (IAgency agencyBean in buildFrom.Agencies)
                {
                    agencies.Agency.Add(this._agencyXmlBuilder.Build(agencyBean));
                }
            }

            return doc;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Validates all the Maintainable Artefacts in the sdmxObjects container are supported by the SDMX v1.0 syntax
        /// </summary>
        /// <param name="sdmxObjects">
        /// The input sdmx objects
        /// </param>
        private static void ValidateSupport(ISdmxObjects sdmxObjects)
        {
            var supportedStructres = new HashSet<SdmxStructureEnumType>
                                         {
                                             SdmxStructureEnumType.AgencyScheme, 
                                             SdmxStructureEnumType.Dsd, 
                                             SdmxStructureEnumType.ConceptScheme, 
                                             SdmxStructureEnumType.CodeList
                                         };

            /* foreach */
            foreach (IMaintainableObject maintainableBean in sdmxObjects.GetAllMaintainables())
            {
                if (!supportedStructres.Contains(maintainableBean.StructureType.EnumType))
                {
                    throw new SdmxNotImplementedException(
                        ExceptionCode.Unsupported,
                        maintainableBean.StructureType.StructureType + " at SMDX v1.0 - please use SDMX v2.0 or v2.1");
                }
            }
        }

        #endregion
    }
}