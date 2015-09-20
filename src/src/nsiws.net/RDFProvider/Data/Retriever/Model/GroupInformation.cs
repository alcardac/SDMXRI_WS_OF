// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GroupInformation.cs" company="Eurostat">
//   Date Created : 2011-12-19
//   Copyright (c) 2011 by the European   Commission, represented by Eurostat. All rights reserved.
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   This class holds TimeSeries Group information
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace RDFProvider.Retriever.Model
{
    using System.Collections.Generic;

    using Estat.Sri.MappingStoreRetrieval.Engine.Mapping;
    using Estat.Sri.MappingStoreRetrieval.Model.MappingStoreModel;

    /// <summary>
    /// This class holds TimeSeries Group information
    /// </summary>
    public class GroupInformation
    {
        #region Constants and Fields

        /// <summary>
        ///   The component mappings for this group
        /// </summary>
        private readonly List<IComponentMapping> _componentMappings = new List<IComponentMapping>();

        /// <summary>
        ///   The set of keys already processed
        /// </summary>
        private readonly IDictionary<ReadOnlyKey, object> _keySet = new Dictionary<ReadOnlyKey, object>();

        /// <summary>
        ///   The group entity
        /// </summary>
        private readonly GroupEntity _thisGroup;

        /// <summary>
        ///   The SQL String
        /// </summary>
        private string _sql;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="GroupInformation"/> class.
        /// </summary>
        /// <param name="thisGroup">
        /// The group entity 
        /// </param>
        public GroupInformation(GroupEntity thisGroup)
        {
            this._thisGroup = thisGroup;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets the component mappings for this group
        /// </summary>
        public List<IComponentMapping> ComponentMappings
        {
            get
            {
                return this._componentMappings;
            }
        }

        /// <summary>
        ///   Gets the set of keys already processed
        /// </summary>
        public IDictionary<ReadOnlyKey, object> KeySet
        {
            get
            {
                return this._keySet;
            }
        }

        /// <summary>
        ///   Gets or sets the group dimension that is a measure component and is not mapped.
        /// </summary>
        public ComponentEntity MeasureComponent { get; set; }

        /// <summary>
        ///   Gets or sets the SQL String
        /// </summary>
        public string SQL
        {
            get
            {
                return this._sql;
            }

            set
            {
                this._sql = value;
            }
        }

        /// <summary>
        ///   Gets the group entity
        /// </summary>
        public GroupEntity ThisGroup
        {
            get
            {
                return this._thisGroup;
            }
        }

        #endregion
    }
}