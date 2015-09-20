// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ComponentValue.cs" company="Eurostat">
//   Date Created : 2012-01-18
//   Copyright (c) 2012 by the European   Commission, represented by Eurostat. All rights reserved.
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   A class that holds a component and value pairs
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Estat.Nsi.DataRetriever.Model
{
    using Estat.Sri.MappingStoreRetrieval.Model.MappingStoreModel;

    /// <summary>
    /// A class that holds a component and value pairs
    /// </summary>
    internal class ComponentValue
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ComponentValue"/> class.
        /// </summary>
        /// <param name="key">
        /// The dsd component.
        /// </param>
        public ComponentValue(ComponentEntity key)
        {
            this.Key = key;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets the DSD Component
        /// </summary>
        public ComponentEntity Key { get; private set; }

        /// <summary>
        ///   Gets or sets the value
        /// </summary>
        public string Value { get; set; }

        #endregion
    }
}