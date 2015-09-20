// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IKeyable.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Model.Data
{
    #region Using directives

    using System;
    using System.Collections.Generic;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.DataStructure;

    #endregion

    /// <summary>
    ///     A Keyable is a dataset item that is represented by a Key.  The only two items are SeriesKey and GroupKey, and this
    ///     item could reflect either, to determine if this is a seriesKey use the <c>isSeries()</c> method.
    /// </summary>
    public interface IKeyable : IAttributable
    {
        #region Public Properties
        
        /// <summary>
        /// Gets the data structure to which this key belongs, this will not be null
        /// </summary>
        IDataStructureObject DataStructure { get; }

        /// <summary>
        /// Gets the dataflow to which this key belongs.  This may be null
        /// if the dataflow is unknown or not applicable.
        /// </summary>
        IDataflowObject Dataflow { get; }
	

        /// <summary>
        ///     Gets the concept that is being cross sectionalised on , this is only relevant if isTimeSeries() is false.
        ///     <p />
        ///     If isTimeSeries() is true, then this call will return null
        /// </summary>
        /// <value> </value>
        string CrossSectionConcept { get; }

        /// <summary>
        ///     Gets the name (or id) of this group, if this is a group (i.e. isSeries() returns null)
        /// </summary>
        /// <value> </value>
        string GroupName { get; }

        /// <summary>
        ///     Gets the list of key values for this key
        /// </summary>
        /// <value> </value>
        IList<IKeyValue> Key { get; }

        /// <summary>
        ///     Gets the observation time as a Date Object, this is only relevant if isTimeSeries() is false.
        ///     <p />
        ///     If isTimeSeries() is true, then this call will return null
        /// </summary>
        /// <value> </value>
        DateTime? ObsAsTimeDate { get; }

        /// <summary>
        ///     Gets the observation time in a SDMX time format, this is only relevant if isTimeSeries() is false.
        ///     <p />
        ///     If isTimeSeries() is true, then this call will return null
        /// </summary>
        /// <value> </value>
        string ObsTime { get; }

        /// <summary>
        ///     Gets a value indicating whether the key belongs to a series key, if false then it belongs to a group
        /// </summary>
        /// <value> </value>
        bool Series { get; }

        /// <summary>
        ///     Gets a colon delimited string representing this key.
        ///     <p />
        ///     Example - if the Key Values are:
        ///     1. FREQ = A
        ///     2. COUNTRY = UK
        ///     3. SEX = M
        ///     <p />
        ///     The Short code would be A:UK:M
        /// </summary>
        /// <value> </value>
        string ShortCode { get; }

        /// <summary>
        ///     Gets the time format for the key, returns null if this is unknown
        /// </summary>
        /// <value> </value>
        TimeFormat TimeFormat { get; }

        /// <summary>
        ///     Gets a value indicating whether the key is a time series key, if false, then this KeyableArtifact will contain
        ///     a time
        /// </summary>
        /// <value> </value>
        bool TimeSeries { get; }

        /// <summary>
        /// Gets a copy of the annotations.
        /// </summary>
        /// <value>
        /// The annotations.
        /// </value>
        IList<IAnnotation> Annotations { get; } 

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Gets the key value for the dimension Id - returns null if the dimension id is not part of the key
        /// </summary>
        /// <param name="dimensionId">
        /// The dimension Id.
        /// </param>
        /// <returns>
        /// The <see cref="string"/> .
        /// </returns>
        string GetKeyValue(string dimensionId);

        #endregion
    }
}