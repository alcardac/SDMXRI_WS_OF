// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SubmitStructureResponseBuilderV21.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The submit structure response builder v 21.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.Structureparser.Builder.XmlSerialization.Registry.Response.V21
{
    using System;
    using System.Collections.Generic;

    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Common;
    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Message;
    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Registry;
    using Org.Sdmxsource.Sdmx.Api.Exception;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;
    using Org.Sdmxsource.Sdmx.Structureparser.Builder.XmlSerialization.V21;
    using Org.Sdmxsource.Util;

    using StatusMessageType = Org.Sdmx.Resources.SdmxMl.Schemas.V21.Registry.StatusMessageType;
    using SubmitStructureResponseType = Org.Sdmx.Resources.SdmxMl.Schemas.V21.Registry.SubmitStructureResponseType;

    /// <summary>
    ///     The submit structure response builder v 21.
    /// </summary>
    public class SubmitStructureResponseBuilderV21 : AbstractResponseBuilder
    {
        #region Fields

        /// <summary>
        ///     The header xml beans builder.
        /// </summary>
        private readonly StructureHeaderXmlBuilder<BasicHeaderType> _headerXmlsBuilder =
            new StructureHeaderXmlBuilder<BasicHeaderType>();

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Build  the error response.
        /// </summary>
        /// <param name="exception">
        /// The exception.
        /// </param>
        /// <param name="errorBean">
        /// The error bean.
        /// </param>
        /// <returns>
        /// The <see cref="RegistryInterface"/>.
        /// </returns>
        /// <exception cref="BuilderException">
        /// Registry could not determine Maintainable Artefact in error
        /// </exception>
        public RegistryInterface BuildErrorResponse(Exception exception, IStructureReference errorBean)
        {
            if (errorBean == null)
            {
                throw new SdmxSemmanticException("Registry could not determine Maintainable Artefact in error", exception);
            }

            var responseType = new RegistryInterface();
            RegistryInterfaceType regInterface = responseType.Content;
            V21Helper.Header = regInterface;
            var returnType = new SubmitStructureResponseType();
            regInterface.SubmitStructureResponse = returnType;
            this.AddSubmissionResult(returnType, errorBean, exception);
            return responseType;
        }

        /// <summary>
        /// The build success response.
        /// </summary>
        /// <param name="beans">
        /// The beans.
        /// </param>
        /// <returns>
        /// The <see cref="RegistryInterface"/>.
        /// </returns>
        public RegistryInterface BuildSuccessResponse(ISdmxObjects beans)
        {
            var responseType = new RegistryInterface();
            RegistryInterfaceType regInterface = responseType.Content;
            regInterface.Header = this._headerXmlsBuilder.Build(beans.Header);
            var returnType = new SubmitStructureResponseType();
            regInterface.SubmitStructureResponse = returnType;


            this.ProcessMaintainables(returnType, beans.GetAllMaintainables());
            return responseType;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Adds submission result.
        /// </summary>
        /// <param name="returnType">
        /// The return type.
        /// </param>
        /// <param name="structureReference">
        /// The structure reference.
        /// </param>
        /// <param name="exception">
        /// The exception.
        /// </param>
        private void AddSubmissionResult(
            SubmitStructureResponseType returnType, IStructureReference structureReference, Exception exception)
        {
            var submissionResult = new SubmissionResultType();
            returnType.SubmissionResult.Add(submissionResult);
            var statusMessageType = new StatusMessageType();
            submissionResult.StatusMessage = statusMessageType;
            this.AddStatus(statusMessageType, exception);
            var submittedStructure = new SubmittedStructureType();
            submissionResult.SubmittedStructure = submittedStructure;
            var refType = new MaintainableReferenceType();
            submittedStructure.MaintainableObject = refType;
            if (ObjectUtil.ValidString(structureReference.MaintainableUrn))
            {
                refType.URN.Add(structureReference.MaintainableUrn);
            }
            else
            {
                var xref = new MaintainableRefType();
                refType.SetTypedRef(xref);
                IMaintainableRefObject maintainableReference = structureReference.MaintainableReference;
                string value = maintainableReference.AgencyId;
                if (!string.IsNullOrWhiteSpace(value))
                {
                    xref.agencyID = maintainableReference.AgencyId;
                }

                string value1 = maintainableReference.MaintainableId;
                if (!string.IsNullOrWhiteSpace(value1))
                {
                    xref.agencyID = maintainableReference.MaintainableId;
                }

                string value2 = maintainableReference.Version;
                if (!string.IsNullOrWhiteSpace(value2))
                {
                    xref.agencyID = maintainableReference.Version;
                }
            }
        }

        /// <summary>
        /// Process the maintainable.
        /// </summary>
        /// <param name="returnType">
        /// The return type.
        /// </param>
        /// <param name="maintainableObjects">
        /// The maintainable Objects.
        /// </param>
        private void ProcessMaintainables(
            SubmitStructureResponseType returnType, IEnumerable<IMaintainableObject> maintainableObjects)
        {
            foreach (IMaintainableObject maint in maintainableObjects)
            {
                this.AddSubmissionResult(returnType, maint.AsReference, null);
            }
        }

        #endregion
    }
}