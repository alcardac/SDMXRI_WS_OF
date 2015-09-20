// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TimeTranscodingFieldOrdinal.cs" company="Eurostat">
//   Date Created : 2013-08-05
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   The time transcoding field ordinal.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Estat.Sri.MappingStoreRetrieval.Model
{
    using System;
    using System.Data;
    using System.Linq;

    using Estat.Sri.MappingStoreRetrieval.Model.MappingStoreModel;

    /// <summary>
    /// The time transcoding field ordinal.
    /// </summary>
    internal class TimeTranscodingFieldOrdinal
    {
        #region Fields

        /// <summary>
        /// The _date column.
        /// </summary>
        private readonly NameOrdinal _dateColumn;

        /// <summary>
        /// The _name ordinals.
        /// </summary>
        private readonly NameOrdinal[] _nameOrdinals;

        /// <summary>
        /// The _period column.
        /// </summary>
        private readonly NameOrdinal _periodColumn;

        /// <summary>
        /// The _year column.
        /// </summary>
        private readonly NameOrdinal _yearColumn;

        /// <summary>
        /// The _last reader.
        /// </summary>
        private IDataReader _lastReader;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TimeTranscodingFieldOrdinal"/> class. 
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        /// <param name="mapping">
        /// The mapping.
        /// </param>
        /// <param name="expression">
        /// The expression.
        /// </param>
        public TimeTranscodingFieldOrdinal(MappingEntity mapping, TimeExpressionEntity expression)
        {
            this._yearColumn = new NameOrdinal(GetColumnName(mapping, expression.YearColumnSysId));
            this._periodColumn = new NameOrdinal(GetColumnName(mapping, expression.PeriodColumnSysId));
            this._dateColumn = new NameOrdinal(GetColumnName(mapping, expression.DateColumnSysId));
            if (this._yearColumn.IsSet)
            {
                this._nameOrdinals = this._periodColumn.IsSet ? new[] { this._yearColumn, this._periodColumn } : new[] { this._yearColumn };
            }
            else
            {
                this._nameOrdinals = new[] { this._dateColumn };
            }
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the date ordinal.
        /// </summary>
        public int DateOrdinal
        {
            get
            {
                return this._dateColumn.Ordinal;
            }
        }

        /// <summary>
        /// Gets the period ordinal.
        /// </summary>
        public int PeriodOrdinal
        {
            get
            {
                return this._periodColumn.Ordinal;
            }
        }

        /// <summary>
        /// Gets the year ordinal.
        /// </summary>
        public int YearOrdinal
        {
            get
            {
                return this._yearColumn.Ordinal;
            }
        }

        #endregion

        #region Public Methods and Operators


        /// <summary>
        /// Builds the ordinal.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <exception cref="System.ArgumentNullException"><paramref name="reader"/> is null</exception>
        public void BuildOrdinal(IDataReader reader)
        {
            if (reader == null)
            {
                throw new ArgumentNullException("reader");
            }

            if (!ReferenceEquals(this._lastReader, reader))
            {
                this._lastReader = reader;
                for (int i = 0; i < this._nameOrdinals.Length; i++)
                {
                    var nameOrdinal = this._nameOrdinals[i];
                    nameOrdinal.Ordinal = reader.GetOrdinal(nameOrdinal.Name);
                }
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the name of the column.
        /// </summary>
        /// <param name="mapping">
        /// The mapping.
        /// </param>
        /// <param name="dateColumnSysId">
        /// The date column sys id.
        /// </param>
        /// <returns>
        /// The column name
        /// </returns>
        private static string GetColumnName(MappingEntity mapping, long dateColumnSysId)
        {
            if (dateColumnSysId > 0)
            {
                return mapping.Columns.First(entity => entity.SysId == dateColumnSysId).Name;
            }

            return null;
        }

        #endregion

        /// <summary>
        /// The name ordinal.
        /// </summary>
        private class NameOrdinal
        {
            #region Fields

            /// <summary>
            /// The _name.
            /// </summary>
            private readonly string _name;

            #endregion

            #region Constructors and Destructors

            /// <summary>
            /// Initializes a new instance of the <see cref="NameOrdinal"/> class. 
            /// Initializes a new instance of the <see cref="T:System.Object"/> class.
            /// </summary>
            /// <param name="name">
            /// The name.
            /// </param>
            public NameOrdinal(string name)
            {
                this._name = name;
                this.Ordinal = -1;
            }

            #endregion

            #region Public Properties

            /// <summary>
            /// Gets a value indicating whether is set.
            /// </summary>
            public bool IsSet
            {
                get
                {
                    return this._name != null;
                }
            }

            /// <summary>
            /// Gets the name.
            /// </summary>
            public string Name
            {
                get
                {
                    return this._name;
                }
            }

            /// <summary>
            /// Gets or sets the ordinal.
            /// </summary>
            public int Ordinal { get; set; }

            #endregion
        }
    }
}