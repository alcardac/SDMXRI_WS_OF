// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ComponentInfo.cs" company="Eurostat">
//   Date Created : 2012-03-28
//   Copyright (c) 2012 by the European   Commission, represented by Eurostat. All rights reserved.
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   A value object class for holding component related information
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Estat.Nsi.StructureRetriever.Model
{
    using Estat.Sri.MappingStoreRetrieval.Engine.Mapping;
    using Estat.Sri.MappingStoreRetrieval.Model.MappingStoreModel;

    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;
    using Org.Sdmxsource.Sdmx.Util.Objects.Reference;

    /// <summary>
    /// A value object class for holding component related information
    /// </summary>
    internal class ComponentInfo
    {
        #region Constants and Fields

        /// <summary>
        ///   A reference to the code list used by the component
        /// </summary>
        private readonly MaintainableRefObjectImpl _codelistRef = new MaintainableRefObjectImpl();

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets the reference to the code list used by the component
        /// </summary>
        public MaintainableRefObjectImpl CodelistRef
        {
            get
            {
                return this._codelistRef;
            }
        }

        /// <summary>
        ///   Gets or sets the <see cref="IComponentMapping" /> information of the component
        /// </summary>
        public IComponentMapping ComponentMapping { get; set; }

        /// <summary>
        ///   Gets or sets the <see cref="MappingEntity" /> used by the component
        /// </summary>
        public MappingEntity Mapping { get; set; }

        #endregion
    }
}