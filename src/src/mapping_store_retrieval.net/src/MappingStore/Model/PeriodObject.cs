// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PeriodObject.cs" company="Eurostat">
//   Date Created : 2013-07-16
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   The period object.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Estat.Sri.MappingStoreRetrieval.Model
{
    using System.Collections.Generic;
    using System.Globalization;

    /// <summary>
    ///     The period object.
    /// </summary>
    public class PeriodObject
    {
        #region Fields

        /// <summary>
        ///     The _codes.
        /// </summary>
        private readonly IList<string> _codes;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="PeriodObject"/> class.
        /// </summary>
        /// <param name="periodLength">
        /// The period Length.
        /// </param>
        /// <param name="periodFormat">
        /// The period Format.
        /// </param>
        /// <param name="id">
        /// The id.
        /// </param>
        public PeriodObject(int periodLength, string periodFormat, string id)
        {
            this.Id = id;
            this._codes = new string[periodLength];
            for (int i = 0; i < periodLength; i++)
            {
                this.Codes[i] = (i + 1).ToString(periodFormat, CultureInfo.InvariantCulture);
            }
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets the codes.
        /// </summary>
        public IList<string> Codes
        {
            get
            {
                return this._codes;
            }
        }

        /// <summary>
        ///     Gets the id.
        /// </summary>
        public string Id { get; private set; }

        #endregion
    }
}