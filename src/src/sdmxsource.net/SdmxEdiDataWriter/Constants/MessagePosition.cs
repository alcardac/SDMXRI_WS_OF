// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MessagePosition.cs" company="Eurostat">
//   Date Created : 2014-07-24
//   //   Copyright (c) 2014 by the European   Commission, represented by Eurostat.   All rights reserved.
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   The message position.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.EdiParser.Constants
{
    /// <summary>
    ///     The message position.
    /// </summary>
    public enum MessagePosition
    {
        /// <summary>
        ///     The message identification.
        /// </summary>
        MessageIdentification, 

        /// <summary>
        ///     The message function.
        /// </summary>
        MessageFunction, 

        /// <summary>
        ///     The codelist maintenance agency.
        /// </summary>
        CodelistMaintenanceAgency, 

        /// <summary>
        ///     The receiver identification.
        /// </summary>
        ReceiverIdentification, 

        /// <summary>
        ///     The sender identification.
        /// </summary>
        SenderIdentification, 

        /// <summary>
        ///     The concept identifier.
        /// </summary>
        ConceptIdentifier, 

        /// <summary>
        ///     The concept name.
        /// </summary>
        ConceptName, 

        /// <summary>
        ///     The codelist identifier.
        /// </summary>
        CodelistIdentifier, 

        /// <summary>
        ///     The code value.
        /// </summary>
        CodeValue, 

        /// <summary>
        ///     The code description.
        /// </summary>
        CodeDescription, 

        /// <summary>
        ///     The key family identifier.
        /// </summary>
        KeyFamilyIdentifier, 

        /// <summary>
        ///     The key family name.
        /// </summary>
        KeyFamilyName, 

        /// <summary>
        ///     The dimension.
        /// </summary>
        Dimension, 

        /// <summary>
        ///     The attribute.
        /// </summary>
        Attribute, 

        /// <summary>
        ///     The codelist reference.
        /// </summary>
        CodelistReference
    }
}