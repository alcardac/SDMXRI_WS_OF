// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IHeader.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Model.Header
{
    #region Using directives

    using System;
    using System.Collections.Generic;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;

    #endregion

    /// <summary>
    ///     A Header Contains any information to go at the beginning of an exchange message.
    ///     Methods are provided to write and read values.
    /// </summary>
    public interface IHeader
    {
        #region Public Properties

        /// <summary>
        ///     Gets or sets the action.
        /// </summary>
        DatasetAction Action { get; set; }

        /// <summary>
        ///     Gets a map of any additional attributes that are stored in the header - the key is the field, such as Header.DSD_REF, the value is the value of that field
        /// </summary>
        /// <value> </value>
        IDictionary<string, string> AdditionalAttribtues { get; }

        /// <summary>
        ///     Gets or sets the data provider reference.
        /// </summary>
        IStructureReference DataProviderReference { get; set; }

        /// <summary>
        ///     Gets or sets the dataset id.
        /// </summary>
        string DatasetId { get; set; }

        /// <summary>
        ///     Gets the embargo date.
        /// </summary>
        DateTime? EmbargoDate { get; }

        /// <summary>
        ///     Gets the extracted.
        /// </summary>
        DateTime? Extracted { get; }

        /// <summary>
        ///     Gets or sets the id.
        /// </summary>
        string Id { get; set; }

        /// <summary>
        ///     Gets the name.
        /// </summary>
        IList<ITextTypeWrapper> Name { get; }

        /// <summary>
        ///     Gets the prepared.
        /// </summary>
        DateTime? Prepared { get; }

        /// <summary>
        ///     Gets the receiver.
        /// </summary>
        IList<IParty> Receiver { get; }

        /// <summary>
        ///     Gets or sets the reporting begin.
        /// </summary>
        DateTime? ReportingBegin { get; set; }

        /// <summary>
        ///     Gets or sets the reporting end.
        /// </summary>
        DateTime? ReportingEnd { get; set; }

        /// <summary>
        ///     Gets or sets the sender.
        /// </summary>
        IParty Sender { get; set; }

        /// <summary>
        ///     Gets the source.
        /// </summary>
        IList<ITextTypeWrapper> Source { get; }

        /// <summary>
        ///     Gets the structures.
        /// </summary>
        IList<IDatasetStructureReference> Structures { get; }

        /// <summary>
        ///     Gets or sets a value indicating whether test.
        /// </summary>
        bool Test { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Gets the header value for a given field - for example getHeaderValue(Header.DSD_REF) would return the value given for the keyFamilyRef (SDMX 2.0)
        /// </summary>
        /// <param name="headerField">The field.
        /// </param>
        /// <returns>
        /// The <see cref="string"/> .
        /// </returns>
        string GetAdditionalAttribtue(string headerField);

        /// <summary>
        /// Gets the structure for the given id
        /// </summary>
        /// <param name="structureId">Structure Id
        /// </param>
        /// <returns>
        /// The <see cref="IDatasetStructureReference"/> .
        /// </returns>
        IDatasetStructureReference GetStructureById(string structureId);

        /// <summary>
        /// Gets a value indicating whether the Header instance has a value stored for this property in its additional attributes @See getAdditionalAttribtues()
        /// </summary>
        /// <param name="headerField">The field.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/> .
        /// </returns>
        bool HasAdditionalAttribtue(string headerField);

        /// <summary>
        /// Adds a receiver to the list of recivers
        /// </summary>
        /// <param name="recevier"></param>
        void AddReciever(IParty recevier);

        /// <summary>
        /// Adds a source to the list of sources
        /// </summary>
        /// <param name="recevier"></param>
        void AddSource(ITextTypeWrapper source);

        /// <summary>
        /// Adds a dataset structure reference to the list
        /// </summary>
        /// <param name="datasetStructureReference"></param>
        void AddStructure(IDatasetStructureReference datasetStructureReference);

        /// <summary>
        /// Adds a name to the list of names
        /// </summary>
        /// <param name="recevier"></param>
        void AddName(ITextTypeWrapper name);

        #endregion
    }
}