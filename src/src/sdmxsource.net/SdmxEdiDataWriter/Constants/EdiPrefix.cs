// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EdiPrefix.cs" company="Eurostat">
//   Date Created : 2014-07-22
//   //   Copyright (c) 2014 by the European   Commission, represented by Eurostat.   All rights reserved.
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   The EDI prefix.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.EdiParser.Constants
{
    /// <summary>
    /// The EDI prefix.
    /// </summary>
    public enum EdiPrefix
    {
        /// <summary>
        /// The null
        /// </summary>
        Null = 0,

        /// <summary>
        /// The message start.
        /// </summary>
        MessageStart = 1, 

        /// <summary>
        /// The interchange header.
        /// </summary>
        InterchangeHeader, 

        /// <summary>
        /// The message identification.
        /// </summary>
        MessageIdentification, 

        /// <summary>
        /// The message function.
        /// </summary>
        MessageFunction, 

        /// <summary>
        /// The end message administration.
        /// </summary>
        EndMessageAdministration, 

        /// <summary>
        /// The end message.
        /// </summary>
        EndMessage, 

        /// <summary>
        /// The contact information.
        /// </summary>
        ContactInformation, 

        /// <summary>
        /// The communication number.
        /// </summary>
        CommunicationNumber, 

        /// <summary>
        /// The data start.
        /// </summary>
        DataStart, 

        /// <summary>
        /// The dataset action.
        /// </summary>
        DatasetAction, 

        /// <summary>
        /// The dataset date time.
        /// </summary>
        DatasetDatetime, 

        /// <summary>
        /// The dataset send method.
        /// </summary>
        DatasetSendMethod, 

        /// <summary>
        /// The dataset missing value Symbol.
        /// </summary>
        DatasetMissingValueSymbol, 

        /// <summary>
        /// The dataset data.
        /// </summary>
        DatasetData, 

        /// <summary>
        /// The dataset data attribute.
        /// </summary>
        DatasetDataAttribute, 

        /// <summary>
        /// The dataset footnote section.
        /// </summary>
        DatasetFootnoteSection, 

        /// <summary>
        /// The dataset attribute scope.
        /// </summary>
        DatasetAttributeScope, 

        /// <summary>
        /// The dataset attribute coded.
        /// </summary>
        DatasetAttributeCoded, 

        /// <summary>
        /// The message id provided by sender.
        /// </summary>
        MessageIdProvidedBySender, 

        /// <summary>
        /// The dataset attribute un-coded.
        /// </summary>
        DatasetAttributeUncoded, 

        /// <summary>
        /// The DSD reference.
        /// </summary>
        DsdReference, 

        /// <summary>
        /// The message agency.
        /// </summary>
        MessageAgency, 

        /// <summary>
        /// The receiving agency.
        /// </summary>
        ReceivingAgency, 

        /// <summary>
        /// The sending agency.
        /// </summary>
        SendingAgency, 

        /// <summary>
        /// The code list.
        /// </summary>
        Codelist, 

        /// <summary>
        /// The code value.
        /// </summary>
        CodeValue, 

        /// <summary>
        /// The DSD.
        /// </summary>
        Dsd, 

        /// <summary>
        /// The attribute.
        /// </summary>
        Attribute, 

        /// <summary>
        /// The dimension.
        /// </summary>
        Dimension, 

        /// <summary>
        /// The concept.
        /// </summary>
        Concept, 

        /// <summary>
        /// The string.
        /// </summary>
        String, 

        /// <summary>
        /// The field length.
        /// </summary>
        FieldLength, 

        /// <summary>
        /// The usage status.
        /// </summary>
        UseageStatus, 

        /// <summary>
        /// The attribute attachment value.
        /// </summary>
        AttributeAttachmentValue, 

        /// <summary>
        /// The code list reference.
        /// </summary>
        CodelistReference, 
    }
}