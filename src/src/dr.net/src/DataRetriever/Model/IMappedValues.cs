// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IMappedValues.cs" company="Eurostat">
//   Date Created : 2011-12-19
//   Copyright (c) 2011 by the European   Commission, represented by Eurostat. All rights reserved.
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   Mapped values collection interface
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Estat.Nsi.DataRetriever.Model
{
    /// <summary>
    /// Mapped values collection interface
    /// </summary>
    internal interface IMappedValues
    {
        #region Public Properties

        /// <summary>
        ///   Gets or sets the Time dimension value
        /// </summary>
        string TimeValue { get; set; }

        /// <summary>
        ///   Gets or sets the frequency value value
        /// </summary>
        string FrequencyValue { get; set; }

        /// <summary>
        /// Gets or sets the dimension at observation value
        /// </summary>
        string DimensionAtObservationValue { get; set; }

        #endregion

        #region Public Methods

        /////// <summary>
        /////// Add the <paramref name="component"/> and it's <paramref name="value"/> to the list
        /////// </summary>
        /////// <param name="component">
        /////// The component. 
        /////// </param>
        /////// <param name="value">
        /////// The mapped/transcoded value. 
        /////// </param>
        /////// <exception cref="ArgumentOutOfRangeException">
        /////// Invalid
        ///////   <see cref="SdmxComponentType"/>
        ///////   -or-
        ///////   Invalid
        ///////   <see cref="AttachmentLevel"/>
        /////// </exception>
        ////void Add(ComponentEntity component, string value);

        /// <summary>
        /// Add the <paramref name="value"/> to component at <paramref name="index"/>. 
        /// The index is specified in constructor
        /// </summary>
        /// <param name="index">The index</param>
        /// <param name="value">The value</param>
        void Add(int index, string value);
/*
        /// <summary>
        /// Clear all values
        /// </summary>
        void Clear();
        */
        #endregion
    }
}