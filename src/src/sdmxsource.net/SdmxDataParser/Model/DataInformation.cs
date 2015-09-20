// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataInformation.cs" company="Eurostat">
//   Date Created : 2013-08-19
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.DataParser.Model
{
    #region Using directives

    using System;
    using System.Collections.Generic;

    using Org.Sdmxsource.Sdmx.Api.Engine;
    using Org.Sdmxsource.Sdmx.Api.Model.Data;

    #endregion

    /// <summary>
    /// The Data Information model.
    /// </summary>
    [Serializable]
    public class DataInformation
    {
        #region Fields

        /// <summary>
        /// The _groups
        /// </summary>
        private readonly int _groups;

        /// <summary>
        /// The _keys
        /// </summary>
        private readonly int _keys;

        /// <summary>
        /// The _observations
        /// </summary>
        private readonly int _observations;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DataInformation"/> class.
        /// </summary>
        /// <param name="dre">
        /// The data reader engine
        /// </param>
        public DataInformation(IDataReaderEngine dre)
        {
            if (dre == null)
            {
                throw new ArgumentException("No DataReaderEngine specified.");
            }

            dre.Reset();
            ISet<string> keySet = new HashSet<string>();
            ISet<string> groupSet = new HashSet<string>();

            while (dre.MoveNextKeyable())
            {
                IKeyable currentKey = dre.CurrentKey;

                if (currentKey.Series)
                {
                    keySet.Add(currentKey.ShortCode);
                }
                else
                {
                    groupSet.Add(currentKey.ShortCode);
                }

                while (dre.MoveNextObservation())
                {
                    this._observations++;
                }
            }

            this._keys = keySet.Count;
            this._groups = groupSet.Count;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the number of series keys
        /// </summary>
        public int NumberOfSeriesKeys
        {
            get
            {
                return this._keys;
            }
        }

        /// <summary>
        /// Gets the number of groups
        /// </summary>
        public int Groups
        {
            get
            {
                return this._groups;
            }
        }

        /// <summary>
        /// Gets the number of observations
        /// </summary>
        public int NumberOfObservations
        {
            get
            {
                return this._observations;
            }
        }

        #endregion
    }
}
