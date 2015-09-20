// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExceptionCode.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Constants
{
    #region Using directives

    using System;

    #endregion

    /// <summary>
    ///     Containing all the general exception codes used.
    /// </summary>
    [Serializable]
    public class ExceptionCode
    {
        #region Static Fields

        /// <summary>
        ///     The _ reference error.
        /// </summary>
        private static ExceptionCode _referenceError = new ExceptionCode("601");

        /// <summary>
        ///     The _ reference error missing parameters.
        /// </summary>
        private static ExceptionCode _referenceErrorMissingParameters = new ExceptionCode("602");

        /// <summary>
        ///     The _ reference error multiple responses expected one.
        /// </summary>
        private static ExceptionCode _referenceErrorMultipleResponsesExpectedOne = new ExceptionCode("605");

        /// <summary>
        ///     The _ reference error no type.
        /// </summary>
        private static ExceptionCode _referenceErrorNoType = new ExceptionCode("604");

        /// <summary>
        ///     The _ reference error unexpected results count.
        /// </summary>
        private static ExceptionCode _referenceErrorUnexpectedResultsCount = new ExceptionCode("607");

        /// <summary>
        ///     The _ reference error unexpected structure.
        /// </summary>
        private static ExceptionCode _referenceErrorUnexpectedStructure = new ExceptionCode("608");

        /// <summary>
        ///     The _ reference error unresolvable.
        /// </summary>
        private static ExceptionCode _referenceErrorUnresolvable = new ExceptionCode("603");

        /// <summary>
        ///     The _ reference error unsupported query for structure.
        /// </summary>
        private static ExceptionCode _referenceErrorUnsupportedQueryForStructure = new ExceptionCode("606");

        /// <summary>
        ///     The _ registry attempt to delete cross referenced structure.
        /// </summary>
        private static ExceptionCode _registryAttemptToDeleteCrossReferencedStructure = new ExceptionCode("1105");

        /// <summary>
        ///     The _ registry attempt to delete final structure.
        /// </summary>
        private static ExceptionCode _registryAttemptToDeleteFinalStructure = new ExceptionCode("1102");

        /// <summary>
        ///     The _ registry attempt to delete non existant structure.
        /// </summary>
        private static ExceptionCode _registryAttemptToDeleteNonExistantStructure = new ExceptionCode("1111");

        /// <summary>
        ///     The _ registry attempt to delete provision with registrations.
        /// </summary>
        private static ExceptionCode _registryAttemptToDeleteProvisionWithRegistrations = new ExceptionCode("1107");

        /// <summary>
        ///     The _ registry attempt to insert lower version under final.
        /// </summary>
        private static ExceptionCode _registryAttemptToInsertLowerVersionUnderFinal = new ExceptionCode("1103");

        /// <summary>
        ///     The _ registry attempt to update final structure.
        /// </summary>
        private static ExceptionCode _registryAttemptToUpdateFinalStructure = new ExceptionCode("1101");

        /// <summary>
        ///     The _ registry dataflow must reference DSD.
        /// </summary>
        private static ExceptionCode _registryDataflowMustReferenceDsd = new ExceptionCode("1113");

        /// <summary>
        ///     The _ registry data source can not be reached.
        /// </summary>
        private static ExceptionCode _registryDatasourceCannotBeReached = new ExceptionCode("1106");

        /// <summary>
        ///     The _ registry insertion deletes cross referenced structure.
        /// </summary>
        private static ExceptionCode _registryInsertionDeletesCrossReferencedStructure = new ExceptionCode("1104");

        /// <summary>
        ///     The _ registry metadataflow must reference dsd.
        /// </summary>
        private static ExceptionCode _registryMetadataflowMustReferenceDsd = new ExceptionCode("1112");

        /// <summary>
        ///     The _ registry no queries found.
        /// </summary>
        private static ExceptionCode _registryNoQueriesFound = new ExceptionCode("1110");

        /// <summary>
        ///     The _ registry subscription multiple response not supported.
        /// </summary>
        private static ExceptionCode _registrySubscriptionMultipleResponseNotSupported = new ExceptionCode("1109");

        /// <summary>
        ///     The _ registry subscription notification exists.
        /// </summary>
        private static ExceptionCode _registrySubscriptionNotificationExists = new ExceptionCode("1108");

        /// <summary>
        ///     The _ security account inactive.
        /// </summary>
        private static ExceptionCode _securityAccountInactive = new ExceptionCode("109");

        /// <summary>
        ///     The _ security auth level registry owner required.
        /// </summary>
        private static ExceptionCode _securityAuthLevelRegistryOwnerRequired = new ExceptionCode("103");

        /// <summary>
        ///     The _ security incorrect password.
        /// </summary>
        private static ExceptionCode _securityIncorrectPassword = new ExceptionCode("106");

        /// <summary>
        ///     The _ security invalid login.
        /// </summary>
        private static ExceptionCode _securityInvalidLogin = new ExceptionCode("104");

        /// <summary>
        ///     The _ security invalid token.
        /// </summary>
        private static ExceptionCode _securityInvalidToken = new ExceptionCode("108");

        /// <summary>
        ///     The _ security no criteria supplied.
        /// </summary>
        private static ExceptionCode _securityNoCriteriaSupplied = new ExceptionCode("110");

        /// <summary>
        ///     The _ security no user logged in.
        /// </summary>
        private static ExceptionCode _securityNoUserLoggedIn = new ExceptionCode("105");

        /// <summary>
        ///     The _ security session limit reached.
        /// </summary>
        private static ExceptionCode _securitySessionLimitReached = new ExceptionCode("107");

        /// <summary>
        ///     The _ security unauthorised.
        /// </summary>
        private static ExceptionCode _securityUnauthorised = new ExceptionCode("102");

        /// <summary>
        ///     The _ security unauthorised reference.
        /// </summary>
        private static ExceptionCode _securityUnauthorisedReference = new ExceptionCode("101");

        /// <summary>
        ///     The _ start date after end date.
        /// </summary>
        private static ExceptionCode _startDateAfterEndDate = new ExceptionCode("401");

        /// <summary>
        ///     The _ structure cross reference missing.
        /// </summary>
        private static ExceptionCode _structureCrossReferenceMissing = new ExceptionCode("009");

        /// <summary>
        ///     The _ structure identifiable missing id.
        /// </summary>
        private static ExceptionCode _structureIdentifiableMissingId = new ExceptionCode("006");

        /// <summary>
        ///     The _ structure identifiable missing name.
        /// </summary>
        private static ExceptionCode _structureIdentifiableMissingName = new ExceptionCode("007");

        /// <summary>
        ///     The _ structure identifiable missing urn.
        /// </summary>
        private static ExceptionCode _structureIdentifiableMissingUrn = new ExceptionCode("005");

        /// <summary>
        ///     The _ structure invalid id.
        /// </summary>
        private static ExceptionCode _structureInvalidId = new ExceptionCode("014");

        /// <summary>
        ///     The _ structure invalid id start alpha.
        /// </summary>
        private static ExceptionCode _structureInvalidIdStartAlpha = new ExceptionCode("016");

        /// <summary>
        ///     The _ structure invalid organisation scheme no content.
        /// </summary>
        private static ExceptionCode _structureInvalidOrganisationSchemeNoContent = new ExceptionCode("017");

        /// <summary>
        ///     The _ structure invalid version.
        /// </summary>
        private static ExceptionCode _structureInvalidVersion = new ExceptionCode("013");

        /// <summary>
        ///     The _ structure maintainable missing agency.
        /// </summary>
        private static ExceptionCode _structureMaintainableMissingAgency = new ExceptionCode("012");

        /// <summary>
        ///     The _ structure not found.
        /// </summary>
        private static ExceptionCode _structureNotFound = new ExceptionCode("008");

        /// <summary>
        ///     The _ structure uri malformed.
        /// </summary>
        private static ExceptionCode _structureUriMalformed = new ExceptionCode("015");

        /// <summary>
        ///     The _ structure urn malformed.
        /// </summary>
        private static ExceptionCode _structureUrnMalformed = new ExceptionCode("001");

        /// <summary>
        ///     The _ structure urn malformed missing prefix.
        /// </summary>
        private static ExceptionCode _structureUrnMalformedMissingPrefix = new ExceptionCode("002");

        /// <summary>
        ///     The _ structure urn malformed unkown prefix.
        /// </summary>
        private static ExceptionCode _structureUrnMalformedUnknownPrefix = new ExceptionCode("003");

        /// <summary>
        ///     The _ structure urn unexpected prefix.
        /// </summary>
        private static ExceptionCode _structureUrnUnexpectedPrefix = new ExceptionCode("011");

        /// <summary>
        ///     The _ unsupported.
        /// </summary>
        private static ExceptionCode _uNSUPPORTED = new ExceptionCode("405");

        /// <summary>
        ///     The _ unsupported datatype.
        /// </summary>
        private static ExceptionCode _unsupportedDataType = new ExceptionCode("917");

        /// <summary>
        ///     The _ unsupported transform.
        /// </summary>
        private static ExceptionCode _unsupportedTransform = new ExceptionCode("406");

        /// <summary>
        ///     The _ ustructure urn malformed part version infomation supplied.
        /// </summary>
        private static ExceptionCode _ustructureUrnMalformedPartVersionInfomationSupplied = new ExceptionCode("004");

        /// <summary>
        ///     The _ web service bad connection.
        /// </summary>
        private static ExceptionCode _webServiceBadConnection = new ExceptionCode("206");

        /// <summary>
        ///     The _ web service bad response.
        /// </summary>
        private static ExceptionCode _webServiceBadResponse = new ExceptionCode("205");

        /// <summary>
        ///     The _ web service configuration missing.
        /// </summary>
        private static ExceptionCode _webServiceConfigurationMissing = new ExceptionCode("201");

        /// <summary>
        ///     The _ web service endpoint type invalid.
        /// </summary>
        private static ExceptionCode _webServiceEndpointTypeInvalid = new ExceptionCode("207");

        /// <summary>
        ///     The _ web service invalid get data.
        /// </summary>
        private static ExceptionCode _webServiceInvalidGetData = new ExceptionCode("210");

        /// <summary>
        ///     The _ web service invalid get schema.
        /// </summary>
        private static ExceptionCode _webServiceInvalidGetSchema = new ExceptionCode("211");

        /// <summary>
        ///     The _ web service protocol missing.
        /// </summary>
        private static ExceptionCode _webServiceProtocolMissing = new ExceptionCode("204");

        /// <summary>
        ///     The _ web service request missing.
        /// </summary>
        private static ExceptionCode _webServiceRequestMissing = new ExceptionCode("202");

        /// <summary>
        ///     The _ web service socket timeout.
        /// </summary>
        private static ExceptionCode _webServiceSocketTimeout = new ExceptionCode("209");

        /// <summary>
        ///     The _ web service unsupported protocol.
        /// </summary>
        private static ExceptionCode _webServiceUnsupportedProtocol = new ExceptionCode("208");

        /// <summary>
        ///     The _ web service url missing.
        /// </summary>
        private static ExceptionCode _webServiceUrlMissing = new ExceptionCode("203");

        /// <summary>
        ///     The _ xml parse exception.
        /// </summary>
        private static ExceptionCode _xmlParseException = new ExceptionCode("800");

        /// <summary>
        ///     The _ object incomplete reference.
        /// </summary>
        private static ExceptionCode _objectIncompleteReference = new ExceptionCode("1206");

        /// <summary>
        ///     The _ object missing required attribute.
        /// </summary>
        private static ExceptionCode _objectMissingRequiredAttribute = new ExceptionCode("1208");

        /// <summary>
        ///     The _ object missing required element.
        /// </summary>
        private static ExceptionCode _objectMissingRequiredElement = new ExceptionCode("1209");

        /// <summary>
        ///     The _ object mutually exclusive.
        /// </summary>
        private static ExceptionCode _objectMutuallyExclusive = new ExceptionCode("1210");

        /// <summary>
        ///     The _ object partial reference.
        /// </summary>
        private static ExceptionCode _objectPartialReference = new ExceptionCode("1201");

        /// <summary>
        ///     The _ object structure construcution error.
        /// </summary>
        private static ExceptionCode _objectStructureConstructionError = new ExceptionCode("1207");

        /// <summary>
        ///     The _ can not resolve parent.
        /// </summary>
        private static ExceptionCode _cannotResolveParent = new ExceptionCode("705");

        /// <summary>
        ///     The _ code ref constains urn and codelist alias.
        /// </summary>
        private static ExceptionCode _codeRefConstainsUrnAndCodelistAlias = new ExceptionCode("713");

        /// <summary>
        ///     The _ code ref missing code id.
        /// </summary>
        private static ExceptionCode _codeRefMissingCodeId = new ExceptionCode("711");

        /// <summary>
        ///     The _ code ref missing code reference.
        /// </summary>
        private static ExceptionCode _codeRefMissingCodeReference = new ExceptionCode("712");

        /// <summary>
        ///     The _ code ref referenced twice.
        /// </summary>
        private static ExceptionCode _codeRefReferencedTwice = new ExceptionCode("715");

        /// <summary>
        ///     The _ database sql query error.
        /// </summary>
        private static ExceptionCode _databaseSqlQueryError = new ExceptionCode("501");

        /// <summary>
        ///     The _ dataset group missing attribute.
        /// </summary>
        private static ExceptionCode _datasetGroupMissingAttribute = new ExceptionCode("908");

        /// <summary>
        ///     The _ dataset group undefined.
        /// </summary>
        private static ExceptionCode _datasetGroupUndefined = new ExceptionCode("913");

        /// <summary>
        ///     The _ dataset group undefined attribute.
        /// </summary>
        private static ExceptionCode _datasetGroupUndefinedAttribute = new ExceptionCode("909");

        /// <summary>
        ///     The _ dataset invalid group.
        /// </summary>
        private static ExceptionCode _datasetInvalidGroup = new ExceptionCode("912");

        /// <summary>
        ///     The _ dataset invalid series key.
        /// </summary>
        private static ExceptionCode _datasetInvalidSeriesKey = new ExceptionCode("904");

        /// <summary>
        ///     The _ dataset missing group key concept.
        /// </summary>
        private static ExceptionCode _datasetMissingGroupKeyConcept = new ExceptionCode("914");

        /// <summary>
        ///     The _ dataset missing value for dimension.
        /// </summary>
        private static ExceptionCode _datasetMissingValueForDimension = new ExceptionCode("901");

        /// <summary>
        ///     The _ dataset obs missing attribute.
        /// </summary>
        private static ExceptionCode _datasetObsMissingAttribute = new ExceptionCode("910");

        /// <summary>
        ///     The _ dataset obs undefined attribute.
        /// </summary>
        private static ExceptionCode _datasetObsUndefinedAttribute = new ExceptionCode("911");

        /// <summary>
        ///     The _ dataset series key order incorrect.
        /// </summary>
        private static ExceptionCode _datasetSeriesKeyOrderIncorrect = new ExceptionCode("918");

        /// <summary>
        ///     The _ dataset series missing attribute.
        /// </summary>
        private static ExceptionCode _datasetSeriesMissingAttribute = new ExceptionCode("906");

        /// <summary>
        ///     The _ dataset series undefined attribute.
        /// </summary>
        private static ExceptionCode _datasetSeriesUndefinedAttribute = new ExceptionCode("907");

        /// <summary>
        ///     The _ dataset undefined dimension.
        /// </summary>
        private static ExceptionCode _datasetUndefinedDimension = new ExceptionCode("905");

        /// <summary>
        ///     The _ dataset undefined group key concept.
        /// </summary>
        private static ExceptionCode _datasetUndefinedGroupKeyConcept = new ExceptionCode("915");

        /// <summary>
        ///     The _ dataset unknown node.
        /// </summary>
        private static ExceptionCode _datasetUnknownNode = new ExceptionCode("916");

        /// <summary>
        ///     The _ dsd missing measure dimension.
        /// </summary>
        private static ExceptionCode _dsdMissingMeasureDimension = new ExceptionCode("903");

        /// <summary>
        ///     The _ dsd missing time dimension.
        /// </summary>
        private static ExceptionCode _dsdMissingTimeDimension = new ExceptionCode("902");

        /// <summary>
        ///     The _ duplicate alias.
        /// </summary>
        private static ExceptionCode _duplicateAlias = new ExceptionCode("709");

        /// <summary>
        ///     The _ duplicate code ref.
        /// </summary>
        private static ExceptionCode _duplicateCodeRef = new ExceptionCode("710");

        /// <summary>
        ///     The _ duplicate concept.
        /// </summary>
        private static ExceptionCode _duplicateConcept = new ExceptionCode("702");

        /// <summary>
        ///     The _ duplicate language.
        /// </summary>
        private static ExceptionCode _duplicateLanguage = new ExceptionCode("708");

        /// <summary>
        ///     The _ duplicate organisation role id.
        /// </summary>
        private static ExceptionCode _duplicateOrganisationRoleId = new ExceptionCode("720");

        /// <summary>
        ///     The _ duplicate referance.
        /// </summary>
        private static ExceptionCode _duplicateReference = new ExceptionCode("718");

        /// <summary>
        ///     The _ duplicate urn.
        /// </summary>
        private static ExceptionCode _duplicateUrn = new ExceptionCode("010");

        /// <summary>
        ///     The _ edi parse exception dim missing codelist.
        /// </summary>
        private static ExceptionCode _ediParseExceptionDimMissingCodelist = new ExceptionCode("801");

        /// <summary>
        ///     The _ email invalid format.
        /// </summary>
        private static ExceptionCode _emailInvalidFormat = new ExceptionCode("403");

        /// <summary>
        ///     The _ end date before start date.
        /// </summary>
        private static ExceptionCode _endDateBeforeStartDate = new ExceptionCode("402");

        /// <summary>
        ///     The _ external structure missing uri.
        /// </summary>
        private static ExceptionCode _externalStructureMissingUri = new ExceptionCode("721");

        /// <summary>
        ///     The _ external structure not found at uri.
        /// </summary>
        private static ExceptionCode _externalStructureNotFoundAtUri = new ExceptionCode("722");

        /// <summary>
        ///     The _ fail validation.
        /// </summary>
        private static ExceptionCode _failValidation = new ExceptionCode("701");

        /// <summary>
        ///     The _ group cannot reference time dimension.
        /// </summary>
        private static ExceptionCode _groupCannotReferenceTimeDimension = new ExceptionCode("723");

        /// <summary>
        ///     The _ hcl duplicate code reference.
        /// </summary>
        private static ExceptionCode _hclDuplicateCodeReference = new ExceptionCode("714");

        /// <summary>
        ///     The _ identifier component rep scheme no type.
        /// </summary>
        private static ExceptionCode _identifierComponentRepSchemeNoType = new ExceptionCode("1202");

        /// <summary>
        ///     The _ identifier expected external or type.
        /// </summary>
        private static ExceptionCode _identifierExpectedExternalOrType = new ExceptionCode("1205");

        /// <summary>
        ///     The _ identifier unknown rep sch type.
        /// </summary>
        private static ExceptionCode _identifierUnknownRepSchType = new ExceptionCode("1203");

        /// <summary>
        ///     The _ identifier unsupported rep sch type.
        /// </summary>
        private static ExceptionCode _identifierUnsupportedRepSchType = new ExceptionCode("1204");

        /// <summary>
        ///     The _ invalid date format.
        /// </summary>
        private static ExceptionCode _invalidDateFormat = new ExceptionCode("404");

        /// <summary>
        ///     The _ java collection empty.
        /// </summary>
        private static ExceptionCode _javaCollectionEmpty = new ExceptionCode("303");

        /// <summary>
        ///     The _ java io exception.
        /// </summary>
        private static ExceptionCode _dotNetIOException = new ExceptionCode("305");

        /// <summary>
        ///     The _ java property not found.
        /// </summary>
        private static ExceptionCode _javaPropertyNotFound = new ExceptionCode("304");

        /// <summary>
        ///     The _ java required object null.
        /// </summary>
        private static ExceptionCode _objectNull = new ExceptionCode("301");

        /// <summary>
        ///     The _ java unexpected argument.
        /// </summary>
        private static ExceptionCode _javaUnexpectedArgument = new ExceptionCode("302");

        /// <summary>
        ///     The _ key family duplicate group id.
        /// </summary>
        private static ExceptionCode _keyFamilyDuplicateGroupId = new ExceptionCode("716");

        /// <summary>
        ///     The _ key family group attribute missing group id.
        /// </summary>
        private static ExceptionCode _keyFamilyGroupAttributeMissingGroupId = new ExceptionCode("703");

        /// <summary>
        ///     The _ key family xs measure reference incorrect dimension type.
        /// </summary>
        private static ExceptionCode _keyFamilyXsMeasureReferenceIncorrectDimensionType = new ExceptionCode("704");

        /// <summary>
        ///     The _ key family xs measure reference uncoded dimension.
        /// </summary>
        private static ExceptionCode _keyFamilyXsMeasureReferenceUncodedDimension = new ExceptionCode("717");

        /// <summary>
        ///     The _ maintinable ref incomplete.
        /// </summary>
        private static ExceptionCode _maintainableRefIncomplete = new ExceptionCode("707");

        /// <summary>
        ///     The _ parent recursive loop.
        /// </summary>
        private static ExceptionCode _parentRecursiveLoop = new ExceptionCode("706");

        /// <summary>
        ///     The _ parse error not sdmx.
        /// </summary>
        private static ExceptionCode _parseErrorNotSdmx = new ExceptionCode("804");

        /// <summary>
        ///     The _ parse error not xml.
        /// </summary>
        private static ExceptionCode _parseErrorNotXml = new ExceptionCode("803");

        /// <summary>
        ///     The _ parse error not xml or edi.
        /// </summary>
        private static ExceptionCode _parseErrorNotXmlOrEdi = new ExceptionCode("802");

        /// <summary>
        ///     The _ partial target id duplicates full target id.
        /// </summary>
        private static ExceptionCode _partialTargetIdDuplicatesFullTargetId = new ExceptionCode("719");

        /// <summary>
        ///     The _ query selection illegal and agency id.
        /// </summary>
        private static ExceptionCode _querySelectionIllegalAndAgencyId = new ExceptionCode("1007");

        /// <summary>
        ///     The _ query selection illegal operator.
        /// </summary>
        private static ExceptionCode _querySelectionIllegalOperator = new ExceptionCode("1008");

        /// <summary>
        ///     The _ query selection illegal and codes in same dimension.
        /// </summary>
        private static ExceptionCode _querySelectionIllegalAndCodesInSameDimension = new ExceptionCode("1003");

        /// <summary>
        ///     The _ query selection illegal and key family.
        /// </summary>
        private static ExceptionCode _querySelectionIllegalAndKeyfamily = new ExceptionCode("1004");

        /// <summary>
        ///     The _ query selection missing concept.
        /// </summary>
        private static ExceptionCode _querySelectionMissingConcept = new ExceptionCode("1001");

        /// <summary>
        ///     The _ query selection missing concept value.
        /// </summary>
        private static ExceptionCode _querySelectionMissingConceptValue = new ExceptionCode("1002");

        /// <summary>
        ///     The _ query selection multiple date from.
        /// </summary>
        private static ExceptionCode _querySelectionMultipleDateFrom = new ExceptionCode("1005");

        /// <summary>
        ///     The _ query selection multiple date to.
        /// </summary>
        private static ExceptionCode _querySelectionMultipleDateTo = new ExceptionCode("1006");

        /// <summary>
        ///     The _ report structure invalid identifier reference.
        /// </summary>
        private static ExceptionCode _reportStructureInvalidIdentifierReference = new ExceptionCode("724");

        #endregion

        #region Fields

        /// <summary>
        ///     The code.
        /// </summary>
        private readonly string code;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionCode"/> class.
        /// </summary>
        /// <param name="code_0">
        /// The code_0.
        /// </param>
        private ExceptionCode(string code_0)
        {
            this.code = code_0;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets or sets the object incomplete reference.
        /// </summary>
        public static ExceptionCode ObjectIncompleteReference
        {
            get
            {
                return _objectIncompleteReference;
            }

            set
            {
                _objectIncompleteReference = value;
            }
        }

        /// <summary>
        ///     Gets or sets the object missing required attribute.
        /// </summary>
        public static ExceptionCode ObjectMissingRequiredAttribute
        {
            get
            {
                return _objectMissingRequiredAttribute;
            }

            set
            {
                _objectMissingRequiredAttribute = value;
            }
        }

        /// <summary>
        ///     Gets or sets the object missing required element.
        /// </summary>
        public static ExceptionCode ObjectMissingRequiredElement
        {
            get
            {
                return _objectMissingRequiredElement;
            }

            set
            {
                _objectMissingRequiredElement = value;
            }
        }

        /// <summary>
        ///     Gets or sets the object mutually exclusive.
        /// </summary>
        public static ExceptionCode ObjectMutuallyExclusive
        {
            get
            {
                return _objectMutuallyExclusive;
            }

            set
            {
                _objectMutuallyExclusive = value;
            }
        }

        /// <summary>
        ///     Gets or sets the object partial reference.
        /// </summary>
        public static ExceptionCode ObjectPartialReference
        {
            get
            {
                return _objectPartialReference;
            }

            set
            {
                _objectPartialReference = value;
            }
        }

        /// <summary>
        ///     Gets or sets the object structure construcution error.
        /// </summary>
        public static ExceptionCode ObjectStructureConstructionError
        {
            get
            {
                return _objectStructureConstructionError;
            }

            set
            {
                _objectStructureConstructionError = value;
            }
        }

        /// <summary>
        ///     Gets or sets the can not resolve parent.
        /// </summary>
        public static ExceptionCode CannotResolveParent
        {
            get
            {
                return _cannotResolveParent;
            }

            set
            {
                _cannotResolveParent = value;
            }
        }

        /// <summary>
        ///     Gets or sets the code ref constains urn and codelist alias.
        /// </summary>
        public static ExceptionCode CodeRefConstainsUrnAndCodelistAlias
        {
            get
            {
                return _codeRefConstainsUrnAndCodelistAlias;
            }

            set
            {
                _codeRefConstainsUrnAndCodelistAlias = value;
            }
        }

        /// <summary>
        ///     Gets or sets the code ref missing code id.
        /// </summary>
        public static ExceptionCode CodeRefMissingCodeId
        {
            get
            {
                return _codeRefMissingCodeId;
            }

            set
            {
                _codeRefMissingCodeId = value;
            }
        }

        /// <summary>
        ///     Gets or sets the code ref missing code reference.
        /// </summary>
        public static ExceptionCode CodeRefMissingCodeReference
        {
            get
            {
                return _codeRefMissingCodeReference;
            }

            set
            {
                _codeRefMissingCodeReference = value;
            }
        }

        /// <summary>
        ///     Gets or sets the code ref referenced twice.
        /// </summary>
        public static ExceptionCode CodeRefReferencedTwice
        {
            get
            {
                return _codeRefReferencedTwice;
            }

            set
            {
                _codeRefReferencedTwice = value;
            }
        }

        /// <summary>
        ///     Gets or sets the database sql query error.
        /// </summary>
        public static ExceptionCode DatabaseSqlQueryError
        {
            get
            {
                return _databaseSqlQueryError;
            }

            set
            {
                _databaseSqlQueryError = value;
            }
        }

        /// <summary>
        ///     Gets or sets the dataset group missing attribute.
        /// </summary>
        public static ExceptionCode DatasetGroupMissingAttribute
        {
            get
            {
                return _datasetGroupMissingAttribute;
            }

            set
            {
                _datasetGroupMissingAttribute = value;
            }
        }

        /// <summary>
        ///     Gets or sets the dataset group undefined.
        /// </summary>
        public static ExceptionCode DatasetGroupUndefined
        {
            get
            {
                return _datasetGroupUndefined;
            }

            set
            {
                _datasetGroupUndefined = value;
            }
        }

        /// <summary>
        ///     Gets or sets the dataset group undefined attribute.
        /// </summary>
        public static ExceptionCode DatasetGroupUndefinedAttribute
        {
            get
            {
                return _datasetGroupUndefinedAttribute;
            }

            set
            {
                _datasetGroupUndefinedAttribute = value;
            }
        }

        /// <summary>
        ///     Gets or sets the dataset invalid group.
        /// </summary>
        public static ExceptionCode DatasetInvalidGroup
        {
            get
            {
                return _datasetInvalidGroup;
            }

            set
            {
                _datasetInvalidGroup = value;
            }
        }

        /// <summary>
        ///     Gets or sets the dataset invalid series key.
        /// </summary>
        public static ExceptionCode DatasetInvalidSeriesKey
        {
            get
            {
                return _datasetInvalidSeriesKey;
            }

            set
            {
                _datasetInvalidSeriesKey = value;
            }
        }

        /// <summary>
        ///     Gets or sets the dataset missing group key concept.
        /// </summary>
        public static ExceptionCode DatasetMissingGroupKeyConcept
        {
            get
            {
                return _datasetMissingGroupKeyConcept;
            }

            set
            {
                _datasetMissingGroupKeyConcept = value;
            }
        }

        /// <summary>
        ///     Gets or sets the dataset missing value for dimension.
        /// </summary>
        public static ExceptionCode DatasetMissingValueForDimension
        {
            get
            {
                return _datasetMissingValueForDimension;
            }

            set
            {
                _datasetMissingValueForDimension = value;
            }
        }

        /// <summary>
        ///     Gets or sets the dataset obs missing attribute.
        /// </summary>
        public static ExceptionCode DatasetObsMissingAttribute
        {
            get
            {
                return _datasetObsMissingAttribute;
            }

            set
            {
                _datasetObsMissingAttribute = value;
            }
        }

        /// <summary>
        ///     Gets or sets the dataset obs undefined attribute.
        /// </summary>
        public static ExceptionCode DatasetObsUndefinedAttribute
        {
            get
            {
                return _datasetObsUndefinedAttribute;
            }

            set
            {
                _datasetObsUndefinedAttribute = value;
            }
        }

        /// <summary>
        ///     Gets or sets the dataset series key order incorrect.
        /// </summary>
        public static ExceptionCode DatasetSeriesKeyOrderIncorrect
        {
            get
            {
                return _datasetSeriesKeyOrderIncorrect;
            }

            set
            {
                _datasetSeriesKeyOrderIncorrect = value;
            }
        }

        /// <summary>
        ///     Gets or sets the dataset series missing attribute.
        /// </summary>
        public static ExceptionCode DatasetSeriesMissingAttribute
        {
            get
            {
                return _datasetSeriesMissingAttribute;
            }

            set
            {
                _datasetSeriesMissingAttribute = value;
            }
        }

        /// <summary>
        ///     Gets or sets the dataset series undefined attribute.
        /// </summary>
        public static ExceptionCode DatasetSeriesUndefinedAttribute
        {
            get
            {
                return _datasetSeriesUndefinedAttribute;
            }

            set
            {
                _datasetSeriesUndefinedAttribute = value;
            }
        }

        /// <summary>
        ///     Gets or sets the dataset undefined dimension.
        /// </summary>
        public static ExceptionCode DatasetUndefinedDimension
        {
            get
            {
                return _datasetUndefinedDimension;
            }

            set
            {
                _datasetUndefinedDimension = value;
            }
        }

        /// <summary>
        ///     Gets or sets the dataset undefined group key concept.
        /// </summary>
        public static ExceptionCode DatasetUndefinedGroupKeyConcept
        {
            get
            {
                return _datasetUndefinedGroupKeyConcept;
            }

            set
            {
                _datasetUndefinedGroupKeyConcept = value;
            }
        }

        /// <summary>
        ///     Gets or sets the dataset unknown node.
        /// </summary>
        public static ExceptionCode DatasetUnknownNode
        {
            get
            {
                return _datasetUnknownNode;
            }

            set
            {
                _datasetUnknownNode = value;
            }
        }

        /// <summary>
        ///     Gets or sets the dsd missing measure dimension.
        /// </summary>
        public static ExceptionCode DsdMissingMeasureDimension
        {
            get
            {
                return _dsdMissingMeasureDimension;
            }

            set
            {
                _dsdMissingMeasureDimension = value;
            }
        }

        /// <summary>
        ///     Gets or sets the dsd missing time dimension.
        /// </summary>
        public static ExceptionCode DsdMissingTimeDimension
        {
            get
            {
                return _dsdMissingTimeDimension;
            }

            set
            {
                _dsdMissingTimeDimension = value;
            }
        }

        /// <summary>
        ///     Gets or sets the duplicate alias.
        /// </summary>
        public static ExceptionCode DuplicateAlias
        {
            get
            {
                return _duplicateAlias;
            }

            set
            {
                _duplicateAlias = value;
            }
        }

        /// <summary>
        ///     Gets or sets the duplicate code ref.
        /// </summary>
        public static ExceptionCode DuplicateCodeRef
        {
            get
            {
                return _duplicateCodeRef;
            }

            set
            {
                _duplicateCodeRef = value;
            }
        }

        /// <summary>
        ///     Gets or sets the duplicate concept.
        /// </summary>
        public static ExceptionCode DuplicateConcept
        {
            get
            {
                return _duplicateConcept;
            }

            set
            {
                _duplicateConcept = value;
            }
        }

        /// <summary>
        ///     Gets or sets the duplicate language.
        /// </summary>
        public static ExceptionCode DuplicateLanguage
        {
            get
            {
                return _duplicateLanguage;
            }

            set
            {
                _duplicateLanguage = value;
            }
        }

        /// <summary>
        ///     Gets or sets the duplicate organisation role id.
        /// </summary>
        public static ExceptionCode DuplicateOrganisationRoleId
        {
            get
            {
                return _duplicateOrganisationRoleId;
            }

            set
            {
                _duplicateOrganisationRoleId = value;
            }
        }

        /// <summary>
        ///     Gets or sets the duplicate referance.
        /// </summary>
        public static ExceptionCode DuplicateReference
        {
            get
            {
                return _duplicateReference;
            }

            set
            {
                _duplicateReference = value;
            }
        }

        /// <summary>
        ///     Gets or sets the duplicate urn.
        /// </summary>
        public static ExceptionCode DuplicateUrn
        {
            get
            {
                return _duplicateUrn;
            }

            set
            {
                _duplicateUrn = value;
            }
        }

        /// <summary>
        ///     Gets or sets the edi parse exception dim missing codelist.
        /// </summary>
        public static ExceptionCode EdiParseExceptionDimMissingCodelist
        {
            get
            {
                return _ediParseExceptionDimMissingCodelist;
            }

            set
            {
                _ediParseExceptionDimMissingCodelist = value;
            }
        }

        /// <summary>
        ///     Gets or sets the email invalid format.
        /// </summary>
        public static ExceptionCode EmailInvalidFormat
        {
            get
            {
                return _emailInvalidFormat;
            }

            set
            {
                _emailInvalidFormat = value;
            }
        }

        /// <summary>
        ///     Gets or sets the end date before start date.
        /// </summary>
        public static ExceptionCode EndDateBeforeStartDate
        {
            get
            {
                return _endDateBeforeStartDate;
            }

            set
            {
                _endDateBeforeStartDate = value;
            }
        }

        /// <summary>
        ///     Gets or sets the external structure missing uri.
        /// </summary>
        public static ExceptionCode ExternalStructureMissingUri
        {
            get
            {
                return _externalStructureMissingUri;
            }

            set
            {
                _externalStructureMissingUri = value;
            }
        }

        /// <summary>
        ///     Gets or sets the external structure not found at uri.
        /// </summary>
        public static ExceptionCode ExternalStructureNotFoundAtUri
        {
            get
            {
                return _externalStructureNotFoundAtUri;
            }

            set
            {
                _externalStructureNotFoundAtUri = value;
            }
        }

        /// <summary>
        ///     Gets or sets the fail validation.
        /// </summary>
        public static ExceptionCode FailValidation
        {
            get
            {
                return _failValidation;
            }

            set
            {
                _failValidation = value;
            }
        }

        /// <summary>
        ///     Gets or sets the group cannot reference time dimension.
        /// </summary>
        public static ExceptionCode GroupCannotReferenceTimeDimension
        {
            get
            {
                return _groupCannotReferenceTimeDimension;
            }

            set
            {
                _groupCannotReferenceTimeDimension = value;
            }
        }

        /// <summary>
        ///     Gets or sets the hcl duplicate code reference.
        /// </summary>
        public static ExceptionCode HclDuplicateCodeReference
        {
            get
            {
                return _hclDuplicateCodeReference;
            }

            set
            {
                _hclDuplicateCodeReference = value;
            }
        }

        /// <summary>
        ///     Gets or sets the identifier component rep scheme no type.
        /// </summary>
        public static ExceptionCode IdentifierComponentRepSchemeNoType
        {
            get
            {
                return _identifierComponentRepSchemeNoType;
            }

            set
            {
                _identifierComponentRepSchemeNoType = value;
            }
        }

        /// <summary>
        ///     Gets or sets the identifier expected external or type.
        /// </summary>
        public static ExceptionCode IdentifierExpectedExternalOrType
        {
            get
            {
                return _identifierExpectedExternalOrType;
            }

            set
            {
                _identifierExpectedExternalOrType = value;
            }
        }

        /// <summary>
        ///     Gets or sets the identifier unknown rep sch type.
        /// </summary>
        public static ExceptionCode IdentifierUnknownRepSchType
        {
            get
            {
                return _identifierUnknownRepSchType;
            }

            set
            {
                _identifierUnknownRepSchType = value;
            }
        }

        /// <summary>
        ///     Gets or sets the identifier unsupported rep sch type.
        /// </summary>
        public static ExceptionCode IdentifierUnsupportedRepSchType
        {
            get
            {
                return _identifierUnsupportedRepSchType;
            }

            set
            {
                _identifierUnsupportedRepSchType = value;
            }
        }

        /// <summary>
        ///     Gets or sets the invalid date format.
        /// </summary>
        public static ExceptionCode InvalidDateFormat
        {
            get
            {
                return _invalidDateFormat;
            }

            set
            {
                _invalidDateFormat = value;
            }
        }

        /// <summary>
        ///     Gets or sets the java collection empty.
        /// </summary>
        public static ExceptionCode JavaCollectionEmpty
        {
            get
            {
                return _javaCollectionEmpty;
            }

            set
            {
                _javaCollectionEmpty = value;
            }
        }

        /// <summary>
        ///     Gets or sets the .NET IO exception.
        /// </summary>
        public static ExceptionCode DotNetIOException
        {
            get
            {
                return _dotNetIOException;
            }

            set
            {
                _dotNetIOException = value;
            }
        }

        /// <summary>
        ///     Gets or sets the java property not found.
        /// </summary>
        public static ExceptionCode JavaPropertyNotFound
        {
            get
            {
                return _javaPropertyNotFound;
            }

            set
            {
                _javaPropertyNotFound = value;
            }
        }

        /// <summary>
        ///     Gets or sets the java required object null.
        /// </summary>
        public static ExceptionCode ObjectNull
        {
            get
            {
                return _objectNull;
            }

            set
            {
                _objectNull = value;
            }
        }

        /// <summary>
        ///     Gets or sets the java unexpected argument.
        /// </summary>
        public static ExceptionCode JavaUnexpectedArgument
        {
            get
            {
                return _javaUnexpectedArgument;
            }

            set
            {
                _javaUnexpectedArgument = value;
            }
        }

        /// <summary>
        ///     Gets or sets the key family duplicate group id.
        /// </summary>
        public static ExceptionCode KeyFamilyDuplicateGroupId
        {
            get
            {
                return _keyFamilyDuplicateGroupId;
            }

            set
            {
                _keyFamilyDuplicateGroupId = value;
            }
        }

        /// <summary>
        ///     Gets or sets the key family group attribute missing groupid.
        /// </summary>
        public static ExceptionCode KeyFamilyGroupAttributeMissingGroupId
        {
            get
            {
                return _keyFamilyGroupAttributeMissingGroupId;
            }

            set
            {
                _keyFamilyGroupAttributeMissingGroupId = value;
            }
        }

        /// <summary>
        ///     Gets or sets the key family xs measure reference incorrect dimension type.
        /// </summary>
        public static ExceptionCode KeyFamilyXsMeasureReferenceIncorrectDimensionType
        {
            get
            {
                return _keyFamilyXsMeasureReferenceIncorrectDimensionType;
            }

            set
            {
                _keyFamilyXsMeasureReferenceIncorrectDimensionType = value;
            }
        }

        /// <summary>
        ///     Gets or sets the key family xs measure reference uncoded dimension.
        /// </summary>
        public static ExceptionCode KeyFamilyXsMeasureReferenceUncodedDimension
        {
            get
            {
                return _keyFamilyXsMeasureReferenceUncodedDimension;
            }

            set
            {
                _keyFamilyXsMeasureReferenceUncodedDimension = value;
            }
        }

        /// <summary>
        ///     Gets or sets the maintinable ref incomplete.
        /// </summary>
        public static ExceptionCode MaintainableRefIncomplete
        {
            get
            {
                return _maintainableRefIncomplete;
            }

            set
            {
                _maintainableRefIncomplete = value;
            }
        }

        /// <summary>
        ///     Gets or sets the parent recursive loop.
        /// </summary>
        public static ExceptionCode ParentRecursiveLoop
        {
            get
            {
                return _parentRecursiveLoop;
            }

            set
            {
                _parentRecursiveLoop = value;
            }
        }

        /// <summary>
        ///     Gets or sets the parse error not sdmx.
        /// </summary>
        public static ExceptionCode ParseErrorNotSdmx
        {
            get
            {
                return _parseErrorNotSdmx;
            }

            set
            {
                _parseErrorNotSdmx = value;
            }
        }

        /// <summary>
        ///     Gets or sets the parse error not xml.
        /// </summary>
        public static ExceptionCode ParseErrorNotXml
        {
            get
            {
                return _parseErrorNotXml;
            }

            set
            {
                _parseErrorNotXml = value;
            }
        }

        /// <summary>
        ///     Gets or sets the parse error not xml or edi.
        /// </summary>
        public static ExceptionCode ParseErrorNotXmlOrEdi
        {
            get
            {
                return _parseErrorNotXmlOrEdi;
            }

            set
            {
                _parseErrorNotXmlOrEdi = value;
            }
        }

        /// <summary>
        ///     Gets or sets the partial target id duplicates full target id.
        /// </summary>
        public static ExceptionCode PartialTargetIdDuplicatesFullTargetId
        {
            get
            {
                return _partialTargetIdDuplicatesFullTargetId;
            }

            set
            {
                _partialTargetIdDuplicatesFullTargetId = value;
            }
        }

        /// <summary>
        ///     Gets or sets the query selection illegal and agency id.
        /// </summary>
        public static ExceptionCode QuerySelectionIllegalAndAgencyId
        {
            get
            {
                return _querySelectionIllegalAndAgencyId;
            }

            set
            {
                _querySelectionIllegalAndAgencyId = value;
            }
        }

        /// <summary>
        ///     Gets or sets the query selection illegal operator.
        /// </summary>
        public static ExceptionCode QuerySelectionIllegalOperator
        {
            get
            {
                return _querySelectionIllegalOperator;
            }

            set
            {
                _querySelectionIllegalOperator = value;
            }
        }

        /// <summary>
        ///     Gets or sets the query selection illegal and codes in same dimension.
        /// </summary>
        public static ExceptionCode QuerySelectionIllegalAndCodesInSameDimension
        {
            get
            {
                return _querySelectionIllegalAndCodesInSameDimension;
            }

            set
            {
                _querySelectionIllegalAndCodesInSameDimension = value;
            }
        }

        /// <summary>
        ///     Gets or sets the query selection illegal and keyfamily.
        /// </summary>
        public static ExceptionCode QuerySelectionIllegalAndKeyfamily
        {
            get
            {
                return _querySelectionIllegalAndKeyfamily;
            }

            set
            {
                _querySelectionIllegalAndKeyfamily = value;
            }
        }

        /// <summary>
        ///     Gets or sets the query selection missing concept.
        /// </summary>
        public static ExceptionCode QuerySelectionMissingConcept
        {
            get
            {
                return _querySelectionMissingConcept;
            }

            set
            {
                _querySelectionMissingConcept = value;
            }
        }

        /// <summary>
        ///     Gets or sets the query selection missing concept value.
        /// </summary>
        public static ExceptionCode QuerySelectionMissingConceptValue
        {
            get
            {
                return _querySelectionMissingConceptValue;
            }

            set
            {
                _querySelectionMissingConceptValue = value;
            }
        }

        /// <summary>
        ///     Gets or sets the query selection multiple date from.
        /// </summary>
        public static ExceptionCode QuerySelectionMultipleDateFrom
        {
            get
            {
                return _querySelectionMultipleDateFrom;
            }

            set
            {
                _querySelectionMultipleDateFrom = value;
            }
        }

        /// <summary>
        ///     Gets or sets the query selection multiple date to.
        /// </summary>
        public static ExceptionCode QuerySelectionMultipleDateTo
        {
            get
            {
                return _querySelectionMultipleDateTo;
            }

            set
            {
                _querySelectionMultipleDateTo = value;
            }
        }

        /// <summary>
        ///     Gets or sets the reference error.
        /// </summary>
        public static ExceptionCode ReferenceError
        {
            get
            {
                return _referenceError;
            }

            set
            {
                _referenceError = value;
            }
        }

        /// <summary>
        ///     Gets or sets the reference error missing parameters.
        /// </summary>
        public static ExceptionCode ReferenceErrorMissingParameters
        {
            get
            {
                return _referenceErrorMissingParameters;
            }

            set
            {
                _referenceErrorMissingParameters = value;
            }
        }

        /// <summary>
        ///     Gets or sets the reference error multiple responses expected one.
        /// </summary>
        public static ExceptionCode ReferenceErrorMultipleResponsesExpectedOne
        {
            get
            {
                return _referenceErrorMultipleResponsesExpectedOne;
            }

            set
            {
                _referenceErrorMultipleResponsesExpectedOne = value;
            }
        }

        /// <summary>
        ///     Gets or sets the reference error no type.
        /// </summary>
        public static ExceptionCode ReferenceErrorNoType
        {
            get
            {
                return _referenceErrorNoType;
            }

            set
            {
                _referenceErrorNoType = value;
            }
        }

        /// <summary>
        ///     Gets or sets the reference error unexpected results count.
        /// </summary>
        public static ExceptionCode ReferenceErrorUnexpectedResultsCount
        {
            get
            {
                return _referenceErrorUnexpectedResultsCount;
            }

            set
            {
                _referenceErrorUnexpectedResultsCount = value;
            }
        }

        /// <summary>
        ///     Gets or sets the reference error unexpected structure.
        /// </summary>
        public static ExceptionCode ReferenceErrorUnexpectedStructure
        {
            get
            {
                return _referenceErrorUnexpectedStructure;
            }

            set
            {
                _referenceErrorUnexpectedStructure = value;
            }
        }

        /// <summary>
        ///     Gets or sets the reference error unresolvable.
        /// </summary>
        public static ExceptionCode ReferenceErrorUnresolvable
        {
            get
            {
                return _referenceErrorUnresolvable;
            }

            set
            {
                _referenceErrorUnresolvable = value;
            }
        }

        /// <summary>
        ///     Gets or sets the reference error unsupported query for structure.
        /// </summary>
        public static ExceptionCode ReferenceErrorUnsupportedQueryForStructure
        {
            get
            {
                return _referenceErrorUnsupportedQueryForStructure;
            }

            set
            {
                _referenceErrorUnsupportedQueryForStructure = value;
            }
        }

        /// <summary>
        ///     Gets or sets the registry attempt to delete cross referenced structure.
        /// </summary>
        public static ExceptionCode RegistryAttemptToDeleteCrossReferencedStructure
        {
            get
            {
                return _registryAttemptToDeleteCrossReferencedStructure;
            }

            set
            {
                _registryAttemptToDeleteCrossReferencedStructure = value;
            }
        }

        /// <summary>
        ///     Gets or sets the registry attempt to delete final structure.
        /// </summary>
        public static ExceptionCode RegistryAttemptToDeleteFinalStructure
        {
            get
            {
                return _registryAttemptToDeleteFinalStructure;
            }

            set
            {
                _registryAttemptToDeleteFinalStructure = value;
            }
        }

        /// <summary>
        ///     Gets or sets the registry attempt to delete non existant structure.
        /// </summary>
        public static ExceptionCode RegistryAttemptToDeleteNonExistantStructure
        {
            get
            {
                return _registryAttemptToDeleteNonExistantStructure;
            }

            set
            {
                _registryAttemptToDeleteNonExistantStructure = value;
            }
        }

        /// <summary>
        ///     Gets or sets the registry attempt to delete provision with registrations.
        /// </summary>
        public static ExceptionCode RegistryAttemptToDeleteProvisionWithRegistrations
        {
            get
            {
                return _registryAttemptToDeleteProvisionWithRegistrations;
            }

            set
            {
                _registryAttemptToDeleteProvisionWithRegistrations = value;
            }
        }

        /// <summary>
        ///     Gets or sets the registry attempt to insert lower version under final.
        /// </summary>
        public static ExceptionCode RegistryAttemptToInsertLowerVersionUnderFinal
        {
            get
            {
                return _registryAttemptToInsertLowerVersionUnderFinal;
            }

            set
            {
                _registryAttemptToInsertLowerVersionUnderFinal = value;
            }
        }

        /// <summary>
        ///     Gets or sets the registry attempt to update final structure.
        /// </summary>
        public static ExceptionCode RegistryAttemptToUpdateFinalStructure
        {
            get
            {
                return _registryAttemptToUpdateFinalStructure;
            }

            set
            {
                _registryAttemptToUpdateFinalStructure = value;
            }
        }

        /// <summary>
        ///     Gets or sets the registry dataflow must reference dsd.
        /// </summary>
        public static ExceptionCode RegistryDataflowMustReferenceDsd
        {
            get
            {
                return _registryDataflowMustReferenceDsd;
            }

            set
            {
                _registryDataflowMustReferenceDsd = value;
            }
        }

        /// <summary>
        ///     Gets or sets the registry datasource can not be reached.
        /// </summary>
        public static ExceptionCode RegistryDatasourceCannotBeReached
        {
            get
            {
                return _registryDatasourceCannotBeReached;
            }

            set
            {
                _registryDatasourceCannotBeReached = value;
            }
        }

        /// <summary>
        ///     Gets or sets the registry insertion deletes cross referenced structure.
        /// </summary>
        public static ExceptionCode RegistryInsertionDeletesCrossReferencedStructure
        {
            get
            {
                return _registryInsertionDeletesCrossReferencedStructure;
            }

            set
            {
                _registryInsertionDeletesCrossReferencedStructure = value;
            }
        }

        /// <summary>
        ///     Gets or sets the registry metadataflow must reference dsd.
        /// </summary>
        public static ExceptionCode RegistryMetadataflowMustReferenceDsd
        {
            get
            {
                return _registryMetadataflowMustReferenceDsd;
            }

            set
            {
                _registryMetadataflowMustReferenceDsd = value;
            }
        }

        /// <summary>
        ///     Gets or sets the registry no queries found.
        /// </summary>
        public static ExceptionCode RegistryNoQueriesFound
        {
            get
            {
                return _registryNoQueriesFound;
            }

            set
            {
                _registryNoQueriesFound = value;
            }
        }

        /// <summary>
        ///     Gets or sets the registry subscription multiple response not supported.
        /// </summary>
        public static ExceptionCode RegistrySubscriptionMultipleResponseNotSupported
        {
            get
            {
                return _registrySubscriptionMultipleResponseNotSupported;
            }

            set
            {
                _registrySubscriptionMultipleResponseNotSupported = value;
            }
        }

        /// <summary>
        ///     Gets or sets the registry subscription notification exists.
        /// </summary>
        public static ExceptionCode RegistrySubscriptionNotificationExists
        {
            get
            {
                return _registrySubscriptionNotificationExists;
            }

            set
            {
                _registrySubscriptionNotificationExists = value;
            }
        }

        /// <summary>
        ///     Gets or sets the report structure invalid identifier reference.
        /// </summary>
        public static ExceptionCode ReportStructureInvalidIdentifierReference
        {
            get
            {
                return _reportStructureInvalidIdentifierReference;
            }

            set
            {
                _reportStructureInvalidIdentifierReference = value;
            }
        }

        /// <summary>
        ///     Gets or sets the security account inactive.
        /// </summary>
        public static ExceptionCode SecurityAccountInactive
        {
            get
            {
                return _securityAccountInactive;
            }

            set
            {
                _securityAccountInactive = value;
            }
        }

        /// <summary>
        ///     Gets or sets the security auth level registry owner required.
        /// </summary>
        public static ExceptionCode SecurityAuthLevelRegistryOwnerRequired
        {
            get
            {
                return _securityAuthLevelRegistryOwnerRequired;
            }

            set
            {
                _securityAuthLevelRegistryOwnerRequired = value;
            }
        }

        /// <summary>
        ///     Gets or sets the security incorrect password.
        /// </summary>
        public static ExceptionCode SecurityIncorrectPassword
        {
            get
            {
                return _securityIncorrectPassword;
            }

            set
            {
                _securityIncorrectPassword = value;
            }
        }

        /// <summary>
        ///     Gets or sets the security invalid login.
        /// </summary>
        public static ExceptionCode SecurityInvalidLogin
        {
            get
            {
                return _securityInvalidLogin;
            }

            set
            {
                _securityInvalidLogin = value;
            }
        }

        /// <summary>
        ///     Gets or sets the security invalid token.
        /// </summary>
        public static ExceptionCode SecurityInvalidToken
        {
            get
            {
                return _securityInvalidToken;
            }

            set
            {
                _securityInvalidToken = value;
            }
        }

        /// <summary>
        ///     Gets or sets the security no criteria supplied.
        /// </summary>
        public static ExceptionCode SecurityNoCriteriaSupplied
        {
            get
            {
                return _securityNoCriteriaSupplied;
            }

            set
            {
                _securityNoCriteriaSupplied = value;
            }
        }

        /// <summary>
        ///     Gets or sets the security no user logged in.
        /// </summary>
        public static ExceptionCode SecurityNoUserLoggedIn
        {
            get
            {
                return _securityNoUserLoggedIn;
            }

            set
            {
                _securityNoUserLoggedIn = value;
            }
        }

        /// <summary>
        ///     Gets or sets the security session limit reached.
        /// </summary>
        public static ExceptionCode SecuritySessionLimitReached
        {
            get
            {
                return _securitySessionLimitReached;
            }

            set
            {
                _securitySessionLimitReached = value;
            }
        }

        /// <summary>
        ///     Gets or sets the security unauthorised.
        /// </summary>
        public static ExceptionCode SecurityUnauthorised
        {
            get
            {
                return _securityUnauthorised;
            }

            set
            {
                _securityUnauthorised = value;
            }
        }

        /// <summary>
        ///     Gets or sets the security unauthorised reference.
        /// </summary>
        public static ExceptionCode SecurityUnauthorisedReference
        {
            get
            {
                return _securityUnauthorisedReference;
            }

            set
            {
                _securityUnauthorisedReference = value;
            }
        }

        /// <summary>
        ///     Gets or sets the start date after end date.
        /// </summary>
        public static ExceptionCode StartDateAfterEndDate
        {
            get
            {
                return _startDateAfterEndDate;
            }

            set
            {
                _startDateAfterEndDate = value;
            }
        }

        /// <summary>
        ///     Gets or sets the structure cross reference missing.
        /// </summary>
        public static ExceptionCode StructureCrossReferenceMissing
        {
            get
            {
                return _structureCrossReferenceMissing;
            }

            set
            {
                _structureCrossReferenceMissing = value;
            }
        }

        /// <summary>
        ///     Gets or sets the structure identifiable missing id.
        /// </summary>
        public static ExceptionCode StructureIdentifiableMissingId
        {
            get
            {
                return _structureIdentifiableMissingId;
            }

            set
            {
                _structureIdentifiableMissingId = value;
            }
        }

        /// <summary>
        ///     Gets or sets the structure identifiable missing name.
        /// </summary>
        public static ExceptionCode StructureIdentifiableMissingName
        {
            get
            {
                return _structureIdentifiableMissingName;
            }

            set
            {
                _structureIdentifiableMissingName = value;
            }
        }

        /// <summary>
        ///     Gets or sets the structure identifiable missing urn.
        /// </summary>
        public static ExceptionCode StructureIdentifiableMissingUrn
        {
            get
            {
                return _structureIdentifiableMissingUrn;
            }

            set
            {
                _structureIdentifiableMissingUrn = value;
            }
        }

        /// <summary>
        ///     Gets or sets the structure invalid id.
        /// </summary>
        public static ExceptionCode StructureInvalidId
        {
            get
            {
                return _structureInvalidId;
            }

            set
            {
                _structureInvalidId = value;
            }
        }

        /// <summary>
        ///     Gets or sets the structure invalid id start alpha.
        /// </summary>
        public static ExceptionCode StructureInvalidIdStartAlpha
        {
            get
            {
                return _structureInvalidIdStartAlpha;
            }

            set
            {
                _structureInvalidIdStartAlpha = value;
            }
        }

        /// <summary>
        ///     Gets or sets the structure invalid organisation scheme no content.
        /// </summary>
        public static ExceptionCode StructureInvalidOrganisationSchemeNoContent
        {
            get
            {
                return _structureInvalidOrganisationSchemeNoContent;
            }

            set
            {
                _structureInvalidOrganisationSchemeNoContent = value;
            }
        }

        /// <summary>
        ///     Gets or sets the structure invalid version.
        /// </summary>
        public static ExceptionCode StructureInvalidVersion
        {
            get
            {
                return _structureInvalidVersion;
            }

            set
            {
                _structureInvalidVersion = value;
            }
        }

        /// <summary>
        ///     Gets or sets the structure maintainable missing agency.
        /// </summary>
        public static ExceptionCode StructureMaintainableMissingAgency
        {
            get
            {
                return _structureMaintainableMissingAgency;
            }

            set
            {
                _structureMaintainableMissingAgency = value;
            }
        }

        /// <summary>
        ///     Gets or sets the structure not found.
        /// </summary>
        public static ExceptionCode StructureNotFound
        {
            get
            {
                return _structureNotFound;
            }

            set
            {
                _structureNotFound = value;
            }
        }

        /// <summary>
        ///     Gets or sets the structure uri malformed.
        /// </summary>
        public static ExceptionCode StructureUriMalformed
        {
            get
            {
                return _structureUriMalformed;
            }

            set
            {
                _structureUriMalformed = value;
            }
        }

        /// <summary>
        ///     Gets or sets the structure urn malformed.
        /// </summary>
        public static ExceptionCode StructureUrnMalformed
        {
            get
            {
                return _structureUrnMalformed;
            }

            set
            {
                _structureUrnMalformed = value;
            }
        }

        /// <summary>
        ///     Gets or sets the structure urn malformed missing prefix.
        /// </summary>
        public static ExceptionCode StructureUrnMalformedMissingPrefix
        {
            get
            {
                return _structureUrnMalformedMissingPrefix;
            }

            set
            {
                _structureUrnMalformedMissingPrefix = value;
            }
        }

        /// <summary>
        ///     Gets or sets the structure urn malformed unkown prefix.
        /// </summary>
        public static ExceptionCode StructureUrnMalformedUnknownPrefix
        {
            get
            {
                return _structureUrnMalformedUnknownPrefix;
            }

            set
            {
                _structureUrnMalformedUnknownPrefix = value;
            }
        }

        /// <summary>
        ///     Gets or sets the structure urn unexpected prefix.
        /// </summary>
        public static ExceptionCode StructureUrnUnexpectedPrefix
        {
            get
            {
                return _structureUrnUnexpectedPrefix;
            }

            set
            {
                _structureUrnUnexpectedPrefix = value;
            }
        }

        /// <summary>
        ///     Gets or sets the unsupported.
        /// </summary>
        public static ExceptionCode Unsupported
        {
            get
            {
                return _uNSUPPORTED;
            }

            set
            {
                _uNSUPPORTED = value;
            }
        }

        /// <summary>
        ///     Gets or sets the unsupported datatype.
        /// </summary>
        public static ExceptionCode UnsupportedDataType
        {
            get
            {
                return _unsupportedDataType;
            }

            set
            {
                _unsupportedDataType = value;
            }
        }

        /// <summary>
        ///     Gets or sets the unsupported transform.
        /// </summary>
        public static ExceptionCode UnsupportedTransform
        {
            get
            {
                return _unsupportedTransform;
            }

            set
            {
                _unsupportedTransform = value;
            }
        }

        /// <summary>
        ///     Gets or sets the ustructure urn malformed part version infomation supplied.
        /// </summary>
        public static ExceptionCode UstructureUrnMalformedPartVersionInfomationSupplied
        {
            get
            {
                return _ustructureUrnMalformedPartVersionInfomationSupplied;
            }

            set
            {
                _ustructureUrnMalformedPartVersionInfomationSupplied = value;
            }
        }

        /// <summary>
        ///     Gets or sets the web service bad connection.
        /// </summary>
        public static ExceptionCode WebServiceBadConnection
        {
            get
            {
                return _webServiceBadConnection;
            }

            set
            {
                _webServiceBadConnection = value;
            }
        }

        /// <summary>
        ///     Gets or sets the web service bad response.
        /// </summary>
        public static ExceptionCode WebServiceBadResponse
        {
            get
            {
                return _webServiceBadResponse;
            }

            set
            {
                _webServiceBadResponse = value;
            }
        }

        /// <summary>
        ///     Gets or sets the web service configuration missing.
        /// </summary>
        public static ExceptionCode WebServiceConfigurationMissing
        {
            get
            {
                return _webServiceConfigurationMissing;
            }

            set
            {
                _webServiceConfigurationMissing = value;
            }
        }

        /// <summary>
        ///     Gets or sets the web service endpoint type invalid.
        /// </summary>
        public static ExceptionCode WebServiceEndpointTypeInvalid
        {
            get
            {
                return _webServiceEndpointTypeInvalid;
            }

            set
            {
                _webServiceEndpointTypeInvalid = value;
            }
        }

        /// <summary>
        ///     Gets or sets the web service invalid get data.
        /// </summary>
        public static ExceptionCode WebServiceInvalidGetData
        {
            get
            {
                return _webServiceInvalidGetData;
            }

            set
            {
                _webServiceInvalidGetData = value;
            }
        }

        /// <summary>
        ///     Gets or sets the web service invalid get schema.
        /// </summary>
        public static ExceptionCode WebServiceInvalidGetSchema
        {
            get
            {
                return _webServiceInvalidGetSchema;
            }

            set
            {
                _webServiceInvalidGetSchema = value;
            }
        }

        /// <summary>
        ///     Gets or sets the web service protocol missing.
        /// </summary>
        public static ExceptionCode WebServiceProtocolMissing
        {
            get
            {
                return _webServiceProtocolMissing;
            }

            set
            {
                _webServiceProtocolMissing = value;
            }
        }

        /// <summary>
        ///     Gets or sets the web service request missing.
        /// </summary>
        public static ExceptionCode WebServiceRequestMissing
        {
            get
            {
                return _webServiceRequestMissing;
            }

            set
            {
                _webServiceRequestMissing = value;
            }
        }

        /// <summary>
        ///     Gets or sets the web service socket timeout.
        /// </summary>
        public static ExceptionCode WebServiceSocketTimeout
        {
            get
            {
                return _webServiceSocketTimeout;
            }

            set
            {
                _webServiceSocketTimeout = value;
            }
        }

        /// <summary>
        ///     Gets or sets the web service unsupported protocol.
        /// </summary>
        public static ExceptionCode WebServiceUnsupportedProtocol
        {
            get
            {
                return _webServiceUnsupportedProtocol;
            }

            set
            {
                _webServiceUnsupportedProtocol = value;
            }
        }

        /// <summary>
        ///     Gets or sets the web service url missing.
        /// </summary>
        public static ExceptionCode WebServiceUrlMissing
        {
            get
            {
                return _webServiceUrlMissing;
            }

            set
            {
                _webServiceUrlMissing = value;
            }
        }

        /// <summary>
        ///     Gets or sets the xml parse exception.
        /// </summary>
        public static ExceptionCode XmlParseException
        {
            get
            {
                return _xmlParseException;
            }

            set
            {
                _xmlParseException = value;
            }
        }

        /// <summary>
        ///     Gets the code.
        /// </summary>
        public string Code
        {
            get
            {
                return this.code;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The equals.
        /// </summary>
        /// <param name="obj">
        /// The obj.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/> .
        /// </returns>
        public override bool Equals(object obj)
        {
            var that = obj as ExceptionCode;
            if (that != null)
            {
                return this.code.Equals(that.code);
            }

            return false;
        }

        /// <summary>
        ///     The get hash code.
        /// </summary>
        /// <returns>
        ///     The <see cref="int" /> .
        /// </returns>
        public override int GetHashCode()
        {
            return this.code.GetHashCode();
        }

        /// <summary>
        ///     The to string.
        /// </summary>
        /// <returns>
        ///     The <see cref="string" /> .
        /// </returns>
        public override string ToString()
        {
            return "Exception Code " + this.code;
        }

        #endregion
    }
}
