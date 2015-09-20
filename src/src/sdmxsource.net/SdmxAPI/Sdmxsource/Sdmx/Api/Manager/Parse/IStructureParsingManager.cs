// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IStructureParsingManager.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Manager.Parse
{
    #region Using directives

    using Org.Sdmxsource.Sdmx.Api.Manager.Retrieval;
    using Org.Sdmxsource.Sdmx.Api.Model;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects;
    using Org.Sdmxsource.Sdmx.Api.Util;

    #endregion

    /// <summary>
    ///     Parses and validates input SDMX-ML data into other data formats
    /// </summary>
    /// <example>
    ///     A sample implementation in C# of <see cref="IStructureParsingManager" /> using an implementation that supports both SDMX v2.0 and v2.1
    ///     <code source="..\ReUsingExamples\Structure\ReUsingStructureParsingManager.cs" lang="cs" />
    /// </example>
    /// <example>
    ///     A sample implementation in C# of <see cref="IStructureParsingManager" /> using a fast implementation that supports only a subset of SDMX v2.0
    ///     <code source="..\ReUsingExamples\Structure\ReUsingStructureParsingManagerFast.cs" lang="cs" />
    /// </example> 
    public interface IStructureParsingManager
    {
        #region Public Methods and Operators

        /// <summary>
        /// Build workspace.
        /// </summary>
        /// <param name="sdmxObjects">
        /// The input SDMX objects.
        /// </param>
        /// <param name="settings">
        /// The settings.
        /// </param>
        /// <param name="retrievalManager">
        /// The retrieval manager.
        /// </param>
        /// <returns>
        /// The <see cref="IStructureWorkspace"/> StructureWorkspace from which structures can be retrieved in any format required.
        /// </returns>
        IStructureWorkspace BuildWorkspace(
            ISdmxObjects sdmxObjects, ResolutionSettings settings, ISdmxObjectRetrievalManager retrievalManager);

        /// <summary>
        /// Parses a structure document OR a Registry document that contains structures.
        /// </summary>
        /// <param name="dataLocation">
        /// The structure location
        /// </param>
        /// <param name="settings">
        /// - addition settings to perform when parsing
        /// </param>
        /// <param name="retrievalManager">
        /// The retrieval manager.
        /// </param>
        /// <returns>
        /// StructureWorkspace - from this structures can be retrieved in any format required
        /// </returns>
        /// <exception cref="SdmxSyntaxException">
        /// - If the structure message is syntactically invalid
        /// </exception>
        /// <exception cref="CrossReferenceException">
        /// - If the structure document references structures that can not be resolved
        /// </exception>
        /// <exception cref="SdmxSemmanticException">
        /// - If the structure message is syntactically correct, but the content is illegal
        /// </exception>
        /// <remarks>
        /// Validates the SDMX-ML against the correct schema, also validates the structure according to the SDMX standards,
        ///     using rules which can not be specified by schema.  Uses the supplied settings to perform any extra operations.  If
        ///     resolve external references is set to true, then these structures will also be validated against the Schema and business logic.
        /// </remarks>
        IStructureWorkspace ParseStructures(
            IReadableDataLocation dataLocation, 
            ResolutionSettings settings, 
            ISdmxObjectRetrievalManager retrievalManager);

        /// <summary>
        /// Parses a structure document OR a Registry document that contains structures.
        /// </summary>
        /// <param name="dataLocation">
        /// - the supplied structures
        /// </param>
        /// <returns>
        /// StructureWorkspace - from this structures can be retrieved in any format required
        /// </returns>
        /// <exception cref="SdmxSyntaxException">
        /// - If the structure message is syntactically invalid
        /// </exception>
        /// <exception cref="CrossReferenceException">
        /// - If the structure document references structures that can not be resolved
        /// </exception>
        /// <exception cref="SdmxSemmanticException">
        /// - If the structure message is syntactically correct, but the content is illegal
        /// </exception>
        /// <remarks>
        /// Validates the SDMX-ML against the correct schema, also validates the structure according to the SDMX standards,
        ///     using rules which can not be specified by schema.
        ///     Uses the default parsing settings, which is to not validate cross references, and therefore no <c>SdmxObjectRetrievalManager</c> is
        ///     required.
        /// </remarks>
        IStructureWorkspace ParseStructures(IReadableDataLocation dataLocation);

        #endregion
    }
}