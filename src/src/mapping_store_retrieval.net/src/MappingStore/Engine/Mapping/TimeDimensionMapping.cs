// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TimeDimensionMapping.cs" company="Eurostat">
//   Date Created : 2011-09-08
//   Copyright (c) 2011 by the European   Commission, represented by Eurostat. All rights reserved.
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   Base class for Time Dimension Trancoding classes
//   Contains some common methods used by all/most Time Transcoding class plus some
//   static methods to create the correct Time Transcoding class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Estat.Sri.MappingStoreRetrieval.Engine.Mapping
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    using Estat.Sri.MappingStoreRetrieval.Helper;
    using Estat.Sri.MappingStoreRetrieval.Model.MappingStoreModel;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Util.Date;

    /// <summary>
    /// Base class for Time Dimension Transcoding classes
    /// Contains some common methods used by all/most Time Transcoding class plus some
    /// static methods to create the correct Time Transcoding class.
    /// </summary>
    public class TimeDimensionMapping : ComponentMapping
    {
        #region Constants and Fields

        /// <summary>
        /// This field holds the TRANSCODING.EXPRESSION contents
        /// </summary>
        private readonly TimeExpressionEntity _expression;

        /// <summary>
        /// The current periodicity
        /// </summary>
        private readonly IPeriodicity _periodicity;

        /// <summary>
        /// This field has the DDB specific SUBSTRING or SUBSTR SQL command
        /// </summary>
        private string _substringCmd;

        /// <summary>
        /// This field holds whether the substring command requires the LEN parameter.
        /// </summary>
        private bool _substringCmdRequiresLen;

        /// <summary>
        /// The cast to string
        /// </summary>
        private string _castToString;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TimeDimensionMapping"/> class. 
        /// Initialize an new instance of the TimeDimensionMapping based class
        /// </summary>
        /// <param name="mapping">
        /// The time dimension mapping
        /// </param>
        /// <param name="expression">
        /// The TRANSCODING.EXPRESSION contents
        /// </param>
        /// <param name="databaseType">
        /// The dissemination database vendor from DB_CONNECTION.DB_TYPE at Mapping Store database. It is used to determine the substring command to use
        /// </param>
        /// <exception cref="TranscodingException">
        /// Occurs when transcoding cannot performed due to incorrect mapping store data
        /// </exception>
        protected TimeDimensionMapping(MappingEntity mapping, TimeExpressionEntity expression, string databaseType)
        {
            this.Mapping = mapping;
            this.Component = mapping.Components[0];
            this._expression = expression;
            this.SetDbType(databaseType);
            if (expression != null)
            {
                this._periodicity = PeriodicityFactory.Create(expression.Freq);
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the number format to invariant culture to speed up <c>int.ToString()</c>
        /// </summary>
        protected static IFormatProvider FormatProvider
        {
            get
            {
                return CultureInfo.InvariantCulture;
            }
        }

        /// <summary>
        /// Gets the TRANSCODING.EXPRESSION contents
        /// </summary>
        protected TimeExpressionEntity Expression
        {
            get
            {
                return this._expression;
            }
        }

        /// <summary>
        /// Gets current periodicity
        /// </summary>
        protected IPeriodicity Periodicity
        {
            get
            {
                return this._periodicity;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Creates the appropriate Time Dimension Mapping object <see cref="ITimeDimension"/>  based on the mapping from the mapping store
        /// e.g. it will create a different object if TimeDimension is mapped to one column, different if it is mapped two column and different if the column is Date type
        /// </summary>
        /// <param name="mapping">
        /// The mapping entity<see cref="MappingEntity"/> of the Time Dimension component
        /// </param>
        /// <param name="frequencyComponentMapping">
        /// The frequency component mapping.
        /// </param>
        /// <param name="databaseType">
        /// The dissemination database vendor. Is needed to generate the correct SQL query where conditions
        /// </param>
        /// <returns>
        /// An Time Dimension Transcoding object<see cref="ITimeDimension"/>
        /// </returns>
        public static ITimeDimension Create(MappingEntity mapping, IComponentMapping frequencyComponentMapping, string databaseType)
        {
            ITimeDimension timeDimensionTranscoding = null;
            if (mapping.Transcoding == null)
            {
                timeDimensionTranscoding = new TimeDimensionSingleFrequency(CreateTimeDimensionMapping(mapping, databaseType));
            }
            else if (mapping.Transcoding.TimeTranscodingCollection.Count == 1)
            {
                timeDimensionTranscoding = new TimeDimensionSingleFrequency(CreateTranscodedTimeDimensionMapping(mapping, databaseType, mapping.Transcoding.TimeTranscodingCollection.First()));
            }
            else if (mapping.Transcoding.TimeTranscodingCollection.Count > 1)
            {
                var timeDimensionMappings = new Dictionary<string, ITimeDimensionMapping>(StringComparer.Ordinal);
                foreach (var transcodingEntity in mapping.Transcoding.TimeTranscodingCollection)
                {
                    var timeDimensionSubTranscoding = CreateTranscodedTimeDimensionMapping(mapping, databaseType, transcodingEntity);
                    timeDimensionMappings[transcodingEntity.FrequencyValue] = timeDimensionSubTranscoding;
                }
                
                timeDimensionTranscoding = new TimeDimensionMultiFrequency(timeDimensionMappings, frequencyComponentMapping);
            }

            if (timeDimensionTranscoding != null)
            {
                timeDimensionTranscoding.Component = mapping.Components[0];
                timeDimensionTranscoding.Mapping = mapping;
            }

            return timeDimensionTranscoding;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Creates the sub string clause.
        /// </summary>
        /// <param name="columnName">Name of the column.</param>
        /// <param name="start">The start.</param>
        /// <param name="length">The length.</param>
        /// <param name="sqlOperator">The SQL operator.</param>
        /// <returns>The WHERE Clause with substring</returns>
        protected string CreateSubStringClause(string columnName, int start, int length, string sqlOperator)
        {
            string yearColumnCast = string.Format(FormatProvider, this._castToString, columnName);
            var normLength = length <= 0 && this._substringCmdRequiresLen ? 100 : length;

            if (normLength > 0)
            {
                return string.Format(
                    FormatProvider,
                    " ( {0}({1},{2},{3}) {4} '{{0}}' )",
                    this._substringCmd,
                    yearColumnCast,
                    start,
                    normLength,
                    sqlOperator);
            }

            return string.Format(
                FormatProvider,
                " ( {0}({1},{2}) {3} '{{0}}' )",
                this._substringCmd,
                yearColumnCast,
                start,
                sqlOperator);
        }

        /// <summary>
        /// Create time dimension mapping without transcoding
        /// </summary>
        /// <param name="mapping">
        /// The mapping entity<see cref="MappingEntity"/> of the Time Dimension component
        /// </param>
        /// <param name="databaseType">
        /// The dissemination database vendor. Is needed to generate the correct SQL query where conditions
        /// </param>
        /// <exception cref="TranscodingException">
        /// Incomplete Mapping. N column Time Dimension mapping without transcoding is not supported, where N not in 0,1
        /// </exception>
        /// <returns>
        /// An Time Dimension Transcoding object<see cref="ITimeDimension"/>
        /// </returns>
        private static ITimeDimensionMapping CreateTimeDimensionMapping(MappingEntity mapping, string databaseType)
        {
            ITimeDimensionMapping timeDimensionTranscoding;
            switch (mapping.Columns.Count)
            {
                case 1:
                    timeDimensionTranscoding = new TimeDimension1To1(mapping, null, databaseType);
                    break;
                case 0:
                    timeDimensionTranscoding = new TimeDimensionConstant(mapping, null, databaseType);
                    break;
                default:
                    throw new TranscodingException(
                        string.Format(
                            CultureInfo.CurrentCulture, 
                            ErrorMessages.TimeDimensionUnsupportedMappingFormat1, 
                            mapping.Columns.Count)); // MAT-496
            }

            return timeDimensionTranscoding;
        }

        /// <summary>
        /// Create time dimension mapping with transcoding 
        /// </summary>
        /// <param name="mapping">
        ///     The mapping entity<see cref="MappingEntity"/> of the Time Dimension component
        /// </param>
        /// <param name="databaseType">
        ///     The dissemination database vendor. Is needed to generate the correct SQL query where conditions
        /// </param>
        /// <param name="timeTranscoding">
        /// The Mapping Store <c>TIME_TRANSCODING</c> object
        /// </param>
        /// <exception cref="TranscodingException">
        /// Incomplete/Invalid mapping or expression
        /// </exception>
        /// <returns>
        /// An Time Dimension Transcoding object<see cref="ITimeDimension"/>
        /// </returns>
        private static ITimeDimensionMapping CreateTranscodedTimeDimensionMapping(MappingEntity mapping, string databaseType, TimeTranscodingEntity timeTranscoding)
        {
            ITimeDimensionMapping timeDimensionTranscoding;
            TimeExpressionEntity expr = TimeExpressionEntity.CreateExpression(timeTranscoding);

            if (!expr.IsDateTime)
            {
                if (!expr.OneColumnMapping)
                {
                    timeDimensionTranscoding = new TimeDimension2Column(mapping, expr, databaseType);
                }
                else
                {
                    if (expr.YearLength != 4)
                    {
                        throw new TranscodingException(ErrorMessages.TimeDimensionYearNo4Digits);
                    }

                    if (expr.Freq != TimeFormatEnumType.Year)
                    {
                        if (expr.PeriodLength <= 0 && expr.YearStart > expr.PeriodStart)
                        {
                            throw new TranscodingException(ErrorMessages.TimeDimensionVariableLenPeriodNotFirst);
                        }

                        if (((expr.YearStart + expr.YearLength) > expr.PeriodStart && expr.YearStart < expr.PeriodStart)
                            ||
                            ((expr.PeriodStart + expr.PeriodLength) > expr.YearStart
                             && expr.YearStart > expr.PeriodStart) || expr.PeriodStart == expr.YearStart)
                        {
                            throw new TranscodingException(ErrorMessages.TimeDimensionYearPeriodOverlap);
                        }
                    }

                    timeDimensionTranscoding = new TimeDimension1Column(mapping, expr, databaseType);
                }
            }
            else
            {
                timeDimensionTranscoding = new TimeDimensionDateType(mapping, expr, databaseType);
            }

            return timeDimensionTranscoding;
        }

        /// <summary>
        /// The dissemination database vendor
        /// </summary>
        /// <param name="dataBaseType">
        /// Database type
        /// </param>
        private void SetDbType(string dataBaseType)
        {
            var providerName = DatabaseType.GetProviderName(dataBaseType);
            var setting = DatabaseType.DatabaseSettings[providerName];
            if (setting != null)
            {
                this._substringCmd = setting.SubstringCommand;
                this._substringCmdRequiresLen = setting.SubstringCommandRequiresLength;
                this._castToString = setting.CastToString;
            }
        }

        #endregion
    }
}