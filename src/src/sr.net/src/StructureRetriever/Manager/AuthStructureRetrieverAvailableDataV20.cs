// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AuthStructureRetrieverAvailableDataV20.cs" company="Eurostat">
//   Date Created : 2011-12-15
//   Copyright (c) 2011 by the European   Commission, represented by Eurostat. All rights reserved.
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   This is an implementation of "IStructureRetriever" interface that can retrieve available data from DDB and dataflows with complete mapping set.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Estat.Nsi.StructureRetriever.Manager
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data.Common;
    using System.Globalization;
    using System.Linq;

    using Estat.Nsi.StructureRetriever.Builders;
    using Estat.Nsi.StructureRetriever.Model;
    using Estat.Sdmxsource.Extension.Manager;
    using Estat.Sri.CustomRequests.Model;

    using log4net;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Exception;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Codelist;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;

    /// <summary>
    /// This is an implementation of <see cref="IAuthMutableStructureSearchManager"/> interface that can retrieve available data from DDB and dataflows with complete mapping set.
    /// </summary>
    /// <example>
    /// Code example for C# 
    /// <code source="ReUsingExamples\StructureRetriever\ReUsingStructureRetriever.cs" lang="cs">
    /// </code>
    /// </example>
    /// <remarks>
    /// It's main job is to retrieve structural metadata from Mapping Store. It can be used with any v3.0 or higher "Mapping Store" complying with the database design specified there. This implementation supports the following special case in order to retrieve a subset of codes for a dimension that can be constrained by the values of other dimensions: If the <c>QueryStructureRequest</c> contains a <c>CodelistRef</c> and <c>DataflowRef</c> with constrains with one <c>Member</c> without <c>MemberValue</c> and optionally any number <c>Member</c> with <c>MemberValue</c> then the this implementation will retrieve the subset of the requested codelist that is used by the dimension referenced in the member without member value, used by the specified dataflow and constrained by the dimension values defined with Member and MemberValues.
    /// </remarks>
    public class AuthStructureRetrieverAvailableDataV20 : AuthCategorisationV20MutableStructureSearchManager
    {
        #region Constants and Fields

        /// <summary>
        ///   The logger object which will be used to log information,warning and error messages
        /// </summary>
        private static readonly ILog _logger = LogManager.GetLogger(typeof(StructureRetrieverAvailableData));

        /// <summary>
        ///   This field holds the <see cref="SpecialRequestManager" /> which manages all special requests
        /// </summary>
        private static readonly ISpecialRequestManager _specialRequestManager = new SpecialRequestManager();

        /// <summary>
        ///   This field holds the builder for <see cref="StructureRetrievalInfo" />
        /// </summary>
        private static readonly IStructureRetrievalInfoBuilder _structureRetrievalInfoBuilder =
            new StructureRetrievalInfoBuilder();

        /// <summary>
        ///   The connection to the "Mapping Store",from which the SDMX Structural metadata will be retrieved
        /// </summary>
        private readonly ConnectionStringSettings _connectionStringSettings;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthStructureRetrieverAvailableDataV20"/> class. 
        /// </summary>
        /// <exception cref="System.ArgumentNullException">
        /// defaultHeader is null
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// connectionStringSettings is null
        /// </exception>
        /// <exception cref="StructureRetrieverException">
        /// Could not establish a connection to the mapping store DB
        ///   <see cref="StructureRetrieverException.ErrorType"/>
        ///   is set to
        ///   <see cref="StructureRetrieverErrorTypes.MappingStoreConnectionError"/>
        /// </exception>
        /// <param name="connectionStringSettings">
        ///     The connection to the "Mapping Store", from which the SDMX Structural metadata will be retrieved 
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// defaultHeader or connectionStringSettings is null
        /// </exception>
        public AuthStructureRetrieverAvailableDataV20(ConnectionStringSettings connectionStringSettings)
            : base(connectionStringSettings)
        {
            if (connectionStringSettings == null)
            {
                throw new ArgumentNullException("connectionStringSettings");
            }

            this._connectionStringSettings = connectionStringSettings;
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Retrieve the <paramref name="queries"/> and populate the <paramref name="mutableObjects"/>
        /// </summary>
        /// <param name="retrievalManager">
        ///     The retrieval manager.
        /// </param>
        /// <param name="mutableObjects">
        ///     The mutable objects.
        /// </param>
        /// <param name="queries">
        ///     The structure queries
        /// </param>
        /// <param name="returnLatest">
        ///     Set to <c>true</c> to retrieve the latest; otherwise set to <c>false</c> to retrieve all versions
        /// </param>
        /// <param name="returnStub">
        ///     Set to <c>true</c> to retrieve artefacts as stubs; otherwise set to <c>false</c> to retrieve full artefacts.
        /// </param>
        /// <param name="allowedDataflows">
        ///     The allowed dataflows.
        /// </param>
        /// <param name="crossReferenceMutableRetrievalManager">
        ///     The cross-reference manager
        /// </param>
        protected override void PopulateMutables(IAuthAdvancedSdmxMutableObjectRetrievalManager retrievalManager, IMutableObjects mutableObjects, IList<IStructureReference> queries, bool returnLatest, StructureQueryDetailEnumType returnStub, IList<IMaintainableRefObject> allowedDataflows, IAuthCrossReferenceMutableRetrievalManager crossReferenceMutableRetrievalManager)
        {
            try
            {
                // try get the codelist reference
                IMaintainableRefObject codelistReference =
                    queries.Where(structureReference => structureReference.TargetReference.EnumType == SdmxStructureEnumType.CodeList)
                        .Select(structureReference => structureReference.MaintainableReference)
                        .FirstOrDefault();

                // try get the dataflow ref
                var dataflowRef = queries.FirstOrDefault(structureReference => structureReference.TargetReference.EnumType == SdmxStructureEnumType.Dataflow) as IConstrainableStructureReference;

                // check if it is special
                if (codelistReference != null && dataflowRef != null && dataflowRef.ConstraintObject != null && queries.Count == 2)
                {
                    // get the dataflow
                    base.PopulateMutables(
                        retrievalManager,
                        mutableObjects,
                        new IStructureReference[] { dataflowRef },
                        returnLatest,
                        returnStub,
                        allowedDataflows,
                        crossReferenceMutableRetrievalManager);

                    // get the partial codelist
                    StructureRetrievalInfo structureRetrievalInfo = _structureRetrievalInfoBuilder.Build(dataflowRef, this._connectionStringSettings, allowedDataflows);
                    if (!ProcessReference(structureRetrievalInfo, codelistReference, mutableObjects))
                    {
                        string message = string.Format(CultureInfo.InvariantCulture, "No codes found for {0}", codelistReference);
                        _logger.Error(message);
                        throw new SdmxNoResultsException(message);
                    }
                }
                else
                {
                    // not special requests
                    base.PopulateMutables(retrievalManager, mutableObjects, queries, returnLatest, returnStub, allowedDataflows, crossReferenceMutableRetrievalManager);
                }
            }
            catch (SdmxException)
            {
                throw;
            }
            catch (StructureRetrieverException e)
            {
                _logger.Error(e.Message, e);
                switch (e.ErrorType)
                {
                    case StructureRetrieverErrorTypes.ParsingError:
                        throw new SdmxSyntaxException(e, ExceptionCode.XmlParseException);
                    case StructureRetrieverErrorTypes.MissingStructure:
                    case StructureRetrieverErrorTypes.MissingStructureRef:
                        throw new SdmxNoResultsException(e.Message);
                    default:
                        throw new SdmxInternalServerException(e.Message);
                }
            }
            catch (DbException e)
            {
                string mesage = "Mapping Store connection error." + e.Message;
                _logger.Error(mesage, e);
                throw new SdmxInternalServerException(mesage);
            }
            catch (Exception e)
            {
                string mesage = e.Message;
                _logger.Error(mesage, e);
                throw new SdmxInternalServerException(mesage);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Process the <paramref name="current"/> and populate the <paramref name="mutableObjects"/>
        /// </summary>
        /// <param name="av">
        /// The current Structure Retrieval state
        /// </param>
        /// <param name="current">
        /// The <see cref="IMaintainableRefObject"/> codelist request 
        /// </param>
        /// <param name="mutableObjects">The output mutable objects</param>
        /// <returns>
        /// <c>true</c> if the partial codelist is found found is larger than 0. Else <c>false</c> 
        /// </returns>
        private static bool ProcessReference(StructureRetrievalInfo av, IMaintainableRefObject current, IMutableObjects mutableObjects)
        {
            ICodelistMutableObject availableData = _specialRequestManager.RetrieveAvailableData(current, av);
            if (availableData != null)
            {
                mutableObjects.AddCodelist(availableData);
                return true;
            }

            return false;
        }

        #endregion
    }
}