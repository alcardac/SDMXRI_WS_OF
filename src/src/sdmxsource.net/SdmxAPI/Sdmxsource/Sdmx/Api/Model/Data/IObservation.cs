// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IObservation.cs" company="Eurostat">
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

    #endregion

    /// <summary>
    ///     An Observation is a data element that defines a time and value, an Observation may also have attributes attached to it.
    ///     <p />
    ///     An Observation can be compared to another, with regards to time.  An obesrvation with a later time period, will return a value > 1
    /// </summary>
    public interface IObservation : IAttributable, IComparable<IObservation>
    {
        #region Public Properties

        /// <summary>
        /// Returns the parent series key for this observation.  The returned object can not be null.
        /// </summary>
        IKeyable SeriesKey { get; }
	
        /// <summary>
        ///     Gets a value indicating whether the is a cross sectional observation,
        ///     the call to get the obs time will still return the observation time of the cross section, also
        ///     available on the series key, however there is an additional cross sectional attribute available
        /// </summary>
        /// <value> </value>
        bool CrossSection { get; }

        /// <summary>
        ///     Gets the cross sectional key value
        /// </summary>
        /// <value> </value>
        IKeyValue CrossSectionalValue { get; }

        /// <summary>
        ///     Gets the observation time as a Date Object
        /// </summary>
        /// <value> </value>
        DateTime? ObsAsTimeDate { get; }

        /// <summary>
        ///     Gets the observation time in a SDMX time format
        /// </summary>
        /// <value> </value>
        string ObsTime { get; }

        /// <summary>
        ///     Gets the time format the observation value is in
        /// </summary>
        /// <value> </value>
        TimeFormat ObsTimeFormat { get; }

        /// <summary>
        ///     Gets the value of the observation
        /// </summary>
        /// <value> </value>
        string ObservationValue { get; }

        /// <summary>
        /// Gets a copy of the annotations.
        /// </summary>
        /// <value>
        /// The annotations.
        /// </value>
        IList<IAnnotation> Annotations { get; }

        #endregion
    }
}