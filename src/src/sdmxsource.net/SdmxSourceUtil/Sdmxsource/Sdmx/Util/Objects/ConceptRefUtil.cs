// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConceptRefUtil.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Util.Objects
{
    #region Using directives

    using System;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Constants.InterfaceConstant;
    using Org.Sdmxsource.Sdmx.Api.Exception;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.ConceptScheme;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;
    using Org.Sdmxsource.Sdmx.Util.Objects.Reference;
    using Org.Sdmxsource.Util;

    #endregion


    /// <summary>
    /// The concept ref util.
    /// </summary>
    public class ConceptRefUtil
    {
        #region Public Methods and Operators

        /// <summary>
        /// The build concept ref.
        /// </summary>
        /// <param name="referencedFrom">
        /// The referenced from.
        /// </param>
        /// <param name="conceptSchemeAgency">
        /// The concept scheme agency.
        /// </param>
        /// <param name="conceptSchemeId">
        /// The concept scheme id.
        /// </param>
        /// <param name="conceptSchemeVersion">
        /// The concept scheme version1.
        /// </param>
        /// <param name="conceptAgency">
        /// The concept agency.
        /// </param>
        /// <param name="conceptId">
        /// The concept id.
        /// </param>
        /// <returns>
        /// The <see cref="ICrossReference"/>.
        /// </returns>
        /// <exception cref="ArgumentException">Throws ArgumentOutOfRangeException
        /// </exception>
        public static ICrossReference BuildConceptRef(
            IIdentifiableObject referencedFrom, 
            string conceptSchemeAgency, 
            string conceptSchemeId, 
            string conceptSchemeVersion, 
            string conceptAgency, 
            string conceptId)
        {
            bool isFreeStanding = string.IsNullOrWhiteSpace(conceptSchemeId);

            if (ObjectUtil.ValidOneString(conceptId, conceptSchemeAgency, conceptSchemeId, conceptSchemeVersion))
            {
                if (string.IsNullOrWhiteSpace(conceptSchemeAgency))
                {
                    conceptSchemeAgency = conceptAgency;
                }

                if (isFreeStanding)
                {
                    return new CrossReferenceImpl(
                        referencedFrom, 
                        conceptSchemeAgency, 
                        ConceptSchemeObject.DefaultSchemeId, 
                        ConceptSchemeObject.DefaultSchemeVersion, 
                        SdmxStructureEnumType.Concept, 
                        conceptId);
                }
                return new CrossReferenceImpl(
                    referencedFrom, 
                    conceptSchemeAgency, 
                    conceptSchemeId, 
                    conceptSchemeVersion, 
                    SdmxStructureEnumType.Concept, 
                    conceptId);
            }

            // TODO Exception
            throw new ArgumentException("Concept Reference missing parameters");
        }

        /// <summary>
        /// The get concept id.
        /// </summary>
        /// <param name="structureReference">
        /// The s ref.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        /// <exception cref="SdmxSemmanticException">Throws Validation Exception
        /// </exception>
        public static string GetConceptId(IStructureReference structureReference)
        {
            if (structureReference.TargetReference.EnumType == SdmxStructureEnumType.Concept)
            {
                return structureReference.ChildReference.Id;
            }

            throw new SdmxSemmanticException("Expecting a Concept Reference got : " + structureReference.TargetReference.GetType());
        }

        #endregion
    }
}