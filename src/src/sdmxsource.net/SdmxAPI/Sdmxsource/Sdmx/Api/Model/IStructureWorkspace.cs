// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IStructureWorkspace.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Model
{
    #region Using directives

    using System.Collections.Generic;
    using System.IO;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.BaseObjects;
    using Org.Sdmxsource.Sdmx.Api.Model.Header;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;
    using Org.Sdmxsource.Sdmx.Api.Util;

    #endregion

    /// <summary>
    ///     The structure workspace is built from an input source of SdmxObjects, the workspace provides means to retrieve the contained sdmxObjects,
    ///     output the contained sdmxObjects in different flavors and also provide a simple mechanism for retrieving subsets.
    ///     The structure workspace has the concept of what it was created with, and can supply, if given enough information the artifacts that
    ///     the workspace artifacts cross reference.
    /// </summary>
    public interface IStructureWorkspace
    {
        #region Public Properties

        /// <summary>
        ///     Gets a map containing identifiable keys against a set of identifiable objects that the
        ///     identifiable key cross references
        /// </summary>
        /// <value> </value>
        IDictionaryOfSets<IIdentifiableObject, IIdentifiableObject> CrossReferences { get; }

        /// <summary>
        ///     Gets the header that was present with the file.
        /// </summary>
        /// <value> </value>
        IHeader Header { get; }

        /// <summary>
        ///     Gets the base objects.
        ///     All the cross-referenced objects must be present for the base objects to be built successfully.
        /// </summary>
        /// <value> </value>
        IObjectsBase BaseObjects { get; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Gets structure sdmxObjects, if include cross references is set to true then it will also include
        ///     any cross referenced sdmxObjects - if there were no cross references supplied, then an exception will
        ///     be thrown
        /// </summary>
        /// <param name="includeCrossReferences">Cross reference. </param>
        /// <returns>
        /// The <see cref="ISdmxObjects"/> .
        /// </returns>
        ISdmxObjects GetStructureObjects(bool includeCrossReferences);

        /// <summary>
        /// Gets a subset of the workspace, based on the query objects.
        /// </summary>
        /// <param name="query">The query </param>
        /// <returns>
        /// The <see cref="IStructureWorkspace"/> . </returns>
        IStructureWorkspace GetSubsetWorkspace(params IStructureReference[] query);

        /// <summary>
        /// Merges a structure workspace into the current workspace. It will not overwrite duplicates.
        /// </summary>
        /// <param name="workspace">The workspace. </param>
        void MergeWorkspace(IStructureWorkspace workspace);

        /// <summary>
        /// Writes all of the structures in the workspace to the specified OutputStream as a SDMX Structure document
        /// </summary>
        /// <param name="structureType">
        /// The structureType 
        /// </param>
        /// <param name="outputStream">
        /// The stream.
        /// </param>
        /// <param name="includeCrossReferences">
        /// The include Cross References.
        /// </param>
        void WriteStructures(SdmxSchema structureType, Stream outputStream, bool includeCrossReferences);

        #endregion
    }
}