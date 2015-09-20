// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SubmitStructureResponseBuilderV21.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The submit structure response builder v 21.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.Structureparser.Builder.XmlSerialization.V21.Response
{
    using System.Collections.Generic;

    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Common;
    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Message;
    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Registry;
    using Org.Sdmxsource.Sdmx.Api.Model;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;
    using Org.Sdmxsource.Sdmx.Api.Model.SubmissionResponse;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.SubmissionResponse;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Util;

    using StatusMessageType = Org.Sdmx.Resources.SdmxMl.Schemas.V21.Common.StatusMessageType;

    /// <summary>
    ///     The submit structure response builder v 21.
    /// </summary>
    public class SubmitStructureResponseBuilderV21
    {
        #region Public Methods and Operators

        /// <summary>
        /// Builds the list of <see cref="ISubmitStructureResponse"/> from <paramref name="registryInterface"/>
        /// </summary>
        /// <param name="registryInterface">
        /// The registry Interface.
        /// </param>
        /// <returns>
        /// The list of <see cref="ISubmitStructureResponse"/> from <paramref name="registryInterface"/>
        /// </returns>
        public IList<ISubmitStructureResponse> Build(RegistryInterface registryInterface)
        {
            // TODO REFACTOR - THIS IS VERY SIMILAR TO SUBMIT SUBSCRIPTION RESPONSE
            IList<ISubmitStructureResponse> returnList = new List<ISubmitStructureResponse>();

            /* foreach */
            foreach (
                SubmissionResultType resultType in registryInterface.Content.SubmitStructureResponse.SubmissionResult)
            {
                IStructureReference structureReference =
                    RefUtil.CreateReference(resultType.SubmittedStructure.MaintainableObject);
                if (resultType.StatusMessage != null && resultType.StatusMessage.status != null)
                {
                    IList<string> messages = new List<string>();
                    if (resultType.StatusMessage.MessageText != null)
                    {
                        // TODO Message Codes and Multilingual
                        foreach (StatusMessageType statusMessageType in resultType.StatusMessage.MessageText)
                        {
                            if (statusMessageType.Text != null)
                            {
                                /* foreach */
                                foreach (TextType tt in statusMessageType.Text)
                                {
                                    messages.Add(tt.TypedValue);
                                }
                            }
                        }
                    }

                    IErrorList errors = null;
                    switch (resultType.StatusMessage.status)
                    {
                        case StatusTypeConstants.Failure:
                            errors = new ErrorListCore(messages, false);
                            break;
                        case StatusTypeConstants.Warning:
                            errors = new ErrorListCore(messages, true);
                            break;
                    }

                    returnList.Add(new SubmitStructureResponseImpl(structureReference, errors));
                }
            }

            return returnList;
        }

        #endregion
    }
}